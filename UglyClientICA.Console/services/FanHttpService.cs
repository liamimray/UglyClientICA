using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class FanHttpService : IFan
{
    private readonly IHttpClient _client;
    public int Id { get; }
    // Had to refactor FanHttpService to depend on IHttpClient interface so that I can use Moq in test case.
    public FanHttpService(IHttpClient client, int id) { _client = client; Id = id; }

    public async Task SetStateAsync(bool on)
    {
        var response = await _client.PostAsync($"api/fans/{Id}",
            new StringContent(on.ToString().ToLower(), System.Text.Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to set fan state for fan {Id}: {response.ReasonPhrase}");
    }

    public async Task<bool> IsOnAsync()
    {
        var response = await _client.GetAsync($"api/fans/{Id}/state");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var fan = JsonSerializer.Deserialize<FanDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return fan.IsOn;
        }
        throw new Exception($"Failed to get fan state for fan {Id}: {response.ReasonPhrase}");
    }

    public async Task<string> GetStateAsync()
    {
        bool isOn = await IsOnAsync();
        return isOn ? "On" : "Off";
    }

    public class FanDTO { public int Id { get; set; } public bool IsOn { get; set; } }
}