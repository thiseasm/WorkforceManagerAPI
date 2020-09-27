using System.Collections.Generic;
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
            var getHistoryResult = _historyRepository.GetAll();
            if (!getHistoryResult.Success)
            {
                return NotFound();
            }

            return getHistoryResult.Data;
        }
    }
}
