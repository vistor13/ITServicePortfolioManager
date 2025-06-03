using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.BLL.Services;

public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public async Task<List<TaskForResponseDto>> GetTasksAsync(TaskFilterDto filter)
    {
        var tasks = await taskRepository.GetFilteredTasksAsync(filter.FromDate,filter.ToDate,filter.SortDescending,filter.AlgorithmName);
        return tasks.Select(task => TaskForResponseDto.ToDto(task)).ToList();
    }

    public async Task<List<TaskForResponseDto>> GetTasksByUserIdAsync(long UserId)
    {
        var tasks = await taskRepository.GetByUserIdAsync(UserId);
        return tasks.Select(task => TaskForResponseDto.ToDto(task)).ToList();
    }
}