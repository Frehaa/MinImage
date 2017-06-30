using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace MinImage
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<Image> list = new List<Image>
            {
                GetImageFromUrl("http://40.media.tumblr.com/908d79921332a12f8b4122fcb71449c5/tumblr_noz619CUUx1ragy7jo1_500.jpg#cgm_id=1863"),
                GetImageFromUrl("https://cdn.awwni.me/u8hj.jpg#cgm_id=24273")
            };
            Window window = new ImageWindow(list);
            window.Open();
        }

        private static Image GetImageFromUrl(string url)
        {
            using (WebClient client = new WebClient())
            using (Stream stream = client.OpenRead(url))
                return Image.FromStream(stream);
        }
        
    }
}
