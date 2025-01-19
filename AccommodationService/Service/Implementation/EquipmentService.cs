using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using AccommodationService.Service.Contract;

namespace AccommodationService.Service.Implementation;

public class EquipmentService(
    IRepositoryManager repositoryManager
    ) : IEquipmentService
{
    public async Task<IEnumerable<Equipment>> GetAll() => await repositoryManager.EquipmentRepository.GetAllAsync();
}