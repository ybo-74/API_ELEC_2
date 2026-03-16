using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FlightsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Flight>> GetAllFlights()
        {
            try
            {
                var repo = new FlightRepository(_configuration);
                return Ok(repo.GetAllFlights());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("completed")]
        public ActionResult<IEnumerable<Flight>> GetCompletedFlights()
        {
            try
            {
                var repo = new FlightRepository(_configuration);
                return Ok(repo.GetCompletedFlights());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult CreateFlight([FromBody] Flight flight)
        {
            try
            {
                if (flight == null) return BadRequest("Flight data is required.");
                var repo = new FlightRepository(_configuration);
                bool created = repo.AddFlight(flight);
                if (created) return Ok(new { message = "Flight created successfully." });
                return BadRequest("Failed to create flight.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateFlight(int id, [FromBody] Flight flight)
        {
            try
            {
                if (flight == null) return BadRequest("Flight data is required.");
                var repo = new FlightRepository(_configuration);
                var existing = repo.GetFlightByID(id);
                if (existing == null) return NotFound($"Flight with ID {id} not found.");
                flight.FlightID = id;
                bool updated = repo.UpdateFlight(flight);
                if (updated) return Ok(new { message = "Flight updated successfully." });
                return BadRequest("Failed to update flight.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteFlight(int id)
        {
            try
            {
                var repo = new FlightRepository(_configuration);
                var existing = repo.GetFlightByID(id);
                if (existing == null) return NotFound($"Flight with ID {id} not found.");
                bool deleted = repo.DeleteFlight(id);
                if (deleted) return Ok(new { message = "Flight deleted successfully." });
                return BadRequest("Failed to delete flight.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
