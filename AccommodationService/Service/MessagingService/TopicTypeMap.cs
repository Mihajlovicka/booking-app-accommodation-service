using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Service.MessagingService;

public static class TopicTypeMap
{
    public static readonly Dictionary<KafkaTopic, Type> Map =
        new() { { KafkaTopic.UserCreated, typeof(User) } };
}

public enum KafkaTopic
{
    UserCreated,
    AnotherTopic,
}
