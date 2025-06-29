using Microsoft.AspNetCore.Mvc;

namespace ClientAPI;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly McpChatService _chatService;

    public ChatController(McpChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        var response = await _chatService.ChatAsync(request.Message);
        return Ok(new { reply = response });
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}