﻿using System.Drawing;
using System.Windows.Forms;

namespace MinImage
{
    public class Window
    {
        protected Form window;
        private bool isMoveable;
        private Point dragStartPoint;
        public delegate void CloseEvent(Window window);
        public event CloseEvent OnClose;
        
        public Window()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            window = new DoubleBufferedForm();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            window.FormBorderStyle = FormBorderStyle.None;

            window.KeyUp += CloseOnEscape;

            window.MouseDown += AllowMoveOnLeftClickDown;
            window.MouseUp += DisallowMoveOnLeftClickUp;
            window.MouseMove += MoveIfMoveable;
            window.KeyUp += ActOnKey;
            window.FormClosed += Window_FormClosed;
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke(this);
        }

        private void ActOnKey(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    window.TopMost = !window.TopMost;
                    break;
                default:
                    break;
            }
        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                Close();
        }

        public void Open()
        {
            Application.Run(window);
        }

        public void Close()
        {
            window.Close();
        }

        public void MakeTopmost()
        {
            window.TopMost = true;
        }

        private void AllowMoveOnLeftClickDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMoveable = true;
                dragStartPoint = e.Location;
            }
        }

        private void DisallowMoveOnLeftClickUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMoveable = false;
        }

        private void MoveIfMoveable(object sender, MouseEventArgs e)
        {
            if (isMoveable)
            {
                Point dragAmount = e.Location;
                dragAmount.Offset(new Point(-dragStartPoint.X, -dragStartPoint.Y));

                Move(dragAmount);
            }
        }

        public void Move(Point amount)
        {
            Point newLocation = window.Location;
            newLocation.Offset(amount);

            window.Location = newLocation;
        }

        public void MoveTo(Point location)
        {
            window.Location = location;
        }

        public void ToggleTop()
        {
            window.TopMost = !window.TopMost;
        }
    }
}
