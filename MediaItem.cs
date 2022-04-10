using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebbedAudio_AIP__Application_In_Parts_
{
    public class MediaItem
    {
        public string FullUrl { get; set; }
        public int TrackNumber { get; set; }
        public string TrackName { get; set; }
        public string Title { get; set; }

        public MediaItem(string url)
        {
            this.FullUrl = url;
            this.TrackName = FullUrl.Split("/")[FullUrl.Split("/").Length - 2]; //Name is usually at last place in link (length - 1)
            this.TrackNumber = int.Parse((FullUrl.Split("/")[FullUrl.Split("/").Length - 1]).Split(".")[0]); //Number is usually at last place in link (length - 1)
            this.Title = TrackName + " " + TrackNumber;
        }
        
    }
}
