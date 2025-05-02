using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Tests
{
    [TestFixture]
    public class DeviceControllerTests
    {
        [Test]
        public async Task SetFanState_CallsSetStateOnCorrectFan()
        {
            var fanMock = new Mock<IFan>();
            fanMock.Setup(f => f.SetStateAsync(true)).Returns(Task.CompletedTask);

            var factoryMock = new Mock<IDeviceFactory>();
            factoryMock.Setup(f => f.CreateFan(It.IsAny<int>())).Returns(fanMock.Object);

            var controller = new DeviceController(factoryMock.Object, 1, 0, 0);

            await controller.SetFanState(1, true);

            fanMock.Verify(f => f.SetStateAsync(true), Times.Once);
        }

        [Test]
        public async Task SetHeaterLevel_CallsSetLevelOnCorrectHeater()
        {
            var heaterMock = new Mock<IHeater>();
            heaterMock.Setup(h => h.SetLevelAsync(3)).Returns(Task.CompletedTask);

            var factoryMock = new Mock<IDeviceFactory>();
            factoryMock.Setup(f => f.CreateHeater(It.IsAny<int>())).Returns(heaterMock.Object);

            var controller = new DeviceController(factoryMock.Object, 0, 1, 0);

            await controller.SetHeaterLevel(1, 3);

            heaterMock.Verify(h => h.SetLevelAsync(3), Times.Once);
        }

        [Test]
        public async Task GetSensorTemperature_ReturnsCorrectTemperature()
        {
            var sensorMock = new Mock<ISensor>();
            sensorMock.Setup(s => s.GetTemperatureAsync()).ReturnsAsync(22.5);

            var factoryMock = new Mock<IDeviceFactory>();
            factoryMock.Setup(f => f.CreateSensor(It.IsAny<int>())).Returns(sensorMock.Object);

            var controller = new DeviceController(factoryMock.Object, 0, 0, 1);

            var temp = await controller.GetSensorTemperature(1);

            Assert.AreEqual(22.5, temp);
        }

        [Test]
        public async Task GetAverageTemperature_ComputesAverageCorrectly()
        {
            var mockSensors = new[] { 20.0, 21.0, 22.0 };
            var sensorMocks = new List<Mock<ISensor>>();

            for (int i = 0; i < 3; i++)
            {
                var mock = new Mock<ISensor>();
                mock.Setup(s => s.GetTemperatureAsync()).ReturnsAsync(mockSensors[i]);
                sensorMocks.Add(mock);
            }

            var factoryMock = new Mock<IDeviceFactory>();
            factoryMock.Setup(f => f.CreateSensor(It.IsAny<int>()))
                       .Returns<int>(id => sensorMocks[id-1].Object);

            var controller = new DeviceController(factoryMock.Object, 0, 0, 3);

            var avg = await controller.GetAverageTemperature();

            Assert.AreEqual((20.0 + 21.0 + 22.0) / 3, avg, 0.01);
        }
    }
}