namespace NoteApp.EventMessages;

public record struct NoteAddRequestEvent(string Title, string Text, Guid NoteId);
public record struct NoteUpdateRequestEvent(string Text, Guid NoteId);
