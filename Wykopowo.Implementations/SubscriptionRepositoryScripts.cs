namespace Wykopowo.Implementations
{
    public static class SubscriptionRepositoryScripts
    {
        public static readonly string CreateDatabase =
            @"CREATE TABLE IF NOT EXISTS Subscriptions (
               Id integer primary key AUTOINCREMENT,
               ChatId integer not null,
               Url varchar(200) not null,
               LastArticleTime integer null,
               UNIQUE(ChatId, Url)
             )";

        public static readonly string CreateSubscription =
            @"INSERT OR IGNORE INTO Subscriptions (ChatId, Url) VALUES (@ChatId, @Url); select last_insert_rowid()";

        public static readonly string RemoveSubscription =
            @"DELETE FROM Subscriptions WHERE ChatId=@ChatId and Url=@Url";

        public static readonly string GetAllSubscriptions =
            @"SELECT Id, ChatId, Url, LastArticleTime  From Subscriptions";

        public static readonly string UpdateLastArticleTime = 
            @"UPDATE Subscriptions SET LastArticleTime = @LastArticleTime WHERE Id = @id";
        
    }
}