using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate JOIN Room ON Roommate.RoomId = Room.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                            string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                            int rentPortionValue = reader.GetInt32(reader.GetOrdinal("RentPortion"));

                            Room room = new Room
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                RentPortion = rentPortionValue,
                                Room = room
                            };

                            roommates.Add(roommate);
                        }

                        return roommates;
                    }
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.FirstName, Roommate.LastName, Roommate.RentPortion, Room.Name FROM Room JOIN Roommate ON Roommate.RoomId = Room.Id WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            Room room = new Room
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = room
                            };
                        }
                        return roommate;
                    }

                }
            }
        }


    }
}