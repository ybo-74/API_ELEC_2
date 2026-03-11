using Microsoft.AspNetCore.Mvc;
using API_ELEC_2.Models;
using API_ELEC_2.Repositories;

namespace API_ELEC_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingRepository _bookingRepository;

        public BookingsController(IConfiguration configuration)
        {
            _bookingRepository = new BookingRepository(configuration);
        }

        // ==================== BOOKING_DETAILS ====================

        // GET api/bookings
        [HttpGet]
        public ActionResult<IEnumerable<Booking_Details>> GetAllBookings()
        {
            try
            {
                var bookings = _bookingRepository.GetAllBookings();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/5
        [HttpGet("{id}")]
        public ActionResult<Booking_Details> GetBookingByID(int id)
        {
            try
            {
                var booking = _bookingRepository.GetBookingByID(id);
                if (booking == null)
                    return NotFound($"Booking with ID {id} not found.");
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/bookings
        [HttpPost]
        public ActionResult CreateBooking([FromBody] Booking_Details booking)
        {
            try
            {
                if (booking == null)
                    return BadRequest("Booking data is required.");
                bool isCreated = _bookingRepository.CreateBooking(booking);
                if (isCreated)
                    return Ok("Booking created successfully.");
                return BadRequest("Failed to create booking.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/bookings/5
        [HttpPut("{id}")]
        public ActionResult UpdateBooking(int id, [FromBody] Booking_Details booking)
        {
            try
            {
                if (booking == null)
                    return BadRequest("Booking data is required.");
                var existing = _bookingRepository.GetBookingByID(id);
                if (existing == null)
                    return NotFound($"Booking with ID {id} not found.");
                bool isUpdated = _bookingRepository.UpdateBooking(id, booking);
                if (isUpdated)
                    return Ok("Booking updated successfully.");
                return BadRequest("Failed to update booking.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/bookings/5
        [HttpDelete("{id}")]
        public ActionResult DeleteBooking(int id)
        {
            try
            {
                var existing = _bookingRepository.GetBookingByID(id);
                if (existing == null)
                    return NotFound($"Booking with ID {id} not found.");
                bool isDeleted = _bookingRepository.DeleteBooking(id);
                if (isDeleted)
                    return Ok("Booking deleted successfully.");
                return BadRequest("Failed to delete booking.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ==================== BOOKING_PAY ====================

        // GET api/bookings/pay
        [HttpGet("pay")]
        public ActionResult<IEnumerable<Booking_Pay>> GetAllBookingPay()
        {
            try
            {
                var list = _bookingRepository.GetAllBookingPay();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/pay/5
        [HttpGet("pay/{id}")]
        public ActionResult<Booking_Pay> GetBookingPayByID(int id)
        {
            try
            {
                var pay = _bookingRepository.GetBookingPayByID(id);
                if (pay == null)
                    return NotFound($"Booking Pay with ID {id} not found.");
                return Ok(pay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/5/pay
        [HttpGet("{bookingId}/pay")]
        public ActionResult<IEnumerable<Booking_Pay>> GetBookingPayByBookingID(int bookingId)
        {
            try
            {
                var list = _bookingRepository.GetBookingPayByBookingID(bookingId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/bookings/pay
        [HttpPost("pay")]
        public ActionResult CreateBookingPay([FromBody] Booking_Pay pay)
        {
            try
            {
                if (pay == null)
                    return BadRequest("Booking Pay data is required.");
                bool isCreated = _bookingRepository.CreateBookingPay(pay);
                if (isCreated)
                    return Ok("Booking Pay created successfully.");
                return BadRequest("Failed to create booking pay.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/bookings/pay/5
        [HttpPut("pay/{id}")]
        public ActionResult UpdateBookingPay(int id, [FromBody] Booking_Pay pay)
        {
            try
            {
                if (pay == null)
                    return BadRequest("Booking Pay data is required.");
                var existing = _bookingRepository.GetBookingPayByID(id);
                if (existing == null)
                    return NotFound($"Booking Pay with ID {id} not found.");
                bool isUpdated = _bookingRepository.UpdateBookingPay(id, pay);
                if (isUpdated)
                    return Ok("Booking Pay updated successfully.");
                return BadRequest("Failed to update booking pay.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/bookings/pay/5
        [HttpDelete("pay/{id}")]
        public ActionResult DeleteBookingPay(int id)
        {
            try
            {
                var existing = _bookingRepository.GetBookingPayByID(id);
                if (existing == null)
                    return NotFound($"Booking Pay with ID {id} not found.");
                bool isDeleted = _bookingRepository.DeleteBookingPay(id);
                if (isDeleted)
                    return Ok("Booking Pay deleted successfully.");
                return BadRequest("Failed to delete booking pay.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ==================== BOOKING_OTHERS ====================

        // GET api/bookings/others
        [HttpGet("others")]
        public ActionResult<IEnumerable<Booking_Others>> GetAllBookingOthers()
        {
            try
            {
                var list = _bookingRepository.GetAllBookingOthers();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/others/5
        [HttpGet("others/{id}")]
        public ActionResult<Booking_Others> GetBookingOthersByID(int id)
        {
            try
            {
                var item = _bookingRepository.GetBookingOthersByID(id);
                if (item == null)
                    return NotFound($"Booking Others with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/bookings/others
        [HttpPost("others")]
        public ActionResult CreateBookingOthers([FromBody] Booking_Others others)
        {
            try
            {
                if (others == null)
                    return BadRequest("Booking Others data is required.");
                bool isCreated = _bookingRepository.CreateBookingOthers(others);
                if (isCreated)
                    return Ok("Booking Others created successfully.");
                return BadRequest("Failed to create booking others.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/bookings/others/5
        [HttpPut("others/{id}")]
        public ActionResult UpdateBookingOthers(int id, [FromBody] Booking_Others others)
        {
            try
            {
                if (others == null)
                    return BadRequest("Booking Others data is required.");
                var existing = _bookingRepository.GetBookingOthersByID(id);
                if (existing == null)
                    return NotFound($"Booking Others with ID {id} not found.");
                bool isUpdated = _bookingRepository.UpdateBookingOthers(id, others);
                if (isUpdated)
                    return Ok("Booking Others updated successfully.");
                return BadRequest("Failed to update booking others.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/bookings/others/5
        [HttpDelete("others/{id}")]
        public ActionResult DeleteBookingOthers(int id)
        {
            try
            {
                var existing = _bookingRepository.GetBookingOthersByID(id);
                if (existing == null)
                    return NotFound($"Booking Others with ID {id} not found.");
                bool isDeleted = _bookingRepository.DeleteBookingOthers(id);
                if (isDeleted)
                    return Ok("Booking Others deleted successfully.");
                return BadRequest("Failed to delete booking others.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ==================== BOOKING_OTHER_DETAILS ====================

        // GET api/bookings/otherdetails
        [HttpGet("otherdetails")]
        public ActionResult<IEnumerable<Booking_OtherDetails>> GetAllBookingOtherDetails()
        {
            try
            {
                var list = _bookingRepository.GetAllBookingOtherDetails();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/otherdetails/5
        [HttpGet("otherdetails/{id}")]
        public ActionResult<Booking_OtherDetails> GetBookingOtherDetailsByID(int id)
        {
            try
            {
                var item = _bookingRepository.GetBookingOtherDetailsByID(id);
                if (item == null)
                    return NotFound($"Booking Other Details with ID {id} not found.");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/bookings/5/otherdetails
        [HttpGet("{bookingId}/otherdetails")]
        public ActionResult<IEnumerable<Booking_OtherDetails>> GetBookingOtherDetailsByBookingID(int bookingId)
        {
            try
            {
                var list = _bookingRepository.GetBookingOtherDetailsByBookingID(bookingId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/bookings/otherdetails
        [HttpPost("otherdetails")]
        public ActionResult CreateBookingOtherDetails([FromBody] Booking_OtherDetails details)
        {
            try
            {
                if (details == null)
                    return BadRequest("Booking Other Details data is required.");
                bool isCreated = _bookingRepository.CreateBookingOtherDetails(details);
                if (isCreated)
                    return Ok("Booking Other Details created successfully.");
                return BadRequest("Failed to create booking other details.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/bookings/otherdetails/5
        [HttpPut("otherdetails/{id}")]
        public ActionResult UpdateBookingOtherDetails(int id, [FromBody] Booking_OtherDetails details)
        {
            try
            {
                if (details == null)
                    return BadRequest("Booking Other Details data is required.");
                var existing = _bookingRepository.GetBookingOtherDetailsByID(id);
                if (existing == null)
                    return NotFound($"Booking Other Details with ID {id} not found.");
                bool isUpdated = _bookingRepository.UpdateBookingOtherDetails(id, details);
                if (isUpdated)
                    return Ok("Booking Other Details updated successfully.");
                return BadRequest("Failed to update booking other details.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/bookings/otherdetails/5
        [HttpDelete("otherdetails/{id}")]
        public ActionResult DeleteBookingOtherDetails(int id)
        {
            try
            {
                var existing = _bookingRepository.GetBookingOtherDetailsByID(id);
                if (existing == null)
                    return NotFound($"Booking Other Details with ID {id} not found.");
                bool isDeleted = _bookingRepository.DeleteBookingOtherDetails(id);
                if (isDeleted)
                    return Ok("Booking Other Details deleted successfully.");
                return BadRequest("Failed to delete booking other details.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}