using System.Collections.Generic;

namespace Wykopowo.Contracts
{
    public interface ITelegramService
    {
        void SendMessages(List<SubscriptionMessage> messages);
    }

    public class SubscriptionMessage
    {
        public long ChatId { get; set; }
        public string Text { get; set; }

        public SubscriptionMessage(long chatId, string text)
        {
            ChatId = chatId;
            Text = text;
        }
    }
}