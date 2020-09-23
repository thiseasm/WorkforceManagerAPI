using Database.Interfaces;
using Domain.Interfaces;

namespace Domain.Services
{
    public class SkillService : ISkillService
    {
        private readonly IDataContext _workforceDbContext;

        public SkillService(IDataContext workforceContext)
        {
            _workforceDbContext = workforceContext;
        }
    }
}
