using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;
using MySql.Data;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;

//https://en.wikipedia.org/wiki/Special:Random

namespace SearchEngine_Crawler
{

    static class Program
    {
        // Operational parameters
        private static int delay = 10000;           // Delay between passes in milliseconds to respect site traffic
        private static bool wikipediaEnabled = true;// Should the program index Wikipedia domains?
        private static bool reutersEnabled = true;  // Should the program index Reuters domains?
        private static bool bbcEnabled = true;      // Should the program index BBC domains?
        private static int cacheLimit = 2048;       // Limits the size of cache for memory management

        // System variables
        private static MySqlConnection conn;                     // Connection to database
        public static Queue<string> cache = new Queue<String>(); // Caches sites already indexed
        public static int siteCount = 0;                         // Tracks the number of sites indexed

        static void Main(string[] args)
        {
            #region INTRODUCTORY TEXT
            Console.WriteLine("===========================");
            Console.WriteLine("===     WEB CRAWLER     ===");
            Console.WriteLine("== BY SOFIA JEANNE RAINS ==");
            Console.WriteLine("=  LIBRARY OF ALEXANDRIA  =");
            Console.WriteLine("===========================");
            Console.WriteLine("======= MAIN  MENU ========\n");
            #endregion

            #region CONNECT TO DATABASE
            // Prepare connection string
            string connectionString = "server=localhost;uid=LOA;pwd=LOAPSWD;database=LibraryOfAlexandria;SslMode=none";

            // Error handling to avoid crash in event of failed connection
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(); // Establish connection
                conn.ConnectionString = connectionString; // Feed string containing credentials and db name
                conn.Open(); // Open connection

                Console.WriteLine("Connected to Database successfully!");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("!!!  Failed to connect to Database  !!!"); // Alert user that database has not been reached
                Console.WriteLine("Press 'E' to view error logs."); // Prompt for error logs
                var a = Console.ReadKey();
                
                // Print error logs
                if (a.KeyChar == 'e')
                    Console.WriteLine(ex.ToString());
            }
            #endregion

            #region ESTABLISH PARAMS
            // Display default configuration
            Console.WriteLine($"Delay: {delay / 100} seconds average between queries.");

            // Prompt to change passrate/delay
            Console.WriteLine("To use custom config, enter Y. Press any other key to continue with default config.");
            var input = Console.ReadKey().KeyChar;

            // If the user wants to change passrate/delay...
            if (input.ToString().ToUpper() == "Y")
            {
                // ...prompt to change it
                Console.WriteLine("Enter delay (s): ");
                delay = Int32.Parse(Console.ReadLine()) * 100;
            }
            #endregion

            #region SETUP
            // Establish the domain crawlers
            Wikipedia wikipedia = new Wikipedia();
            Reuters reuters = new Reuters();
            Bbc bbc = new Bbc();

            // Iteration counter
            int i = 0;
            #endregion

            #region ITERATIVE OPERATION
            // Loop the program
            while (true)
            {
                i++;
                // Clear the console and update UI
                Console.Clear();
                Console.WriteLine("========== SYSTEM ===========");
                Console.WriteLine($"SYSTEM STATE    | Iteration: {i}");
                Console.WriteLine($"SYSTEM STATE    | Total queue length: {(wikipedia.queue.Count + reuters.queue.Count + bbc.queue.Count)}");
                Console.WriteLine($"SYSTEM STATE    | RAM Cache usage: {cache.Count} / {cacheLimit}");

                // Crawl each domain
                if (wikipediaEnabled)
                    wikipedia.Crawl(0);
                if (reutersEnabled)
                    reuters.Crawl(0);
                if (bbcEnabled)
                    bbc.Crawl(0);

                // Delay the next pass to avoid DoS on target web server
                Console.WriteLine("========== DELAY ============");
                Console.Write("SYSTEM STATE    | DELAY BEFORE NEXT QUERY: ");

                using (var progress = new ProgressBar())
                {
                    for (int x = 0; x <= 100; x++)
                    {
                        progress.Report((double)x / 100);
                        Thread.Sleep(delay / 100);
                    }
                }

                // Progress report every 10 iterations
                if((i % 10) == 0)
                {
                    Console.Clear();
                    Console.WriteLine("========== SYSTEM ===========");
                    Console.WriteLine($"SYSTEM STATE    | Iteration: {i}");
                    Console.WriteLine($"SYSTEM STATE    | Total queue length: {(wikipedia.queue.Count + reuters.queue.Count + bbc.queue.Count)}");
                    if (bbcEnabled)
                        Console.WriteLine($"SYSTEM STATE    | BBC queue length: {(bbc.queue.Count)}");
                    if (reutersEnabled)
                        Console.WriteLine($"SYSTEM STATE    | Reuters queue length: {(reuters.queue.Count)}");
                    if (wikipediaEnabled)    
                        Console.WriteLine($"SYSTEM STATE    | Wikipedia queue length: {(wikipedia.queue.Count)}");
                    Console.WriteLine($"SYSTEM STATE    | RAM Cache usage: {Program.cache.Count} / 2048");

                    // Give user time to read the previous statements
                    using (var progress = new ProgressBar())
                    {
                        for (int x = 0; x <= 100; x++)
                        {
                            progress.Report((double)x / 100);
                            Thread.Sleep(delay / 100);
                        }
                    }

                    Console.WriteLine("========== QUEUE ============");
                    foreach (var link in wikipedia.queue)
                    {
                        Console.WriteLine($"WIKIPEDIA       | {link}");
                    }

                    foreach (var link in bbc.queue)
                    {
                        Console.WriteLine($"BBC UK          | {link}");
                    }

                    foreach (var link in reuters.queue)
                    {
                        Console.WriteLine($"REUTERS INTL    | {link}");
                    }

                    using (var progress = new ProgressBar())
                    {
                        for (int x = 0; x <= 100; x++)
                        {
                            progress.Report((double)x / 100);
                            Thread.Sleep(delay / 100);
                        }
                    }
                }

            }
            #endregion
        }

        public static void IndexSite(string URL, string title, string metadesc)
        {
            // Search database for site
            string query = $"SELECT * FROM core WHERE URL='{URL}'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            // If site is not already indexed...
            if (!dataReader.Read())
            {
                // Close dataReader to free up connection for next query
                dataReader.Close();

                // ... Load URL, Keywords, Title into database
                query = $"INSERT INTO core (URL, Title, Metadesc) VALUES('{URL}', '{title}', '{metadesc}')";
                cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                // Cache site to prevent it from being indexed again shortly.

            }
            else
            {
                // Close dataReader to free up connection for next subroutine call
                dataReader.Close();
            }
        }

        // Caches site to Queue cache - if the queue is too long it will replace the eldest entry.
        public static void Cache(string URL)
        {
            // Dequeue if count exceeds limit
            if (cache.Count >= cacheLimit)
            {
                cache.Dequeue();
            }

            // Append the new URL
            cache.Enqueue(URL);
        }



    }
}
