using System.Collections.Generic;

namespace Wykopowo.Contracts
{
    public interface ISubscriptionRepository
    {
        string CreateSubscription(Subscription subscription);
        void RemoveSubscription(long chatId, string url);
        List<Subscription> GetAllSubscriptions();
        void UpdateLastArticleTime(long lastArticleTime, string subscriptionId);
    }
}