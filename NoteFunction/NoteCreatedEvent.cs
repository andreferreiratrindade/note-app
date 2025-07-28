using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NoteFunction.Entities;

namespace NoteFunction;

public class NoteCreatedEvent
{
    private readonly ILogger<NoteCreatedEvent> _logger;

    public NoteCreatedEvent(ILogger<NoteCreatedEvent> logger)
    {
        _logger = logger;
    }

   
    public void Run([CosmosDBTrigger(
            databaseName: "Notes",
            containerName: "NotesContainers",
            Connection = "CosmosDbConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<object> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            //_logger.LogInformation("First document Text: " + input[0].Text);
        }

        
    }
}

