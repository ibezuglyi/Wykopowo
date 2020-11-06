using System.Collections.Generic;
using System.Linq;
using PubSub;
using Telegram.Bot.Args;
using Wykopowo.Contracts;
using Wykopowo.Implementations;

namespace Wykopowo
{
    public class Bootstrapper
    {
        public void Initialize(string dbFile, string token)
        {
            SubscriptionRepository = new SubscriptionRepository("Data Source=" + dbFile);
            TelegramService = new TelegramService(token);
            TelegramService.OnMessage += (sender, args) => { Hub.Default.Publish<MessageEventArgs>(args); };
            Hub.Default.Subscribe((MessageEventArgs args) =>
            {
                SubscriptionRepository.CreateSubscription(new Subscription(args.Message.Chat.Id,  "https://wykop.pl/"));
            });

            Hub.Default.Subscribe((List<NewsLine> headers) =>
            {
                var chatIds = SubscriptionRepository.GetAllSubscriptions();
                var messages = headers.SelectMany(m => chatIds.Select(id => new SubscriptionMessage(id.ChatId, m.ToHtml()))).ToList();
                 TelegramService.SendMessages(messages);
            });
        }

        public TelegramService TelegramService { get; private set; }
        public SubscriptionRepository SubscriptionRepository { get; private set; }

        public void PublishNews(List<NewsLine> news)
        {
            Hub.Default.Publish(news);
        }
    }
}