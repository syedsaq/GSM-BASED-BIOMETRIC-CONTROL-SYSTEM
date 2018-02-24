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
using Enrollment;
using System.IO;


namespace PROJECT
{
    public partial class RegistrationForm : Form
    {
        private DPFP.Template Template;
        private static String path = @"C:\Users\shah\Desktop\Fyp Project\Hassnain Shah\PROJECT1 HS version_2\PROJECT1\Fing\";
        CaptureForm capture = new CaptureForm();
        private SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");

        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.Activate();
            textArrivalHH.MaxLength = 2;
            textArrivalMM.MaxLength = 2;
            textExitHH.MaxLength = 2;
            textExitMM.MaxLength = 2;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login main = new Login();
            main.ShowDialog();
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsLetter(e.KeyChar) || e.KeyChar == 8 ? false : true;


        }

        private void txtph_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsNumber(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 32 ? false : true;

        }

        private void txtem_Leave(object sender, EventArgs e)
        {
            if (txtem.Text.ToString().Contains("@") && txtem.Text.ToString().Contains("."))
            {

            }
            else
            {
                MessageBox.Show("please enter a valid email address");
                txtem.Text = string.Empty;
            }
        }

        private void txtph_Leave(object sender, EventArgs e)
        {
            if (txtph.Text.Length > 5 && txtph.Text.Length <= 11)
            {

            }
            else
            {
                MessageBox.Show("please enter a valid PHONE NUMBER");
                txtph.Text = string.Empty;

            }

        }

        private void Backbtn_Click(object sender, EventArgs e)
        {
            capture.OnTemplate -= OnTemplate;
            capture = new CaptureForm();
            Template = new DPFP.Template();

            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
        }

        private void OnTemplate(DPFP.Template template)
        {

            Template = template;
            if (Template != null)
            {
                MessageBox.Show("The fingerprint template is ready for fingerprint verification.", "Fingerprint Enrollment");
                Save.Enabled = true;

            }
            else
                MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
            if (capture.getPrintImage() != null)
            {
                Bitmap bmp = capture.getPrintImage();
                pictureBox6.Image = bmp;
            }
            else
            {

            }
            capture.Hide();
        }

        private void textArrivalHH_TextChanged(object sender, EventArgs e)
        {
            if (textArrivalHH.Text.Length > 0)
                if (Convert.ToInt32(textArrivalHH.Text) > 24)
                {
                    textArrivalHH.Text = "24";
                }
        }

        private void textArrivalMM_TextChanged(object sender, EventArgs e)
        {
            if (textArrivalMM.Text.Length > 0)
                if (Convert.ToInt32(textArrivalMM.Text) > 59)
                {
                    textArrivalMM.Text = "59";
                }
        }

        private void textExitHH_TextChanged(object sender, EventArgs e)
        {
            if (textExitHH.Text.Length > 0)
                if (Convert.ToInt32(textExitHH.Text) > 24)
                {
                    textExitHH.Text = "24";
                }
        }

        private void textExitMM_TextChanged(object sender, EventArgs e)
        {
            if (textExitMM.Text.Length > 0)
                if (Convert.ToInt32(textExitMM.Text) > 59)
                {
                    textExitMM.Text = "59";
                }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {

                String name = txtname.Text.Trim();
                String phone = txtph.Text.Trim();
                String mail = txtem.Text.Trim();
                String filename = phone;
                string filePath = path + filename + ".fpt";
                String ariveTime = textArrivalHH.Text + ":" + textArrivalMM.Text;
                String exitTime = textExitHH.Text + ":" + textExitMM.Text;
                if (name.Length > 2 && phone.Length > 5 && mail.Length > 3 && ariveTime.Length > 1 && exitTime.Length > 1)
                {

                    // (id,name,phoneno,email,filename)
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Person](name, email, phone, timeArival, timeExit) VALUES('" + name + "','" + mail + "','" + phone + "','" + ariveTime + "', '" + exitTime + "')", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //Save File on Drive
                    FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write);
                    Template.Serialize(fs);
                    fs.Close();

                    MessageBox.Show("Record Has been saved Sucsessfully!", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                    txtname.Text = String.Empty;
                    txtph.Text = String.Empty;
                    txtem.Text = String.Empty;
                    pictureBox6.Image = null;

                }
                else
                {
                    MessageBox.Show("Record Cannot Be Added Unless it is valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("An Error occured During Saving File \n"+ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                capture.OnTemplate -= OnTemplate;
                capture = new CaptureForm();
                Template = new DPFP.Template();
            }
        }

        //private void Capturebtn_Click(object sender, EventArgs e)
        //{
        //    capture.OnTemplate += OnTemplate;
        //    capture.Visible = true;
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString("HH:mm");
            label8.Text = DateTime.Now.ToString("MMM dd yyyy");
            label10.Text = DateTime.Now.ToString("dddd");
            //lblSecond.Location = new Point(lblTime.Location.X + lblTime.Width - 5, lblSecond.Location.Y);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Capturebtn_Click(object sender, EventArgs e)
        {
            capture.OnTemplate += OnTemplate;
              capture.Visible = true;
        }

        private void RegistrationForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        
    }
}


