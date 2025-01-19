namespace AccommodationService.Repository.Contract;

public interface IRepositoryManager
{
   IAccommodationRepository AccommodationRepository { get; }
   IAddressRepository AddressRepository { get; }
   IEquipmentRepository EquipmentRepository { get; }
   IUserRepository UserRepository { get; }
}