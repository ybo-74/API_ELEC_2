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
        public AirlinesController(IConfiguration configuration) => _configuration = configuration;

        [HttpGet]
        public IEnumerable<Airline> GetAirlines()
        {
            var repo = new AirlineRepository(_configuration);
            return repo.GetAllAirlines();
        }
    }
}
