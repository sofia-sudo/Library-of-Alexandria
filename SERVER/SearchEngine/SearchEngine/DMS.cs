using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SearchEngine
{
    public static class DMS
    {
        private static MySqlConnection conn;

        static DMS()
        {
            // Prepare connection string
            string connectionString = "server=localhost;uid=LOA;pwd=LOAPSWD;database=LibraryOfAlexandria;SslMode=none";

            // Connect to database
            conn = new MySql.Data.MySqlClient.MySqlConnection(); // Establish connection
            conn.ConnectionString = connectionString; // Feed string containing credentials and db name
            conn.Open(); // Open connection

        }

        public static List<Result> Query(string _query, bool exact=true)
        {
            List<Result> results = new List<Result>();
            string query = ""; ;
            int hits = 0;

            // Generate SQL query
            if (exact)
            {
                query = $"SELECT * FROM core WHERE TITLE LIKE '%{_query}%'" +
                                                 $" OR METADESC LIKE '%{_query}%'";
            }
            else
            {
                // Filter out filler words // foreach word append 'OR TITLE LIKE %VAR%'
            }

            // Query the DB
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                results.Add(new Result(dataReader.GetInt32(0), dataReader.GetString(2), 
                    dataReader.GetString(1), dataReader.GetString(3)));

                hits++;
            }

            dataReader.Close();

            return (results);
        }
    }
}
