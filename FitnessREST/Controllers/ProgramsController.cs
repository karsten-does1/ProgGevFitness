using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessDL.Models;
using FitnessBL.Services;

namespace FitnessREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly ProgramService _programService;

        public ProgramsController(ProgramService programService)
        {
            _programService = programService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllPrograms()
        {
            var programs = await _programService.GetAllProgramsAsync();
            return Ok(programs);
        }

       
        [HttpGet("{code}")]
        public async Task<IActionResult> GetProgramByCode(string code)
        {
            var program = await _programService.GetProgramByCodeAsync(code);
            if (program == null) return NotFound();
            return Ok(program);
        }

        
        [HttpGet("customer/{customerId}/programs")]
        public async Task<IActionResult> GetProgramsByCustomer(int customerId)
        {
            var programs = await _programService.GetProgramsByCustomerAsync(customerId);
            if (programs == null) return NotFound();
            return Ok(programs);
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateProgram(FitnessProgram program)
        {
            var createdProgram = await _programService.CreateProgramAsync(program);
            return CreatedAtAction(nameof(GetProgramByCode), new { code = createdProgram.ProgramCode }, createdProgram);
        }

       
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateProgram(string code, FitnessProgram program)
        {
            if (code != program.ProgramCode) return BadRequest("Code mismatch");
            var result = await _programService.UpdateProgramAsync(program);
            if (!result) return NotFound();
            return NoContent();
        }

      
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteProgram(string code)
        {
            var result = await _programService.DeleteProgramAsync(code);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
    
    

