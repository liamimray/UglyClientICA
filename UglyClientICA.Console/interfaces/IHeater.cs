public interface IHeater : IDevice
{
    Task SetLevelAsync(int level);
    Task<int> GetLevelAsync();
}