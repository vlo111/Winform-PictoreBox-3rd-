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



        public Form1()
        {
            InitializeComponent();




            // BG
            imgBack = Image.FromFile(Application.StartupPath + "\\files\\letter.png");

            Graphics g = this.CreateGraphics();

            // Fit width
            zoom = ((float)pictureBox.Width / (float)imgBack.Width) * (imgBack.HorizontalResolution / g.DpiX);

            pictureBox.Paint += new PaintEventHandler(imageBox_Paint);




            // EZD
            imgEzd = Image.FromFile(Application.StartupPath + "\\files\\for.png");

            Graphics fg = this.CreateGraphics();

            // Fit width
            fg_zoom = ((float)pictureBox1.Width / (float)imgEzd.Width) * (imgEzd.HorizontalResolution / fg.DpiX);

            pictureBox1.Paint += new PaintEventHandler(EzdImageBox_Paint);

            pictureBox1.Parent = pictureBox;
        }

        //private void pictureBox_MouseMove(object sender, EventArgs e)
        //{
        //    if (bg_check.Checked)
        //    {
        //        MouseEventArgs mouse = e as MouseEventArgs;

        //        if (mouse.Button == MouseButtons.Left)
        //        {
        //            Point mousePosNow = mouse.Location;

        //            int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
        //            int deltaY = mousePosNow.Y - mouseDown.Y;

        //            imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
        //            imgy = (int)(starty + (deltaY / zoom));

        //            pictureBox.Refresh();
        //        }
        //    }
        //}

        //private void imageBox_MouseDown(object sender, EventArgs e)
        //{
        //    if (bg_check.Checked)
        //    {
        //        MouseEventArgs mouse = e as MouseEventArgs;

        //        if (mouse.Button == MouseButtons.Left)
        //        {
        //            if (!mousepressed)
        //            {
        //                mousepressed = true;
        //                mouseDown = mouse.Location;
        //                startx = imgx;
        //                starty = imgy;
        //            }
        //        }
        //    }
        //}

        //private void imageBox_MouseUp(object sender, EventArgs e)
        //{
        //    if (bg_check.Checked)
        //    {
        //        mousepressed = false;
        //    }
        //}



























        #region Fields BG

        Image imgBack;

        Point bg_mouseDown;
        int startx = 0; // offset of image when mouse was pressed
        int starty = 0;
        int imgx = 0; // current offset of image
        int imgy = 0;

        bool mousepressed = false;  // true as long as left mousebutton is pressed
        float zoom = 1;

        #endregion



        #region Fields FG

        Image imgEzd;
        Point fg_mouseDown;
        int fg_startx = 0; // offset of image when mouse was pressed
        int fg_starty = 0;
        int fg_imgx = 0; // current offset of image
        int fg_imgy = 0;

        bool fg_mousepressed = false;  // true as long as left mousebutton is pressed
        float fg_zoom = 1;

        #endregion


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (fg_check.Checked)
            {
                MouseEventArgs mouse = e as MouseEventArgs;

                if (mouse.Button == MouseButtons.Left)
                {
                    if (!fg_mousepressed)
                    {
                        fg_mousepressed = true;
                        fg_mouseDown = mouse.Location;
                        fg_startx = fg_imgx;
                        fg_starty = fg_imgy;
                    }
                }
            }

            if (bg_check.Checked)
            {
                MouseEventArgs mouse = e as MouseEventArgs;

                if (mouse.Button == MouseButtons.Left)
                {
                    if (!mousepressed)
                    {
                        mousepressed = true;
                        bg_mouseDown = mouse.Location;
                        startx = imgx;
                        starty = imgy;
                    }
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bg_check.Checked)
            {
                MouseEventArgs mouse = e as MouseEventArgs;

                if (mouse.Button == MouseButtons.Left)
                {

                    Point mousePosNow = mouse.Location;

                    int deltaX = mousePosNow.X - bg_mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                    int deltaY = mousePosNow.Y - bg_mouseDown.Y;

                    imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                    imgy = (int)(starty + (deltaY / zoom));

                    pictureBox.Refresh();
                }
            }
            if (fg_check.Checked)
            {
                MouseEventArgs mouse = e as MouseEventArgs;

                if (mouse.Button == MouseButtons.Left)
                {

                    Point mousePosNow = mouse.Location;

                    int deltaX = mousePosNow.X - fg_mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                    int deltaY = mousePosNow.Y - fg_mouseDown.Y;

                    fg_imgx = (int)(fg_startx + (deltaX / fg_zoom));  // calculate new offset of image based on the current zoom factor
                    fg_imgy = (int)(fg_starty + (deltaY / fg_zoom));

                    pictureBox1.Refresh();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (fg_check.Checked)
            {
                fg_mousepressed = false;
            }

            if (bg_check.Checked)
            {
                mousepressed = false;
            }
        }

        private void EzdImageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(fg_zoom, fg_zoom);
            e.Graphics.DrawImage(imgEzd, fg_imgx, fg_imgy);
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(zoom, zoom);
            e.Graphics.DrawImage(imgBack, imgx, imgy);
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                if (bg_check.Checked)
                {
                    switch (keyData)
                    {
                        case Keys.Right:
                            imgx -= (int)(pictureBox.Width * 0.1F / zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Left:
                            imgx += (int)(pictureBox.Width * 0.1F / zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Down:
                            imgy -= (int)(pictureBox.Height * 0.1F / zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.Up:
                            imgy += (int)(pictureBox.Height * 0.1F / zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageDown:
                            imgy -= (int)(pictureBox.Height * 0.90F / zoom);
                            pictureBox.Refresh();
                            break;

                        case Keys.PageUp:
                            imgy += (int)(pictureBox.Height * 0.90F / zoom);
                            pictureBox.Refresh();
                            break;
                    }
                }

                if (fg_check.Checked)
                {
                    switch (keyData)
                    {
                        case Keys.Right:
                            fg_imgx -= (int)(pictureBox1.Width * 0.1F / fg_zoom);
                            pictureBox1.Refresh();
                            break;

                        case Keys.Left:
                            fg_imgx += (int)(pictureBox1.Width * 0.1F / fg_zoom);
                            pictureBox1.Refresh();
                            break;

                        case Keys.Down:
                            fg_imgy -= (int)(pictureBox1.Height * 0.1F / fg_zoom);
                            pictureBox1.Refresh();
                            break;

                        case Keys.Up:
                            fg_imgy += (int)(pictureBox1.Height * 0.1F / fg_zoom);
                            pictureBox1.Refresh();
                            break;

                        case Keys.PageDown:
                            fg_imgy -= (int)(pictureBox1.Height * 0.90F / fg_zoom);
                            pictureBox1.Refresh();
                            break;

                        case Keys.PageUp:
                            fg_imgy += (int)(pictureBox1.Height * 0.90F / fg_zoom);
                            pictureBox1.Refresh();
                            break;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (bg_check.Checked)
            {
                float oldzoom = zoom;

                if (e.Delta > 0)
                {
                    zoom += 0.1F;
                }

                else if (e.Delta < 0)
                {
                    zoom = Math.Max(zoom - 0.1F, 0.01F);
                }

                MouseEventArgs mouse = e as MouseEventArgs;
                Point mousePosNow = mouse.Location;

                int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
                int y = mousePosNow.Y - pictureBox.Location.Y;

                int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
                int oldimagey = (int)(y / oldzoom);

                int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
                int newimagey = (int)(y / zoom);

                imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
                imgy = newimagey - oldimagey + imgy;

                pictureBox.Refresh();  // calls imageBox_Paint
            }

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

                int x = mousePosNow.X - pictureBox1.Location.X;    // Where location of the mouse in the pictureframe
                int y = mousePosNow.Y - pictureBox1.Location.Y;

                int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
                int oldimagey = (int)(y / oldzoom);

                int newimagex = (int)(x / fg_zoom);     // Where in the IMAGE will it be when the new zoom i made
                int newimagey = (int)(y / fg_zoom);

                fg_imgx = newimagex - oldimagex + fg_imgx;  // Where to move image to keep focus on one point
                fg_imgy = newimagey - oldimagey + fg_imgy;

                pictureBox1.Refresh();  // calls imageBox_Paint
            }
        }
    }
}