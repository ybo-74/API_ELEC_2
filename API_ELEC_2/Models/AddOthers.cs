namespace API_ELEC_2.Models
{
    public class AddOthers
    {
        public int AddOthersID { get; set; }
        public int BookingID { get; set; }
        public int PaxID { get; set; }
        public int MealID { get; set; }
        public int SeatID { get; set; }
        public int BaggageID { get; set; }
        public int InsuranceID { get; set; }
        public bool IsMeal { get; set; }
        public bool IsSeat { get; set; }
        public bool IsBaggage { get; set; }
        public bool IsInsurance { get; set; }
        public bool IsActive { get; set; }
    }
}
