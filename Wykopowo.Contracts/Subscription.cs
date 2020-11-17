using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wykopowo.Contracts
{
    public class Subscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public long ChatId { get; set; }
        public string Url { get; set; }
        public long? LastArticleTime { get; set; }

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