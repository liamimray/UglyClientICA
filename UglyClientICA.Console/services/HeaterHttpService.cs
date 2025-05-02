public class HeaterHttpService : IHeater
{
    private readonly IHttpClient _client;
    public int Id { get; }
    public HeaterHttpService(IHttpClient client, int id) { _client = client; Id = id; }

    public async Task SetLevelAsync(int level)
    {
        var response = await _client.PostAsync($"api/heat/{Id}",
            new StringContent(level.ToString(), System.Text.Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to set heater level {Id}: {response.ReasonPhrase}");
    }

    public async Task<int> GetLevelAsync()
    {
        var response = await _client.GetAsync($"api/heat/{Id}/level");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (int.TryParse(content, out int level)) return level;
        }
        throw new Exception($"Failed to get heater level {Id}: {response.ReasonPhrase}");
    }

    public async Task<string> GetStateAsync()
    {
        int level = await GetLevelAsync();
        return $"Level {level}";
    }
}