using AccommodationService.Model.Messages;

namespace AccommodationService.Service.MessagingService;

public static class TopicTypeMap
{
    public static readonly Dictionary<KafkaTopic, Type> Map =
        new() { { KafkaTopic.UserCreated, typeof(UserDto) } , { KafkaTopic.AccommodationCreated, typeof(AccommodationCreatedDto)}};


}

public enum KafkaTopic
{
    UserCreated,
    AccommodationCreated
}
