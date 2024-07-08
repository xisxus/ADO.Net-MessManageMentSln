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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MessManageMent
{
    public partial class Form1 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate input
            if (!ValidateInput())
                return;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Admin (Email, Username, Password) VALUES (@Email, @Username, @Password)";
               
                cmd.Parameters.AddWithValue("@Email", textBox1.Text);
                cmd.Parameters.AddWithValue("@Username", textBox2.Text);
                cmd.Parameters.AddWithValue("@Password", textBox3.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Registration Complete.");

                Form2 lForm = new Form2();
                lForm.Show();
                this.Hide();
            }
        }

        private bool ValidateInput()
        {
            
            if (string.IsNullOrWhiteSpace(textBox1.Text) || !textBox1.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return false;
            }

            
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter a username.");
                return false;
            }

           
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter a password.");
                return false;
            }

            return true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Form2 lForm = new Form2();
            lForm.Show();
            this.Hide();
        }
    }
}
