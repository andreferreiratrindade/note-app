using System.Text;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using NoteApp.EventMessages;
using NoteApp.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/note", async (NoteVM note, IConfiguration configuration) =>
{       

        var connectionString = configuration.GetValue<string>("QueueClientConnection");

        var queue = new QueueClient(connectionString, "note-create-request");
        await  queue.CreateIfNotExistsAsync();
        var response = await queue.SendMessageAsync(
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(new NoteAddRequestEvent(note.Title, note.Text, note.NoteId)))));
        return Results.Created( "/note/", note);
});

app.MapPatch("/note/{noteId}", async (string noteId,NoteUpdateVM note, IConfiguration configuration) =>
{
    var connectionString = configuration.GetValue<string>("QueueClientConnection");

    var queue = new QueueClient(connectionString, "note-update-request");
    await  queue.CreateIfNotExistsAsync();
    var response = await queue.SendMessageAsync(
        Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(new NoteUpdateRequestEvent( note.Text, new Guid(noteId))))));
    
    
    return Results.Ok(note);
});

app.Run();

