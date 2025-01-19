using AccommodationService.Model.Entity;

namespace AccommodationService.Service.Contract;

public interface IEquipmentService
{
    Task<IEnumerable<Equipment>> GetAll();
}