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
using MessManageMent.ViewModel;

namespace MessManageMent
{
    public partial class Form5 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        int intCost = 0;
        public Form5()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadMemberCombo();
            ClearAll();
            LoadCost();
        }

        private void LoadCost()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("AllCost", con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView3.RowTemplate.Height = 20;
                
                dataGridView3.DataSource = dt;

                dataGridView3.AllowUserToAddRows = false;

                sda.Dispose();
            }
        }

        private void ClearAll()
        {
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            comboBox1.SelectedIndex = 0;
            dateTimePicker3.Value = DateTime.Now;

            intCost = 0;
            button9.Enabled = false;
            button8.Text = "Save";

            if (dataGridView1.DataSource == null)
            {
                dataGridView1.Rows.Clear();
            }
            else
            {
                dataGridView1.DataSource = (dataGridView1.DataSource as DataTable).Clone();
            }
        }

        private void LoadMemberCombo()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Member Where Status = 1", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // Create a new row
                DataRow topRow = dt.NewRow();
                // Assign values to columns by name
                topRow["MemId"] = 0;
                topRow["MemName"] = "--Select--";
                // Insert the row at the top
                dt.Rows.InsertAt(topRow, 0);

                // Set ValueMember and DisplayMember after adding the top row
                comboBox1.ValueMember = "MemId";
                comboBox1.DisplayMember = "MemName";
                comboBox1.DataSource = dt;
            }
        }


        bool ValidateMasterDetailForm()
        {
            bool isValid = true;
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show(" required");
                isValid = false;
            }
            return isValid;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            if (ValidateMasterDetailForm())
            {
                int intCostId = 0;

                // For Cost table
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddOrUpdateCost", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", intCost);
                    cmd.Parameters.AddWithValue("@MemId", Convert.ToInt16(comboBox1.SelectedValue));
                    cmd.Parameters.AddWithValue("@Month", dateTimePicker3.Value);
                    cmd.Parameters.AddWithValue("@Rent", Convert.ToInt32(textBox4.Text));
                    cmd.Parameters.AddWithValue("@InternetBill", Convert.ToInt32(textBox7.Text));
                    cmd.Parameters.AddWithValue("@MaidBill", Convert.ToInt32(textBox5.Text));
                    cmd.Parameters.AddWithValue("@ElectricBill", Convert.ToInt32(textBox8.Text));
                    cmd.Parameters.AddWithValue("@GasBill", Convert.ToInt32(textBox6.Text));
                    cmd.Parameters.AddWithValue("@FridgeBill", Convert.ToInt32(textBox9.Text));
                    cmd.Parameters.AddWithValue("@CostOfFood", Convert.ToInt32(textBox10.Text));
                    cmd.Parameters.AddWithValue("@Meal", Convert.ToInt32(textBox11.Text));

                   

                    // Set parameter values
                    cmd.Parameters.AddWithValue("@MealRate", 0);
                    cmd.Parameters.AddWithValue("@ConsumeFood", 0);
                    cmd.Parameters.AddWithValue("@RestOfMoney", 0);


                    intCostId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // For OtherCost table
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (item.IsNewRow) break;
                        else
                        {
                            SqlCommand cmd = new SqlCommand("AddOrUpdateOtherCost", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(item.Cells["Id"].Value == DBNull.Value ? "0" : item.Cells["Id"].Value)); ;
                            cmd.Parameters.AddWithValue("@MemId", Convert.ToInt16(comboBox1.SelectedValue));
                            cmd.Parameters.AddWithValue("@CostName", item.Cells["CostName"].Value);
                            cmd.Parameters.AddWithValue("@Taka", item.Cells["Taka"].Value);
                            cmd.Parameters.AddWithValue("@costId", intCostId);
                            cmd.Parameters.AddWithValue("@month", dateTimePicker3.Value);


                            cmd.ExecuteNonQuery();
                        }
                    }

                }

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UpdateMeal", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InputDate", dateTimePicker3.Value);


                    cmd.ExecuteNonQuery();
                }


                LoadCost();
                ClearAll();
                MessageBox.Show("Submitted Successfully");

            }
        }

        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow.Index != -1)
            {
                DataGridViewRow dgvRow = dataGridView3.CurrentRow;
                intCost = Convert.ToInt32(dgvRow.Cells[0].Value);
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("ViewCostById", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@CostId", intCost);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    //--Master---
                    DataRow dr = ds.Tables[0].Rows[0];
                   
                    comboBox1.SelectedValue = Convert.ToInt32(dr["MemId"].ToString());
                    dateTimePicker3.Value = Convert.ToDateTime(dr["MonthCalc"].ToString());

                    textBox4.Text = dr["Rent"].ToString();
                    textBox5.Text = dr["MaidBill"].ToString();
                    textBox6.Text = dr["GasBill"].ToString();
                    textBox7.Text = dr["InternetBill"].ToString();
                    textBox8.Text = dr["ElectricBill"].ToString();
                    textBox9.Text = dr["FridgeBill"].ToString();
                    textBox10.Text = dr["CostOfFood"].ToString();
                    textBox11.Text = dr["Meal"].ToString();
                   


                    //--Details---
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = ds.Tables[1];
                    
                    button8.Text = "Update";
                    button9.Enabled = true;
                    tabControl1.SelectedIndex = 0;
                }
            }
        }

        private void BasicCost_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this record?", "Master Details", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("ViewCostById", con);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@CostId", intCost);
                    DataSet ds = new DataSet();
                    sqlDataAdapter.Fill(ds);
                    DataRow dataRow = ds.Tables[0].Rows[0];
                    //if (dataRow["Picture"] != DBNull.Value)
                    //{
                    //    string image = dataRow["Picture"].ToString();
                    //    string filePath = Application.StartupPath + "\\image\\";
                    //    File.Delete(filePath + image);
                    //}



                    SqlCommand cmd = new SqlCommand("AllCostDelete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CostId", intCost);
                    cmd.ExecuteNonQuery();
                    
                    ClearAll();
                    LoadCost();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
