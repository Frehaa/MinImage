using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

using Mono.Options;

namespace MinImage
{
    static class Program
    {
        static readonly Mutex mutex = new Mutex(true, "MinImage-hitotsu-kudasai");
        static readonly ApplicationManager manager = new ApplicationManager();

        
        [STAThread]
        static void Main(string[] args)
        {
            bool openAtCursor = false;
            bool paste = false;
            bool keepOnTop = false;
            bool showHelp = false;

            var p = new OptionSet() {
                "Usage: MinImage [Image Url] [OPTIONS]+",
                "",
                "Options:",
                { "c|cursor", "Open the window at the cursor's position.", v => openAtCursor = v != null },
                { "p|paste", "Opens window with image from clipboard.", v => paste = v != null },
                { "t|top", "Keeps window on top.", v => keepOnTop = v != null },
                { "h|help", "Prints this message.", v => showHelp = v != null },

            };

            List<string> extra; 
            try
            {
                extra = p.Parse(args);
            } 
            catch (OptionException e)
            {
                throw e;
            }


            if (showHelp)
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }

            ImageWindow window;
            if (paste)
            {
                window = new ImageWindow(Clipboard.GetImage());
            } 
            else
            {
                if (extra.Count == 0 && !paste)
                    throw new ArgumentException("No files given");
                window = new ImageWindow(CreateImageList(extra));
            }

            if (openAtCursor)
                window.MoveTo(Cursor.Position);
            if (keepOnTop)
                window.ToggleTop();

            window.Open();
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
