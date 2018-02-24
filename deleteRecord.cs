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
using System.IO;

namespace PROJECT
{
    public partial class deleteRecord : Form
    {
        SqlConnection con;
        SqlDataAdapter sda;
        DataSet ds;
       
        SqlCommandBuilder scmbd;
        public deleteRecord()
        {
            InitializeComponent();
        }

        private void deleteRecord_Load(object sender, EventArgs e)
        {
            loadRecords();
        }
        void loadRecords()
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = @"Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True";
                con.Open();
                sda = new SqlDataAdapter("select id as 'ID', name as 'Name', email as 'Email', phone as 'Phone', timeArival as 'Time Arrival', timeExit as 'Time Exit' from [Person]", con);
                ds = new System.Data.DataSet();
                sda.Fill(ds, "Employees Details");
                bunifuCustomDataGrid1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        //private void DisplayData()
        //{
        //    con.Open();
        //    DataTable dt = new DataTable();
        //    sda = new SqlDataAdapter("select * from Person", con);
        //    sda.Fill(dt);
        //    bunifuCustomDataGrid1.DataSource = dt;
        //    con.Close();
        //}
        ////Clear Data  
        //private void ClearData()
        //{
        //    txt_Name.Text = "";
        //    txt_State.Text = "";
        //    ID = 0;
        //}
        void deleteFile(string filename)
        {
            string path= @"C: \Users\shah\Desktop\Fyp Project\Hassnain Shah\PROJECT1 HS version_2\PROJECT1\Fing\";
            string[] filesnames = Directory.GetFiles(path, "*.fpt");
            foreach(string name in filesnames)
            {
              string p_name=""+name.Substring(name.LastIndexOf('\\')+1);
                if (p_name == filename + ".fpt")
                {
                    File.Delete(name);
                }
            }
           
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {

            try
            {

                SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");
                SqlCommand cmd;
                cmd = new SqlCommand("delete Person where id=@id", con);

                con.Open();

                cmd.Parameters.AddWithValue("@id", bunifuCustomDataGrid1.CurrentRow.Cells["id"].Value.ToString());
               // MessageBox.Show(bunifuCustomDataGrid1.CurrentRow.Cells["phone"].Value.ToString());
                deleteFile(bunifuCustomDataGrid1.CurrentRow.Cells["phone"].Value.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record deleted Successfully");

                ds.Tables[0].Rows.Clear();
                loadRecords();
              


            }
            catch (Exception ex)
            {
                MessageBox.Show("error", ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {

            this.Hide();
            Login main = new Login();
            main.ShowDialog();
            this.Close();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm main = new MainForm();
            main.ShowDialog();
            this.Close();

        }
    }
}
