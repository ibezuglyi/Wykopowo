namespace Wykopowo.Contracts
{
    public class Subscription
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Url { get; set; }

        public Subscription(long chatId, string url)
        {
            ChatId = chatId;
            Url = url;
        }

        public Subscription()
        {
            
        }
        
    }
}