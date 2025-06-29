using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using System.Text.Json;

public class McpChatService : IAsyncDisposable
{
    private readonly McpClient _mcpClient;
    private readonly IChatClient _chatClient;
    private readonly ILogger<McpChatService> _logger;
    private readonly List<FunctionTool> _mcpTools;

    public McpChatService(ILogger<McpChatService> logger)
    {
        var _mcpClient = new McpClie
        _logger = logger;

        var clientOptions = new McpClientOptions
        {
            ClientInfo = new() { Name = "demo-client", Version = "1.0.0" }
        };

        var serverConfig = new McpServerConfig
        {
            Id = "demo-server",
            Name = "Demo Server",
            TransportType = TransportTypes.StdIo,
            TransportOptions = new Dictionary<string, string>
            {
                ["command"] = @"..\MCPServer\bin\Debug\net9.0\MCPServer.exe"
            }
        };

        _mcpClient = McpClientFactory.CreateAsync(serverConfig, clientOptions, loggerFactory: LoggerFactory.Create(builder => builder.AddConsole())).Result;

        var ollamaChatClient = new OllamaChatClient(
            new Uri("http://localhost:11434/"),
            "llama3.2:3b"
        );

        _chatClient = new ChatClientBuilder(ollamaChatClient)
            .UseLogging(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseFunctionInvocation()
            .Build();

        _mcpTools = _mcpClient.ListToolsAsync().Result.ToList();
    }

    public async Task<string> ChatAsync(string userInput)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a helpful assistant."),
            new(ChatRole.User, userInput)
        };

        var response = await _chatClient.GetResponseAsync(
            messages,
            new ChatOptions { Tools = _mcpTools });

        var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Assistant);

        return assistantMessage != null
            ? string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()))
            : "(No assistant message received)";
    }

    public async ValueTask DisposeAsync()
    {
        if (_mcpClient is not null)
            await _mcpClient.DisposeAsync();
    }
}
