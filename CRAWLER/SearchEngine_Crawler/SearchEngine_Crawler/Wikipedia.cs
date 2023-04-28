using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;
using HtmlAgilityPack;

namespace SearchEngine_Crawler
{
    public class Wikipedia : Crawler
    {
        public override string homeURL { get; set; }
        public override List<String> bannedDomains { get; set; }

        public Wikipedia()
        {
            this.homeURL = "https://en.wikipedia.org/wiki/Special:Random"; // Set home URL

            // Populate banned domains list
            // Reason for ban is next to the entry.
            bannedDomains = new List<String>();
            bannedDomains.Add("/wiki/Special:");        // Robots.txt requests no repeated calls
            bannedDomains.Add("/wiki/Main_Page");       // Pointless to index
            bannedDomains.Add("/wiki/Wikipedia");       // About pages, pointless to index
            bannedDomains.Add("/wiki/File:");           // Picture/file, not web page
            bannedDomains.Add("/wiki/Help:");           // Pointless to index
            bannedDomains.Add("/wiki/Portal:");         // Pointless to index
            bannedDomains.Add("/wiki/Talk:");           // Pointless to index
            bannedDomains.Add("/wiki/Edit:");           // Pointless to index
            bannedDomains.Add("/wiki/Template:");       // Does not contain information
            bannedDomains.Add("/wiki/Template_Talk:");  // Does not contain information
        }


        // Extracts metadesc from the current web doc
        public override void ExtractMetadesc()
        {
            /* try
            {
                // Retrieve the meta nodes
                var metadescElement = pageContent.DocumentNode.SelectNodes("//meta");

                /* This code is a bit messy, this is unfortunately because whilst HTML meta tag names are not case sensitive,
                 * C# code is case sensitive and therefore all case combinations for the attributes must be considered.
                 * To my knowledge there is not a better way to do this
                 */
            /*
                foreach (var tag in metadescElement)
                {
                    if (tag != null)
                    {
                        // Determine: Is this element the tag that contains the description?
                        if (tag.Attributes["name"].Value == "DESCRIPTION"
                         || tag.Attributes["Name"].Value == "DESCRIPTION"
                         || tag.Attributes["NAME"].Value == "DESCRIPTION")
                        {
                            // Save the data to the meta tag
                            metadesc = tag.Attributes["content"].Value;
                            if (metadesc == null) metadesc = tag.Attributes["Content"].Value;
                            if (metadesc == null) metadesc = tag.Attributes["CONTENT"].Value;
                        }
                    }
                }
            }
            catch (NullReferenceException e)
            {
                metadesc = "This site did not provide a description.";
            } */

            // Wikipedia does not provide a meta description.
            metadesc = "We are unable to retrieve descriptions of wikipedia pages at this time.";

        }

        // Extracts hyperlinks from the current web doc
        public override void ExtractHyperlinks()
        {
            // Creates a list of all the hyperlink tags
            var links = pageContent.DocumentNode.SelectNodes("//a[@href]");

            // Extract the link from the tags
            if (links != null)
            {
                foreach (var link in links) // Loop for each link
                {
                    var hyperlink = link.Attributes["href"].Value; // Extract the actual link from tag

                    // Check suitability of link before queueing
                    if (hyperlink.StartsWith("/wiki/") && !hyperlink.Contains("(identifier)"))
                    {
                        bool banned = false;

                        // Check link against banned domains before accessing
                        foreach(string domain in bannedDomains)
                        {
                            if (hyperlink.StartsWith(domain))
                            {
                                banned = true;
                            }
                        }

                        // If the link is not of a banned domain...
                        if (!banned)
                        {
                            // ... queue it
                            queue.Enqueue("https://en.wikipedia.org" + hyperlink);
                        }
                    }
                }
            }
        }

    }
}
