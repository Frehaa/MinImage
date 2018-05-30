using System.Windows.Forms;

namespace MinImage
{
    public class DoubleBufferedForm : Form
    {
        public DoubleBufferedForm() : base()
        {
            DoubleBuffered = true;
        }
    }
}
