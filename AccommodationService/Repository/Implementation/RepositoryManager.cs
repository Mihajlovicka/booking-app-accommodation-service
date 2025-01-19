using AccommodationService.Repository.Contract;

namespace AccommodationService.Repository.Implementation;

public class RepositoryManager(
    IAccommodationRepository accommodationRepository,
    IAddressRepository addressRepository,
    IEquipmentRepository equipmentRepository,
    IUserRepository userRepository
    ) : IRepositoryManager
{
    public IAccommodationRepository AccommodationRepository { get; } = accommodationRepository;
    public IAddressRepository AddressRepository { get; } = addressRepository;
    public IEquipmentRepository EquipmentRepository { get; } = equipmentRepository;
    public IUserRepository UserRepository { get; } = userRepository;
}