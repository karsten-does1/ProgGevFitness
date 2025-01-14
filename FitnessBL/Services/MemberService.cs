using FitnessDL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitnessBL.Services
{
    public class MemberService
    {
        private readonly FitnessDbContext _context;
        private readonly ILogger<MemberService> _logger;

        public MemberService(FitnessDbContext context, ILogger<MemberService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region GET
       
        public async Task<Member?> GetMemberByIdAsync(int memberId)
        {
            return await _context.Members.FindAsync(memberId);
        }
        #endregion

        public async Task<Member> AddMemberAsync(Member newMember)
        {
            
            if (string.IsNullOrWhiteSpace(newMember.FirstName) ||
                string.IsNullOrWhiteSpace(newMember.LastName) ||
                string.IsNullOrWhiteSpace(newMember.Email) ||
                string.IsNullOrWhiteSpace(newMember.Address) ||
                newMember.Birthday == default)
            {
                throw new ArgumentException("All required fields must be provided.");
            }

            
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == newMember.Email);
            if (existingMember != null)
            {
                throw new InvalidOperationException("A member with this email already exists.");
            }

         
            if (newMember.Birthday >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Birthday must be in the past.");
            }

            
            if (string.IsNullOrWhiteSpace(newMember.Membertype))
            {
                newMember.Membertype = "Bronze"; 
            }


            _context.Members.Add(newMember);
            await _context.SaveChangesAsync();

            return newMember;
        }


        #region PUT 
        public async Task<bool> UpdateMemberAsync(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.FirstName) || string.IsNullOrWhiteSpace(member.LastName))
            {
                _logger.LogWarning("Validation failed: First name and last name are required.");
                throw new ArgumentException("First name and last name are required.");
            }

            var existingMember = await _context.Members.FindAsync(member.MemberId);
            if (existingMember == null)
            {
                _logger.LogWarning("Member with ID {Id} not found for update.", member.MemberId);
                return false;
            }

            if (await _context.Members.AnyAsync(m => m.Email == member.Email && m.MemberId != member.MemberId))
            {
                _logger.LogWarning("Validation failed: A member with the email {Email} already exists.", member.Email);
                throw new ArgumentException("A member with the same email already exists.");
            }

            _context.Entry(existingMember).CurrentValues.SetValues(member);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Member with ID {Id} updated successfully.", member.MemberId);
            return true;
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteMemberAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                _logger.LogWarning("Member with ID {Id} not found for deletion.", memberId);
                return false;
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Member with ID {Id} deleted successfully.", memberId);
            return true;
        }
        #endregion
    }
}
