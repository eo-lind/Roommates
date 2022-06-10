using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;

namespace Roommates.Repositories
{
    ///  This class is responsible for interacting with Roommate data.
        public class RoommateRepository : BaseRepository
    {
        ///  When new RoommateRepository is instantiated, pass the connection string along to the BaseRepository
        public RoommateRepository(string connectionString) : base(connectionString) { }

        ///  Returns a single roommate with the given id.
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            };
                        }
                        return roommate;
                    }

                }
            }

        }

        ///  Add a new roommate to the database
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, Room) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@firstName, @lastName, @rentPortion, @moveInDate, @room)";
                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@room", roommate.Room);
                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;
                }
            }

            // when this method is finished we can look in the database and see the new room.
        }

        ///  Get a list of all Roommates in the database
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // A list to hold the rooms we retrieve from the database.
                        List<Roommate> roommates = new List<Roommate>();

                        // Read() will return true if there's more data to read
                        while (reader.Read())
                        {
                            // The "ordinal" is the numeric position of the column in the query results.
                            //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                            int idColumnPosition = reader.GetOrdinal("Id");

                            // We user the reader's GetXXX methods to get the value for a particular ordinal.
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                            int rentPortion = reader.GetInt32(rentPortionColumnPosition);

                            //DateTime moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                            //DateTime moveInDate = reader.GetDateTime(moveInDateColumnPosition);

                            // Now let's create a new roommate object using the data from the database.
                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                RentPortion = rentPortion,
                                //MoveInDate
                            };

                            // ...and add that room object to our list.
                            roommates.Add(roommate);
                        }
                        // Return the list of roommates who whomever called this method.
                        return roommates;
                    }

                }
            }
        }
    }
}