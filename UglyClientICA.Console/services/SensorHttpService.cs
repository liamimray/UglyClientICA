public class SensorHttpService : ISensor
{
    private readonly HttpClient _client;
    public int Id { get; }
    public SensorHttpService(HttpClient client, int id) { _client = client; Id = id; }

    public async Task<double> GetTemperatureAsync()
    {
        var response = await _client.GetAsync($"api/sensor/{Id}");
        if (response.IsSuccessStatusCode)
        {
            var str = await response.Content.ReadAsStringAsync();
            if (double.TryParse(str, out double value)) return value;
        }
        throw new Exception($"Failed to get temperature from sensor {Id}: {response.ReasonPhrase}");
    }

    public async Task<string> GetStateAsync() => $"{await GetTemperatureAsync():F1}°C";
}