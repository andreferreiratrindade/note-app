namespace NoteFunction.Entities;

public class NoteEntity
{

    public string Title { get; set; }
    public string Text { get; set; }
    
    public Guid NoteId { get; set; }
    public string id => NoteId.ToString();
}
