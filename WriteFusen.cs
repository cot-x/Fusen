using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace Yuki
{
    class WriteFusen : Form
    {
        private bool mouseDown = false;
        private Point point;
        public TextBox textBox;

        public WriteFusen()
        {
            NewFusen("");
        }

        public void NewFusen(string memo)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "New";
            this.BackColor = Color.Red;
            this.Opacity = 0.8;
            this.Size = new Size(256, 124);
            this.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Yuki.Main.ico"));
            this.MouseDown += new MouseEventHandler(fusen_MouseDown);
            this.MouseUp += new MouseEventHandler(fusen_MouseUp);
            this.MouseMove += new MouseEventHandler(fusen_MouseMove);

            textBox = new TextBox();
            textBox.Bounds = new Rectangle(16, 16, 140, 92);
            textBox.Multiline = true;
            textBox.Text = memo;
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            this.Controls.Add(textBox);

            Button buttonOK = new Button();
            buttonOK.Text = "OK";
            buttonOK.Bounds = new Rectangle(172, 80, 64, 32);
            buttonOK.Click += new EventHandler(buttonOK_Click);
            this.Controls.Add(buttonOK);

            this.Show();
        }

        void buttonOK_Click(object sender, EventArgs e)
        {
            if (textBox.Text == "")
                return;

            Fusens fusens;
            fusens.data.memo = textBox.Text;
            fusens.data.point = new Point(64, 64);
            Fusen fusen = new Fusen();
            fusens.form = fusen.CreateFusen(textBox.Text, fusens.data.point);
            FusenController.list.Add(fusens);

            MainNotifyIcon.fusenctrl.WriteFusens();
            this.Close();
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK_Click(sender, e);
        }

        void fusen_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(e.X - point.X + this.Location.X, e.Y - point.Y + this.Location.Y);
            }
        }

        void fusen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDown = false;
        }

        void fusen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point = e.Location;
                mouseDown = true;
            }
        }
    }
}
