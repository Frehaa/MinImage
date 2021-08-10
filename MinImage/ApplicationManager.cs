using System.Collections.Generic;
using System.Windows.Forms;

namespace MinImage
{
    public class ApplicationManager : ApplicationContext
    {
        private readonly ICollection<Window> windows = new List<Window>();
        private int window_count = 0;

        public ApplicationManager()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }


        public void Add(Window window)
        {
            windows.Add(window);
            window_count++;
            window.OnClose += Remove;
            window.Open();
        }

        public void Remove(Window window)
        {
            windows.Remove(window);
            window_count--;
            if (window_count == 0)
            {
                ExitThread();
            }
        } 

    }
}
