using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace Yuki
{
    struct Fusens
    {
        public Form form;
        public Data data;
    }

    class FusenController
    {
        private const string fileName = "data.dat";
        private FusenData fusenData;
        public static ArrayList list = new ArrayList();

        public static void DeleteFusen(IntPtr hWnd)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (((Fusens)list[i]).form.Handle == hWnd)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        public FusenController()
        {
            fusenData = new FusenData(fileName);
        }

        public void CreateFusens()
        {
            fusenData.OpenReadFile();
            while (true)
            {
                Fusens fusens;
                fusens.data = fusenData.Read();
                if (fusens.data.memo == "")
                    break;

                Fusen fusen = new Fusen();
                fusens.form = fusen.CreateFusen(fusens.data.memo, fusens.data.point);
                list.Add(fusens);
            }
            fusenData.Close();
        }

        public void WriteFusens()
        {
            fusenData.OpenWriteFile();
            foreach (Fusens fusens in list)
                fusenData.Write(fusens.data.memo, fusens.form.DesktopLocation);
            fusenData.Flush();
            fusenData.Close();
        }
    }
}
