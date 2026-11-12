using System.Text.Json.Serialization;

namespace liminal_api.Modules.LevelClass.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SurvivalClassType
    {
        STANDARD,
        ENVIRONMENTAL,
        PSYCHOLOGICAL,
        COMBINATION,
        UTOPIAN,
        DYSTOPIAN,
        UNCLASSIFIED,
        CHANGING,
        OTHER
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Legitimacy
    {
        VERIFIED,
        UNVERIFIED,
        DISPUTED
    }
}
