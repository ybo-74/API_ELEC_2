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

        //get all employees
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        employeeID = Convert.ToInt32(reader["employeeID"]),
                        firstName = reader["firstName"].ToString(),
                        lastName = reader["lastName"].ToString(),
                        position = reader["position"].ToString(),
                        birthDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["birthDate"]))
                    });
                }

            } return employees; 
        }


        //get employee by id
        public Employee GetEmployeeID(int id)
        {
            Employee employee = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM employees WHERE employeeID = @employeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@employeeID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    employee= new Employee
                    {
                        employeeID = Convert.ToInt32(reader["employeeID"]),
                        firstName = reader["firstName"].ToString(),
                        lastName = reader["lastName"].ToString(),
                        position = reader["position"].ToString(),
                        birthDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["birthDate"]))
                    };
                } if (employee == null) {
                    throw new Exception($"Employee with ID {id} not found.");
                   }
            } return employee;
        }


        //create new employee
        public bool CreateEmployee(Employee employee)
        {
            using (SqlConnection conn= new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO employees (employeeID, firstName, lastName, position, birthDate) 
                VALUES (@employeeID, @firstName, @lastName, @position, @birthDate)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@employeeID", employee.employeeID);
                cmd.Parameters.AddWithValue("@firstName", employee.firstName);
                cmd.Parameters.AddWithValue("@lastName", employee.lastName);
                cmd.Parameters.AddWithValue("@position", employee.position);
                cmd.Parameters.AddWithValue("@birthDate", employee.birthDate.ToDateTime(TimeOnly.MinValue));

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


        //update employee
        public bool UpdateEmployee(int id, Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE employees SET firstName = @firstName, lastName = @lastName, position = @position, birthDate = @birthDate 
                WHERE employeeID = @employeeID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@employeeID", id);
                cmd.Parameters.AddWithValue("@firstName", employee.firstName);
                cmd.Parameters.AddWithValue("@lastName", employee.lastName);
                cmd.Parameters.AddWithValue("@position", employee.position);
                cmd.Parameters.AddWithValue("@birthDate", employee.birthDate.ToDateTime(TimeOnly.MinValue));

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        //delete empliyee
        public bool DeleteEmployee(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM employees WHERE employeeID = @employeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@employeeID", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }
}
