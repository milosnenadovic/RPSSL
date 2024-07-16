using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RPSSL.GameService.Common.Services;
using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.UtcNow;
                if (!(entry.Entity.CreatedBy ?? "").Equals("system", StringComparison.OrdinalIgnoreCase))
                    entry.Entity.CreatedBy = _currentUserService.CurrentUser.UserId.ToString();
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModified = DateTime.UtcNow;
                if (!(entry.Entity.LastModifiedBy ?? "").Equals("system", StringComparison.OrdinalIgnoreCase))
                    entry.Entity.LastModifiedBy = _currentUserService.CurrentUser.UserId.ToString();
            }
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableIdentityEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.UtcNow;
                if (!(entry.Entity.CreatedBy ?? "").Equals("system", StringComparison.OrdinalIgnoreCase))
                    entry.Entity.CreatedBy = _currentUserService.CurrentUser.UserId.ToString();
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModified = DateTime.UtcNow;
                if (!(entry.Entity.LastModifiedBy ?? "").Equals("system", StringComparison.OrdinalIgnoreCase))
                    entry.Entity.LastModifiedBy = _currentUserService.CurrentUser.UserId.ToString();
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}