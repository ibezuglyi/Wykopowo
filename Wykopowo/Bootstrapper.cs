using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            SubscriptionRepository = new SubscriptionMongoDbRepository(dbFile);
            TelegramService = new TelegramService(token);
            TelegramService.OnMessage += (sender, args) => { Hub.Default.Publish<MessageEventArgs>(args); };
            OnSubscriptionCreating();
            OnNewHeaderAdding();
        }

        private void OnNewHeaderAdding()
        {
            Hub.Default.Subscribe((List<NewsLine> headers) =>
            {
                Task.Run(async () =>
                {
                    var subscriptions = SubscriptionRepository.GetAllSubscriptions();
                    var messages = headers.SelectMany(m =>
                        subscriptions.Where(s => s.LastArticleTime == null || s.LastArticleTime.Value < m.GetTime())
                            .Select(subscription => new SubscriptionMessage(subscription, m))).ToList();
                    await TelegramService.SendMessages(messages);
                    UpdateLastArticleTimes(subscriptions, messages);
                }).GetAwaiter().GetResult();
            });
        }

        private void UpdateLastArticleTimes(List<Subscription> subscriptions, List<SubscriptionMessage> messages)
        {
            if (!messages.Any())
            {
                return;
            }

            foreach (var subscription in subscriptions)
            {
                var newestMessage = messages.Where(m => m.SubscriptionId == subscription.Id)
                    .OrderByDescending(r => r.SubscriptionLastArticleTime).First();
                SubscriptionRepository.UpdateLastArticleTime(newestMessage.SubscriptionLastArticleTime,
                    newestMessage.SubscriptionId);
            }
        }

        private void OnSubscriptionCreating()
        {
            Hub.Default.Subscribe((MessageEventArgs args) =>
            {
                SubscriptionRepository.CreateSubscription(new Subscription(args.Message.Chat.Id,
                    "https://wykop.pl/"));
            });
        }

        public TelegramService TelegramService { get; private set; }
        public ISubscriptionRepository SubscriptionRepository { get; private set; }

        public void PublishNews(List<NewsLine> news)
        {
            Hub.Default.Publish(news);
        }
    }
}