using WeConnect.Data;

namespace WeConnect.Models;

public class MessgageService
{
    MessagesDB _messagesDB;
    CrudDB<Message> _crudDB;

    public MessgageService(MessagesDB messagesDB, CrudDB<Message> crudDB)
    {
        _messagesDB = messagesDB;
        _crudDB = crudDB;
    }

    public async Task<int?> Create(string content, int senderId, int conversationId)
    {
        var message = new Message(content, senderId, conversationId);
        return _crudDB.Create(message, QueryGenerator<Message>.InsertQuery(message));
    }

    public async Task<List<Message>> GetAll(int conversationId, User user)
    {
        List<Message> selectedMessages = _messagesDB.GetById(conversationId, user);
        if (selectedMessages == null || selectedMessages.Count() < 1)
        {
            return null;
        }
        else
        {
            return selectedMessages;
        }
        // try
        // {
        //     List<Message> allMessages = _messageManager.Get();
        //     foreach (Message item in allMessages)
        //     {
        //         if (item.ConversationId == data)
        //         {
        //             selectedMessages.Add(item);
        //         }
        //     }
        //     if (selectedMessages.Count() < 1)
        //     {
        //         throw new InvalidOperationException();
        //     }
        //     return selectedMessages;
        // }
        // catch (InvalidOperationException)
        // {
        //     return null;
        // }
    }

    public async Task<List<Message>> GetBySearch(string searc, User user) //söka i meddelandet efter datum eller ord?
    {
        throw new NotImplementedException();
    }

    public async Task<Message> GetOne(int data1, User user) //hämta specifikt medd? vet ej
    {
        throw new NotImplementedException();
    }

    public async Task<int?> Remove(Message obj) //man ska kunna ta bort sitt medd = is_visible = true
    {
        throw new NotImplementedException();
    }

    public async Task<int?> Update(Message obj) //man kan redigera sitt meddel inom en viss tid, lägg en bool is_edited ?
    {
        throw new NotImplementedException();
    }
}
