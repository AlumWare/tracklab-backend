using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Repositorio base para todos los repositorios
/// </summary>
/// <remarks>
///     Esta clase se utiliza para definir las operaciones CRUD básicas para todos los repositorios.
///     Esta clase implementa la interfaz IBaseRepository.
/// </remarks>
/// <typeparam name="TEntity">
///     El tipo de entidad para el repositorio
/// </typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;

    /// <summary>
    ///     Constructor por defecto para el repositorio base
    /// </summary>
    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    // inheritedDoc
    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    // inheritedDoc
    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    // inheritedDoc
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    // inheritedDoc
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    // inheritedDoc
    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }
}