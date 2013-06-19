﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CiberIs.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OfficeOpenXml;

namespace CiberIs.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        public ActionResult Index()
        {
            return View();
        }

        public string LastNotPaid()
        {
            var purchase = _mongoDb.GetCollection<Purchase>("Purchases").AsQueryable().Where(x => !x.IsPaidFor).OrderBy(x => x.Time).FirstOrDefault();
            return purchase != null && purchase.Time.HasValue ? purchase.Time.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        public FileResult SeeReport(string dateFrom, string dateTo)
        {
            var dFrom = DateTime.Parse(dateFrom);
            var dTo = DateTime.Parse(dateTo).AddDays(1);
            var purchases =
                _mongoDb.GetCollection<Purchase>("Purchases")
                        .AsQueryable()
                        .Where(x => !x.IsPaidFor && x.Time != null && x.Time > dFrom && x.Time <= dTo)
                        .OrderBy(x => x.Time).ToList();
            var file = GetFileReport(purchases.ToList());
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult EffectuateReport(string dateFrom, string dateTo)
        {
            var dFrom = DateTime.Parse(dateFrom);
            var dTo = DateTime.Parse(dateTo).AddDays(1);
            var purchases =
                _mongoDb.GetCollection<Purchase>("Purchases")
                        .AsQueryable()
                        .Where(x => !x.IsPaidFor && x.Time != null && x.Time > dFrom && x.Time <= dTo)
                        .OrderBy(x => x.Time).ToList();
            var file = GetFileReport(purchases);
            foreach (var purchase in purchases)
            {
                purchase.IsPaidFor = true;
                _mongoDb.GetCollection<Purchase>("Purchases").Save(purchase);
            }
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private byte[] GetFileReport(IList<Purchase> purchases)
        {
            using (var pck = new ExcelPackage())
            {
                //Add the Content sheet
                var ws = pck.Workbook.Worksheets.Add("Oppsummering");
                var purchaseByPerson = purchases.OrderBy(x => x.Buyer).GroupBy(x => x.Buyer)
                                                .Select(x => new { Sum = x.Sum(y => y.Price), x.First().Buyer, Count = x.Count() }).ToList();
                ws.Cells[1, 1].Value = "Ansattnummer";
                ws.Cells[1, 2].Value = "Sum (kr)";
                ws.Cells[1, 3].Value = "Antall is";
                for (var i = 0; i < purchaseByPerson.Count; i++)
                {
                    var purchase = purchaseByPerson[i];
                    ws.Cells[i + 2, 1].Value = purchase.Buyer;
                    ws.Cells[i + 2, 2].Value = purchase.Sum;
                    ws.Cells[i + 2, 3].Value = purchase.Count;
                }
                return pck.GetAsByteArray();
            }
        }
    }
}