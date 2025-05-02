public class SystemHttpClient : IHttpClient
{
    private readonly HttpClient _client;
    public SystemHttpClient(HttpClient client) { _client = client; }
    public Task<HttpResponseMessage> GetAsync(string requestUri) => _client.GetAsync(requestUri);
    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => _client.PostAsync(requestUri, content);
}