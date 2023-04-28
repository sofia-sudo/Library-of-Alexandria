using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using HtmlAgilityPack;

namespace SearchEngine_Crawler
{
    public abstract class Crawler
    {
        public abstract string homeURL { get; set; }                // The default URL for this crawler
        public abstract List<String> bannedDomains { get; set; }    // List of all banned domains

        public string currentURL;                                   // Current URL loaded into system
        public string title;                                        // Title of current URL
        public string metadesc;
        public HtmlDocument pageContent;                            // HTMLDoc containing all HTML of current page
        public Queue<string> queue = new Queue<String>();           // URL queue - to crawl

        // Constructor
        public Crawler() {}

        // Crawls X number of sites from the queue
        public void Crawl(int passes)
        {
            for(int i=0; i <= passes; i++) // Loop for x passes
            {
                // If queue is empty...
                if (queue.Count < 1)
                {
                    // ... append the home URL
                    queue.Enqueue(homeURL);
                }

                // Load the next URL
                NextURL();

                // Extract relevant data
                ExtractHyperlinks();
                ExtractMetadesc();

                // Index the site if it's not the home URL (this may not be indexable)
                if (currentURL != homeURL)
                    Program.IndexSite(currentURL, title, metadesc);

                
            }
        }

        // Moves on to the next URL and loads it
        public void NextURL()
        {
            // Pop the next URL from the queue
            currentURL = queue.Dequeue();
            Program.siteCount++;

            // Check cache for this URL - has it already been indexed?
            if (Program.cache.Contains(currentURL))
            {
                NextURL(); // Recursively call NextURL() to get a new URL
                return; // Break;
            }

            // Cache this URL
            Program.Cache(currentURL);

            // Load the content of the site into pageContent
            try
            {
                HtmlWeb web = new HtmlWeb();
                pageContent = web.Load(currentURL);
            }
            catch (Exception e)
            {
                // Invalid URL
                NextURL();
                return;
            }

            // Update UI
            switch (homeURL)
            {
                case "https://www.reuters.com/":
                    Console.WriteLine("========== REUTERS ==========");
                    break;
                case "https://www.bbc.co.uk/news":
                    Console.WriteLine("============ BBC ============");
                    break;
                case "https://en.wikipedia.org/wiki/Special:Random":
                    Console.WriteLine("========= WIKIPEDIA =========");
                    break;
                default:
                    Console.WriteLine("========== DOMAIN ===========");
                    break;
            }
            Console.WriteLine($"NOW SCRAPING    | URL {Program.siteCount}: {currentURL}");

            // Extract title of web page
            title = pageContent.DocumentNode.SelectSingleNode("//title").InnerText;
            Console.WriteLine($"NOW SCRAPING    | {title}");
        }

        public void QCCheck()
        {
            // Safeguard Logic here
        }

        // These voids are left blank as they will be specific to the site
        // The crawler will navigate each site in a different manner, prioritising different links
        // Each child of inheritance will overwrite these with an appropriate subroutine
        public abstract void ExtractHyperlinks();
        public abstract void ExtractMetadesc();

    }
}
