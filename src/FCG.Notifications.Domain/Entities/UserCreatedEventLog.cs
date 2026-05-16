using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Notifications.Domain.Entities
{
	public class UserCreatedEventLog : EventLog
	{
		[BsonElement("userId"), BsonRepresentation(BsonType.Int32)]
		public int? UserId { get; set; }

		[BsonElement("email"), BsonRepresentation(BsonType.String)]
		public string? Email { get; set; }
	}
}
