using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BookingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET /api/bookings/available
        [HttpGet("available")]
        public ActionResult<IEnumerable<AvailableFlight>> GetAvailableFlights()
        {
            try
            {
                var repo = new BookingRepository(_configuration);
                return Ok(repo.GetAvailableFlights());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET /api/bookings/details/{bookingId}
        [HttpGet("details/{bookingId}")]
        public ActionResult<IEnumerable<BookingDetail>> GetBookingDetails(int bookingId)
        {
            try
            {
                var repo = new BookingRepository(_configuration);
                var details = repo.GetBookingDetails(bookingId);
                if (details == null || !details.Any())
                    return NotFound($"No booking details found for BookingID {bookingId}.");
                return Ok(details);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST /api/bookings
        [HttpPost]
        public ActionResult CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                if (request == null) return BadRequest("Booking data is required.");
                var repo = new BookingRepository(_configuration);
                int newBookingID = repo.CreateBooking(request);
                return Ok(new { BookingID = newBookingID, message = "Booking created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT /api/bookings/{id} → update passenger info
        [HttpPut("{id}")]
        public ActionResult UpdatePassenger(int id, [FromBody] BookingPax pax)
        {
            try
            {
                if (pax == null) return BadRequest("Passenger data is required.");
                var repo = new BookingRepository(_configuration);
                var existing = repo.GetBookingByID(id);
                if (existing == null) return NotFound($"Booking with ID {id} not found.");
                bool updated = repo.UpdatePassenger(id, pax);
                if (updated) return Ok(new { message = "Passenger info updated successfully." });
                return BadRequest("Failed to update passenger info.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT /api/bookings/{id}/transfer
        [HttpPut("{id}/transfer")]
        public ActionResult TransferFlight(int id, [FromBody] TransferRequest request)
        {
            try
            {
                if (request == null) return BadRequest("Transfer data is required.");
                var repo = new BookingRepository(_configuration);
                var existing = repo.GetBookingByID(id);
                if (existing == null) return NotFound($"Booking with ID {id} not found.");
                bool transferred = repo.TransferFlight(id, request.NewFlightID);
                if (transferred) return Ok(new { message = "Flight transfer successful." });
                return BadRequest("Failed to transfer flight.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE /api/bookings/{id} → soft cancel
        [HttpDelete("{id}")]
        public ActionResult CancelBooking(int id)
        {
            try
            {
                var repo = new BookingRepository(_configuration);
                var existing = repo.GetBookingByID(id);
                if (existing == null) return NotFound($"Booking with ID {id} not found.");
                bool cancelled = repo.CancelBooking(id);
                if (cancelled) return Ok(new { message = "Booking cancelled successfully." });
                return BadRequest("Failed to cancel booking.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
