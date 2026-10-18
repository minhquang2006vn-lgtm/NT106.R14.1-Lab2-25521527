using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace lap2_baitaptuan2
{
    public partial class lab2_bai7 : Form
    {
        public lab2_bai7()
        {
            InitializeComponent();
        }

        private void lab2_bai7_Load(object sender, EventArgs e)
        {
            treeView1.BeforeExpand += TreeView1_BeforeExpand;
            treeView1.AfterSelect += TreeView1_AfterSelect;

            LoadDriveNodes();
        }

        private void LoadDriveNodes()
        {
            treeView1.Nodes.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    TreeNode driveNode = new TreeNode(drive.Name)
                    {
                        Tag = drive.RootDirectory.FullName
                    };

                    driveNode.Nodes.Add("...");
                    treeView1.Nodes.Add(driveNode);
                }
            }
        }

        private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode currentNode = e.Node;

            if (currentNode.Nodes.Count == 1 && currentNode.Nodes[0].Text == "...")
            {
                currentNode.Nodes.Clear();
                string path = currentNode.Tag.ToString();

                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path);

                    foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
                    {
                        if ((subDir.Attributes & FileAttributes.Hidden) != 0) continue;

                        TreeNode dirNode = new TreeNode(subDir.Name)
                        {
                            Tag = subDir.FullName
                        };
                        dirNode.Nodes.Add("...");
                        currentNode.Nodes.Add(dirNode);
                    }

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        if ((file.Attributes & FileAttributes.Hidden) != 0) continue;

                        TreeNode fileNode = new TreeNode(file.Name)
                        {
                            Tag = file.FullName
                        };
                        currentNode.Nodes.Add(fileNode);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Bạn không có quyền truy cập vào thư mục này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;

            string path = e.Node.Tag.ToString();

            if (File.Exists(path))
            {
                DisplayFileContent(path);
            }
            else
            {
                pictureBox1.Visible = false;
                richTextBox1.Visible = false;
            }
        }

        private void DisplayFileContent(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();

            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif")
            {
                try
                {
                    richTextBox1.Visible = false;

                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                    pictureBox1.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Không thể đọc định dạng ảnh này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (ext == ".txt" || ext == ".log" || ext == ".json" || ext == ".cs" || ext == ".xml" || ext == ".ini")
            {
                try
                {
                    pictureBox1.Visible = false;
                    richTextBox1.Text = File.ReadAllText(filePath);
                    richTextBox1.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Không thể đọc file văn bản này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                pictureBox1.Visible = false;
                richTextBox1.Visible = false;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}