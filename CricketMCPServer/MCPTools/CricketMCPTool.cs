using System.ComponentModel;
using CricketMCP.CricketService;
using ModelContextProtocol.Server;

namespace CricketMCP.MCPTools;

[McpServerToolType]
public class CricketMCPTool
{
    private static CricketService.CricketService _cricketService;
    public CricketMCPTool(CricketService.CricketService cricketService)
    {
        _cricketService = cricketService;
    }
    [McpServerTool, Description("Get the ranking of Team")]
    public async Task<TeamRanking> GetTeamRankings()
    {
        var rankings =  await _cricketService.GetTeamRankingsAsync();
        return rankings;
    }
    
    [McpServerTool, Description("Get All players")]
    public async Task<Player> GetAllPlayers()
    {
        var players =  await _cricketService.GetPlayersAsync();
        return players;
    }
    
    [McpServerTool, Description("Get a player by name")]
    public async Task<PlayerData> GetPlayerByName([Description("The name of the player we want data")]string name)
    {
        var player =  await _cricketService.GetPlayersByNameAsync(name);
        return player;
    }
}