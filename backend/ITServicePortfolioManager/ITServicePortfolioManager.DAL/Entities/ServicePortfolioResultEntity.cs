using System.ComponentModel.DataAnnotations;
using ITServicePortfolioManager.DAL.Entities.CommonEntity;
using ITServicePortfolioManager.DAL.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.Entities;


[EntityTypeConfiguration(typeof(ServicePortfolioResultEntityTypeConfiguration))]
public class ServicePortfolioResultEntity : BaseEntity 
{
    [Required]
    public double CompanyIncome { get; set; }
    [Required]
    public double[] ProvidersIncome { get; set; } = [];
   
    [Required]
    public List<PortfolioCellEntity> Portfolio { get; set; } = [];
    
    public TaskEntity Task { get; set; } = null!;
    [Required]
    public long TaskId { get; set; } 
}