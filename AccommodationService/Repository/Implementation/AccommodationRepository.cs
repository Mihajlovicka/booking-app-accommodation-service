using System.Linq.Expressions;
using AccommodationService.Data;
using AccommodationService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Repository.Implementation;

public class AccommodationRepository(AppDbContext context)
    : CrudRepository<Accommodation>(context), IAccommodationRepository
{
    public async Task<bool> ExistsAsync(Expression<Func<Accommodation, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<IEnumerable<Accommodation>> GetAllByOwnerIdAsync(int ownerId)
    {
        return await _dbSet.Where(a => a.OwnerId == ownerId)
        .Include(a => a.Address)
        .Include(a => a.Pictures)
        .Include(a => a.Equipment)
        .ToListAsync();
    }

    public async Task<Accommodation?> GetByExternalIdAsync(string externalId)
    {
        return await _dbSet
            .Include(a => a.Address)
            .Include(a => a.Pictures)
            .Include(a => a.Equipment)
            .FirstOrDefaultAsync(a => a.ExternalId.ToString() == externalId);
    }

    public async Task<int> DeleteAllByOwner(int id)
    {
        var accommodations = await _dbSet
            .Where(a => a.OwnerId == id)
            .ToListAsync();

        if (accommodations.Count == 0)
            return 0;

        _dbSet.RemoveRange(accommodations);
        return await _context.SaveChangesAsync();
    }

}