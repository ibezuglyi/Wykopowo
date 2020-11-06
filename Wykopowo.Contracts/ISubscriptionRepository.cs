using Wykopowo.Contracts;

namespace Wykopowo
{
    public interface ISubscriptionRepository
    {
        long CreateSubscription(Subscription subscription);
        void RemoveSubscription(long chatId, string url);
    }
}