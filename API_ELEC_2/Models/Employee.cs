namespace API_ELEC_2.Models
{
    public class Employee
    {
        public int employeeID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string position { get; set; }
        public DateOnly birthDate { get; set; }
    }
}
