using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CricketMCP.CricketService;

public class CricketService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;
    private readonly string _baseUrl;

    public CricketService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    { 
        _httpClient = httpClientFactory.CreateClient("LongTimeoutClient");
        _baseUrl = configuration.GetValue<string>("CricketApi:BaseUrl");
        _apiToken = configuration.GetValue<string>("CricketApi:ApiToken");
    }

    public async Task<TeamRanking> GetTeamRankingsAsync()
    {
        var url = $"{_baseUrl}/team-rankings?api_token={_apiToken}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            // You can log or throw an error here as needed
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TeamRanking>(json);
    }
    
    public async Task<Player> GetPlayersAsync()
    {
        var url = $"{_baseUrl}/players?api_token={_apiToken}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            // You can log or throw an error here as needed
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Player>(json);
    }
    
    public async Task<PlayerData> GetPlayersByNameAsync(string name)
    {
        var url = $"{_baseUrl}/players?api_token={_apiToken}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            // You can log or throw an error here as needed
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Player>(json).data.FirstOrDefault(x => x.fullname.ToLower().Contains(name.ToLower()));
    }
}
