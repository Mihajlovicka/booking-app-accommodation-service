using System.Linq.Expressions;
using AccommodationService.Model.Entity;

namespace AccommodationService.Repository.Contract;

public interface IAccommodationRepository : ICrudRepository<Accommodation>
{
    Task<bool> ExistsAsync(Expression<Func<Accommodation, bool>> predicate);

}