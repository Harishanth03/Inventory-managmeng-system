using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectUC
{
    internal class UserTracking
    {

        public void SystemTracking(string Useraame, string Date, string Time, string Activity, string From)
        {
            try
            {
                // Connect Firebase and System
                IFirebaseConfig connction = new FirebaseConfig()
                {
                    AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
                    BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
                };
                IFirebaseClient client;

                client = new FireSharp.FirebaseClient(connction);

                // Create a dictionary to insert the data
                TrackingData trackingData = new TrackingData()
                {
                    userdata = Useraame,
                    Datedata = Date,
                    Timedata = Time,
                    Activitydata = Activity,
                    Fromdata = From
                };

                // Use Push to generate an auto-incremented key
                var insert = client.Push("Tracking/", trackingData);

                if (insert.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Successful insertion
                    
                }
                else
                {
                    // Handle the error
                    MessageBox.Show($"Error inserting tracking data. Status code: {insert.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Create another internal class========================================================================
        internal class TrackingData
        {
            public string userdata { get; set; }
            public string Datedata { get; set; }
            public string Timedata { get; set; }
            public string Activitydata { get; set; }
            public string Fromdata { get; set; }
        }



    }


}
