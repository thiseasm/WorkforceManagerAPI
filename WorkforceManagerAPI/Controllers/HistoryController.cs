﻿using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WorkforceManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryController(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        // GET: api/History
        [HttpGet]
        public ActionResult<IEnumerable<HistoryEntry>> GetHistory()
        {
            return _historyRepository.GetAll();
        }

        // GET: api/History/id
        [HttpGet("{id}")]
        public  ActionResult<IEnumerable<HistoryEntry>> GetHistoryForEmployee(int id)
        {
            var employeeHistory = _historyRepository.GetEntriesForEmployee(id);

            if (employeeHistory == null)
                return NotFound();
            
            return employeeHistory;
        }
    }
}
