using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;


namespace Yuki
{
    struct Data
    {
        public string memo;
        public Point point;
    }

    class FusenData : Form
    {
        private string fileName;
        private FileStream fileStream;

        public FusenData(string fileName)
        {
            this.fileName = fileName;
        }

        public void OpenReadFile()
        {
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
        }

        public void OpenWriteFile()
        {
            fileStream = new FileStream(fileName, FileMode.Truncate, FileAccess.Write);
        }

        public void Flush()
        {
            fileStream.Flush();
        }

        public new void Close()
        {
            fileStream.Close();
        }

        public void Write(string memo, Point point)
        {
            UnicodeEncoding uniEncoding = new UnicodeEncoding();

            string data = memo;
            data += "\n";
            data += point.X.ToString();
            data += ",";
            data += point.Y.ToString();
            data += "\n";

            fileStream.Write(uniEncoding.GetBytes(data), 0, uniEncoding.GetByteCount(data));
            Flush();
        }

        public Data Read()
        {
            string memo = "", x = "", y = "", buffer;
            Point point;
            Data data = new Data();

            memo = GetStringFromStream(fileStream);
            buffer = GetStringFromStream(fileStream);

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == ',')
                {
                    for (int j = i + 1; j < buffer.Length; j++)
                        y += buffer[j];
                    break;
                }
                x += buffer[i];
            }

            if (memo == "")
            {
                data.memo = memo;
                return data;
            }

            point = new Point(int.Parse(x), int.Parse(y));

            data.memo = memo;
            data.point = point;

            return data;
        }

        public static string GetStringFromStream(Stream stream)
        {
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                int memory1 = stream.ReadByte();
                int memory2 = stream.ReadByte();
                int memory = memory1 + (memory2 << 8);
                if (memory1 == -1 || memory2 == -1 || memory == '\n')
                    break;
                sb.Append((char)memory);
            }

            return sb.ToString();
        }
    }
}