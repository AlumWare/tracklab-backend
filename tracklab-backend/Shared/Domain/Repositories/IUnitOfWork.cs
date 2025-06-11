namespace TrackLab.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}