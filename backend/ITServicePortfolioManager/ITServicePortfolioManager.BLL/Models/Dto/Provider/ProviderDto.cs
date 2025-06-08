using ITServicePortfolioManager.BLL.Models.Dto.Service;

namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record ProviderDto(List<List<ServiceDto>> ServicesGroups);