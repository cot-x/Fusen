using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Yuki
{
    class Fusen
    {
        private Form fusen;
        private bool mouseDown = false;
        private Point point;

        public Form CreateFusen(string memo, Point point)
        {
            fusen = new Form();
            fusen.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Yuki.Main.ico"));
            fusen.FormBorderStyle = FormBorderStyle.None;
            fusen.ShowInTaskbar = false;
            fusen.Size = new Size(134, 192);
            fusen.Text = memo;
            fusen.BackColor = Color.Orange;
            fusen.Paint += new PaintEventHandler(fusen_Paint);
            fusen.Opacity = 0.8;
            fusen.MouseDown += new MouseEventHandler(fusen_MouseDown);
            fusen.MouseUp += new MouseEventHandler(fusen_MouseUp);
            fusen.MouseMove += new MouseEventHandler(fusen_MouseMove);
            fusen.Show();
            fusen.DesktopLocation = point;
            fusen.SendToBack();
            fusen.Activated += new EventHandler(fusen_Activated);

            return fusen;
        }

        void fusen_Activated(object sender, EventArgs e)
        {
            fusen.SendToBack();
        }

        void fusen_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                fusen.Location = new Point(e.X - point.X + fusen.Location.X, e.Y - point.Y + fusen.Location.Y);
            }
        }

        void fusen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
                MainNotifyIcon.fusenctrl.WriteFusens();
            }
            else if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem editMenu = new MenuItem();
                editMenu.Text = "編集";
                editMenu.Click += new EventHandler(editMenu_Click);
                contextMenu.MenuItems.Add(editMenu);
                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Text = "削除";
                deleteMenu.Click += new EventHandler(deleteMenu_Click);
                contextMenu.MenuItems.Add(deleteMenu);
                fusen.ContextMenu = contextMenu;
                fusen.ContextMenu = contextMenu;
            }
        }

        void deleteMenu_Click(object sender, EventArgs e)
        {
            FusenController.DeleteFusen(fusen.Handle);
            fusen.Close();
            MainNotifyIcon.fusenctrl.WriteFusens();
        }

        void editMenu_Click(object sender, EventArgs e)
        {
            WriteFusen writeFusen = new WriteFusen();
            writeFusen.textBox.Text = fusen.Text;
            FusenController.DeleteFusen(fusen.Handle);
            fusen.Close();
            MainNotifyIcon.fusenctrl.WriteFusens();
        }

        void fusen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point = e.Location;
                mouseDown = true;
            }
        }

        static void fusen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(
                (sender as Form).ClientSize.Width / 2 - 5,
                (sender as Form).ClientSize.Height / 16,
                10, 10));

            PointF point = new PointF(10,
                (sender as Form).ClientSize.Height / 16 + (sender as Form).ClientSize.Height / 16 + 10);
            e.Graphics.DrawString((sender as Form).Text, new Font(FontFamily.GenericSansSerif, 10), Brushes.Black,
                new RectangleF(point, new SizeF((sender as Form).ClientSize.Width - 10,
                    (sender as Form).ClientSize.Height - point.Y - 20)));

        }
    }
}
