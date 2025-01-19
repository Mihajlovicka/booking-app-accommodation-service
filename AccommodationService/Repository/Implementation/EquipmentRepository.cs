using AccommodationService.Data;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Repository.Implementation;

public class EquipmentRepository(AppDbContext context) : IEquipmentRepository
{
    public async Task<Equipment?> GetByName(string name)
        => await Task.FromResult(context.Equipments.FirstOrDefault(x => x.Name == name));
    
    public async Task<IEnumerable<Equipment>> GetAllAsync() => await context.Equipments.ToListAsync();
}