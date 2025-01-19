using AccommodationService.Model.Entity;

namespace AccommodationService.Repository.Contract;

public interface IEquipmentRepository
{
    Task<Equipment?> GetByName(string name);
    Task<IEnumerable<Equipment>> GetAllAsync();
}