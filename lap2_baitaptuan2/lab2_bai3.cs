using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai3 : Form
    {
        public lab2_bai3()
        {
            InitializeComponent();
        }

        private void lab2_bai3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string inputPath = "input3.txt";

                if (!File.Exists(inputPath))
                {
                    MessageBox.Show("File input3.txt không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string content = File.ReadAllText(inputPath, Encoding.UTF8);
                richTextBox1.Text = content;

                MessageBox.Show("Đọc file input3.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string inputPath = "input3.txt";
                string outputPath = "output3.txt";

                if (!File.Exists(inputPath))
                {
                    MessageBox.Show("Vui lòng đọc file input3.txt trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] lines = File.ReadAllLines(inputPath, Encoding.UTF8);
                StringBuilder outputContent = new StringBuilder();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        double result = EvaluateExpression(line);

                        string resultStr = (result % 1 == 0) ? result.ToString() : Math.Round(result, 2).ToString();
                        outputContent.AppendLine($"{line} = {resultStr}");
                    }
                    catch
                    {
                        outputContent.AppendLine($"{line} = Lỗi biểu thức");
                    }
                }

                richTextBox2.Text = outputContent.ToString();

                File.WriteAllText(outputPath, outputContent.ToString(), Encoding.UTF8);

                MessageBox.Show("Tính toán và ghi file output3.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính toán/ghi file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát khỏi bài tập này",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        #region 

        private int Precedence(char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            return 0;
        }

        private double EvaluateExpression(string expression)
        {
            Stack<double> values = new Stack<double>();
            Stack<char> ops = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (c == ' ') continue;

                if (char.IsDigit(c) || c == '.')
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        sb.Append(expression[i]);
                        i++;
                    }
                    i--;
                    double number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                    values.Push(number);
                }
                else if (c == '(')
                {
                    ops.Push(c);
                }
                else if (c == ')')
                {
                    while (ops.Count > 0 && ops.Peek() != '(')
                    {
                        values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    }
                    if (ops.Count > 0) ops.Pop();
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    int prevIndex = i - 1;
                    while (prevIndex >= 0 && expression[prevIndex] == ' ')
                    {
                        prevIndex--;
                    }

                    bool isUnaryMinus = (c == '-') && (prevIndex < 0 || "+-*/(".Contains(expression[prevIndex]));

                    if (isUnaryMinus)
                    {
                        values.Push(0);
                    }

                    while (ops.Count > 0 && Precedence(ops.Peek()) >= Precedence(c))
                    {
                        values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    }
                    ops.Push(c);
                }
            }

            while (ops.Count > 0)
            {
                values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
            }

            if (values.Count > 0)
            {
                return values.Pop();
            }

            throw new ArgumentException("Biểu thức không hợp lệ");
        }

        private double ApplyOp(char op, double b, double a)
        {
            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/':
                    if (b == 0) throw new DivideByZeroException("Chia cho 0");
                    return a / b;
            }
            return 0;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();

            richTextBox1.Focus();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}