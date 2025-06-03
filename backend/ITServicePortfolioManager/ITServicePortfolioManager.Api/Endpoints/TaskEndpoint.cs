using ITServicePortfolioManager.Api.Contracts.Request;
using ITServicePortfolioManager.Api.Contracts.Response.Task;
using ITServicePortfolioManager.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITServicePortfolioManager.Api.Endpoints;

public static class TaskEndpoint
{
    public static void MapTaskEndpoint(this IEndpointRouteBuilder app)
    {
        var endPoints = app.MapGroup("api/task/").WithTags("Tasks");
        endPoints.MapGet("tasks", GetTasksByUserId).RequireAuthorization();
        endPoints.MapPost("filtered-tasks", GetFilteredTasks);

    }
    private static async Task<IResult> GetFilteredTasks( [FromServices] ITaskService taskService,[FromBody] TaskFilteredRequest filter)
    {
        var tasks = await taskService.GetTasksAsync(TaskFilteredRequest.ToDto(filter));
        var responses = tasks
            .Select(TaskResponse.MapToResponse)
            .ToList();

        return Results.Ok(responses);
    }
    private static async Task<IResult> GetTasksByUserId([FromServices] ITaskService taskService, HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        
        var result = await taskService.GetTasksByUserIdAsync(Int64.Parse(userId));
        var responses = result
            .Select(TaskResponse.MapToResponse)
            .ToList();

        return Results.Ok(responses);
    }
}