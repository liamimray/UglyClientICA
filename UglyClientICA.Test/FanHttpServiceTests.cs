using NUnit.Framework;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApp.Tests
{
    [TestFixture]
    public class FanHttpServiceTests
    {
        [Test]
        public async Task IsOnAsync_Returns_True_If_Fan_IsOn()
        {
            // Arrange
            var responseJson = JsonSerializer.Serialize(new { Id = 1, IsOn = true });
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson)
            };

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync("api/fans/1/state"))
                .ReturnsAsync(httpResponse);

            var service = new FanHttpService(clientMock.Object, 1);

            // Act
            var isOn = await service.IsOnAsync();

            // Assert
            Assert.IsTrue(isOn);
        }

        [Test]
        public async Task SetStateAsync_Posts_Correct_Content()
        {
            // Arrange
            var fanId = 2;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x =>
                    x.PostAsync($"api/fans/{fanId}",
                        It.Is<HttpContent>(c =>
                            c.ReadAsStringAsync().Result == "true")))
                .ReturnsAsync(httpResponse);

            var service = new FanHttpService(clientMock.Object, fanId);

            // Act & Assert (should not throw)
            Assert.DoesNotThrowAsync(async () => await service.SetStateAsync(true));
            clientMock.Verify(x => x.PostAsync($"api/fans/{fanId}", It.IsAny<HttpContent>()), Times.Once);
        }

        [Test]
        public void SetStateAsync_Throws_On_Error_Response()
        {
            // Arrange
            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var service = new FanHttpService(clientMock.Object, 3);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await service.SetStateAsync(true));
        }
    }
}