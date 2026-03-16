using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AirportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Airport>> GetAllAirports()
        {
            try
            {
                var repo = new AirportRepository(_configuration);
                return Ok(repo.GetAllAirports());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult CreateAirport([FromBody] Airport airport)
        {
            try
            {
                if (airport == null) return BadRequest("Airport data is required.");
                var repo = new AirportRepository(_configuration);
                bool created = repo.AddAirport(airport);
                if (created) return Ok(new { message = "Airport created successfully." });
                return BadRequest("Failed to create airport.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAirport(int id, [FromBody] Airport airport)
        {
            try
            {
                if (airport == null) return BadRequest("Airport data is required.");
                var repo = new AirportRepository(_configuration);
                var existing = repo.GetAirportByID(id);
                if (existing == null) return NotFound($"Airport with ID {id} not found.");
                airport.OtherDetailsID = id;
                bool updated = repo.UpdateAirport(airport);
                if (updated) return Ok(new { message = "Airport updated successfully." });
                return BadRequest("Failed to update airport.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAirport(int id)
        {
            try
            {
                var repo = new AirportRepository(_configuration);
                var existing = repo.GetAirportByID(id);
                if (existing == null) return NotFound($"Airport with ID {id} not found.");
                bool deleted = repo.DeleteAirport(id);
                if (deleted) return Ok(new { message = "Airport deleted successfully." });
                return BadRequest("Failed to delete airport.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
