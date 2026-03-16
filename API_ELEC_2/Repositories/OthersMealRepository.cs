using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class OthersMealRepository
    {
        private readonly string _connectionString;

        public OthersMealRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<OthersMeal> GetAllMeals()
        {
            var list = new List<OthersMeal>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Type FROM OthersMeal", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new OthersMeal { ID = (int)reader["ID"], Type = reader["Type"].ToString() ?? string.Empty });
                }
            }
            return list;
        }

        public OthersMeal GetByID(int id)
        {
            OthersMeal item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Type FROM OthersMeal WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = new OthersMeal { ID = (int)reader["ID"], Type = reader["Type"].ToString() ?? string.Empty };
                    }
                }
            }
            return item;
        }

        public bool Add(OthersMeal meal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO OthersMeal (Type) VALUES (@Type)", connection))
                {
                    command.Parameters.AddWithValue("@Type", meal.Type);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(OthersMeal meal)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE OthersMeal SET Type = @Type WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", meal.ID);
                    command.Parameters.AddWithValue("@Type", meal.Type);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM OthersMeal WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
