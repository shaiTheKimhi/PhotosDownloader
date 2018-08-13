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
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Form1)this.sender).url = textBox1.Text;
            ((Form1)this.sender).ImagesLimit = int.Parse(textBox2.Text);
            ((Form1)this.sender).PopupEntry();
            this.Close();
        }
        private bool checkUrl(string url)
        {
            //TODO :  check that url is valid
            return true;
        }
        public bool isNumeric(string number)
        {
            //TODO : check number is valid
            return true;
        }
    }
}
