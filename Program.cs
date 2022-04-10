﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WebbedAudio_AIP__Application_In_Parts_
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://101audiobooks.com/brandon-sanderson-elantris-audiobook/4/"; //make possible to input multiple links
            string downloadFolder = @"C:\Users\hugok\source\repos\WebbedAudio AIP (Application In Parts)\Downloads\";

            scrape(url, downloadFolder);

            Thread.Sleep(3000);
        }

        static void scrape(string url, string downloadFolder)
        {
            List<Tuple<string, int>> subsites = Scraper.GetSubsites(url);

            subsites.Insert(0, new Tuple<string, int>(url, subsites.Count));
            subsites = subsites.OrderBy(x => x.Item2).ToList();

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
