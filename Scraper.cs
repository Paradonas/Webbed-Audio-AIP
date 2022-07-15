using System;
using System.Collections.Generic;
using System.Net.Http;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace WebbedAudio_AIP__Application_In_Parts_
{
    public class Scraper
    {
        static string GetSiteHtml(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result) //gets response from requested url
                {
                    using (HttpContent content = response.Content)
                    {
                        return content.ReadAsStringAsync().Result; //gets content (html)
                    }
                }
            }
        }

        public static List<Tuple<string, int>> GetSubsites(string url)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(GetSiteHtml(url));

            List<Tuple<string, int>> subsites = new List<Tuple<string, int>>();
            int numbSubsites = 0;

            var mediaElementNodes = doc.DocumentNode.CssSelect("div.page-links > a");

            foreach (var mediaNode in mediaElementNodes)
            {
                string link = mediaNode.Attributes["href"].Value;
                subsites.Add(new Tuple<string, int>(link, numbSubsites));
                numbSubsites++;
            }

            return subsites;
        }

        public static List<Tuple<string, int>> GetSubsitesAsync(string url)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(GetSiteHtml(url));

            List<Tuple<string, int>> subsites = new List<Tuple<string, int>>();
            int numbSubsites = 0;

            var mediaElementNodes = doc.DocumentNode.CssSelect("div.page-links > a");

            Parallel.ForEach<HtmlNode>(mediaElementNodes, (mediaNode) =>
            {
                string link = mediaNode.Attributes["href"].Value;
                subsites.Add(new Tuple<string, int>(link, numbSubsites));
                numbSubsites++;
            });

            return subsites;
        }

        public static List<MediaItem> GetMediaElements(string url)
        {
            List<MediaItem> mediaElements = new List<MediaItem>();

            var doc = new HtmlDocument();
            doc.LoadHtml(GetSiteHtml(url));

            var mediaElemenNodes = doc.DocumentNode.CssSelect("audio.wp-audio-shortcode > a");

            foreach (var node in mediaElemenNodes)
            {
                string link = node.InnerHtml.CleanInnerText(); //gets text inside tag (<a> this text </a>)
                mediaElements.Add(new MediaItem(link));
            }
            
            return mediaElements;
        }

        public static void DownloadMedia(List<MediaItem> mediaItems, string downloadFolder)
        {
            WebClient client = new WebClient();

            foreach (var media in mediaItems)
            {
                string downloadFile = $@"{downloadFolder}{media.Title}.mp3";
                using (StreamWriter sw = new StreamWriter(downloadFile)) //we need to create file before initializing content
                {
                }
                client.DownloadFile(new Uri(media.FullUrl), downloadFile); //initializes content
            }
        }
    }  
}
