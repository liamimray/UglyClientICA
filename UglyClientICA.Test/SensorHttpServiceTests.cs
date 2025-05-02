using NUnit.Framework;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace UglyClientICA.Tests
{
    [TestFixture]
    public class SensorHttpServiceTests
    {
        [Test]
        public async Task GetTemperatureAsync_ParsesTemperatureCorrectly()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("23.45")
            };

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync("api/sensor/1"))
                .ReturnsAsync(httpResponse);

            var service = new SensorHttpService(clientMock.Object, 1);

            var temp = await service.GetTemperatureAsync();

            Assert.AreEqual(23.45, temp, 0.01);
        }
    }
}