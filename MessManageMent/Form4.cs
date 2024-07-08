using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessManageMent.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace MessManageMent
{
    public partial class Form4 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            LoadMemberCombo();
        }

        private void LoadMemberCombo()
        {
        }

            private void button1_Click(object sender, EventArgs e)
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("ViewCostOFMonthTest5", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@inputDate", dateTimePicker1.Value);
                    //sda.SelectCommand.Parameters.AddWithValue("@memId", Convert.ToInt16(comboBox1.SelectedValue));
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    List<MessViewModel> list = new List<MessViewModel>();
                    MessViewModel messView;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            messView = new MessViewModel();

                            messView.Rent = Convert.ToInt32(row["Rent"]);
                            messView.Id = Convert.ToInt32(row["MemId"]);
                            messView.ElectricBill = Convert.ToInt32(row["ElectricBill"]);
                            messView.FridgeBill = Convert.ToInt32(row["FridgeBill"]);
                            messView.GasBill = Convert.ToInt32(row["GasBill"]);
                            messView.ConsumeFood = Convert.ToInt32(row["ConsumeFood"]);
                            messView.InternetBill = Convert.ToInt32(row["InternetBill"]);
                            messView.CostOfFood = Convert.ToInt32(row["CostOfFood"]);
                            messView.Meal = Convert.ToInt32(row["Meal"]);
                            messView.MealRate = Convert.ToInt32(row["MealRate"]);
                            messView.otherCost = Convert.ToInt32(row["OtherCost"]);
                            messView.MemName = row["MemName"].ToString();
                            messView.TotalMoney = Convert.ToInt32(row["TotalCost"]);



                            messView.Picture = Application.StartupPath + "\\images\\" + row["Picture"].ToString();

                            list.Add(messView);
                        }

                        using (ReportMess report = new ReportMess(list))
                        {
                            report.ShowDialog();
                        }
                    }
                    else
                    {

                        MessageBox.Show("No data available to generate report.");
                    }



                }
            }


        }
    } 

