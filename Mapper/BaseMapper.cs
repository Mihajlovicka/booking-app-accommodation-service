namespace AccommodationService.Mapper;

public abstract class BaseMapper<TDto, TEntity> : IBaseMapper<TDto, TEntity>
{
    public abstract Task<TEntity> Map(TDto source);
    
    public virtual TDto ReverseMap(TEntity destination)
    {
        throw new NotImplementedException("Reverse mapping is not implemented.");
    }
}