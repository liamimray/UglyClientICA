using NUnit.Framework;
using Moq;
using System.Net.Http;

namespace UglyClientICA.Tests
{
    [TestFixture]
    public class HttpDeviceFactoryTests
    {
        [Test]
        public void CreateFan_ReturnsFanInstance()
        {
            var clientMock = new Mock<IHttpClient>();
            var factory = new HttpDeviceFactory(clientMock.Object);

            var fan = factory.CreateFan(1);
            Assert.IsInstanceOf<IFan>(fan);
        }

        [Test]
        public void CreateHeater_ReturnsHeaterInstance()
        {
            var clientMock = new Mock<IHttpClient>();
            var factory = new HttpDeviceFactory(clientMock.Object);

            var heater = factory.CreateHeater(1);
            Assert.IsInstanceOf<IHeater>(heater);
        }

        [Test]
        public void CreateSensor_ReturnsSensorInstance()
        {
            var clientMock = new Mock<IHttpClient>();
            var factory = new HttpDeviceFactory(clientMock.Object);

            var sensor = factory.CreateSensor(1);
            Assert.IsInstanceOf<ISensor>(sensor);
        }
    }
}