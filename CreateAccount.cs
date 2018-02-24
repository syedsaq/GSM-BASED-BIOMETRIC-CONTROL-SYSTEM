using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PROJECT
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Backbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login l = new Login();
            l.ShowDialog();
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please enter user name and password.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (textBox1.Text.Length > 2 && textBox2.Text.Length > 2 && textBox3.Text.Length > 4)
            {

                SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");


                con.Open();

                //SqlCommand cmd = new SqlCommand("INSERT INTO Table_3 (id,name,phoneno,address) VALUES('" + txtid.Text + "','" + txtname.Text + "','"+txtph.Text+"','"+txtadd.Text+"')",con);
                SqlCommand cmd = new SqlCommand("INSERT INTO Operator (name, password, phone) VALUES('" + textBox1.Text + "','" + textBox2.Text + "', '" + textBox3.Text + "')", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Account Created!", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter valid user name and password.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsLetter(e.KeyChar) || e.KeyChar == 8 ? false : true;

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 32 ? false : true;

        }
    }
}

