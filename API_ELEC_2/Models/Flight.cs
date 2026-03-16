namespace API_ELEC_2.Models
{
    public class Flight
    {
        public int FlightID { get; set; }
        public int OriginID { get; set; }
        public int DestinationID { get; set; }
        public int AirlineID { get; set; }
        public DateTime TravelDate { get; set; }
        public string TravelTime { get; set; } = string.Empty;
        public int MaxPax { get; set; }
        public int CurrentPax { get; set; }

        // JOIN fields
        public string AirlineCode { get; set; } = string.Empty;
        public string AirlineName { get; set; } = string.Empty;
        public string OriginCode { get; set; } = string.Empty;
        public string OriginName { get; set; } = string.Empty;
        public string DestinationCode { get; set; } = string.Empty;
        public string DestinationName { get; set; } = string.Empty;
    }
}
