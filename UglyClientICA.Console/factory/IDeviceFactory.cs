public interface IDeviceFactory
{
    IFan CreateFan(int id);
    IHeater CreateHeater(int id);
    ISensor CreateSensor(int id);
}