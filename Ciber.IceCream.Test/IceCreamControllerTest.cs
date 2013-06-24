using System.Web.Http;
using CiberIs.Controllers;
using CiberIs.Models;
using FakeItEasy;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace Ciber.IceCream.Test
{
    [TestFixture]
    public class IceCreamControllerTest
    {
        private IceCreamController _controller;
        private IMongoDb _mongoDb;

        [SetUp]
        public void Init()
        {
            _mongoDb = A.Fake<IMongoDb>();
            _controller = new IceCreamController(_mongoDb);
        }

        [Test]
        public void WhenPostingNullValueExceptionIsThrown()
        {
            Assert.Throws<HttpResponseException>(() => _controller.Post(null));
        }

        [Test]
        public void InsertIsCalledWhenPostingIceCream()
        {
            _controller.Post(new CiberIs.Models.IceCream {Id = new ObjectId()});
            A.CallTo(() => _mongoDb.Insert(A<CiberIs.Models.IceCream>.Ignored, "IceCreams")).MustHaveHappened();
        }

        [Test]
        public void SuccessIsTrueWhenPostingIceCream()
        {
            var result = _controller.Post(new CiberIs.Models.IceCream { Id = new ObjectId() });
            Assert.IsTrue(result.success);
        }

        [Test]
        public void SuccessIsFalseWhenPostingIceCreamAndExceptionIsThrown()
        {
            A.CallTo(() => _mongoDb.Insert(A<CiberIs.Models.IceCream>.Ignored, A<string>.Ignored)).Throws(new MongoException("Insert failed"));
            var result = _controller.Post(new CiberIs.Models.IceCream { Id = new ObjectId() });
            Assert.IsFalse(result.success);
        }
    }
}
