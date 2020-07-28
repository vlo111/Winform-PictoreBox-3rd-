using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testPictures
{
    public partial class Form1 : Form
    {
        #region Fields




        bool mousepressed = false;  // true as long as left mousebutton is pressed

        Image imgEzd;

        int fg_startx = 0; // offset of image when mouse was pressed
        int fg_starty = 0;

        int fg_imgx = 0; // current offset of image
        int fg_imgy = 0;

        float fg_zoom = 1;

        Point fg_mouseDown;

        Image imgBack;

        int bg_startx = 0; // offset of image when mouse was pressed
        int bg_starty = 0;

        int bg_imgx = 0; // current offset of image
        int bg_imgy = 0;

        float bg_zoom = 1;

        Point bg_mouseDown;

        #endregion

        public Form1()
        {
            InitializeComponent();

            imgBack = Image.FromFile(Application.StartupPath + "\\files\\letter.png");

            imgEzd = Image.FromFile(Application.StartupPath + "\\files\\for.png");

            Graphics g = this.CreateGraphics();

            // Fit width
            fg_zoom = ((float)pictureBox.Width / (float)imgEzd.Width) * (imgEzd.HorizontalResolution / g.DpiX);

            // Fit width
            bg_zoom = ((float)pictureBox.Width / (float)imgBack.Width) * (imgBack.HorizontalResolution / g.DpiX);

            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);
        }

        private void pictureBox_MouseMove(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (fg_check.Checked)
                {
                    Point mousePosNow = mouse.Location;

                    int deltaX = mousePosNow.X - fg_mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                    int deltaY = mousePosNow.Y - fg_mouseDown.Y;

                    fg_imgx = (int)(fg_startx + (deltaX / fg_zoom));  // calculate new offset of image based on the current zoom factor
                    fg_imgy = (int)(fg_starty + (deltaY / fg_zoom));

                    //pictureBox.Refresh();
                }

                if (bg_check.Checked)
                {
                    Point mousePosNow = mouse.Location;

                    int deltaX = mousePosNow.X - bg_mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                    int deltaY = mousePosNow.Y - bg_mouseDown.Y;

                    bg_imgx = (int)(bg_startx + (deltaX / bg_zoom));  // calculate new offset of image based on the current zoom factor
                    bg_imgy = (int)(bg_starty + (deltaY / bg_zoom));
                }

                pictureBox.Refresh();
            }
        }

        private void imageBox_MouseDown(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (fg_check.Checked)
                {

                    if (!mousepressed)
                    {
                        mousepressed = true;
                        fg_mouseDown = mouse.Location;
                        fg_startx = fg_imgx;
                        fg_starty = fg_imgy;
                    }
                }
                if (bg_check.Checked)
                {

                    if (!mousepressed)
                    {
                        mousepressed = true;
                        bg_mouseDown = mouse.Location;
                        bg_startx = bg_imgx;
                        bg_starty = bg_imgy;
                    }
                }
            }
        }

        private void imageBox_MouseUp(object sender, EventArgs e)
        {
            mousepressed = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (fg_check.Checked)
            {
                float oldzoom = fg_zoom;

                if (e.Delta > 0)
                {
                    fg_zoom += 0.1F;
                }

                else if (e.Delta < 0)
                {
                    fg_zoom = Math.Max(fg_zoom - 0.1F, 0.01F);
                }

                MouseEventArgs mouse = e as MouseEventArgs;
                Point mousePosNow = mouse.Location;

                int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
                int y = mousePosNow.Y - pictureBox.Location.Y;

                int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
                int oldimagey = (int)(y / oldzoom);

                int newimagex = (int)(x / fg_zoom);     // Where in the IMAGE will it be when the new zoom i made
                int newimagey = (int)(y / fg_zoom);

                fg_imgx = newimagex - oldimagex + fg_imgx;  // Where to move image to keep focus on one point
                fg_imgy = newimagey - oldimagey + fg_imgy;

                // pictureBox.Refresh();  // calls imageBox_Paint
            }

            if (bg_check.Checked)
            {
                float oldzoom = bg_zoom;

                if (e.Delta > 0)
                {
                    bg_zoom += 0.1F;
                }

                else if (e.Delta < 0)
                {
                    bg_zoom = Math.Max(bg_zoom - 0.1F, 0.01F);
                }

                MouseEventArgs mouse = e as MouseEventArgs;
                Point mousePosNow = mouse.Location;

                int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
                int y = mousePosNow.Y - pictureBox.Location.Y;

                int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
                int oldimagey = (int)(y / oldzoom);

                int newimagex = (int)(x / bg_zoom);     // Where in the IMAGE will it be when the new zoom i made
                int newimagey = (int)(y / bg_zoom);

                bg_imgx = newimagex - oldimagex + bg_imgx;  // Where to move image to keep focus on one point
                bg_imgy = newimagey - oldimagey + bg_imgy;
            }

            pictureBox.Refresh();  // calls imageBox_Paint
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            label1.Text = $@"BG zoom{bg_zoom}";
            label2.Text = $@"BG X{bg_zoom} Y{bg_imgy}";

            e.Graphics.ScaleTransform(bg_zoom, bg_zoom);
            e.Graphics.DrawImage(imgBack, bg_imgx, bg_imgy);

            label3.Text = $@"FG zoom{fg_zoom}";
            label4.Text = $@"FG X{fg_zoom} Y{fg_imgy}";

            e.Graphics.ScaleTransform(fg_zoom, fg_zoom);
            e.Graphics.DrawImage(imgEzd, fg_imgx, fg_imgy);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (fg_check.Checked)
            {
                const int WM_KEYDOWN = 0x100;
                const int WM_SYSKEYDOWN = 0x104;

                if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
                {
                    switch (keyData)
                    {
                        case Keys.Right:
                            fg_imgx -= (int)(pictureBox.Width * 0.1F / fg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Left:
                            fg_imgx += (int)(pictureBox.Width * 0.1F / fg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Down:
                            fg_imgy -= (int)(pictureBox.Height * 0.1F / fg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Up:
                            fg_imgy += (int)(pictureBox.Height * 0.1F / fg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageDown:
                            fg_imgy -= (int)(pictureBox.Height * 0.90F / fg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageUp:
                            fg_imgy += (int)(pictureBox.Height * 0.90F / fg_zoom);
                            pictureBox.Refresh();
                            break;
                    }
                }
            }

            if (bg_check.Checked)
            {
                const int WM_KEYDOWN = 0x100;
                const int WM_SYSKEYDOWN = 0x104;

                if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
                {
                    switch (keyData)
                    {
                        case Keys.Right:
                            bg_imgx -= (int)(pictureBox.Width * 0.1F / bg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Left:
                            bg_imgx += (int)(pictureBox.Width * 0.1F / bg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Down:
                            bg_imgy -= (int)(pictureBox.Height * 0.1F / bg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Up:
                            bg_imgy += (int)(pictureBox.Height * 0.1F / bg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageDown:
                            bg_imgy -= (int)(pictureBox.Height * 0.90F / bg_zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageUp:
                            bg_imgy += (int)(pictureBox.Height * 0.90F / bg_zoom);
                            pictureBox.Refresh();
                            break;
                    }
                }
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}