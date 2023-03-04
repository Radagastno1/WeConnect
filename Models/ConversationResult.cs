namespace WeConnect.Models;
public class ConversationResult
{
    public List<Conversation> Conversations{get;set;}
    public Conversation Conversation{get;set;}
    public bool ConversationExists {get;set;}
}