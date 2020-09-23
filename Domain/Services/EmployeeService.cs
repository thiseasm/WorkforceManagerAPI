using Database.Interfaces;
using Domain.Interfaces;

namespace Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IDataContext _workforceDbContext;

        public EmployeeService(IDataContext workforceContext)
        {
            _workforceDbContext = workforceContext;
        }
    }
}
