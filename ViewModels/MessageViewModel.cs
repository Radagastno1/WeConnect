namespace WeConnect.ViewModels;

public class MessageViewModel
{
    public int ID { get; set; }
    public string Content { get; set; }
    public DateOnly DateCreated { get; set; }
    public string Reciever { get; set; }
    public string Sender { get; set; }
    public int SenderId { get; set; }
    public int ConversationId { get; set; }
    public MyViewModel MyViewModel { get; set; }
}
