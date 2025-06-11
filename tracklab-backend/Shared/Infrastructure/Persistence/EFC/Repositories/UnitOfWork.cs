using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}