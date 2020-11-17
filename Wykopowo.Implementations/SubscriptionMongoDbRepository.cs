using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Wykopowo.Contracts;

namespace Wykopowo.Implementations
{
    public class SubscriptionMongoDbRepository : ISubscriptionRepository
    {
        private MongoClient _client;
        private IMongoCollection<Subscription> _subscriptions;

        public SubscriptionMongoDbRepository(string connectionString)
        {
            _client = new MongoClient(connectionString);
            var database = _client.GetDatabase("WYKOPOWODB");
            _subscriptions = database.GetCollection<Subscription>("Subscriptions");
        }

        public string CreateSubscription(Subscription subscription)
        {
            _subscriptions.InsertOne(subscription);
            return subscription.Id;
        }

        public void RemoveSubscription(long chatId, string url)
        {
            _subscriptions.DeleteOne(s => s.ChatId == chatId && s.Url == url);
        }

        public List<Subscription> GetAllSubscriptions()
        {
            return _subscriptions.Find(subscription => true).ToList();
        }

        public void UpdateLastArticleTime(long lastArticleTime, string subscriptionId)
        {
            var updater =
                Builders<Subscription>.Update.Set(subscription => subscription.LastArticleTime, lastArticleTime);
            _subscriptions.FindOneAndUpdate(r => r.Id == subscriptionId, updater);
        }
    }
}