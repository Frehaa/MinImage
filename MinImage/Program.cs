using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace MinImage
{
    static class Program
    {
        static readonly Mutex mutex = new Mutex(true, "MinImage-hitotsu-kudasai");

        [STAThread]
        static void Main(string[] args)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                if (args.Length == 0)
                    throw new ArgumentException("No files given");

                List<Image> list = CreateImageList(args);
                ImageWindow window = new ImageWindow(list);
                window.Open();
            } else {
                MessageBox.Show("Test");
            }            
        }

        private static IEnumerable<Image> CreateImageList(ICollection<string> uri)
        {
            return uri.Select(u =>
            {
                if (IsValidURL(u)) return GetImageFromUrl(u);
                else if (TryIsLocalFilePath(u)) return GetImageFromFile(u);
                else throw new ArgumentException("Unknown argument");
            });
        }

        private static Image GetImageFromFile(string imageURI)
        {
            return Image.FromFile(imageURI);
        }

        private static bool IsLocalFilePath(string filePath)
        {
            try
            {
                return TryIsLocalFilePath(filePath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryIsLocalFilePath(string filePath)
        {
            Uri uri = new Uri(filePath);
            return uri.IsFile;
        }

        private static bool IsValidURL(string url)
        {
            try
            {
                return TryIsValidURL(url);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryIsValidURL(string url)
        {
            Uri uri = new Uri(url);
            String scheme = uri.Scheme;

            return scheme.Equals("http") || scheme.Equals("https");
        }

        private static Image GetImageFromUrl(string url)
        {
            using (WebClient client = new WebClient())
            using (Stream stream = client.OpenRead(url))
                return Image.FromStream(stream);
        }
        
    }
}
