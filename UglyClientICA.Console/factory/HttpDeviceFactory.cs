public class HttpDeviceFactory : IDeviceFactory
{
    private readonly IHttpClient _client;
    public HttpDeviceFactory(IHttpClient client) { _client = client; }
    public IFan CreateFan(int id) => new FanHttpService(_client, id);
    public IHeater CreateHeater(int id) => new HeaterHttpService(_client, id);
    public ISensor CreateSensor(int id) => new SensorHttpService(_client, id);
}
