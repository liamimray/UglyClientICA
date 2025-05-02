public interface IFan : IDevice
{
    Task SetStateAsync(bool on);
    Task<bool> IsOnAsync();
}