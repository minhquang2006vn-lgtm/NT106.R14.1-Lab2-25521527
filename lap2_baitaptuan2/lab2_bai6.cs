using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai6 : Form
    {
        private string dbPath = "MonAnDB.db";
        private string connectionString => $"Data Source={dbPath};Version=3;";

        public class MonAnModel
        {
            public int IDMA { get; set; }
            public string TenMonAn { get; set; }
            public string HinhAnh { get; set; }
            public string TenNguoiDang { get; set; }
        }

        public class NguoiDungModel
        {
            public int IDNCC { get; set; }
            public string HoVaTen { get; set; }
            public override string ToString() => HoVaTen;
        }

        private List<MonAnModel> dsMonAn = new List<MonAnModel>();

        public lab2_bai6()
        {
            InitializeComponent();
        }

        private void lab2_bai6_Load(object sender, EventArgs e)
        {
            KhoiTaoCSDL();
            CaiDatListView();
            TaiDanhSachNguoiDung();
            TaiDanhSachMonAn();
            GanSuKienTuDong();
        }

        private void KhoiTaoCSDL()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string qNguoiDung = @"
                    CREATE TABLE IF NOT EXISTS NguoiDung (
                        IDNCC INTEGER PRIMARY KEY AUTOINCREMENT,
                        HoVaTen TEXT NOT NULL,
                        QuyenHan TEXT
                    );";

                string qMonAn = @"
                    CREATE TABLE IF NOT EXISTS MonAn (
                        IDMA INTEGER PRIMARY KEY AUTOINCREMENT,
                        TenMonAn TEXT NOT NULL,
                        HinhAnh TEXT,
                        IDNCC INTEGER,
                        FOREIGN KEY (IDNCC) REFERENCES NguoiDung(IDNCC)
                    );";

                using (var cmd = new SQLiteCommand(qNguoiDung, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SQLiteCommand(qMonAn, conn)) cmd.ExecuteNonQuery();

                string checkQuery = "SELECT COUNT(*) FROM NguoiDung";
                using (var cmd = new SQLiteCommand(checkQuery, conn))
                {
                    long count = (long)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string seedQuery = @"
                            INSERT INTO NguoiDung (HoVaTen, QuyenHan) VALUES ('Nguyễn Văn A', 'User');
                            INSERT INTO NguoiDung (HoVaTen, QuyenHan) VALUES ('Trần Thị B', 'Admin');
                            INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES ('Cơm tấm', 'comtam.jpg', 1);
                            INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES ('Phở bò', 'phobo.jpg', 2);";
                        using (var seedCmd = new SQLiteCommand(seedQuery, conn)) seedCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CaiDatListView()
        {
            if (listView1 != null)
            {
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                listView1.GridLines = true;
                listView1.Columns.Clear();
                listView1.Columns.Add("ID", 40);
                listView1.Columns.Add("Tên món ăn", 150);
                listView1.Columns.Add("Người đóng góp", 130);
                listView1.Columns.Add("Hình ảnh", 100);
            }
        }

        private void TaiDanhSachNguoiDung()
        {
            if (comboBox1 == null) return;
            comboBox1.Items.Clear();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDNCC, HoVaTen FROM NguoiDung WHERE HoVaTen NOT LIKE '%.jpg' AND HoVaTen NOT LIKE '%.png' AND HoVaTen NOT LIKE '%.jpeg'";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(new NguoiDungModel
                        {
                            IDNCC = Convert.ToInt32(reader["IDNCC"]),
                            HoVaTen = reader["HoVaTen"].ToString()
                        });
                    }
                }
            }
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void TaiDanhSachMonAn()
        {
            dsMonAn.Clear();
            if (listView1 != null) listView1.Items.Clear();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT m.IDMA, m.TenMonAn, m.HinhAnh, n.HoVaTen 
                    FROM MonAn m 
                    INNER JOIN NguoiDung n ON m.IDNCC = n.IDNCC";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mon = new MonAnModel
                        {
                            IDMA = Convert.ToInt32(reader["IDMA"]),
                            TenMonAn = reader["TenMonAn"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString(),
                            TenNguoiDang = reader["HoVaTen"].ToString()
                        };

                        dsMonAn.Add(mon);

                        if (listView1 != null)
                        {
                            ListViewItem item = new ListViewItem(mon.IDMA.ToString());
                            item.SubItems.Add(mon.TenMonAn);
                            item.SubItems.Add(mon.TenNguoiDang);
                            item.SubItems.Add(mon.HinhAnh);
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void GanSuKienTuDong()
        {
            if (button1 != null)
            {
                button1.Click -= button1_Click;
                button1.Click += button1_Click;
            }
            if (button2 != null)
            {
                button2.Click -= button2_Click;
                button2.Click += button2_Click;
            }
            if (button3 != null)
            {
                button3.Click -= button3_Click;
                button3.Click += button3_Click;
            }
            if (button4 != null)
            {
                button4.Click -= button4_Click;
                button4.Click += button4_Click;
            }
            if (button5 != null)
            {
                button5.Click -= button5_Click;
                button5.Click += button5_Click;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(ofd.FileName);
                TextBox txtImg = TimTextBoxGanNhat(button1);
                if (txtImg != null) txtImg.Text = fileName;
                else if (textBox2 != null) textBox2.Text = fileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tenMon = "";
            string hinhAnh = "";

            TextBox txtA = textBox3;
            TextBox txtB = textBox2;

            if (txtA != null && (txtA.Text.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || txtA.Text.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
            {
                hinhAnh = txtA.Text.Trim();
                if (txtB != null) tenMon = txtB.Text.Trim();
            }
            else if (txtB != null && (txtB.Text.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || txtB.Text.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
            {
                hinhAnh = txtB.Text.Trim();
                if (txtA != null) tenMon = txtA.Text.Trim();
            }
            else
            {
                if (txtA != null) tenMon = txtA.Text.Trim();
                if (txtB != null) hinhAnh = txtB.Text.Trim();
            }

            if (string.IsNullOrWhiteSpace(tenMon))
            {
                MessageBox.Show("Vui lòng nhập tên món ăn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nguoiDung = comboBox1?.SelectedItem as NguoiDungModel;
            if (nguoiDung == null)
            {
                MessageBox.Show("Vui lòng chọn người đóng góp từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES (@ten, @anh, @idncc)";
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", tenMon);
                    cmd.Parameters.AddWithValue("@anh", hinhAnh);
                    cmd.Parameters.AddWithValue("@idncc", nguoiDung.IDNCC);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Thêm món ăn mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (textBox2 != null) textBox2.Clear();
            if (textBox3 != null) textBox3.Clear();
            TaiDanhSachMonAn();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dsMonAn.Count == 0)
            {
                MessageBox.Show("Chưa có món ăn nào trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Random rand = new Random();
            int index = rand.Next(0, dsMonAn.Count);
            var monChon = dsMonAn[index];

            if (textBox4 != null) textBox4.Text = monChon.TenMonAn.ToUpper();

            if (textBox5 != null) textBox5.Text = monChon.TenNguoiDang;
            if (textBox1 != null && textBox1 != textBox2 && textBox1 != textBox3 && textBox1 != textBox4)
                textBox1.Text = monChon.TenNguoiDang;

            string imgPath = Path.Combine(Application.StartupPath, "Images", monChon.HinhAnh);
            if (pictureBox1 != null)
            {
                if (File.Exists(imgPath))
                {
                    pictureBox1.Image = Image.FromFile(imgPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("DELETE FROM MonAn;", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SQLiteCommand("DELETE FROM NguoiDung WHERE HoVaTen LIKE '%.jpg' OR HoVaTen LIKE '%.png';", conn)) cmd.ExecuteNonQuery();

                using (var cmd = new SQLiteCommand("DELETE FROM sqlite_sequence WHERE name='MonAn';", conn)) cmd.ExecuteNonQuery();

                string resetSeedQuery = @"
                    INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES ('Cơm tấm', 'comtam.jpg', 1);
                    INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES ('Phở bò', 'phobo.jpg', 2);";
                using (var cmd = new SQLiteCommand(resetSeedQuery, conn)) cmd.ExecuteNonQuery();
            }

            TaiDanhSachNguoiDung();
            TaiDanhSachMonAn();

            if (textBox2 != null) textBox2.Clear();
            if (textBox3 != null) textBox3.Clear();
            if (textBox4 != null) textBox4.Clear();
            if (textBox5 != null) textBox5.Clear();
            if (pictureBox1 != null) pictureBox1.Image = null;

            MessageBox.Show("Đã làm sạch CSDL và khôi phục về 2 món mặc định ban đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TextBox txtUser = TimTextBoxGanNhat(button5);
            if (txtUser == null || string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenNguoiMoi = txtUser.Text.Trim();

            if (tenNguoiMoi.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || tenNguoiMoi.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Tên người dùng không được chứa đuôi file ảnh!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string insertUserQuery = "INSERT INTO NguoiDung (HoVaTen, QuyenHan) VALUES (@hoten, 'User');";
                using (var cmd = new SQLiteCommand(insertUserQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@hoten", tenNguoiMoi);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Thêm người dùng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtUser.Clear();
            TaiDanhSachNguoiDung();
        }

        private TextBox TimTextBoxGanNhat(Button btn)
        {
            if (btn == null || btn.Parent == null) return null;
            TextBox ganNhat = null;
            double khoangCachNhoNhat = double.MaxValue;

            foreach (Control c in btn.Parent.Controls)
            {
                if (c is TextBox txt)
                {
                    double dx = btn.Left - txt.Left;
                    double dy = btn.Top - txt.Top;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist < khoangCachNhoNhat)
                    {
                        khoangCachNhoNhat = dist;
                        ganNhat = txt;
                    }
                }
            }
            return ganNhat;
        }

        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter_1(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}