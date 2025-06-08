
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ErrorOr;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ITaskService
{
    Task<ErrorOr<List<TaskForResponseDto>>> GetTasksAsync(TaskFilterDto filter);
    Task<ErrorOr<List<TaskForResponseDto>>> GetTasksByUserIdAsync(long UserId);
}