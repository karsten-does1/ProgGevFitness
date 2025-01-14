using FitnessDL.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBL.Services
{
    public class ReservationService
    {
        private readonly FitnessDbContext _context;

        public ReservationService(FitnessDbContext context)
        {
            _context = context;
        }

        private async Task<bool> CanAddTimeSlotAsync(int memberId, DateOnly date)
        {
            var totalTimeSlots = await _context.ReservationTimeSlots
                .Include(r => r.Reservation)
                .Where(r => r.Reservation.MemberId == memberId && r.Reservation.Date == date)
                .CountAsync();

            return totalTimeSlots < 4;
        }

        private async Task<bool> IsTimeSlotAvailableAsync(int equipmentId, int timeSlotId, DateOnly date)
        {
            return !await _context.ReservationTimeSlots
                .Include(r => r.Reservation)
                .AnyAsync(r =>
                    r.EquipmentId == equipmentId &&
                    r.TimeSlotId == timeSlotId &&
                    r.Reservation.Date == date);
        }
        private bool IsValidReservationDate(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return date > today && date <= today.AddDays(7);
        }

        public async Task<Reservation?> CreateReservationAsync(Reservation reservation)
        {
            if (!IsValidReservationDate(reservation.Date))
                throw new InvalidOperationException("Reservation date must be in the future and within one week.");

            if (!await CanAddTimeSlotAsync(reservation.MemberId, reservation.Date))
                throw new InvalidOperationException("Customer has exceeded the daily time slot limit (4 per day).");

            foreach (var timeSlot in reservation.ReservationTimeSlots)
            {
                if (!await IsTimeSlotAvailableAsync(timeSlot.EquipmentId, timeSlot.TimeSlotId, reservation.Date))
                    throw new InvalidOperationException("One or more time slots are already reserved for the selected equipment.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation?> UpdateReservationAsync(Reservation updatedReservation)
        {
            var existingReservation = await _context.Reservations
                .Include(r => r.ReservationTimeSlots)
                .FirstOrDefaultAsync(r => r.ReservationId == updatedReservation.ReservationId);

            if (existingReservation == null)
                throw new InvalidOperationException("Reservation not found.");

            if (!IsValidReservationDate(updatedReservation.Date))
                throw new InvalidOperationException("Reservation date must be in the future and within one week.");

            _context.Entry(existingReservation).CurrentValues.SetValues(updatedReservation);

            existingReservation.ReservationTimeSlots.Clear();
            foreach (var timeSlot in updatedReservation.ReservationTimeSlots)
            {
                if (!await IsTimeSlotAvailableAsync(timeSlot.EquipmentId, timeSlot.TimeSlotId, updatedReservation.Date))
                    throw new InvalidOperationException("One or more time slots are already reserved for the selected equipment.");

                existingReservation.ReservationTimeSlots.Add(timeSlot);
            }

            await _context.SaveChangesAsync();
            return existingReservation;
        }

        public async Task<bool> DeleteReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
