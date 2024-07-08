using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessManageMent
{
    public partial class Form6 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        int intMemId = 0;
        string strPreviousImage = "";
        bool defaultImage = true;
        OpenFileDialog ofd = new OpenFileDialog();
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            ClearAll();
            LoadAllMember();
        }

        private void LoadAllMember()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("ViewAllMembers", con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dt.Columns.Add("Image", typeof(byte[]));

                foreach (DataRow dr in dt.Rows)
                {

                    string imagePath = dr["Picture"].ToString();
                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(Application.StartupPath + "\\images\\" + imagePath))
                    {
                        dr["Image"] = File.ReadAllBytes(Application.StartupPath + "\\images\\" + imagePath);
                    }
                    else
                    {

                        dr["Image"] = File.ReadAllBytes(Application.StartupPath + "\\noimage.jpg");
                    }
                }

                dataGridView2.RowTemplate.Height = 80;
                dataGridView2.DataSource = dt;

                dataGridView2.AllowUserToAddRows = false;

                ((DataGridViewImageColumn)dataGridView2.Columns[dataGridView2.Columns.Count - 1]).ImageLayout = DataGridViewImageCellLayout.Stretch;

                sda.Dispose();
            }
        }

        private void ClearAll()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            radioButton1.Checked = true;
            checkBox2.Checked = true;
            intMemId = 0;

            //button33.Text = "Save";
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\images\\noimage.jpg");
            defaultImage = true;
        }
        bool ValidateMasterDetailForm()
        {
            bool isValid = true;
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Employee name is required");
                isValid = false;
            }
            return isValid;
        }

        string SaveImage(string imgPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(imgPath);
            string ext = Path.GetExtension(imgPath);
            fileName = fileName.Length <= 15 ? fileName : fileName.Substring(0, 15);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            pictureBox1.Image.Save(Application.StartupPath + "\\images\\" + fileName);
            return fileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddMember AddMemForm = new AddMember();
            AddMemForm.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ValidateMasterDetailForm())
            {
                int memId = 0;

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddOrUpdateMember", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemId", intMemId);
                    cmd.Parameters.AddWithValue("@MemCode", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@MemName", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@JoinDate", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@Gender", radioButton1.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@Status", checkBox2.Checked ? 1 : 0);

                    if (defaultImage)
                    {
                        cmd.Parameters.AddWithValue("@Picture", DBNull.Value);
                    }
                    else if (intMemId > 0 && strPreviousImage != "")
                    {
                        cmd.Parameters.AddWithValue("@Picture", strPreviousImage);
                        if (ofd.FileName != strPreviousImage)
                        {
                            var filename = Application.StartupPath + "\\images\\" + strPreviousImage;
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                                pictureBox1.Image = null;
                                //System.IO.File.Delete(filename);
                            }
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Picture", SaveImage(ofd.FileName));
                    }

                    memId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                LoadAllMember();
                ClearAll();
                MessageBox.Show("Submitted Successfully");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Images(.jpg,.png,.png)|*.png;*.jpg; *.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
                if (intMemId == 0)
                {
                    defaultImage = false;
                    strPreviousImage = "";
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(Application.StartupPath + "\\images\\noimage.jpg");
            defaultImage = true;
            strPreviousImage = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Index != -1)
            {
                DataGridViewRow dgvRow = dataGridView2.CurrentRow;
                intMemId = Convert.ToInt32(dgvRow.Cells[0].Value);

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("viewmemberbyid", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@MemId", intMemId);

                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    DataRow dr = ds.Tables[0].Rows[0];

                    textBox1.Text = dr["MemCode"].ToString();
                    textBox2.Text = dr["MemName"].ToString();
                    textBox3.Text = dr["Address"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dr["DateOfBirth"].ToString());
                    dateTimePicker2.Value = Convert.ToDateTime(dr["JoinDate"].ToString());

                    if (Convert.ToBoolean(dr["Status"].ToString()))
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }

                    if ((dr["Gender"].ToString().Trim()) == "Male")
                    {
                        radioButton1.Checked = true;
                        radioButton2.Checked = false;
                    }
                    else if ((dr["Gender"].ToString().Trim()) == "Female")
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = true;
                    }
                    else
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                    }

                    if (dr["Picture"] == DBNull.Value)
                    {
                        pictureBox1.Image = new Bitmap(Application.StartupPath + "\\images\\noimage.jpg");
                    }
                    else
                    {
                        string image = dr["Picture"].ToString();
                        pictureBox1.Image = new Bitmap(Application.StartupPath + "\\images\\" + dr["Picture"].ToString());
                        strPreviousImage = dr["Picture"].ToString();
                        defaultImage = false;
                    }

                    button4.Text = "Update";
                    tabControl1.SelectedIndex = 1;
                }
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
