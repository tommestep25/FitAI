using FitAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitAI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<WeightLog> WeightLogs { get; }
    DbSet<InBodyLog> InBodyLogs { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<WorkoutTemplateDay> WorkoutTemplateDays { get; }
    DbSet<WorkoutSession> WorkoutSessions { get; }
    DbSet<WorkoutSessionExercise> WorkoutSessionExercises { get; }
    DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises { get; }
    DbSet<WorkoutSet> WorkoutSets { get; }
    DbSet<UserProfile> UserProfiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}