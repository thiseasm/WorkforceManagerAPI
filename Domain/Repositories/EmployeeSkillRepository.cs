using System;
using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    public class EmployeeSkillRepository : IEmployeeSkillRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public EmployeeSkillRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext ?? throw new ArgumentNullException();
            _workforceDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public Result AddEntries(List<EmployeeSkill> employeeSkills)
        {
            var result = new Result();
            try
            {
                _workforceDbContext.EmployeeSkills.UpdateRange(employeeSkills);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result RemoveEntries(List<EmployeeSkill> employeeSkills)
        {
            var result = new Result();
            try
            {
                _workforceDbContext.RemoveRange(employeeSkills);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
