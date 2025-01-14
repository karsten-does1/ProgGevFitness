using FitnessDL.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBL.Services
{
    public class SessionService
    {
        private readonly FitnessDbContext _context;

        public SessionService(FitnessDbContext context)
        {
            _context = context;
        }

        

        #region Loop sessies
        public async Task<IEnumerable<RunningsessionMain>> GetRunningSessionsByCustomerAsync(int customerId)
        {
            return await _context.RunningsessionMains
                .Include(s => s.RunningsessionDetails)
                .Where(s => s.MemberId == customerId)
                .ToListAsync();
        }
        #endregion

        #region Fiets sessies
        public async Task<IEnumerable<Cyclingsession>> GetCyclingSessionsByCustomerAsync(int customerId)
        {
            return await _context.Cyclingsessions
                    .Where(s => s.MemberId == customerId) 
                    .ToListAsync();
        }
        #endregion

        #region statistics
        public async Task<dynamic> GetCustomerStatisticsAsync(int customerId)
        {
            
            var runningSessions = await _context.RunningsessionMains
                .Where(rs => rs.MemberId == customerId)
                .Select(rs => new
                {
                    rs.Duration,
                    SessionType = "Running"
                })
                .ToListAsync();

            
            var cyclingSessions = await _context.Cyclingsessions
                .Where(cs => cs.MemberId == customerId)
                .Select(cs => new
                {
                    cs.Duration,
                    SessionType = "Cycling"
                })
                .ToListAsync();


            var runningStats = runningSessions.Any()
               ? new
                {
                   TotalSessions = runningSessions.Count,
                   TotalDuration = runningSessions.Sum(rs => rs.Duration) / 60.0,
                   LongestSession = runningSessions.Max(rs => rs.Duration),
                   ShortestSession = runningSessions.Min(rs => rs.Duration),
                   AvgDuration = runningSessions.Average(rs => rs.Duration)
                }
                 : null;

            var cyclingStats = cyclingSessions.Any()
                ? new
                {
                    TotalSessions = cyclingSessions.Count,
                    TotalDuration = cyclingSessions.Sum(cs => cs.Duration) / 60.0,
                    LongestSession = cyclingSessions.Max(cs => cs.Duration),
                    ShortestSession = cyclingSessions.Min(cs => cs.Duration),
                    AvgDuration = cyclingSessions.Average(cs => cs.Duration)
                }
                : null;

            return new { RunningStats = runningStats, CyclingStats = cyclingStats };
        }

     

        #endregion

        #region session per x datum
        public async Task<IEnumerable<dynamic>> GetSessionsForCustomerAsync(int customerId, int year, int month)
        {
           
            var runningSessions = _context.RunningsessionMains
                .Where(rs => rs.MemberId == customerId && rs.Date.Year == year && rs.Date.Month == month)
                .Select(rs => new
                {
                    rs.Date,
                    rs.Duration,
                    rs.AvgSpeed,
                    SessionType = "Running" 
                });

       
            var cyclingSessions = _context.Cyclingsessions
                .Where(cs => cs.MemberId == customerId && cs.Date.Year == year && cs.Date.Month == month)
                .Select(cs => new
                {
                    cs.Date,
                    cs.Duration,
                    AvgSpeed = 0.0, 
                    SessionType = "Cycling" 
                });


            var combinedSessions = await runningSessions
                .Union(cyclingSessions)
                .OrderBy(s => s.Date)
                .ToListAsync();

            return combinedSessions;
        }

        public async Task<IEnumerable<dynamic>> GetSessionsPerMonthAsync(int customerId, int year)
        {
            
            var runningSessions = _context.RunningsessionMains
                .Where(rs => rs.MemberId == customerId && rs.Date.Year == year)
                .GroupBy(rs => rs.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalSessions = g.Count(),
                    Type = "Running"
                });

            
            var cyclingSessions = _context.Cyclingsessions
                .Where(cs => cs.MemberId == customerId && cs.Date.Year == year)
                .GroupBy(cs => cs.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalSessions = g.Count(),
                    Type = "Cycling"
                });

            
            var combinedSessions = await runningSessions
                .Union(cyclingSessions) 
                .OrderBy(s => s.Month) 
                .ToListAsync();

            return combinedSessions;
        }

        
        #endregion
    }


}

