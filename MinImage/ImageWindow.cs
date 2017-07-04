using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MinImage
{
    public class ImageWindow : Window
    {
        private IList<Image> images;
        private int currentIndex = 0;

        public Image CurrentImage
        {
            get
            {
                return images[currentIndex];
            }
        }        

        public ImageWindow(Image image) : this(new List<Image> { image })
        {}

        public ImageWindow(ICollection<Image> images)
        {
            this.images = new List<Image>(images);

            window.BackgroundImageLayout = ImageLayout.Center;
            window.BackColor = Color.Black;

            AddEventListeners();
            UpdateImage();
        }

        private void AddEventListeners()
        {
            window.KeyUp += RotateLeftOnCtrlLeft;
            window.KeyUp += RotateRightOnCtrlRight;
            window.KeyUp += NextImageOnRight;
            window.KeyUp += PreviousImageOnLeft;
            window.DoubleClick += ToggleFullScreenOnDoubleClick;
        }

        private void ToggleFullScreenOnDoubleClick(object sender, EventArgs e)
        {
            if (window.WindowState == FormWindowState.Maximized)
                window.WindowState = FormWindowState.Normal;
            else
                window.WindowState = FormWindowState.Maximized;
        }

        private void UpdateImage()
        {
            Screen screen = Screen.FromControl(window);

            if (CurrentImage.Size.Width > screen.Bounds.Width || CurrentImage.Size.Height > screen.Bounds.Height)
                ResizeImageToFitScreen();

            window.BackgroundImage = CurrentImage;
            window.Size = CurrentImage.Size;
            window.Refresh();
            
            Console.WriteLine("Height: " + screen.Bounds.Height + " width: " + screen.Bounds.Width);
        }

        private void ResizeImageToFitScreen()
        {
            //throw new NotImplementedException();
        }

        private void RotateLeftOnCtrlLeft(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Left | Keys.Control))
                RotateLeft();
        }

        private void RotateRightOnCtrlRight(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Right | Keys.Control))
                RotateRight();
        }

        private void PreviousImageOnLeft(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Left))
                PreviousImage();
        }

        private void NextImageOnRight(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Right))
                NextImage();
        }
        
        public void RotateLeft()
        {
            CurrentImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            UpdateImage();
        }

        public void RotateRight()
        {   
            CurrentImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            UpdateImage();
        }

        public void NextImage()
        {
            if (++currentIndex >= images.Count)
                currentIndex = 0;
            UpdateImage();

            Console.WriteLine("NextImage");
        }

        public void PreviousImage()
        {
            if (--currentIndex < 0)
                currentIndex = images.Count-1;
            UpdateImage();

            Console.WriteLine("PreviousImage");
        }

        private Image ResizeImage(Image image)
        {
            Image resizedImage;

            return image;
        }
    }
}
