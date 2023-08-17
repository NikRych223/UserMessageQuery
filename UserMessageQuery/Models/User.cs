namespace UserMessageQuery.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<Message> Messages { get; set; }
    }
}
