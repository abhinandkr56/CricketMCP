using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;

// Configure Semantic Kernel
var builder = Kernel.CreateBuilder();
builder.Services.AddOpenAIChatCompletion(
    modelId: "llama3.2",
    apiKey: null, // No API key needed for Ollama
    endpoint: new Uri("http://localhost:11434/v1") // Ollama server endpoint
);
var kernel = builder.Build();

// Set up MCP Client
var (command, argument) = GetCommandAndArguments(args);
await using IMcpClient mcpClient = await McpClientFactory.CreateAsync(
    new StdioClientTransport(new()
    {
        Command = command,
        Arguments =argument,
        Name = "McpServer",
    }));

// Retrieve and load tools from the server
IList<McpClientTool> tools = await mcpClient.ListToolsAsync().ConfigureAwait(false);

// List all available tools from the MCP server
Console.WriteLine("\n\nAvailable MCP Tools:");
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name}: {tool.Description}");
}

// Register MCP tools with Semantic Kernel
#pragma warning disable SKEXP0001 // Suppress diagnostics for experimental features
kernel.Plugins.AddFromFunctions("McpTools", tools.Select(t => t.AsKernelFunction()));
#pragma warning restore SKEXP0001

// Chat loop
Console.WriteLine("Chat with the AI. Type 'exit' to stop.");
var history = new ChatHistory();
history.AddSystemMessage("You are an assistant that can call MCP tools to process user queries.");

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    Console.Write("User > ");
    var input = Console.ReadLine();
    if (input?.Trim().ToLower() == "exit") break;

    history.AddUserMessage(input);

    // Enable auto function calling
    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    Console.WriteLine($"Assistant > {result.Content}");
    history.AddMessage(result.Role, result.Content ?? string.Empty);
}

static (string command, string[] arguments) GetCommandAndArguments(string[] args)
{
    return args switch
    {
        [var script] when script.EndsWith(".dll") => ("dotnet", [script]),
        [var script] when Directory.Exists(script) || (File.Exists(script) && script.EndsWith(".csproj")) => ("dotnet", ["run", "--project", script]),
        _ => ("dotnet", ["run", "--project", "../CricketMCPServer"]) // fallback to known location
    };
}
