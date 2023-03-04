namespace WeConnect.Models;
public class Conversation
{
    public int ID{get;set;}
    public DateTime DateCreated{get;set;}
    public User Creator{get;set;}
    public List<User> Participants{get;set;}
    public string ParticipantsNames{get;set;}
    public int ParticipantId{get;set;}
    public int CreatorId{get;set;}
    public List<Message> Messages {get;set;} = new();
    public Conversation(){}
    public Conversation(User aCreator, List<User> aParticipants)
    {
        Creator = aCreator;
        DateCreated = DateTime.Now;
        Participants = aParticipants;
    }

    public override string ToString()
    {
        return $"[{ID}] {ParticipantsNames}";
    }
}