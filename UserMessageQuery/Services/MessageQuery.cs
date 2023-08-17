using UserMessageQuery.Interfaces;
using UserMessageQuery.Models;

namespace UserMessageQuery.Services
{
    public class MessageQuery : IMessageQuery
    {
        private List<User> _users = new();

        public MessageQuery() { }

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public void RemoveOldMessage(int maxAllMessages)
        {
            if (Users.Sum(user => user.Messages.Count) > maxAllMessages)
            {
                var oldestMessage = Users.SelectMany(user => user.Messages).OrderBy(msg => msg.CreateTime).FirstOrDefault();

                foreach (var user in Users)
                {
                    user.Messages.Remove(oldestMessage);
                }
            }
        }
        public void RemoveOldMessageByUserName(int maxUserMessages, string userName)
        {
            var currentUser = Users.FirstOrDefault(user => user.UserName == userName);

            if (currentUser.Messages.Count > maxUserMessages)
            {
                currentUser.Messages.RemoveAt(maxUserMessages);
            }
        }
    }
}
