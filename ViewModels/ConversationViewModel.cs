namespace WeConnect.ViewModels;

public class ConversationViewModel
{
    public int ID { get; set; }
    public DateTime DateCreated { get; set; }
    public List<MessageViewModel> Messages { get; set; }
    public string ParticipantsNames{get;set;}
    public MyViewModel MyViewModel{get;set;}
}
