using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NoteFunction.Entities;

namespace NoteFunction;

public class NoteUpdateEvent
{
    private readonly ILogger<NoteUpdateEvent> _logger;
    private readonly IConfiguration _configuration;

    

    public NoteUpdateEvent(ILogger<NoteUpdateEvent> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
    }

    [Function(nameof(NoteUpdateEvent))]
    public async Task Run([QueueTrigger("note-update-request")] QueueMessage message )
    {
        try
        {


            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
            var cosmosClient = new CosmosClient(_configuration.GetValue<string>("endpointCosmoDb"),
                _configuration.GetValue<string>("primaryKeyCosmoDb"));

            Database database = cosmosClient.GetDatabase("Notes");
            await database.CreateContainerIfNotExistsAsync("NotesContainers", "/id");
            Container container = database.GetContainer("NotesContainers");
            var note = JsonConvert.DeserializeObject<NoteEntity>(message.MessageText);
            PartitionKey partionKey= new (note.NoteId.ToString());
            var noteEntity = container.GetItemLinqQueryable<NoteEntity>().Where(x => x.id == note.NoteId.ToString()).FirstOrDefault();
            
            noteEntity.Text = note.Text;
            await container.UpsertItemAsync(noteEntity, partionKey);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error processing message: {message.MessageText}");    
            throw;    
        }
    }
    

}