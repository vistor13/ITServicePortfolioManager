using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ITServicePortfolioManager.DAL.Interfaces;
using ErrorOr;
namespace ITServicePortfolioManager.BLL.Services;

public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public async Task<ErrorOr<List<TaskForResponseDto>>> GetTasksAsync(TaskFilterDto filter)
    {
        var tasks = await taskRepository.GetFilteredTasksAsync(filter.UserId,filter.FromDate,filter.ToDate,filter.SortDescending,filter.AlgorithmName);
        if(tasks.Count == 0)
            return Error.NotFound(
                "TasksNotFoundFiltered",
                Messages.Messages.Error.TasksNotFoundByFilter
            );
        return tasks.Select(task => TaskForResponseDto.ToDto(task)).ToList();
    }

    public async Task<ErrorOr<List<TaskForResponseDto>>> GetTasksByUserIdAsync(long UserId)
    {
        var tasks = await taskRepository.GetByUserIdAsync(UserId);
        if(tasks.Count == 0)
            return Error.NotFound(
                "TasksNotFoundForUser",
                Messages.Messages.Error.TasksNotFoundForUser
            );
        return tasks.Select(task => TaskForResponseDto.ToDto(task)).ToList();
    }
}