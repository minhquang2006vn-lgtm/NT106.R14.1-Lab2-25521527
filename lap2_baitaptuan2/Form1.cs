using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             lab2_bai1 f1 = new lab2_bai1();
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lab2_bai2 f2 = new lab2_bai2();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lab2_bai3 f3 = new lab2_bai3();
            f3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lab2_bai4 f4 = new lab2_bai4();
            f4.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            lab2_bai5 f5 = new lab2_bai5();
            f5.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            lab2_bai6 f6 = new lab2_bai6();
            f6.Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            lab2_bai7 f7 = new lab2_bai7();
            f7.Show();
        }
    }
}