using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    // this interacts with chore data & uses BaseRepository's connection property via inheritance
    public class ChoreRepository : BaseRepository
    {
        // passes connection string to BaseRepository during instantiation
        public ChoreRepository(string connectionString) : base(connectionString) { }

        // returns a single chore with the given id
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        // add a new chore to the database
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                        OUTPUT INSERTED.Id
                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
        // get all of the chores in the db (as a list)
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // list that holds the chores retrieved from db
                        List<Chore> chores = new List<Chore>();

                        // Read() will return true if there's more data to read
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            // creates a new chore object with data from the db
                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue
                            };

                            // add the chore object to the list
                            chores.Add(chore);
                        }
                        // return the list of chores
                        return chores;
                    }
                }
            }
        }
    }
}
