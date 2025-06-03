using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.Task;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ITaskService
{
    Task<List<TaskForResponseDto>> GetTasksAsync(TaskFilterDto filter);
    Task<List<TaskForResponseDto>> GetTasksByUserIdAsync(long UserId);
}