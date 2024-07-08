using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessManageMent
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public void loadform(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
                this.mainpanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            loadform(new Form4());
            pictureBox1.Image = new Bitmap(Application.StartupPath + "\\images\\lullu.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new Form4());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new Form5());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new Form6());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 lForm = new Form2();
            lForm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
