namespace API_ELEC_2.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int FlightID { get; set; }
        public DateTime BookingDate { get; set; }
    }

    public class BookingPax
    {
        public int PaxID { get; set; }
        public int BookingID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }
    }

    // GET /api/bookings/available
    public class AvailableFlight
    {
        public int FlightID { get; set; }
        public string OriginCode { get; set; } = string.Empty;
        public string OriginName { get; set; } = string.Empty;
        public string DestinationCode { get; set; } = string.Empty;
        public string DestinationName { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public string TravelTime { get; set; } = string.Empty;
        public string AirlineCode { get; set; } = string.Empty;
        public int SeatsLeft { get; set; }
    }

    // GET /api/bookings/details/{bookingId}
    public class BookingDetail
    {
        public int BookingID { get; set; }
        public int PaxID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public int FlightID { get; set; }
        public DateTime TravelDate { get; set; }
        public string TravelTime { get; set; } = string.Empty;
    }

    // POST /api/bookings request body
    public class CreateBookingRequest
    {
        public int FlightID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }
    }

    // PUT /api/bookings/{id}/transfer
    public class TransferRequest
    {
        public int NewFlightID { get; set; }
    }
}
