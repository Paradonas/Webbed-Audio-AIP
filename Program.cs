using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebbedAudio_AIP__Application_In_Parts_
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://goldenaudiobooks.com/markus-zusak-the-book-thief-audiobook/"; //make possible to input multiple links. On hold, multiple links can be queued for download by the planned async.

            string currentProjFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string downloadFolder = @$"{currentProjFolder}\Downloads\"; //Download folder should be pickable by the file xplorer and relative.

            scrape(url, downloadFolder);

            Thread.Sleep(3000);
        }

        static void scrape(string url, string downloadFolder)
        {
            var watch = Stopwatch.StartNew();

            List<Tuple<string, int>> subsites = Scraper.GetSubsites(url);
           
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Subsite acquire time: {elapsedMs}ms");

            subsites.Insert(0, new Tuple<string, int>(url, subsites.Count)); //inserts first url because GetSubsites() only gets subsites, not og site
            subsites = subsites.OrderBy(x => x.Item2).ToList(); //sorts list by Item 2 (site number)

            for (int i = 0; i < subsites.Count; i++)
            {
                Tuple<string, int> subsite = subsites[i];
                url = subsite.Item1;

                List<MediaItem> mediaElements = Scraper.GetMediaElements(url);

                foreach (MediaItem media in mediaElements)
                {
                    Console.WriteLine($"found mp3 files: {media.Title}");
                }

                Scraper.DownloadMedia(mediaElements, downloadFolder);
                Console.WriteLine(subsite.Item2 == subsites.Count - 1 ? //displays different message depending on which sites media is being downloaded
                    $"Final media elements downloaded." : $"media elements downloaded from {i + 1} site, {subsites.Count - i - 1} sites remaining....");
            }
        }
    }
}
