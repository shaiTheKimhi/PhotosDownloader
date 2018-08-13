using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Web;

namespace downloader
{
    public partial class Form1 : Form
    {
        const int x = 200, y = 150;
        List<PictureBox> pictures = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool loaded = pictures.Count != 0;
            int index = 0;
            int line = 0;
            int i = 0;
            string url = "https://www.google.com/search?q=" + textBox1.Text + "&tbm=isch";
            int len = int.Parse(textBox2.Text);
            List<string> images = GetUrl(SyncHttpRequest(url), len);
            Point pos = zero.Location;
            foreach(string item in images)
            {
                byte[] arr = GetImage(item);
                if (!loaded)
                {
                    using (var ms = new MemoryStream(arr))
                    {
                        //pictureBox1.Image = Image.FromStream(ms);
                        pictures.Add(CreateImage(Image.FromStream(ms), pos));
                    }
                    pos.X += x + 25;
                    if (pos.X > this.Width)
                    {
                        pos.X = zero.Location.X;
                        pos.Y = zero.Location.Y + (y + 10) * (line++);
                        if(pos.Y > this.Height)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    using (var ms = new MemoryStream(arr))
                    {
                        pictures[index++].Image = Image.FromStream(ms);
                    }
                }
            }
        }
        private PictureBox CreateImage(Image img, Point pos)
        {
            PictureBox pic = new PictureBox();
            pic.Size = new Size(x, y);
            pic.Location = pos;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = img;
            this.Controls.Add(pic);
            return pic;
        }
        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000);

                    return bytes;
                }
            }

            return null;
        }
        private string SyncHttpRequest(string url)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream st = response.GetResponseStream())
            {
                if (st == null)
                    return "";
                using (StreamReader sr = new StreamReader(st))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }
        /// <summary>
        /// This functions returns the url of the images from the html page
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private List<String> GetUrl(string html, int i = int.MaxValue)
        {
            var urls = new List<string>();
            int index = html.IndexOf("class=\"images_table\"", StringComparison.Ordinal);
            int index2 = 0, j = 0;
            index = html.IndexOf("<img", index, StringComparison.Ordinal);

            while(index >= 0 && j < i)
            {
                index = html.IndexOf("src=\"", index, StringComparison.Ordinal);
                index = index + 5;
                index2 = html.IndexOf("\"", index, StringComparison.Ordinal);
                urls.Add(html.Substring(index, index2 - index));
                index = html.IndexOf("<img", index, StringComparison.Ordinal);
                j++;
            }
            return urls;
        }
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream);
            mStream.Dispose();
            return bm;
        }
    }
}
