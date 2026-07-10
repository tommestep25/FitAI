using FitAI.Application.Features.Dashboard;
using FitAI.Application.Features.Users;
using FitAI.Application.Features.Exercises;
using Microsoft.Extensions.DependencyInjection;
using FitAI.Application.Features.WorkoutSessions;
using FitAI.Application.Features.WorkoutTemplates;
using FitAI.Application.Features.WorkoutSessionDetail;
using FitAI.Application.Features.WorkoutSets;
using FitAI.Application.Features.WorkoutSessions;

namespace FitAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IDashboardQueryService, DashboardQueryService>();
        services.AddScoped<IExerciseQueryService, ExerciseQueryService>();
        services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
        services.AddScoped<IWorkoutTemplateQueryService, WorkoutTemplateQueryService>();
        services.AddScoped<IWorkoutSessionDetailQueryService,WorkoutSessionDetailQueryService>();
        services.AddScoped<IWorkoutSetService, WorkoutSetService>();
        services.AddScoped<ICompleteWorkoutService, CompleteWorkoutService>();
        return services;
    }
}