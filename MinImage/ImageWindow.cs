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
        private ScrollBar scrollBar;
        private PictureBox pictureBox;

        public Image CurrentImage
        {
            get
            {
                return images[currentIndex];
            }
        }

        public ImageWindow(Image image) : this(new List<Image> { image })
        {}

        public ImageWindow(IEnumerable<Image> images)
        {
            this.images = new List<Image>(images);

            //scrollBar = new VScrollBar();
            //scrollBar.MinimumSize = window.Size;
            //scrollBar.Hide();
            //pictureBox = new PictureBox();

            //window.Controls.Add(pictureBox);

            //pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
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


            //pictureBox.Image = CurrentImage;

            window.BackgroundImage = CurrentImage;
            window.Size = CurrentImage.Size;
            window.Refresh();
            
            Console.WriteLine("Height: " + screen.Bounds.Height + " width: " + screen.Bounds.Width);
        }

        private void ResizeImageToFitScreen()
        {
            var screen = Screen.FromControl(window);

            double width = screen.Bounds.Width;
            double height = screen.Bounds.Height;
            double ratio = width / height;

            Console.WriteLine($"width = {width}, height = {height}, ratio = {ratio}");

            var image = images[currentIndex];

            double imageWidth = image.Width;
            double imageHeight = image.Height;
            double imageRatio = imageWidth / imageHeight;
            Console.WriteLine($"imageWidth = {imageWidth}, imageHeight = {imageHeight}, imageRatio = {imageRatio}");

            int newWidth = (int) imageWidth;
            int newHeight = (int) imageHeight;
            double newRatio = 0d;
            
            if (imageWidth > imageHeight)
            {
                newWidth = (int) width;
                newHeight = (int)(newWidth * imageRatio);

            }
            else
            {
                newHeight = (int)(height);
                newWidth = (int)(newHeight * imageRatio);
            }
            newRatio = (double)newWidth / newHeight;
            
            Console.WriteLine($"newWidth = {newWidth}, newHeight = {newHeight}, newRatio = {newRatio}");
            images[currentIndex] = new Bitmap(image, newWidth, newHeight);
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
