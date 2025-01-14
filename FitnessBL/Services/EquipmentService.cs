using FitnessDL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessBL.Services
{
    public class EquipmentService
    {
        private readonly FitnessDbContext _context;

        public EquipmentService(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            return await _context.Equipment.ToListAsync();
        }

        public async Task<Equipment?> GetEquipmentByIdAsync(int equipmentId)
        {
            return await _context.Equipment.FindAsync(equipmentId);
        }

        public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();
            return equipment;
        }

        public async Task<bool> UpdateMaintenanceStatusAsync(int equipmentId, bool isInMaintenance)
        {
            var existingEquipment = await _context.Equipment.FindAsync(equipmentId);
            if (existingEquipment == null) return false;

            
            existingEquipment.IsInMaintenance = isInMaintenance;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEquipmentAsync(int equipmentId)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.EquipmentId == equipmentId);

            if (equipment == null) return false;

          
            if (equipment.Reservations.Any(r => r.Date.ToDateTime(new TimeOnly(0, 0)) >= DateTime.Now))
            {
                return false; 
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}