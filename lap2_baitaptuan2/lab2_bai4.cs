using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai4 : Form
    {
        private List<SinhVien> inputList = new List<SinhVien>();
        private List<SinhVien> outputList = new List<SinhVien>();
        private int currentIndex = 0;

        public lab2_bai4()
        {
            InitializeComponent();

            btnAdd.Click += btnAdd_Click;
            btnWrite.Click += btnWrite_Click;
            btnRead.Click += btnRead_Click;
            btnBack.Click += btnBack_Click;
            btnNext.Click += btnNext_Click;
        }

        private void lab2_bai4_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNameInput.Text))
                {
                    MessageBox.Show("Vui lòng nhập Họ và Tên sinh viên!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(txtIDInput.Text.Trim(), @"^\d{8}$"))
                {
                    MessageBox.Show("Mã số sinh viên phải là một số có đúng 8 chữ số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(txtPhoneInput.Text.Trim(), @"^0\d{9}$"))
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng số 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txtCourse1Input.Text.Trim(), out float c1) || c1 < 0 || c1 > 10 ||
                    !float.TryParse(txtCourse2Input.Text.Trim(), out float c2) || c2 < 0 || c2 > 10 ||
                    !float.TryParse(txtCourse3Input.Text.Trim(), out float c3) || c3 < 0 || c3 > 10)
                {
                    MessageBox.Show("Điểm của từng học phần phải nằm trong khoảng từ 0 đến 10!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SinhVien sv = new SinhVien
                {
                    Name = txtNameInput.Text.Trim(),
                    ID = int.Parse(txtIDInput.Text.Trim()),
                    Phone = txtPhoneInput.Text.Trim(),
                    Course1 = c1,
                    Course2 = c2,
                    Course3 = c3,
                    Average = 0
                };

                inputList.Add(sv);
                MessageBox.Show($"Đã thêm sinh viên: {sv.Name} vào danh sách tạm!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputList.Count == 0)
                {
                    MessageBox.Show("Danh sách sinh viên tạm đang trống! Vui lòng nhập thông tin và bấm 'Thêm SV' trước khi ghi file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string filePath = Path.Combine(Application.StartupPath, "input4.txt");

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    foreach (var sv in inputList)
                    {
                        sw.WriteLine(sv.Name);
                        sw.WriteLine(sv.ID);
                        sw.WriteLine(sv.Phone);
                        sw.WriteLine(sv.Course1);
                        sw.WriteLine(sv.Course2);
                        sw.WriteLine(sv.Course3);
                    }
                }

                MessageBox.Show($"Đã ghi {inputList.Count} sinh viên xuống file input4.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi file input4.txt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = Path.Combine(Application.StartupPath, "input4.txt");

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Không tìm thấy file tại đường dẫn:\n" + filePath, "Lỗi thiếu file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string rawContent = File.ReadAllText(filePath);
                rtbContent.Text = rawContent;

                outputList.Clear();
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i += 6)
                {
                    if (i + 5 >= lines.Length) break;

                    string name = lines[i].Trim();
                    string idStr = lines[i + 1].Trim();
                    string phone = lines[i + 2].Trim();
                    string c1Str = lines[i + 3].Trim();
                    string c2Str = lines[i + 4].Trim();
                    string c3Str = lines[i + 5].Trim();

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(idStr)) continue;

                    if (!Regex.IsMatch(idStr, @"^\d{8}$"))
                    {
                        MessageBox.Show($"MSSV của sinh viên '{name}' không đúng 8 chữ số!", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!Regex.IsMatch(phone, @"^0\d{9}$"))
                    {
                        MessageBox.Show($"SĐT của sinh viên '{name}' không đúng 10 chữ số! Bỏ qua sinh viên này.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue; 
                    }

                    if (!float.TryParse(c1Str, out float c1) || c1 < 0 || c1 > 10 ||
                        !float.TryParse(c2Str, out float c2) || c2 < 0 || c2 > 10 ||
                        !float.TryParse(c3Str, out float c3) || c3 < 0 || c3 > 10)
                    {
                        MessageBox.Show($"Điểm số của sinh viên '{name}' phải nằm trong khoảng 0 - 10!", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SinhVien sv = new SinhVien
                    {
                        Name = name,
                        ID = int.Parse(idStr),
                        Phone = phone,
                        Course1 = c1,
                        Course2 = c2,
                        Course3 = c3
                    };

                    float avg = (sv.Course1 + sv.Course2 + sv.Course3) / 3f;
                    sv.Average = (float)Math.Round(avg, 2);

                    outputList.Add(sv);
                }

                if (outputList.Count == 0)
                {
                    MessageBox.Show("File input4.txt không có dữ liệu hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string outputPath = Path.Combine(Application.StartupPath, "output4.txt");
                using (StreamWriter sw = new StreamWriter(outputPath, false))
                {
                    foreach (var sv in outputList)
                    {
                        sw.WriteLine(sv.Name);
                        sw.WriteLine(sv.ID);
                        sw.WriteLine(sv.Phone);
                        sw.WriteLine(sv.Course1);
                        sw.WriteLine(sv.Course2);
                        sw.WriteLine(sv.Course3);
                        sw.WriteLine(sv.Average);
                    }
                }

                currentIndex = 0;
                DisplayStudentInfo(currentIndex);

                MessageBox.Show("Đọc file và tính ĐTB thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chi tiết lỗi: " + ex.Message, "Lỗi đọc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (outputList == null || outputList.Count == 0) return;

            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayStudentInfo(currentIndex);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (outputList == null || outputList.Count == 0) return;

            if (currentIndex < outputList.Count - 1)
            {
                currentIndex++;
                DisplayStudentInfo(currentIndex);
            }
        }

        private void DisplayStudentInfo(int index)
        {
            if (outputList == null || index < 0 || index >= outputList.Count) return;

            SinhVien sv = outputList[index];

            txtNameOutput.Text = sv.Name;
            txtIDOutput.Text = sv.ID.ToString();
            txtPhoneOutput.Text = sv.Phone;
            txtCourse1Output.Text = sv.Course1.ToString();
            txtCourse2Output.Text = sv.Course2.ToString();
            txtCourse3Output.Text = sv.Course3.ToString();
            txtAverageOutput.Text = sv.Average.ToString("0.00");

            lblPage.Text = $"{index + 1} / {outputList.Count}";
        }

        private void ClearInputFields()
        {
            txtNameInput.Clear();
            txtIDInput.Clear();
            txtPhoneInput.Clear();
            txtCourse1Input.Clear();
            txtCourse2Input.Clear();
            txtCourse3Input.Clear();
            txtAverageInput.Clear();
            txtNameInput.Focus();
        }
    }

    public class SinhVien
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Phone { get; set; }
        public float Course1 { get; set; }
        public float Course2 { get; set; }
        public float Course3 { get; set; }
        public float Average { get; set; }
    }
}