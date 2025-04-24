public interface IFanApi
{
    Task SetFanState(int fanId, bool isOn);
    Task<FanDTO> GetFanState(int fanId);
}