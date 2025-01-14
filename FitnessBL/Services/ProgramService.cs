using FitnessDL.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBL.Services
{
    public class ProgramService
    {
        private readonly FitnessDbContext _context;

        public ProgramService(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FitnessProgram>> GetAllProgramsAsync()
        {
            return await _context.Programs.ToListAsync();
        }

        public async Task<FitnessProgram?> GetProgramByCodeAsync(string programCode)
        {
            return await _context.Programs.FindAsync(programCode);
        }

        public async Task<IEnumerable<FitnessProgram>> GetProgramsByCustomerAsync(int customerId)
        {
            return await _context.Programs
                .Include(p => p.Members) 
                .Where(p => p.Members.Any(m => m.MemberId == customerId)) 
                .ToListAsync();
        }

        public async Task<FitnessProgram> CreateProgramAsync(FitnessProgram program)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> UpdateProgramAsync(FitnessProgram program)
        {
            var existingProgram = await _context.Programs.FindAsync(program.ProgramCode);
            if (existingProgram == null) return false;

            _context.Entry(existingProgram).CurrentValues.SetValues(program);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProgramAsync(string programCode)
        {
            var program = await _context.Programs.FindAsync(programCode);
            if (program == null) return false;

            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
