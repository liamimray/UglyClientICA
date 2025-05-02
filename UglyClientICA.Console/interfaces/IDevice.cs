public interface IDevice {
    int Id {get;}
    Task<string> GetStateAsync();
}