namespace NoteApp.ViewModels;

public class NoteVM
{
    public NoteVM()
    {
        NoteId = Guid.NewGuid();
    }
    public string Title { get; set; }
    public string Text { get; set; }
    
    public Guid NoteId { get; }

}