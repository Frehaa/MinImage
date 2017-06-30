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
            AddEventListeners();
            RefreshImage();
        }

        private void AddEventListeners()
        {
            window.KeyUp += RotateLeftOnCtrlLeft;
            window.KeyUp += RotateRightOnCtrlRight;
            window.KeyUp += NextImageOnRight;
            window.KeyUp += PreviousImageOnLeft;
        }

        private void RefreshImage()
        {
            window.BackgroundImage = CurrentImage;
            window.Size = CurrentImage.Size;
            window.Refresh();
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
            RefreshImage();
        }

        public void RotateRight()
        {   
            CurrentImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            RefreshImage();
        }

        public void NextImage()
        {
            if (++currentIndex >= images.Count)
                currentIndex = 0;
            RefreshImage();

            Console.WriteLine("NextImage");
        }

        public void PreviousImage()
        {
            if (--currentIndex < 0)
                currentIndex = images.Count-1;
            RefreshImage();

            Console.WriteLine("PreviousImage");
        }
    }
}
