namespace AccommodationService.Mapper;

public interface IBaseMapper<TDto, TEntity>
{
    Task<TEntity> Map(TDto source);
    TDto ReverseMap(TEntity destination);
}