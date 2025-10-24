using System.Linq.Expressions;
using AccommodationService.Model.Entity;

namespace AccommodationService.Repository.Contract;

public interface IAccommodationRepository : ICrudRepository<Accommodation>
{
    Task<bool> ExistsAsync(Expression<Func<Accommodation, bool>> predicate);

    Task<IEnumerable<Accommodation>> GetAllByOwnerIdAsync(int ownerId);

    Task<int> DeleteAllByOwner(int ownerId);

    Task<Accommodation?> GetByExternalIdAsync(string externalId);
}