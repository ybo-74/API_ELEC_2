using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;
using Microsoft.Extensions.Configuration;

namespace API_ELEC_2.Repositories
{
    public class FlightRepository
    {
        private readonly string _connectionString;

        public FlightRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            var flights = new List<Flight>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                SELECT 
                    f.FlightID,
                    f.OriginID,
                    f.DestinationID,
                    f.AirlineID,
                    f.TravelDate,
                    f.TravelTime,
                    a.AirlineCode,
                    a.Description AS AirlineName,
                    o.Code AS OriginCode,
                    o.Description AS OriginName,
                    d.Code AS DestinationCode,
                    d.Description AS DestinationName
                FROM FD_Details f
                JOIN FD_Airline a ON f.AirlineID = a.AirlineID
                JOIN FD_OtherDetails o ON f.OriginID = o.OtherDetailsID
                JOIN FD_OtherDetails d ON f.DestinationID = d.OtherDetailsID";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flights.Add(new Flight
                        {
                            FlightID = reader.GetInt32(reader.GetOrdinal("FlightID")),
                            OriginID = reader.GetInt32(reader.GetOrdinal("OriginID")),
                            DestinationID = reader.GetInt32(reader.GetOrdinal("DestinationID")),
                            AirlineID = reader.GetInt32(reader.GetOrdinal("AirlineID")),
                            TravelDate = reader.GetDateTime(reader.GetOrdinal("TravelDate")),
                            TravelTime = reader["TravelTime"]?.ToString() ?? string.Empty,
                            AirlineCode = reader["AirlineCode"]?.ToString() ?? string.Empty,
                            AirlineName = reader["AirlineName"]?.ToString() ?? string.Empty,
                            OriginCode = reader["OriginCode"]?.ToString() ?? string.Empty,
                            OriginName = reader["OriginName"]?.ToString() ?? string.Empty,
                            DestinationCode = reader["DestinationCode"]?.ToString() ?? string.Empty,
                            DestinationName = reader["DestinationName"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }

            return flights;
        }

        public void AddFlight(Flight flight)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                INSERT INTO FD_Details (OriginID, DestinationID, TravelTime, TravelDate, AirlineID)
                VALUES (@OriginID, @DestinationID, @TravelTime, @TravelDate, @AirlineID)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OriginID", flight.OriginID);
                    command.Parameters.AddWithValue("@DestinationID", flight.DestinationID);
                    command.Parameters.AddWithValue("@TravelTime", flight.TravelTime);
                    command.Parameters.AddWithValue("@TravelDate", flight.TravelDate);
                    command.Parameters.AddWithValue("@AirlineID", flight.AirlineID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFlight(Flight flight)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                UPDATE FD_Details
                SET OriginID = @OriginID,
                    DestinationID = @DestinationID,
                    TravelTime = @TravelTime,
                    TravelDate = @TravelDate,
                    AirlineID = @AirlineID
                WHERE FlightID = @FlightID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID", flight.FlightID);
                    command.Parameters.AddWithValue("@OriginID", flight.OriginID);
                    command.Parameters.AddWithValue("@DestinationID", flight.DestinationID);
                    command.Parameters.AddWithValue("@TravelTime", flight.TravelTime);
                    command.Parameters.AddWithValue("@TravelDate", flight.TravelDate);
                    command.Parameters.AddWithValue("@AirlineID", flight.AirlineID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFlight(int flightID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM FD_Details WHERE FlightID = @FlightID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID", flightID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
