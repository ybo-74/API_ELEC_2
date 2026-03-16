using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Repositories;
using API_ELEC_2.Models;

namespace API_ELEC_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtherDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OtherDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ─────────────────────────────────────────
        // MEALS
        // ─────────────────────────────────────────

        [HttpGet("meals")]
        public ActionResult<IEnumerable<OthersMeal>> GetAllMeals()
        {
            try
            {
                var repo = new OthersMealRepository(_configuration);
                return Ok(repo.GetAllMeals());
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("meals/{id}")]
        public ActionResult<OthersMeal> GetMealByID(int id)
        {
            try
            {
                var repo = new OthersMealRepository(_configuration);
                var item = repo.GetByID(id);
                if (item == null) return NotFound($"Meal with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("meals")]
        public ActionResult CreateMeal([FromBody] OthersMeal meal)
        {
            try
            {
                if (meal == null) return BadRequest("Meal data is required.");
                var repo = new OthersMealRepository(_configuration);
                bool created = repo.Add(meal);
                if (created) return Ok(new { message = "Meal created successfully." });
                return BadRequest("Failed to create meal.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("meals/{id}")]
        public ActionResult UpdateMeal(int id, [FromBody] OthersMeal meal)
        {
            try
            {
                if (meal == null) return BadRequest("Meal data is required.");
                var repo = new OthersMealRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Meal with ID {id} not found.");
                meal.ID = id;
                bool updated = repo.Update(meal);
                if (updated) return Ok(new { message = "Meal updated successfully." });
                return BadRequest("Failed to update meal.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("meals/{id}")]
        public ActionResult DeleteMeal(int id)
        {
            try
            {
                var repo = new OthersMealRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Meal with ID {id} not found.");
                bool deleted = repo.Delete(id);
                if (deleted) return Ok(new { message = "Meal deleted successfully." });
                return BadRequest("Failed to delete meal.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        // ─────────────────────────────────────────
        // SEATS
        // ─────────────────────────────────────────

        [HttpGet("seats")]
        public ActionResult<IEnumerable<OthersSeat>> GetAllSeats()
        {
            try
            {
                var repo = new OthersSeatRepository(_configuration);
                return Ok(repo.GetAllSeats());
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("seats/{id}")]
        public ActionResult<OthersSeat> GetSeatByID(int id)
        {
            try
            {
                var repo = new OthersSeatRepository(_configuration);
                var item = repo.GetByID(id);
                if (item == null) return NotFound($"Seat with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("seats")]
        public ActionResult CreateSeat([FromBody] OthersSeat seat)
        {
            try
            {
                if (seat == null) return BadRequest("Seat data is required.");
                var repo = new OthersSeatRepository(_configuration);
                bool created = repo.Add(seat);
                if (created) return Ok(new { message = "Seat created successfully." });
                return BadRequest("Failed to create seat.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("seats/{id}")]
        public ActionResult UpdateSeat(int id, [FromBody] OthersSeat seat)
        {
            try
            {
                if (seat == null) return BadRequest("Seat data is required.");
                var repo = new OthersSeatRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Seat with ID {id} not found.");
                seat.ID = id;
                bool updated = repo.Update(seat);
                if (updated) return Ok(new { message = "Seat updated successfully." });
                return BadRequest("Failed to update seat.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("seats/{id}")]
        public ActionResult DeleteSeat(int id)
        {
            try
            {
                var repo = new OthersSeatRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Seat with ID {id} not found.");
                bool deleted = repo.Delete(id);
                if (deleted) return Ok(new { message = "Seat deleted successfully." });
                return BadRequest("Failed to delete seat.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        // ─────────────────────────────────────────
        // INSURANCE
        // ─────────────────────────────────────────

        [HttpGet("insurance")]
        public ActionResult<IEnumerable<OthersInsurance>> GetAllInsurance()
        {
            try
            {
                var repo = new OthersInsuranceRepository(_configuration);
                return Ok(repo.GetAllInsurance());
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("insurance/{id}")]
        public ActionResult<OthersInsurance> GetInsuranceByID(int id)
        {
            try
            {
                var repo = new OthersInsuranceRepository(_configuration);
                var item = repo.GetByID(id);
                if (item == null) return NotFound($"Insurance with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("insurance")]
        public ActionResult CreateInsurance([FromBody] OthersInsurance insurance)
        {
            try
            {
                if (insurance == null) return BadRequest("Insurance data is required.");
                var repo = new OthersInsuranceRepository(_configuration);
                bool created = repo.Add(insurance);
                if (created) return Ok(new { message = "Insurance created successfully." });
                return BadRequest("Failed to create insurance.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("insurance/{id}")]
        public ActionResult UpdateInsurance(int id, [FromBody] OthersInsurance insurance)
        {
            try
            {
                if (insurance == null) return BadRequest("Insurance data is required.");
                var repo = new OthersInsuranceRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Insurance with ID {id} not found.");
                insurance.ID = id;
                bool updated = repo.Update(insurance);
                if (updated) return Ok(new { message = "Insurance updated successfully." });
                return BadRequest("Failed to update insurance.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("insurance/{id}")]
        public ActionResult DeleteInsurance(int id)
        {
            try
            {
                var repo = new OthersInsuranceRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Insurance with ID {id} not found.");
                bool deleted = repo.Delete(id);
                if (deleted) return Ok(new { message = "Insurance deleted successfully." });
                return BadRequest("Failed to delete insurance.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        // ─────────────────────────────────────────
        // BAGGAGE
        // ─────────────────────────────────────────

        [HttpGet("baggage")]
        public ActionResult<IEnumerable<OthersBaggage>> GetAllBaggage()
        {
            try
            {
                var repo = new OthersBaggageRepository(_configuration);
                return Ok(repo.GetAllBaggage());
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("baggage/{id}")]
        public ActionResult<OthersBaggage> GetBaggageByID(int id)
        {
            try
            {
                var repo = new OthersBaggageRepository(_configuration);
                var item = repo.GetByID(id);
                if (item == null) return NotFound($"Baggage with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost("baggage")]
        public ActionResult CreateBaggage([FromBody] OthersBaggage baggage)
        {
            try
            {
                if (baggage == null) return BadRequest("Baggage data is required.");
                var repo = new OthersBaggageRepository(_configuration);
                bool created = repo.Add(baggage);
                if (created) return Ok(new { message = "Baggage created successfully." });
                return BadRequest("Failed to create baggage.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("baggage/{id}")]
        public ActionResult UpdateBaggage(int id, [FromBody] OthersBaggage baggage)
        {
            try
            {
                if (baggage == null) return BadRequest("Baggage data is required.");
                var repo = new OthersBaggageRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Baggage with ID {id} not found.");
                baggage.ID = id;
                bool updated = repo.Update(baggage);
                if (updated) return Ok(new { message = "Baggage updated successfully." });
                return BadRequest("Failed to update baggage.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("baggage/{id}")]
        public ActionResult DeleteBaggage(int id)
        {
            try
            {
                var repo = new OthersBaggageRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"Baggage with ID {id} not found.");
                bool deleted = repo.Delete(id);
                if (deleted) return Ok(new { message = "Baggage deleted successfully." });
                return BadRequest("Failed to delete baggage.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        // ─────────────────────────────────────────
        // OTHERSDETAILS
        // ─────────────────────────────────────────

        [HttpGet]
        public ActionResult<IEnumerable<OthersDetails>> GetAllOthersDetails()
        {
            try
            {
                var repo = new OthersDetailsRepository(_configuration);
                return Ok(repo.GetAll());
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("{id}")]
        public ActionResult<OthersDetails> GetOthersDetailsByID(int id)
        {
            try
            {
                var repo = new OthersDetailsRepository(_configuration);
                var item = repo.GetByID(id);
                if (item == null) return NotFound($"OthersDetails with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpGet("booking/{id}")]
        public ActionResult<IEnumerable<OthersDetails>> GetOthersDetailsByBookingID(int id)
        {
            try
            {
                var repo = new OthersDetailsRepository(_configuration);
                var items = repo.GetByBookingID(id);
                if (items == null || !items.Any())
                    return NotFound($"No OthersDetails found for BookingID {id}.");
                return Ok(items);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPost]
        public ActionResult CreateOthersDetails([FromBody] OthersDetails item)
        {
            try
            {
                if (item == null) return BadRequest("OthersDetails data is required.");
                var repo = new OthersDetailsRepository(_configuration);
                bool created = repo.Add(item);
                if (created) return Ok(new { message = "OthersDetails created successfully." });
                return BadRequest("Failed to create OthersDetails.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOthersDetails(int id, [FromBody] OthersDetails item)
        {
            try
            {
                if (item == null) return BadRequest("OthersDetails data is required.");
                var repo = new OthersDetailsRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"OthersDetails with ID {id} not found.");
                item.OthersID = id;
                bool updated = repo.Update(item);
                if (updated) return Ok(new { message = "OthersDetails updated successfully." });
                return BadRequest("Failed to update OthersDetails.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOthersDetails(int id)
        {
            try
            {
                var repo = new OthersDetailsRepository(_configuration);
                var existing = repo.GetByID(id);
                if (existing == null) return NotFound($"OthersDetails with ID {id} not found.");
                bool deleted = repo.Delete(id);
                if (deleted) return Ok(new { message = "OthersDetails deleted successfully." });
                return BadRequest("Failed to delete OthersDetails.");
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }
    }
}
