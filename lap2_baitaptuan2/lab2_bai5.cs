using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai5 : Form
    {
        private Dictionary<string, decimal> giaGocPhim = new Dictionary<string, decimal>()
        {
            { "Đào, phở và piano", 45000m },
            { "Mai", 100000m },
            { "Gặp lại chị bầu", 70000m },
            { "Tarot", 90000m }
        };

        private Dictionary<string, List<int>> phongChieuTheoPhim = new Dictionary<string, List<int>>()
        {
            { "Đào, phở và piano", new List<int> { 1, 2, 3 } },
            { "Mai", new List<int> { 2, 3 } },
            { "Gặp lại chị bầu", new List<int> { 1 } },
            { "Tarot", new List<int> { 3 } }
        };

        private Dictionary<string, decimal> heSoGiaGhe = new Dictionary<string, decimal>();

        private Dictionary<int, HashSet<string>> ghesDaMuaTheoPhong = new Dictionary<int, HashSet<string>>()
        {
            { 1, new HashSet<string>() },
            { 2, new HashSet<string>() },
            { 3, new HashSet<string>() }
        };

        private Dictionary<string, decimal> doanhThuPhim = new Dictionary<string, decimal>();
        private Dictionary<string, int> veBanRaPhim = new Dictionary<string, int>();

        private ComboBox cboPhim;
        private ComboBox cboPhong;
        private TextBox txtHoTen;
        private RichTextBox rtbHoaDon;
        private ProgressBar progressBar;
        private CheckedListBox chkGhe;

        public lab2_bai5()
        {
            InitializeComponent();
        }

        private void lab2_bai5_Load(object sender, EventArgs e)
        {
            TimKiemControlTuDong();
            KhoiTaoDuLieuBanDau();
        }

        private void TimKiemControlTuDong()
        {
            var allControls = LayTatCaControls(this);

            var listComboBox = allControls.OfType<ComboBox>().ToList();

            if (listComboBox.Count >= 2)
            {
                var cb1 = listComboBox.FirstOrDefault(c => c.Name.Equals("comboBox1", StringComparison.OrdinalIgnoreCase));
                var cb2 = listComboBox.FirstOrDefault(c => c.Name.Equals("comboBox2", StringComparison.OrdinalIgnoreCase));

                if (cb1 != null && cb2 != null)
                {
                    cboPhim = cb1;
                    cboPhong = cb2;
                }
                else
                {
                    
                    var sortedList = listComboBox.OrderBy(c => c.Top).ThenBy(c => c.Left).ToList();
                    cboPhim = sortedList[0];
                    cboPhong = sortedList[1];
                }
            }
            else if (listComboBox.Count == 1)
            {
                cboPhim = listComboBox[0];
            }

            txtHoTen = allControls.OfType<TextBox>().FirstOrDefault();
            rtbHoaDon = allControls.OfType<RichTextBox>().FirstOrDefault();
            progressBar = allControls.OfType<ProgressBar>().FirstOrDefault();
            chkGhe = allControls.OfType<CheckedListBox>().FirstOrDefault();

            if (cboPhim != null)
            {
                cboPhim.SelectedIndexChanged -= ComboBoxPhim_SelectedIndexChanged;
                cboPhim.SelectedIndexChanged += ComboBoxPhim_SelectedIndexChanged;
            }

            if (cboPhong != null)
            {
                cboPhong.SelectedIndexChanged -= ComboBoxPhong_SelectedIndexChanged;
                cboPhong.SelectedIndexChanged += ComboBoxPhong_SelectedIndexChanged;
            }

            if (chkGhe != null)
            {
                chkGhe.ItemCheck -= ChkGhe_ItemCheck;
                chkGhe.ItemCheck += ChkGhe_ItemCheck;
            }
        }

        private IEnumerable<Control> LayTatCaControls(Control root)
        {
            var controls = root.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => LayTatCaControls(ctrl)).Concat(controls);
        }

        private void KhoiTaoDuLieuBanDau()
        {
            string[] veVot = { "A1", "A5", "C1", "C5" };
            foreach (var g in veVot) heSoGiaGhe[g] = 0.25m;

            string[] veThuong = { "A2", "A3", "A4", "C2", "C3", "C4", "B1", "B5" };
            foreach (var g in veThuong) heSoGiaGhe[g] = 1.0m;

            string[] veVIP = { "B2", "B3", "B4" };
            foreach (var g in veVIP) heSoGiaGhe[g] = 2.0m;

            foreach (var key in giaGocPhim.Keys)
            {
                doanhThuPhim[key] = 0;
                veBanRaPhim[key] = 0;
            }

            if (cboPhim != null)
            {
                cboPhim.DataSource = giaGocPhim.Keys.ToList();
            }
        }

        private void btnDocFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Chọn file input5.txt"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(ofd.FileName);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            string tenPhim = parts[0].Trim();
                            if (decimal.TryParse(parts[1].Trim(), out decimal giaGoc))
                            {
                                List<int> dsPhong = parts[2].Split(',')
                                                            .Select(p => Regex.Match(p.Trim(), @"\d+").Value)
                                                            .Where(v => !string.IsNullOrEmpty(v))
                                                            .Select(int.Parse)
                                                            .ToList();

                                giaGocPhim[tenPhim] = giaGoc;
                                phongChieuTheoPhim[tenPhim] = dsPhong;
                                if (!doanhThuPhim.ContainsKey(tenPhim)) doanhThuPhim[tenPhim] = 0;
                                if (!veBanRaPhim.ContainsKey(tenPhim)) veBanRaPhim[tenPhim] = 0;
                            }
                        }
                    }

                    if (cboPhim != null)
                    {
                        cboPhim.DataSource = giaGocPhim.Keys.ToList();
                    }
                    MessageBox.Show("Đọc dữ liệu từ file input5.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ComboBoxPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPhim == null || cboPhim.SelectedItem == null) return;

            string phimChon = cboPhim.SelectedItem.ToString();
            if (cboPhong != null && phongChieuTheoPhim.ContainsKey(phimChon))
            {
                cboPhong.DataSource = phongChieuTheoPhim[phimChon];
                CapNhatTrangThaiGhe();
            }
        }

        private void ComboBoxPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTrangThaiGhe();
        }

        private void CapNhatTrangThaiGhe()
        {
            if (cboPhong == null || cboPhong.SelectedItem == null || chkGhe == null) return;

            string textPhong = cboPhong.SelectedItem.ToString();
            string digitsOnly = Regex.Match(textPhong, @"\d+").Value;

            if (int.TryParse(digitsOnly, out int phongHienTai))
            {
                if (!ghesDaMuaTheoPhong.ContainsKey(phongHienTai))
                {
                    ghesDaMuaTheoPhong[phongHienTai] = new HashSet<string>();
                }

                HashSet<string> dsGheDaBan = ghesDaMuaTheoPhong[phongHienTai];

                for (int i = 0; i < chkGhe.Items.Count; i++)
                {
                    string tenGhe = chkGhe.Items[i].ToString().Trim();
                    bool daBan = dsGheDaBan.Contains(tenGhe);

                    chkGhe.SetItemChecked(i, false);
                }
            }
        }

        private void ChkGhe_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int count = chkGhe.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) count++;
            else if (e.NewValue == CheckState.Unchecked) count--;

            if (count > 2)
            {
                e.NewValue = CheckState.Unchecked;
                MessageBox.Show("Mỗi khách hàng không được chọn nhiều hơn 2 vé!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboPhim == null || cboPhim.SelectedItem == null) return;

            string hoTen = txtHoTen != null ? txtHoTen.Text.Trim() : "";
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (txtHoTen != null) txtHoTen.Focus();
                return;
            }

            if (chkGhe == null || chkGhe.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phim = cboPhim.SelectedItem.ToString();
            string textPhong = cboPhong.SelectedItem.ToString();
            int phong = int.Parse(Regex.Match(textPhong, @"\d+").Value);

            decimal giaChuan = giaGocPhim[phim];
            decimal tongTien = 0;
            List<string> dsTenGheChon = new List<string>();

            foreach (var item in chkGhe.CheckedItems)
            {
                string tenGhe = item.ToString().Trim();
                dsTenGheChon.Add(tenGhe);

                decimal giaVe = heSoGiaGhe.ContainsKey(tenGhe) ? giaChuan * heSoGiaGhe[tenGhe] : giaChuan;
                tongTien += giaVe;

                ghesDaMuaTheoPhong[phong].Add(tenGhe);
            }

            doanhThuPhim[phim] += tongTien;
            veBanRaPhim[phim] += dsTenGheChon.Count;

            if (rtbHoaDon != null)
            {
                rtbHoaDon.Clear();
                rtbHoaDon.AppendText("============= HÓA ĐƠN VÉ PHIM =============\n");
                rtbHoaDon.AppendText($"Họ và tên KH: {hoTen}\n");
                rtbHoaDon.AppendText($"Tên phim    : {phim}\n");
                rtbHoaDon.AppendText($"Phòng chiếu : Phòng {phong}\n");
                rtbHoaDon.AppendText($"Vé đã chọn  : {string.Join(", ", dsTenGheChon)}\n");
                rtbHoaDon.AppendText($"Thành tiền  : {tongTien:N0} VNĐ\n");
                rtbHoaDon.AppendText("=================================================");
            }

            MessageBox.Show("Thanh toán vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CapNhatTrangThaiGhe();
        }

        private async void btnXuatFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                FileName = "output5.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (progressBar != null)
                {
                    progressBar.Value = 0;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;

                    for (int i = 0; i <= 50; i += 10)
                    {
                        progressBar.Value = i;
                        await Task.Delay(40);
                    }
                }

                var danhSachXepHang = doanhThuPhim.OrderByDescending(p => p.Value).ToList();

                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.WriteLine("================ BÁO CÁO THỐNG KÊ DOANH THU PHÒNG VÉ ================");
                    sw.WriteLine(string.Format("{0,-20} | {1,-8} | {2,-8} | {3,-10} | {4,-15} | {5,-8}",
                        "Tên Phim", "Vé Bán", "Vé Tồn", "Tỷ Lệ Bán", "Doanh Thu", "Xếp Hạng"));
                    sw.WriteLine(new string('-', 80));

                    for (int index = 0; index < danhSachXepHang.Count; index++)
                    {
                        var item = danhSachXepHang[index];
                        string tenPhim = item.Key;
                        decimal doanhThu = item.Value;
                        int veBan = veBanRaPhim[tenPhim];
                        int tongGhe = phongChieuTheoPhim[tenPhim].Count * 15;
                        int veTon = tongGhe - veBan;
                        double tyLe = tongGhe > 0 ? (double)veBan / tongGhe * 100 : 0;
                        int xepHang = index + 1;

                        sw.WriteLine(string.Format("{0,-20} | {1,-8} | {2,-8} | {3,-9:F1}% | {4,-11:N0} VNĐ | {5,-8}",
                            tenPhim, veBan, veTon, tyLe, doanhThu, xepHang));
                    }
                }

                if (progressBar != null)
                {
                    for (int i = 60; i <= 100; i += 10)
                    {
                        progressBar.Value = i;
                        await Task.Delay(40);
                    }
                }

                MessageBox.Show("Xuất file output5.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtHoTen != null) txtHoTen.Clear();
            if (rtbHoaDon != null) rtbHoaDon.Clear();
            if (cboPhim != null && cboPhim.Items.Count > 0) cboPhim.SelectedIndex = 0;
            CapNhatTrangThaiGhe();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (cboPhim == null || cboPhim.SelectedItem == null) return;

            string hoTen = txtHoTen != null ? txtHoTen.Text.Trim() : "";
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (txtHoTen != null) txtHoTen.Focus();
                return;
            }

            if (chkGhe == null || chkGhe.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phim = cboPhim.SelectedItem.ToString();
            string textPhong = cboPhong.SelectedItem.ToString();
            int phong = int.Parse(Regex.Match(textPhong, @"\d+").Value);

            decimal giaChuan = giaGocPhim[phim];
            decimal tongTien = 0;
            List<string> dsTenGheChon = new List<string>();

            foreach (var item in chkGhe.CheckedItems)
            {
                string tenGhe = item.ToString().Trim();
                dsTenGheChon.Add(tenGhe);

                decimal giaVe = heSoGiaGhe.ContainsKey(tenGhe) ? giaChuan * heSoGiaGhe[tenGhe] : giaChuan;
                tongTien += giaVe;

                ghesDaMuaTheoPhong[phong].Add(tenGhe);
            }

            doanhThuPhim[phim] += tongTien;
            veBanRaPhim[phim] += dsTenGheChon.Count;

            if (rtbHoaDon != null)
            {
                rtbHoaDon.Clear();
                rtbHoaDon.AppendText("============= HÓA ĐƠN VÉ PHIM =================\n");
                rtbHoaDon.AppendText($"Họ và tên KH: {hoTen}\n");
                rtbHoaDon.AppendText($"Tên phim    : {phim}\n");
                rtbHoaDon.AppendText($"Phòng chiếu : Phòng {phong}\n");
                rtbHoaDon.AppendText($"Vé đã chọn  : {string.Join(", ", dsTenGheChon)}\n");
                rtbHoaDon.AppendText($"Thành tiền  : {tongTien:N0} VNĐ\n");
            }

            MessageBox.Show("Thanh toán vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CapNhatTrangThaiGhe();
    }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát bài tập này không?",
                                                 "Xác nhận thoát",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                FileName = "output5.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (progressBar != null)
                {
                    progressBar.Value = 0;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;

                    for (int i = 0; i <= 50; i += 10)
                    {
                        progressBar.Value = i;
                        await Task.Delay(40);
                    }
                }

                var danhSachXepHang = doanhThuPhim.OrderByDescending(p => p.Value).ToList();

                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.WriteLine("================ BÁO CÁO THỐNG KÊ DOANH THU PHÒNG VÉ ================");
                    sw.WriteLine(string.Format("{0,-20} | {1,-8} | {2,-8} | {3,-10} | {4,-15} | {5,-8}",
                        "Tên Phim", "Vé Bán", "Vé Tồn", "Tỷ Lệ Bán", "Doanh Thu", "Xếp Hạng"));
                    sw.WriteLine(new string('-', 80));

                    for (int index = 0; index < danhSachXepHang.Count; index++)
                    {
                        var item = danhSachXepHang[index];
                        string tenPhim = item.Key;
                        decimal doanhThu = item.Value;
                        int veBan = veBanRaPhim[tenPhim];
                        int tongGhe = phongChieuTheoPhim[tenPhim].Count * 15;
                        int veTon = tongGhe - veBan;
                        double tyLe = tongGhe > 0 ? (double)veBan / tongGhe * 100 : 0;
                        int xepHang = index + 1;

                        sw.WriteLine(string.Format("{0,-20} | {1,-8} | {2,-8} | {3,-9:F1}% | {4,-11:N0} VNĐ | {5,-8}",
                            tenPhim, veBan, veTon, tyLe, doanhThu, xepHang));
                    }
                }

                if (progressBar != null)
                {
                    for (int i = 60; i <= 100; i += 10)
                    {
                        progressBar.Value = i;
                        await Task.Delay(40);
                    }
                }

                MessageBox.Show("Xuất file output5.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Chọn hoặc tạo file input5.txt"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = ofd.FileName;

                    string hoTen = txtHoTen != null ? txtHoTen.Text.Trim() : "";
                    var dsGheChon = chkGhe != null ? chkGhe.CheckedItems.Cast<object>().Select(x => x.ToString().Trim()).ToList() : new List<string>();

                    if (!string.IsNullOrEmpty(hoTen) && cboPhim != null && cboPhim.SelectedItem != null && dsGheChon.Count > 0)
                    {
                        string phim = cboPhim.SelectedItem.ToString();
                        string textPhong = cboPhong != null && cboPhong.SelectedItem != null ? cboPhong.SelectedItem.ToString() : "1";
                        string phong = System.Text.RegularExpressions.Regex.Match(textPhong, @"\d+").Value;

                        string thongTinNhap = $"{hoTen} | {phim} | Phòng {phong} | Ghế: {string.Join(",", dsGheChon)}";

                        using (StreamWriter sw = File.AppendText(filePath))
                        {
                            sw.WriteLine(thongTinNhap);
                        }
                    }

                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            string tenPhim = parts[0].Trim();
                            if (decimal.TryParse(parts[1].Trim(), out decimal giaGoc))
                            {
                                List<int> dsPhong = parts[2].Split(',')
                                                            .Select(p => System.Text.RegularExpressions.Regex.Match(p.Trim(), @"\d+").Value)
                                                            .Where(v => !string.IsNullOrEmpty(v))
                                                            .Select(int.Parse)
                                                            .ToList();

                                giaGocPhim[tenPhim] = giaGoc;
                                phongChieuTheoPhim[tenPhim] = dsPhong;
                                if (!doanhThuPhim.ContainsKey(tenPhim)) doanhThuPhim[tenPhim] = 0;
                                if (!veBanRaPhim.ContainsKey(tenPhim)) veBanRaPhim[tenPhim] = 0;
                            }
                        }
                    }

                    if (cboPhim != null)
                    {
                        cboPhim.DataSource = giaGocPhim.Keys.ToList();
                    }

                    MessageBox.Show("Ghi nhận và cập nhật dữ liệu từ file input5.txt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xử lý file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}