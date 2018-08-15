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
using System.Threading;

namespace downloader
{
    public partial class Form1 : Form
    {
        const int yMargin = 10, xMargin = 25;
        const int x = 100, y = 75;
        List<PictureBox> pictures = new List<PictureBox>();
        public string url = "";
        public int ImagesLimit = int.MaxValue, page = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Getting data from popup
            InputPopup f = new InputPopup(this);
            this.Enabled = false;
            f.Show();
            //Now this.url and this.ImagesLimit should contain needed data

           
        }
        public void PopupEntry()
        {
            this.Enabled = true;
            //Makes http request
            bool loaded = pictures.Count != 0;
            int index = 0;
            string url = "https://www.google.com/search?q=" + this.url+ "&tbm=isch";
            int len = this.ImagesLimit;
            List<string> images = GetUrl(SyncHttpRequest(url), len);
            Point pos = zero.Location;
            foreach (string item in images)
            {
                byte[] arr = GetImage(item);
                if (!loaded)
                {
                    PictureBox pic;
                    using (var ms = new MemoryStream(arr))
                    {
                        //pictureBox1.Image = Image.FromStream(ms);
                        pic = CreateImage(Image.FromStream(ms), pos);
                        pictures.Add(pic);
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
            LoadPictures();
        }
        private void LoadPictures()
        {
            int line = 0;
            int amount = AmountOfPictures();
            MessageBox.Show(amount.ToString());
            Point pos = zero.Location;
            foreach (PictureBox pic in pictures)
            {
                pic.Location = pos;
                this.Controls.Add(pic);

                pos.X += x + 25;
                if (pos.X + x > this.Width)
                {
                    pos.X = zero.Location.X;
                    pos.Y = zero.Location.Y + (y + 10) * (line++);
                    if (pos.Y + y > this.Height)
                    {
                        //TODO: fix pictures don't get out of form
                        return;
                    }
                }
                
            }
        }
        private int AmountOfPictures()
        {
            return (this.Size.Width - zero.Location.X / (x + xMargin)) * (this.Size.Height - zero.Location.Y / (y + yMargin));
        }
        private PictureBox CreateImage(Image img, Point pos)
        {
            PictureBox pic = new PictureBox();
            pic.Size = new Size(x, y);
            pic.Location = pos;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = img;
            //this.Controls.Add(pic);
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
        }
        //TOOD : change to async http request (very important)
        public string SyncHttpRequest(string url)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            button1_Click(null, null);
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
