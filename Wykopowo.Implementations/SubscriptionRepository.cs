using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Wykopowo.Contracts;

namespace Wykopowo.Implementations
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public SQLiteConnection Connection { get; }

        public SubscriptionRepository(SQLiteConnection connection)
        {
            Connection = connection;
            Connection.Open();
            EnsureDatabase();
        }

        private void EnsureDatabase()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Execute(SubscriptionRepositoryScripts.CreateDatabase);
            }
        }

        public SubscriptionRepository(string connectionString) : this(new SQLiteConnection(connectionString))
        {
        }


        public long CreateSubscription(Subscription subscription)
        {
            var subscriptionId = Connection
                .Query<long>(SubscriptionRepositoryScripts.CreateSubscription, subscription)
                .First();
            return subscriptionId;
        }

        public void RemoveSubscription(long chatId, string url)
        {
            Connection.Query(SubscriptionRepositoryScripts.RemoveSubscription, new Subscription(chatId, url));
        }

        public List<Subscription> GetAllSubscriptions()
        {
            return Connection.Query<Subscription>(SubscriptionRepositoryScripts.GetAllSubscriptions).ToList();
        }

        public void UpdateLastArticleTime(long lastArticleTime, long subscriptionId)
        {
            Connection.Query(SubscriptionRepositoryScripts.UpdateLastArticleTime,
                new {LastArticleTime = lastArticleTime, Id = subscriptionId});
        }
    }
}