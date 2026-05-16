using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Notifications.Domain.Entities
{
	public abstract class EventLog
	{
		[BsonId]
		[BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		[BsonElement("message"), BsonRepresentation(BsonType.String)]
		public string? Message { get; set; }
	}
}
