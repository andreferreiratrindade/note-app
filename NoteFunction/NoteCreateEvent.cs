using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NoteFunction.Entities;

namespace NoteFunction;

public class NoteCreateEvent
{
    private readonly ILogger<NoteCreateEvent> _logger;
    private readonly IConfiguration _configuration;

    public NoteCreateEvent(ILogger<NoteCreateEvent> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [Function(nameof(NoteCreateEvent))]
    public async Task Run([QueueTrigger("note-create-request")] QueueMessage message)
    {

        _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        
        var cosmosClient = new CosmosClient(_configuration.GetValue<string>("endpointCosmoDb"),
            _configuration.GetValue<string>("primaryKeyCosmoDb"));

        Database database = cosmosClient.GetDatabase("Notes");
        await database.CreateContainerIfNotExistsAsync("NotesContainers", "/id");
        Container container = database.GetContainer("NotesContainers");

        var noteEntity = JsonConvert.DeserializeObject<NoteEntity>(message.MessageText);
        await container.CreateItemAsync(noteEntity);

    }
}