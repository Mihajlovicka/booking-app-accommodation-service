using AccommodationService.Mapper;
using AccommodationService.Mapper.AccommodationMapper;
using AccommodationService.Mapper.EquipmentMapper;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using AccommodationService.Repository.Implementation;
using AccommodationService.Service.Contract;
using AccommodationService.Service.Implementation;

namespace AccommodationService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // Scoped services registration
        services.AddScoped<IAccommodationService, Service.Implementation.AccommodationService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IUserService, UserService>();

        // Mapper-related scoped services
        services.AddScoped<IMapperManager, MapperManager>();
        services.AddScoped<IBaseMapper<EquipmentDto, Equipment>, EquipmentDtoToEquipmentMapper>();
        services.AddScoped<IBaseMapper<CreateAccommodationDto, Accommodation>, CreateAccommodationDtoToAccommodationMapper>();
        services.AddScoped<IBaseMapper<AddressDto, Address>, AddressDtoToAddressMapper>();
        
        // Repository-related scoped services
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        return services;
    }
}