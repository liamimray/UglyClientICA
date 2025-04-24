public interface ISensorApi
{
    Task<double> GetSensorTemperature(int sensorId);
}