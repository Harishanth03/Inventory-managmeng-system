using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net.NetworkInformation;

namespace FinalProjectUC
{
    public partial class Loginform : Form
    {

        //Assign global variable====================================================================================================
        public static string User;
        public static string Userrole;
        public static string name;
        public static string phone;


        public Loginform()
        {
            InitializeComponent();
        }
        //Connect Firebase and System
        IFirebaseConfig connction = new FirebaseConfig()
        {
            AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
            BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private void Loginform_Load(object sender, EventArgs e)
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

            CheckInternet check = new CheckInternet();
            try
            {
                //check the internet connection=====================================================================================
                if (check.CheckInternetConnection())
                {
                    client = new FireSharp.FirebaseClient(connction);
                }
                else
                {
                    MessageBox.Show("No internet connection. Please check your connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit(); // or take appropriate action
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}. Check your connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // or take appropriate action
            }

        }


        private void SigninButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(User_Name_TextBox.Text) && string.IsNullOrEmpty(Password_TextBox.Text))
            {
                MessageBox.Show("Username and Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(User_Name_TextBox.Text))
            {
                MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(Password_TextBox.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
            else
            {
                FirebaseResponse respons = client.Get("User/Register/");
                Dictionary<string, Register_Login> result = respons.ResultAs<Dictionary<string, Register_Login>>();

                bool userFound = false;
                bool passwordCorrect = false;

                foreach (var get in result)
                {
                    User = get.Value.Username;
                    string password = get.Value.Password;
                    string role = get.Value.Type;
                    name = get.Value.name;
                    phone = get.Value.phoneNumber.ToString();

                    if(role == "Admin")
                    {
                        Userrole = "Admin";
                    }
                    else
                    {
                        Userrole = "User";
                    }
                    //UserType = get.Value.Type;

                    if (User == User_Name_TextBox.Text)
                    {
                        userFound = true;

                        if (password == Password_TextBox.Text)
                        {
                            passwordCorrect = true;
                            break;
                        }
                    }
                }

                if (userFound)
                {
                    if (passwordCorrect)
                    {
                        MessageBox.Show("Hello " + User + " Login Successfully", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Track("Login to the System");
                        Dasgboard dash = new Dasgboard();
                        dash.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Username not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            User_Name_TextBox.Clear();
            Password_TextBox.Clear();
        }

        //Tracking function=========================================================================================================
        public void Track(string Activity)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");  // Fix the date format
            string time = DateTime.Now.ToString("HH:mm:ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(User,date,time,Activity,"Login");

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res = MessageBox.Show("Are your sure want to exit the system","Exit",MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
            
           
        }


        //UsertypeCode==============================================================================================================

    }
}
