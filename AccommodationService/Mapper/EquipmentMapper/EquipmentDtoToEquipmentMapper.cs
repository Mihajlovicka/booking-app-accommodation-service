using AccommodationService.Model.Dto;
using AccommodationService.Repository.Contract;

namespace AccommodationService.Mapper.EquipmentMapper;

public class EquipmentDtoToEquipmentMapper(
    IRepositoryManager repositoryManager
    ) : BaseMapper<EquipmentDto, Model.Entity.Equipment>
{
    public override async Task<Model.Entity.Equipment> Map(EquipmentDto source)
    {
        var model = await repositoryManager.EquipmentRepository.GetByName(source.Name);
        return model ?? new Model.Entity.Equipment();
    }

    public override EquipmentDto ReverseMap(Model.Entity.Equipment destination)
    {
        return new EquipmentDto()
        {
            Name = destination.Name,
            Selected = false //change mapping later for view
        };
    }
}