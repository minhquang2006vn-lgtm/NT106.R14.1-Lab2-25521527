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
    public partial class lab2_bai1 : Form
    {
        public lab2_bai1()
        {
            InitializeComponent();
        }

        private void lab2_bai1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string inputPath = "input1.txt";

                if (!File.Exists(inputPath))
                {
                    MessageBox.Show("File input1.txt không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (StreamReader sr = new StreamReader(inputPath, Encoding.UTF8))
                {
                    richTextBox1.Text = sr.ReadToEnd(); 
                }

                MessageBox.Show("Đọc file input1.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = "output1.txt";

                string upperContent = richTextBox1.Text.ToUpper();

                using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    sw.Write(upperContent);
                }

                MessageBox.Show("Ghi file output1.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}