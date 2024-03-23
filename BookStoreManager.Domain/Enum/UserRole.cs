using System.Text.Json.Serialization;

namespace BookStoreManager.Domain.Enum
{
    public enum UserRole
    {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    Admin,
    [JsonConverter(typeof(JsonStringEnumConverter))]
    User
    }
}

//UNUSED