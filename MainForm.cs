using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

       

        private void label1_Click(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login main = new Login();
            main.ShowDialog();
            this.Close();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            this.Hide();
            Verification vrifaction = new Verification();
            vrifaction.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.ShowDialog();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label9.Text = DateTime.Now.ToString("HH:mm");
            label8.Text = DateTime.Now.ToString(" MMM dd yyyy");
            label10.Text = DateTime.Now.ToString("dddd");
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            UpdateRecord ur = new UpdateRecord();
            ur.ShowDialog();
            this.Close();
        }

        private void bunifuTileButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            deleteRecord dr = new deleteRecord();
            dr.ShowDialog();
            this.Close();

        }
    }
}
