using NUnit.Framework;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace UglyClientICA.Tests
{
    [TestFixture]
    public class HeaterHttpServiceTests
    {
        [Test]
        public async Task GetLevelAsync_ParsesHeaterLevelCorrectly()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("3")
            };

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync("api/heat/1/level"))
                .ReturnsAsync(httpResponse);

            var service = new HeaterHttpService(clientMock.Object, 1);

            var level = await service.GetLevelAsync();

            Assert.AreEqual(3, level);
        }
    }
}