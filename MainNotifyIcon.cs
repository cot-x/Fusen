using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Yuki
{
    class MainNotifyIcon
    {
        private NotifyIcon notifyIcon;
        public static FusenController fusenctrl;

        [STAThread]
        public static void Main()
        {
            new MainNotifyIcon();
            Application.Run();
        }

        MainNotifyIcon()
        {
            // NotifyIconをセットする
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "ふ～せん";
            notifyIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Yuki.Main.ico"));
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuNew = new MenuItem();
            MenuItem menuExit = new MenuItem();
            menuNew.Text = "メモる";
            menuNew.Click += new EventHandler(menuNew_Click);
            contextMenu.MenuItems.Add(menuNew);
            menuExit.Text = "E&xit";
            menuExit.Click += new EventHandler(menuExit_Click);
            contextMenu.MenuItems.Add(menuExit);
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Visible = true;

            // 付箋を読み込み（表示）
            fusenctrl = new FusenController();
            fusenctrl.CreateFusens();
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        void menuNew_Click(object sender, EventArgs e)
        {
            new WriteFusen();
        }
    }
}
