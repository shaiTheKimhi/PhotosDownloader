using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace downloader
{
    public partial class InputPopup : Form
    {
        Form sender;
        public InputPopup(Form sender)
        {
            InitializeComponent();
            this.sender = sender;
        }

        private void InputPopup_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkSearch(textBox1.Text) || !isNumeric(textBox2.Text))
                return;
            ((Form1)this.sender).url = textBox1.Text;
            ((Form1)this.sender).ImagesLimit = int.Parse(textBox2.Text);
            ((Form1)this.sender).PopupEntry();
            this.Close();
        }
        private bool checkSearch(string url)
        {
            string invalid = "!@#%^&*\"";
            foreach(char c in invalid)
            {
                if (url.Contains(c))
                    return false;
            }
            return true;
        }
        public bool isNumeric(string number)
        {
            foreach(char c in number)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
