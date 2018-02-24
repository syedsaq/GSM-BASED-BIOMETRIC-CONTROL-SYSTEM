using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT
{
    public partial class Verification : Form, DPFP.Capture.EventHandler
    {
        SqlConnection con = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");

        private DPFP.Capture.Capture capteror;
        private DPFP.Template tamplete;
        private DPFP.Verification.Verification verifor;

        private String[] files;
        private int fileCounter = 0;
        private string path = @"C:\Users\shah\Desktop\Fyp Project\Hassnain Shah\PROJECT1 HS version_2\PROJECT1\Fing";
        bool isVarifed = false;

        Notifier noti = new Notifier();

        public Verification()
        {
            InitializeComponent();
        }

        protected virtual void Init()
        {
            capteror = new DPFP.Capture.Capture();                  // Create a capture operation.
            capteror.EventHandler = this;                           // Subscribe for capturing events.
            verifor = new DPFP.Verification.Verification();     // Create a fingerprint tamplete verifor
        }

        private void Verify(string path)
        {
            FileStream fs = File.OpenRead(path);
            tamplete = new DPFP.Template(fs);
        }


        protected void Start()
        {
            capteror.StartCapture();
            SetPrompt("Using the fingerprint reader, scan your fingerprint.");
        }

        protected void Stop()
        {
            capteror.StopCapture();
        }


        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {

            MakeReport("The fingerprint sample was captured.");
            SetPrompt("Scan the same fingerprint again.");
            while (!isVarifed)
                Process(Sample);

            Stop();
            fileCounter = 0;
            files = Directory.GetFiles(path);
            if (files.Length > 0)
            {
                Verify(files[fileCounter]);
                fileCounter++;
            }
            isVarifed = false;
            Init();
            Start();
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The finger was removed from the fingerprint reader.");
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was touched.");
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was connected.");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was disconnected.");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                MakeReport("The quality of the fingerprint sample is good.");
            else
                MakeReport("The quality of the fingerprint sample is poor.");
        }
        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
            Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
            Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
            return bitmap;

        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        protected void SetStatus(string status)
        {
            StatusText.AppendText(status + "\r\n");
        }

        protected void SetPrompt(string prompt)
        {
            Prompt.Text = prompt;
        }
        protected void MakeReport(string message)
        {
            StatusText.AppendText(message + "\r\n");
        }

        private void DrawPicture(Bitmap bitmap)
        {
            pictureBox6.Image = bitmap;
        }

        protected void Process(DPFP.Sample Sample)
        {
            DrawPicture(ConvertSampleToBitmap(Sample));
            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                // Compare the feature set with our tamplete
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                verifor.Verify(features, tamplete, ref result);
                if (result.Verified)
                {
                    MakeReport("The fingerprint was VERIFIED.");
                    displayData(files[fileCounter - 1]);
                    fileCounter = 0; 
                    isVarifed = true;
                  
                }
                else
                {
                    MakeReport("The fingerprint was NOT VERIFIED.");
                    Stop();
                    loadNextFile();
                    Init();
                    Start();

                }

            }
        }


       

      
        private void loadNextFile()
        {

            if (fileCounter < files.Length)
            {
                Verify(files[fileCounter]);
                fileCounter++;
                isVarifed = false;
            }
            else
            {
                lblName.Text = "";
                lblphone.Text = "";
                lblemail.Text = "";
                pictureBox6.Image = null;
                isVarifed = true;
                MessageBox.Show("No file in DB to Match", "Search Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fileCounter = 0;
            }
        }

        private void displayData(string filename)
        {
            DateTime dateTime = DateTime.Now;
            DateTime thisDay = DateTime.Today;
            String[] tokens = filename.Split('\\');
            String file = tokens[tokens.Length - 1];
            tokens = file.Split('.');
            file = tokens[0];
             
            try
            {
                SqlCommand cmd = new SqlCommand("select name, email, phone, timeArival, timeExit, id from [Person] where phone ='" + file + "' ", con);
                con.Open();
                SqlDataReader datareader = cmd.ExecuteReader();
                if (datareader.Read())
                {
                    lblName.Text = Convert.ToString(datareader.GetValue(0));
                    lblphone.Text = Convert.ToString(datareader.GetValue(2));
                    lblemail.Text = Convert.ToString(datareader.GetValue(1));
                    lblArival.Text = datareader.GetValue(3).ToString();
                    lblExit.Text = datareader.GetValue(4).ToString();
                    
                    string timeNow = (dateTime.Hour + ":" + dateTime.Minute);
                    string dateNow = thisDay.Day + "-" + thisDay.Month + "-" + thisDay.Year;
                    makeEntry(datareader.GetValue(5).ToString(), dateNow, timeNow);

                    if (!noti.sendSMS(txtCellNo.Text.ToString(),
                       "(Na:" + lblName.Text.Replace(' ', '_')
                       + ", Ti:" + lblArival.Text.Replace(':', '_')
                       + ", TO:" + lblExit.Text.Replace(':', '_')
                       + ", TV:" + timeNow.Replace(':', '_')
                       + " )"))
                    {
                        //MessageBox.Show("Error Sending Message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
 
            }
            catch 
            {
                MessageBox.Show("Could'nt loaded form Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();

            }
        }
        private void Verification_FormClosed(object sender, FormClosedEventArgs e)
        {

            capteror = new DPFP.Capture.Capture();
            capteror.EventHandler = this;
            verifor = new DPFP.Verification.Verification();
            Stop();

        }

       

        private void makeEntry(String id, string date, string time )
        {
            SqlConnection locatCon = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT timeIn FROM Entry WHERE id=" + id + " AND date ='" + date + "'", locatCon);
                locatCon.Open();
                SqlDataReader datareader = cmd.ExecuteReader();
                if (datareader.Read())
                {
                    locatCon.Close();
                    exitTimeEntry(id, date, time);
                }
                else
                {
                    locatCon.Close();
                    arivalTimeEntry(id, date, time);
                }
            }
            catch 
            {
               // MessageBox.Show("Exception"+ex);
            }
            finally
            {
                locatCon.Close();
            }

        }

        private void arivalTimeEntry(String id, string date, string time)
        {
            SqlConnection locatCon = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("INSERT INTO Entry(id, date, timeIn) Values(" + id + ", '" + date + "', '" + time + "') ", locatCon);
            locatCon.Open();
            cmd.ExecuteNonQuery();
            locatCon.Close();


        }

        private void exitTimeEntry(String id, string date, string time)
        {
            SqlConnection locatCon = new SqlConnection("Data Source=SAQLAIN;Initial Catalog=EnrolmentSystem;Integrated Security=True");
            string qry = "UPDATE Entry SET timeOut=@t_out  WHERE id=" + id + " AND date = '" + date + "'";
            locatCon.Open();
            using(SqlCommand cmd= locatCon.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = qry;

                cmd.Parameters.Add(new SqlParameter("t_out", time));
                int result = cmd.ExecuteNonQuery();
            }
            locatCon.Close();

        }

        private void txtCellNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString("HH:mm");
            label8.Text = DateTime.Now.ToString("MMM dd yyyy");
            label10.Text = DateTime.Now.ToString("dddd");
        }

        private void Verification_Load(object sender, EventArgs e)
        {
            timer1.Start();
            files = Directory.GetFiles(path);
            if (files.Length > 0)
            {
                Verify(files[fileCounter]);
                fileCounter++;
            }
            else
            {
                MessageBox.Show("There are No Records in Database...!");
            }
            Init();
            Start();
        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login main = new Login();
            main.ShowDialog();
            this.Close();
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            //  capture = new CaptureForm();
            //   Tamplate = new DPFP.Template();
            // capteror = new DPFP.Capture.Capture();
            // capteror.EventHandler = this;
            // verifor = new DPFP.Verification.Verification();
            Stop();
            this.Hide();
            MainForm main = new MainForm();
            main.ShowDialog();
            this.Close();


        }
    }
}
