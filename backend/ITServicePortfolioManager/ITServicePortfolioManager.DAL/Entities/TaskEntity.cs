using System.ComponentModel.DataAnnotations;
using ITServicePortfolioManager.DAL.Entities.CommonEntity;
using ITServicePortfolioManager.DAL.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.Entities;
[EntityTypeConfiguration(typeof(TaskEntityTypeConfiguration))]
public class TaskEntity : BaseEntity
{
    [Required]
    public string NameAlgorithm { get; set; }
    
    [Required]
    public int CountProvider { get; set; }
    
    [Required]
    public List<GroupServiceEntity> Groups { get; set; } = [];
    
    [Required]
    public int TotalHumanResource { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public long UserId { get; set; }
    
    public UserEntity User { get; set; }
}