using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfirmationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfirmationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET /api/confirmation
        [HttpGet]
        public ActionResult<IEnumerable<Confirmation>> GetAllConfirmations()
        {
            try
            {
                var repo = new ConfirmationRepository(_configuration);
                return Ok(repo.GetAllConfirmations());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET /api/confirmation/booking/{id}
        [HttpGet("booking/{id}")]
        public ActionResult<IEnumerable<Confirmation>> GetByBookingID(int id)
        {
            try
            {
                var repo = new ConfirmationRepository(_configuration);
                var confirmations = repo.GetByBookingID(id);
                if (confirmations == null || !confirmations.Any())
                    return NotFound($"No confirmations found for BookingID {id}.");
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST /api/confirmation
        [HttpPost]
        public ActionResult CreateConfirmation([FromBody] CreateConfirmationRequest request)
        {
            try
            {
                if (request == null) return BadRequest("Confirmation data is required.");
                var repo = new ConfirmationRepository(_configuration);
                bool created = repo.Add(request);
                if (created) return Ok(new { message = "Confirmation created successfully." });
                return BadRequest("Failed to create confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT /api/confirmation/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateConfirmation(int id, [FromBody] Confirmation confirmation)
        {
            try
            {
                if (confirmation == null) return BadRequest("Confirmation data is required.");
                var repo = new ConfirmationRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Confirmation with ID {id} not found.");
                confirmation.ConfirmationID = id;
                bool updated = repo.Update(confirmation);
                if (updated) return Ok(new { message = "Confirmation updated successfully." });
                return BadRequest("Failed to update confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE /api/confirmation/{id} → soft delete Status = 'Cancelled'
        [HttpDelete("{id}")]
        public ActionResult SoftDeleteConfirmation(int id)
        {
            try
            {
                var repo = new ConfirmationRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Confirmation with ID {id} not found.");
                bool deleted = repo.SoftDelete(id);
                if (deleted) return Ok(new { message = "Confirmation cancelled successfully." });
                return BadRequest("Failed to cancel confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
