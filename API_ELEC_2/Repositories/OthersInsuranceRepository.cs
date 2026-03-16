using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class OthersInsuranceRepository
    {
        private readonly string _connectionString;

        public OthersInsuranceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<OthersInsurance> GetAllInsurance()
        {
            var list = new List<OthersInsurance>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Type FROM OthersInsurance", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new OthersInsurance { ID = (int)reader["ID"], Type = reader["Type"].ToString() ?? string.Empty });
                }
            }
            return list;
        }

        public OthersInsurance GetByID(int id)
        {
            OthersInsurance item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Type FROM OthersInsurance WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = new OthersInsurance { ID = (int)reader["ID"], Type = reader["Type"].ToString() ?? string.Empty };
                    }
                }
            }
            return item;
        }

        public bool Add(OthersInsurance insurance)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO OthersInsurance (Type) VALUES (@Type)", connection))
                {
                    command.Parameters.AddWithValue("@Type", insurance.Type);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(OthersInsurance insurance)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE OthersInsurance SET Type = @Type WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", insurance.ID);
                    command.Parameters.AddWithValue("@Type", insurance.Type);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM OthersInsurance WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
