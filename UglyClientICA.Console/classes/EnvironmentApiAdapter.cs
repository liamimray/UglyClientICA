using System.Net.Http;
using System.Text.Json;

public class EnvironmentApiAdapter 
    : IFanApi, IHeaterApi, ISensorApi, IEnvironmentReset
{
    private readonly HttpClient _client;

    public EnvironmentApiAdapter(HttpClient client)
    {
        _client = client;
    }

    // IFanApi
    public async Task SetFanState(int fanId, bool isOn)
    {
        var response = await _client.PostAsync(
            $"api/fans/{fanId}",
            new StringContent(isOn.ToString().ToLower(), System.Text.Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
    }

    public async Task<FanDTO> GetFanState(int fanId)
    {
        var response = await _client.GetAsync($"api/fans/{fanId}/state");
        response.EnsureSuccessStatusCode();
        var fanJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FanDTO>(fanJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // IHeaterApi
    public async Task SetHeaterLevel(int heaterId, int level)
    {
        var response = await _client.PostAsync(
            $"api/heat/{heaterId}",
            new StringContent(level.ToString(), System.Text.Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetHeaterLevel(int heaterId)
    {
        var response = await _client.GetAsync($"api/heat/{heaterId}/level");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return int.Parse(content);
    }

    // ISensorApi
    public async Task<double> GetSensorTemperature(int sensorId)
    {
        var response = await _client.GetAsync($"api/sensor/{sensorId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return double.Parse(content);
    }
    
    // IEnvironmentReset
    public async Task Reset()
    {
        var response = await _client.PostAsync("api/Envo/reset", null);
        response.EnsureSuccessStatusCode();
    }
}