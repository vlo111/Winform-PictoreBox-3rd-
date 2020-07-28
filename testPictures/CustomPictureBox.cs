using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace testPictures
{
    public class CustomPictureBox : PictureBox
    {
        public CustomPictureBox(IContainer container)
        {
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserMouse, true);
            TabStop = true;
            container.Add(this);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (this.Focused)
                ControlPaint.DrawFocusRectangle(pe.Graphics, ClientRectangle);
            base.OnPaint(pe);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            int x = this.Location.X;
            int y = this.Location.Y;

            if (e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
                x += 1;
            }
            else if (e.KeyCode == Keys.Left)
                x -= 1;
            else if (e.KeyCode == Keys.Up)
                y -= 1;
            else if (e.KeyCode == Keys.Down)
                y += 1;
            this.Location = new Point(x, y);
            base.OnPreviewKeyDown(e);
        }
    }
}
