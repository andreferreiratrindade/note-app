using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using NoteApp.ViewModels;

namespace NoteApp;
[ApiController]
[Route("api/note/v1/")]
public class NoteController: Controller
{

    private readonly IConfiguration _configuration;

    public NoteController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] NoteVM note)
    {
        var id = Guid.NewGuid();
        try
        {
            var connectionString = "UseDevelopmentStorage=true";

      
            var queue = new QueueClient(connectionString, "noteRequest");
            var response =  await queue.SendMessageAsync(note.Text);
            return Ok($"Azurite Queue with name: {note.Text} added");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error accessing Azurite Queue service: {ex.Message}");
        }
        
    }
}