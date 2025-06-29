using CricketMCP.CricketService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Polly;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services
    .AddMcpServer(o => o.InitializationTimeout = TimeSpan.FromHours(1))
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton<CricketService>();
builder.Services.AddHttpClient("LongTimeoutClient", client =>
{
    client.Timeout = TimeSpan.FromMinutes(10);
});
await builder.Build().RunAsync();