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
        public IEnumerable<Flight> GetFlights()
        {
            var repo = new FlightRepository(_configuration);
            return repo.GetAllFlights();
        }

        [HttpPost]
        public IActionResult CreateFlight([FromBody] Flight flight)
        {
            if (flight == null) return BadRequest("Flight is null");
            var repo = new FlightRepository(_configuration);
            repo.AddFlight(flight);
            return Ok(new { message = "Flight created successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFlight(int id, [FromBody] Flight flight)
        {
            if (flight == null) return BadRequest("Flight is null");
            flight.FlightID = id;
            var repo = new FlightRepository(_configuration);
            repo.UpdateFlight(flight);
            return Ok(new { message = "Flight updated successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var repo = new FlightRepository(_configuration);
            repo.DeleteFlight(id);
            return Ok(new { message = "Flight deleted successfully." });
        }
    }
}
