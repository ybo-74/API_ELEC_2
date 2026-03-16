using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddOthersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AddOthersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET /api/addothers/booking/{bookingId}
        [HttpGet("booking/{bookingId}")]
        public ActionResult<IEnumerable<AddOthers>> GetByBookingID(int bookingId)
        {
            try
            {
                var repo = new AddOthersRepository(_configuration);
                var items = repo.GetByBookingID(bookingId);
                if (items == null || !items.Any())
                    return NotFound($"No add-ons found for BookingID {bookingId}.");
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET /api/addothers/pax/{paxId}
        [HttpGet("pax/{paxId}")]
        public ActionResult<IEnumerable<AddOthers>> GetByPaxID(int paxId)
        {
            try
            {
                var repo = new AddOthersRepository(_configuration);
                var items = repo.GetByPaxID(paxId);
                if (items == null || !items.Any())
                    return NotFound($"No add-ons found for PaxID {paxId}.");
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST /api/addothers
        [HttpPost]
        public ActionResult CreateAddOthers([FromBody] AddOthers item)
        {
            try
            {
                if (item == null) return BadRequest("Add-on data is required.");

                var repo = new AddOthersRepository(_configuration);

                // Block if booking is cancelled
                if (repo.IsBookingCancelled(item.BookingID))
                    return BadRequest($"Booking {item.BookingID} has been cancelled. Cannot add extras to a cancelled booking.");

                bool created = repo.AddAddOthers(item);
                if (created) return Ok(new { message = "Add-on created successfully." });
                return BadRequest("Failed to create add-on.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT /api/addothers/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateAddOthers(int id, [FromBody] AddOthers item)
        {
            try
            {
                if (item == null) return BadRequest("Add-on data is required.");

                var repo = new AddOthersRepository(_configuration);

                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Add-on with ID {id} not found.");

                // Block if booking is cancelled
                if (repo.IsBookingCancelled(existing.BookingID))
                    return BadRequest($"Booking {existing.BookingID} has been cancelled. Cannot update extras on a cancelled booking.");

                item.AddOthersID = id;
                bool updated = repo.UpdateAddOthers(item);
                if (updated) return Ok(new { message = "Add-on updated successfully." });
                return BadRequest("Failed to update add-on.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE /api/addothers/{id} → soft delete IsActive = 0
        [HttpDelete("{id}")]
        public ActionResult SoftDeleteAddOthers(int id)
        {
            try
            {
                var repo = new AddOthersRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Add-on with ID {id} not found.");
                bool deleted = repo.SoftDelete(id);
                if (deleted) return Ok(new { message = "Add-on deactivated successfully." });
                return BadRequest("Failed to deactivate add-on.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
