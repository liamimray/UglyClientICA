public interface IHeaterApi
{
    Task SetHeaterLevel(int heaterId, int level);
    Task<int> GetHeaterLevel(int heaterId);
}