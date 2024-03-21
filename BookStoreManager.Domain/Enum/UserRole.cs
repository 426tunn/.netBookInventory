using System.Text.Json.Serialization;

namespace BookStoreManager.Domain.Enum
{
    public enum UserRole
    {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    User,
    [JsonConverter(typeof(JsonStringEnumConverter))]
    Admin
    }
}