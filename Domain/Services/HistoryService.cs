using Database.Interfaces;
using Domain.Interfaces;

namespace Domain.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IDataContext _workforceDbContext;

        public HistoryService(IDataContext workforceContext)
        {
            _workforceDbContext = workforceContext;
        }
    }
}
