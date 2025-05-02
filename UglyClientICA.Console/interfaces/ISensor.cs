public interface ISensor : IDevice
{
    Task<double> GetTemperatureAsync();
}