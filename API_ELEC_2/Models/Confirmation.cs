namespace API_ELEC_2.Models
{
    public class Confirmation
    {
        public int ConfirmationID { get; set; }
        public int BookingID { get; set; }
        public int FlightID { get; set; }
        public string ConfirmationCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ConfirmationDate { get; set; }
    }

    public class CreateConfirmationRequest
    {
        public int BookingID { get; set; }
        public int FlightID { get; set; }
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
