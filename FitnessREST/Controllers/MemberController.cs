using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessDL.Models;
using FitnessBL.Services;


namespace FitnessREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MembersController(MemberService memberService)
        {
            _memberService = memberService;
        }

        #region GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound("Member not found.");
            return Ok(member);
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] Member newMember)
        {
            try
            {
                var createdMember = await _memberService.AddMemberAsync(newMember);
                return CreatedAtAction(nameof(GetMember), new { id = createdMember.MemberId }, createdMember);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        #endregion

        
        #region PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, Member member)
        {
            if (id != member.MemberId) return BadRequest("ID mismatch");
            var result = await _memberService.UpdateMemberAsync(member);
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion

        
        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion
    }
}

