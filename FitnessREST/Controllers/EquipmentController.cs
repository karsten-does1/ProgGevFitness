using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessDL.Models;
using FitnessBL.Services;
using System.Text.Json;

namespace FitnessREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentService _equipmentService;

        public EquipmentController(EquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipment()
        {
            var equipment = await _equipmentService.GetAllEquipmentAsync();
            return Ok(equipment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipmentById(int id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
            if (equipment == null) return NotFound();
            return Ok(equipment);
        }

   
        [HttpPost]
        public async Task<IActionResult> CreateEquipment([FromBody] JsonElement body)
        {
            
            if (!body.TryGetProperty("deviceType", out var deviceTypeElement) || string.IsNullOrEmpty(deviceTypeElement.GetString()))
            {
                return BadRequest("DeviceType is required.");
            }

            var isInMaintenance = body.TryGetProperty("isInMaintenance", out var isInMaintenanceElement) && isInMaintenanceElement.GetBoolean();

           
            var newEquipment = new Equipment
            {
                DeviceType = deviceTypeElement.GetString(),
                IsInMaintenance = isInMaintenance
            };

            
            var createdEquipment = await _equipmentService.CreateEquipmentAsync(newEquipment);

            return CreatedAtAction(nameof(GetEquipmentById), new { id = createdEquipment.EquipmentId }, createdEquipment);
        }

      
        [HttpPatch("{id}/maintenance")]
        public async Task<IActionResult> UpdateMaintenanceStatus(int id, [FromBody] bool isInMaintenance)
        {
            var success = await _equipmentService.UpdateMaintenanceStatusAsync(id, isInMaintenance);

            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var result = await _equipmentService.DeleteEquipmentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

