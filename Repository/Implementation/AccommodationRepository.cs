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

}