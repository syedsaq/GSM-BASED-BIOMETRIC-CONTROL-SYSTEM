using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();

                // Add all port names to the combo box:
                foreach (string port in ports)
                {
                    this.cboPortName.Items.Add(port);
                }

            }
            catch
            {

            }

        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_btn_Click(object sender, EventArgs e)
        {

            if (Nametextbox.Text.ToString().Length < 1 || Passwordtextbox.ToString().Length < 1)
            {
                MessageBox.Show("Enter user name or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (cboPortName.SelectedIndex < 0)
             {
               MessageBox.Show("Select COM Port\n PORT that has GSM Connected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {

                SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");

                SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) From Operator where name ='" + Nametextbox.Text + "' and password ='" + Passwordtextbox.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                     Notifier.comPort = cboPortName.SelectedItem.ToString();
                    this.Hide();
                    MainForm main = new MainForm();
                    main.ShowDialog();
                    this.Close();

                }
                else
                    MessageBox.Show("Please check username and password!");
            }
        }

        private void Signupbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateAccount cr = new CreateAccount();
            cr.ShowDialog();
            this.Close();
        }

        private void Nametextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsLetter(e.KeyChar) || e.KeyChar == 8 ? false : true;
        }

        private void Passwordtextbox_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 32 ? false : true;
        }

        private void Passwordtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login_btn_Click(null, null);
            }
        }

        

     
    } }










