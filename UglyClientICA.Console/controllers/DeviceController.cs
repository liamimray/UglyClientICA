public class DeviceController
{
    private readonly IDeviceFactory _factory;
    private readonly int _fanCount, _heaterCount, _sensorCount;

    public DeviceController(IDeviceFactory factory, int fanCount, int heaterCount, int sensorCount)
    {
        _factory = factory;
        _fanCount = fanCount;
        _heaterCount = heaterCount;
        _sensorCount = sensorCount;
    }

    public async Task SetFanState(int fanId, bool isOn) => await _factory.CreateFan(fanId).SetStateAsync(isOn);
    public async Task SetHeaterLevel(int heaterId, int level) => await _factory.CreateHeater(heaterId).SetLevelAsync(level);
    public async Task<double> GetSensorTemperature(int sensorId) => await _factory.CreateSensor(sensorId).GetTemperatureAsync();

    public async Task ShowDeviceStates()
    {
        Console.WriteLine("Fan States:");
        for (int i = 1; i <= _fanCount; i++)
            Console.WriteLine($"  Fan {i}: {await _factory.CreateFan(i).GetStateAsync()}");

        Console.WriteLine("Heater Levels:");
        for (int i = 1; i <= _heaterCount; i++)
            Console.WriteLine($"  Heater {i}: {await _factory.CreateHeater(i).GetStateAsync()}");

        Console.WriteLine("Sensor Temperatures:");
        for (int i = 1; i <= _sensorCount; i++)
            Console.WriteLine($"  Sensor {i}: {await _factory.CreateSensor(i).GetStateAsync()}");
    }

    public async Task<double> GetAverageTemperature()
    {
        double total = 0;
        for (int i = 1; i <= _sensorCount; i++)
            total += await GetSensorTemperature(i);
        return total / _sensorCount;
    }

    public async Task SetAllFans(bool state)
    {
        for (int i = 1; i <= _fanCount; i++)
            await SetFanState(i, state);
    }

    public async Task SetAllHeaters(int level)
    {
        for (int i = 1; i <= _heaterCount; i++)
            await SetHeaterLevel(i, level);
    }

    // Add more high-level methods as needed
}