using API_ELEC_2.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace API_ELEC_2.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;
        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Employee> GetAllEmployees() {
        
        }
    }
}
