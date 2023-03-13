namespace WeConnect.ViewModels;

public class ConversationViewModel
{
    public int ID { get; set; }
    public DateTime DateCreated { get; set; }
    public List<MessageViewModel> Messages { get; set; } = new();
    public string ParticipantsNames{get;set;}
}
