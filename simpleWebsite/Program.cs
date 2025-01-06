var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", async context => {
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.MapPost("/chat", async (HttpContext content) => {
    var chatMessage = await content.Request.ReadFromJsonAsync<ChatMessage>();

    if (chatMessage == null || string.IsNullOrWhiteSpace(chatMessage.Nickname) || string.IsNullOrWhiteSpace(chatMessage.Message)) {
        content.Response.StatusCode = 400;
        await content.Response.WriteAsync("Invalid request. Nickname and message are required.");
        return;
    }

    var response = new {
        Success = true,
        ReceivedAt = DateTime.UtcNow.TimeOfDay.ToString(@"hh\:mm\:ss"),
        Echo = $"{chatMessage.Nickname}: {chatMessage.Message}"
    };

    await content.Response.WriteAsJsonAsync(response);
});

app.Run();

public class ChatMessage
{
    public string Nickname { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}