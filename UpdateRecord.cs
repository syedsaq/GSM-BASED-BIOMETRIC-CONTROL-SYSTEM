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
    public partial class UpdateRecord : Form
    {
        SqlConnection con;
        SqlDataAdapter sda;
        DataSet ds;
        SqlCommandBuilder scmbd;

       

        // SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");
        public UpdateRecord()
        {
            InitializeComponent();
            
        }

        private void UpdateRecord_Load(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = @"Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True";
                con.Open();
                sda = new SqlDataAdapter("select id as 'ID', name as 'Name', email as 'Email', phone as 'Phone No', timeArival as 'Time Arrival', timeExit as 'Time Exit' from [Person]",con);
                ds = new System.Data.DataSet();
                sda.Fill(ds, "Details");
                bunifuCustomDataGrid1.DataSource = ds.Tables[0];
            }  
            catch(Exception ex)
            {
                MessageBox.Show("Error\n" +ex.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally{
                con.Close();
            }

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
            this.Hide();
            MainForm main = new MainForm();
            main.ShowDialog();
            this.Close();

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Login main = new Login();
            main.ShowDialog();
            this.Close();

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            try
            {

                scmbd = new SqlCommandBuilder(sda);
                sda.Update(ds, "Details");
                MessageBox.Show("Records Successfully Updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void bunifuCustomDataGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bunifuFlatButton2.Enabled = true;
            }
        }
    }
}
