using UserMessageQuery.Models;

namespace UserMessageQuery.Interfaces
{
    public interface IMessageQuery
    {
        public List<User> Users { get; set; }
        public void RemoveOldMessage(int maxAllMessages);
        public void RemoveOldMessageByUserName(int maxUserMessages,string userName);
    }
}
