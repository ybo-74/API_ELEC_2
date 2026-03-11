namespace API_ELEC_2.Models
{
    public class Booking_Pay
    {
        public int PayID { get; set; }
        public int BookingID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public DateOnly Birthdate { get; set; }
        public int Age { get; set; }
    }
}