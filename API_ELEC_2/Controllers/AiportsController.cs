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
        public AirportsController(IConfiguration configuration) => _configuration = configuration;

        [HttpGet]
        public IEnumerable<Airport> GetAirports()
        {
            var repo = new AirportRepository(_configuration);
            return repo.GetAllAirports();
        }
    }
}
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
        public AirportsController(IConfiguration configuration) => _configuration = configuration;

        [HttpGet]
        public IEnumerable<Airport> GetAirports()
        {
            var repo = new AirportRepository(_configuration);
            return repo.GetAllAirports();
        }
    }
}
