using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Response;
using static FinalProjectUC.UserTracking;


namespace FinalProjectUC
{
    public partial class Usertrackingform : UserControl
    {
        public Usertrackingform()
        {
            InitializeComponent();
        }
        //Connect Firebase and System===============================================================================================
        IFirebaseConfig connction = new FirebaseConfig()
        {
            AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
            BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        private void Usertrackingform_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(connction);
               
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message + "check your cpnnection");
            }
            TrackingTable();
        }

        //Datagridview coding====================================================================================================
        public void TrackingTable()
        {
            FirebaseResponse response = client.Get("Tracking/");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("There is no Data In the Database");
            }
            else
            {
                Dictionary<string, TrackingData> getUser = response.ResultAs<Dictionary<string, TrackingData>>();

                if (getUser != null)
                {
                    foreach (var get in getUser)
                    {
                        Vehicle_GridView.Rows.Add(
                            get.Value.userdata,
                            get.Value.Datedata,
                            get.Value.Timedata,
                            get.Value.Activitydata,
                            get.Value.Fromdata
                        );
                    }
                }
                else
                {
                    MessageBox.Show("No data in the database");
                }
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Vehicle_GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
