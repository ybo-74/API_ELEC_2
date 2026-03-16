using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class OthersBaggageRepository
    {
        private readonly string _connectionString;

        public OthersBaggageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<OthersBaggage> GetAllBaggage()
        {
            var list = new List<OthersBaggage>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Weight FROM OthersBaggage", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new OthersBaggage { ID = (int)reader["ID"], Weight = Convert.ToDecimal(reader["Weight"]) });
                }
            }
            return list;
        }

        public OthersBaggage GetByID(int id)
        {
            OthersBaggage item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Weight FROM OthersBaggage WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = new OthersBaggage { ID = (int)reader["ID"], Weight = Convert.ToDecimal(reader["Weight"]) };
                    }
                }
            }
            return item;
        }

        public bool Add(OthersBaggage baggage)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO OthersBaggage (Weight) VALUES (@Weight)", connection))
                {
                    command.Parameters.AddWithValue("@Weight", baggage.Weight);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(OthersBaggage baggage)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE OthersBaggage SET Weight = @Weight WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", baggage.ID);
                    command.Parameters.AddWithValue("@Weight", baggage.Weight);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM OthersBaggage WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
