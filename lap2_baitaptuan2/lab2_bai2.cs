using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai2 : Form
    {
        public lab2_bai2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lab2_bai2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"; 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = ofd.SafeFileName.ToString(); 
                        string fileUrl = fs.Name.ToString();           

                        textBox1.Text = fileName;
                        textBox2.Text = fileUrl;

                        FileInfo fileInfo = new FileInfo(ofd.FileName);
                        textBox3.Text = fileInfo.Length.ToString() + " bytes";
                    }

                    using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8))
                    {
                        string content = sr.ReadToEnd();

                        richTextBox1.Text = content;

                        
                        int charCount = content.Length;

                       
                        string[] lines = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        int lineCount = lines.Length;

                        
                        char[] delimiters = new char[] { ' ', '\r', '\n', '\t' };
                        string[] words = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        int wordCount = words.Length;

                        textBox4.Text = lineCount.ToString();
                        textBox5.Text = wordCount.ToString();
                        textBox6.Text = charCount.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}