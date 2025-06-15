using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;


namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }
}