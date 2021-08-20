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
        static readonly OptionSet optionsParser = new OptionSet();

        [STAThread]
        static void Main(string[] args)
        {
            bool useClipboard = false;
            bool openAtCursor = false;
            bool onTop = false;

            optionsParser.Add("p|paste", "Displays image using content from clipboard.", _ => useClipboard = true);
            optionsParser.Add("c|cursor", "Opens the window at the position of the cursor.", _ => openAtCursor = true);
            optionsParser.Add("t|top", "Prevents the window from being hidden behind other windows.", _ => onTop = true);
            optionsParser.Add("h|help", "Prints this message.", _ => ShowHelp()); 
            

            List<string> extra;
            try
            {
                extra = optionsParser.Parse(args);
                foreach (var item in optionsParser.ArgumentSources)
                {
                    Console.WriteLine(item);

                }
                foreach (var w in extra)
                {
                    Console.WriteLine(w);
                }
            } 
            catch (OptionException e)
            {
                ShowHelp();
                return;
            }

            ImageWindow window;
            if (useClipboard)
            {
                window = new ImageWindow(Clipboard.GetImage());
            } 
            else
            {
                if (extra.Count == 0)
                    throw new ArgumentException("No files given");
                window = new ImageWindow(CreateImageList(args));
            }

            if (onTop) window.MakeTopmost();
            if (openAtCursor) window.MoveTo(Cursor.Position);
            
            window.Open();
        }

        private static void ShowHelp()
        {
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine($"Usage: {name} [options]* [image urls]*");
            optionsParser.WriteOptionDescriptions(Console.Out);
            Environment.Exit(0);
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
            string scheme = uri.Scheme;

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
