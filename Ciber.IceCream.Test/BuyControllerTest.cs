using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using CiberIs.Controllers;
using CiberIs.Models;
using FakeItEasy;
using MongoDB.Driver;
using NUnit.Framework;

namespace Ciber.IceCream.Test
{
    using CiberIs.Badges;

    [TestFixture]
    public class BuyControllerTest
    {
        private BuyController _controller;
        private IMongoDb _mongoDb;

        [SetUp]
        public void Init()
        {
            _mongoDb = A.Fake<IMongoDb>();
            _controller = new BuyController(_mongoDb, A.Fake<IBadgeService>());
        }

        [Test]
        public void BuyIceThrowsHttpExceptionIfThereIsNoSuchIce()
        {
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(null);
            Assert.Throws<HttpResponseException>(
                () => _controller.Post(new FormDataCollection(Enumerable.Empty<KeyValuePair<string, string>>())));
        }

        [Test]
        public void BuyIceThrowsHttpExceptionIfThereAreZeroIcesLeft()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 0};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            var exception =
                Assert.Throws<HttpResponseException>(
                    () => _controller.Post(new FormDataCollection(Enumerable.Empty<KeyValuePair<string, string>>())));
            Assert.AreEqual(exception.Response.StatusCode, HttpStatusCode.Conflict);
        }

        [Test]
        public void BuyIceCallsSaveWithUpdatedQuantity()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 2};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("buyer", "561")
                };
            _controller.Post(new FormDataCollection(keyValuePairs));
            A.CallTo(() => _mongoDb.Save(A<CiberIs.Models.IceCream>.Ignored, A<string>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void BuyIceReducesQuantityWithOne()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 2};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("buyer", "561")
                };
            _controller.Post(new FormDataCollection(keyValuePairs));
            Assert.AreEqual(1, ice.Quantity);
        }

        [Test]
        public void BuyIceInsertsPurchase()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 2, Price = 7};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("buyer", "561")
                };
            _controller.Post(new FormDataCollection(keyValuePairs));
            A.CallTo(
                () =>
                _mongoDb.Insert(A<Purchase>.That.Matches(x => x.Buyer == 561 && x.Price == ice.Price), "Purchases"))
             .MustHaveHappened();
        }

        [Test]
        public void BuyIceWhenExceptionReturnsSuccssFalse()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 2, Price = 7};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            A.CallTo(() => _mongoDb.Save(A<CiberIs.Models.IceCream>.Ignored, A<string>.Ignored))
             .Throws(new MongoException("sorry"));
            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("buyer", "561")
                };
            var result = _controller.Post(new FormDataCollection(keyValuePairs));
            Assert.IsFalse(result.success);
        }

        [Test]
        public void BuyIceWhenNothingIsWrongReturnsSuccssTrue()
        {
            var ice = new CiberIs.Models.IceCream {Quantity = 2, Price = 7};
            A.CallTo(() => _mongoDb.FindById<CiberIs.Models.IceCream>(A<string>.Ignored, A<string>.Ignored))
             .Returns(ice);
            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("buyer", "561")
                };
            var result = _controller.Post(new FormDataCollection(keyValuePairs));
            Assert.IsTrue(result.success);
        }
    }
}