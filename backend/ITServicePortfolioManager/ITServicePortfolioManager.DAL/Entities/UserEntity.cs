using ITServicePortfolioManager.DAL.Entities.CommonEntity;

namespace ITServicePortfolioManager.DAL.Entities;

public class UserEntity : BaseEntity
{
    public string UserName { get; set; }
    
    public string HashedPassword { get; set; }
    
    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}