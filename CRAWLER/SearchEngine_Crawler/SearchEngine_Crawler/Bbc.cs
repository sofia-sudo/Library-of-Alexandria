using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SearchEngine_Crawler
{
    public class Bbc : Crawler
    {
        public override string homeURL { get; set; }
        public override List<String> bannedDomains { get; set; }

        public List<String> approvedDomains;

        public Bbc()
        {
            this.homeURL = "https://www.bbc.co.uk/news";

            #region DOMAIN_FILTERS
            // Define the banned domains
            bannedDomains = new List<String>();

            // Forbidden by Robots.Txt
            bannedDomains.Add("/bitesize/search$");
            bannedDomains.Add("/bitesize/search/");
            bannedDomains.Add("/bitesize/search?");
            bannedDomains.Add("/cbbc/search$");
            bannedDomains.Add("/cbbc/search/");
            bannedDomains.Add("/cbbc/search?");
            bannedDomains.Add("/cbeebies/search$");
            bannedDomains.Add("/cbeebies/search/");
            bannedDomains.Add("/cbeebies/search?");
            bannedDomains.Add("/chwilio/");
            bannedDomains.Add("/chwilio$");
            bannedDomains.Add("/chwilio?");
            bannedDomains.Add("/iplayer/bigscreen/");
            bannedDomains.Add("/iplayer/cbbc/episodes/");
            bannedDomains.Add("/iplayer/cbbc/search");
            bannedDomains.Add("/iplayer/cbeebies/episodes/");
            bannedDomains.Add("/iplayer/cbeebies/search");
            bannedDomains.Add("/iplayer/search");
            bannedDomains.Add("/indepthtoolkit/smallprox$");
            bannedDomains.Add("/indepthtoolkit/smallprox/");
            bannedDomains.Add("/modules/musicnav/language/");
            bannedDomains.Add("/news/0");
            bannedDomains.Add("/radio/aod/");
            bannedDomains.Add("/radio/aod$");
            bannedDomains.Add("/radio/imda");
            bannedDomains.Add("/radio/player/");
            bannedDomains.Add("/radio/player$");
            bannedDomains.Add("/search/");
            bannedDomains.Add("/search$");
            bannedDomains.Add("/search?");
            bannedDomains.Add("/sport/videos/*");
            bannedDomains.Add("/sounds/player/");
            bannedDomains.Add("/sounds/player$");
            bannedDomains.Add("/ugc$");
            bannedDomains.Add("/ugc/");
            bannedDomains.Add("/ugcsupport$");
            bannedDomains.Add("/ugcsupport/");
            bannedDomains.Add("/userinfo/");
            bannedDomains.Add("/userinfo");
            bannedDomains.Add("/food/favourites");
            bannedDomains.Add("/food/menus/*/shopping-list");
            bannedDomains.Add("/food/recipes/*/shopping-list");
            bannedDomains.Add("/food/search*?*");
            bannedDomains.Add("/sounds/search$");
            bannedDomains.Add("/sounds/search/");
            bannedDomains.Add("/sounds/search?");
            bannedDomains.Add("/ws/includes");
            bannedDomains.Add("/rd/search$");
            bannedDomains.Add("/rd/search/");
            bannedDomains.Add("/rd/search?");
            bannedDomains.Add("/help/");


            // Define the approved domains
            approvedDomains = new List<String>();

            approvedDomains.Add("/news/");
            approvedDomains.Add("/sport");
            #endregion
        }

        public override void ExtractMetadesc()
        {
            // Has a metadesc been found?
            bool found = false;
            try
            {
                // Retrieve the meta nodes
                var metadescElement = pageContent.DocumentNode.SelectNodes("//meta");

                Console.WriteLine($"METADESCRIPTION | {metadescElement.Count} meta tags identified.");
                /* This code is a bit messy, this is unfortunately because whilst HTML meta tag names are not case sensitive,
                 * C# code is case sensitive and therefore all case combinations for the attributes must be considered.
                 * To my knowledge there is not a better way to do this
                 */
                foreach (HtmlNode tag in metadescElement)
                {
                    try
                    {

                        if (tag != null)
                        {
                            // Determine: Is this element the tag that contains the description?
                            if ( //tag.Attributes["name"].Value.ToUpper() == "OG:DESCRIPTION"
                              tag.Attributes["name"].Value.ToUpper() == "DESCRIPTION")
                            // || tag.Attributes["NAME"].Value.ToUpper() == "OG:DESCRIPTION:")
                            {
                                // Save the data to the meta tag
                                metadesc = tag.Attributes["content"].Value;
                                if (metadesc == null) metadesc = tag.Attributes["Content"].Value;
                                if (metadesc == null) metadesc = tag.Attributes["CONTENT"].Value;
                                found = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // BREAK;
                    }
                }
            }
            catch (NullReferenceException e)
            {
                metadesc = "This site did not provide a description.";
                Console.WriteLine("METADESCRIPTION | Critical failure whilst parsing metadesc. Abandoning.");
            }

            if (found) // UI update: Was a metadesc found?
                Console.WriteLine("METADESCRIPTION | Metadescription extracted!");
            else
                Console.WriteLine("METADESCRIPTION | Failed to locate metadescription.");

        }

        // Extracts hyperlinks from the current web doc
        public override void ExtractHyperlinks()
        {
            // Creates a list of all the hyperlink tags
            var links = pageContent.DocumentNode.SelectNodes("//a[@href]");

            // Print link count
            Console.WriteLine($"HYPERLINKS      | {links.Count} links extracted.");
            int validLinkCount = 0;
            int externalLinkCount = 0;
            int cachedLinkCount = 0;
            int bannedLinkCount = 0;

            // Extract the link from the tags
            if (links != null)
            {
                foreach (var link in links) // Loop for each link
                {
                    var hyperlink = link.Attributes["href"].Value; // Extract the actual link from tag
                    bool isInternal = true;

                    if (!hyperlink.Contains("www.bbc") && !hyperlink.StartsWith("/"))
                    {
                        isInternal = false;
                        externalLinkCount++;
                    }


                    // Check link isn't cached
                    if (!Program.cache.Contains(hyperlink) && isInternal)
                    {
                        bool banned = false;

                        // Check link against banned domains before accessing
                        foreach (string domain in bannedDomains)
                        {
                            if (hyperlink.Contains(domain))
                            {
                                banned = true;
                                bannedLinkCount++;
                            }
                        }

                        // If the link is not of a banned domain...
                        if (!banned)
                        {
                            // ... and is an approved domain ...
                            foreach (string domain in approvedDomains)
                            {
                                if (hyperlink.Contains(domain))
                                {
                                    // ... queue it
                                    if (hyperlink.Contains($"https://account.bbc.com/account?lang=en-GB&amp;ptrt="))
                                        hyperlink.TrimStart(@"https://account.bbc.com/account?lang=en-GB&amp;ptrt=".ToCharArray());

                                    queue.Enqueue(hyperlink);
                                    validLinkCount++;
                                }
                            }
                        }

                    }
                    else
                    {
                        cachedLinkCount++;
                    }
                }
            }

            // Print extraction results
            Console.WriteLine($"HYPERLINKS      | {validLinkCount} valid links extracted and queued.");
            Console.WriteLine($"HYPERLINKS      | {bannedLinkCount} links discarded due to being from banned domains.");
            Console.WriteLine($"HYPERLINKS      | {cachedLinkCount} links discarded due to being cached already.");
            Console.WriteLine($"HYPERLINKS      | {externalLinkCount} links discarded due to being external.");
        }
    }
}
