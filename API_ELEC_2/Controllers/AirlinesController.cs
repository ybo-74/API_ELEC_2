using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirlinesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AirlinesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Airline>> GetAllAirlines()
        {
            try
            {
                var repo = new AirlineRepository(_configuration);
                return Ok(repo.GetAllAirlines());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult CreateAirline([FromBody] Airline airline)
        {
            try
            {
                if (airline == null) return BadRequest("Airline data is required.");
                var repo = new AirlineRepository(_configuration);
                bool created = repo.AddAirline(airline);
                if (created) return Ok(new { message = "Airline created successfully." });
                return BadRequest("Failed to create airline.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAirline(int id, [FromBody] Airline airline)
        {
            try
            {
                if (airline == null) return BadRequest("Airline data is required.");
                var repo = new AirlineRepository(_configuration);
                var existing = repo.GetAirlineByID(id);
                if (existing == null) return NotFound($"Airline with ID {id} not found.");
                airline.AirlineID = id;
                bool updated = repo.UpdateAirline(airline);
                if (updated) return Ok(new { message = "Airline updated successfully." });
                return BadRequest("Failed to update airline.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAirline(int id)
        {
            try
            {
                var repo = new AirlineRepository(_configuration);
                var existing = repo.GetAirlineByID(id);
                if (existing == null) return NotFound($"Airline with ID {id} not found.");
                bool deleted = repo.DeleteAirline(id);
                if (deleted) return Ok(new { message = "Airline deleted successfully." });
                return BadRequest("Failed to delete airline.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
