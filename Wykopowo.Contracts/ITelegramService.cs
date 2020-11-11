using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wykopowo.Contracts
{
    public interface ITelegramService
    {
        Task SendMessages(List<SubscriptionMessage> messages);
    }

    public class SubscriptionMessage
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
        public long SubscriptionLastArticleTime { get; }
        public int SubscriptionId { get; set; }
        
        public SubscriptionMessage(Subscription sub, NewsLine message)
        {
            SubscriptionId = sub.Id;
            ChatId = sub.ChatId;
            Text = message.ToHtml();
            SubscriptionLastArticleTime = message.GetTime();
        }

        
    }
}