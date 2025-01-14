using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessDL.Models;
using System.Linq;
using System.Threading.Tasks;
using FitnessBL.Services;

namespace FitnessREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public TrainingSessionsController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Loop sessies
        [HttpGet("customer/{customerId}/sessions")]
        public async Task <IActionResult> GetRunningSessionsByCustomer(int customerId)
        {
            var sessions = await _sessionService.GetRunningSessionsByCustomerAsync(customerId);
            if (!sessions.Any()) return NotFound("No sessions found for the specified customer");
            return Ok(sessions);
        }
        #endregion

        #region Fiets sessies
        [HttpGet("customer/{customerId}/cycling")]
        public async Task<IActionResult> GetCyclingSessionsByCustomer(int customerId)
        {
            var sessions = await _sessionService.GetCyclingSessionsByCustomerAsync(customerId);
            if (!sessions.Any()) return NotFound("No sessions found for the specified customer");
            return Ok(sessions);
        }
        #endregion

        #region statistics
        [HttpGet("customer/{customerId}/statistics")]
        public async Task<IActionResult> GetCustomerStatistics(int customerId)
        {
            var stats = await _sessionService.GetCustomerStatisticsAsync(customerId);
            return Ok(stats);
        }

       
        #endregion

        #region session per x datum

        [HttpGet("customer/{customerId}/sessionsByDate")]
        public async Task<IActionResult> GetSessionsForCustomer(int customerId, int year, int month)
        {
            var sessions = await _sessionService.GetSessionsForCustomerAsync(customerId, year, month);
            if (!sessions.Any()) return NotFound("No sessions found for the specified customer and date");
            return Ok(sessions);
        }

        [HttpGet("customer/{customerId}/sessions-per-month")]
        public async Task<IActionResult> GetSessionsPerMonth(int customerId, int year)
        {
            var stats = await _sessionService.GetSessionsPerMonthAsync(customerId, year);
            return Ok(stats);
        }

      
        #endregion
    }
}