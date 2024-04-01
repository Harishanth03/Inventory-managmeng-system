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
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Guna.UI2.WinForms.Suite;
using System.Diagnostics;
using Google.Type;

namespace FinalProjectUC
{
    public partial class Vehicle_Details : UserControl
    {
        public Vehicle_Details()
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

        private void Vehicle_Details_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(connction);
              //  Vehicle_Grid_View();
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message + "check your cpnnection");
            }
            
        }
        public void clear()
        {
            S_Number_TextBox.Clear();
            Vehicle_Number_TextBox.Clear();
            Vehicle_Model_TextBox.Clear();
            Ch_TextBox.Clear();
            Engine_Number_TextBox.Clear();
            Value_TextBox.Clear() ;
            provide_textBox.Clear();
            Remark_textBox.Clear() ;
        }

        private void Vehicle_Add_Button_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(S_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Model_TextBox.Text) || string.IsNullOrEmpty(Ch_TextBox.Text) || string.IsNullOrEmpty(Engine_Number_TextBox.Text) ||
                string.IsNullOrEmpty(Pur_Date_TimePicker.Text) || string.IsNullOrEmpty(Value_TextBox.Text) || string.IsNullOrEmpty(provide_textBox.Text) || string.IsNullOrEmpty(Branch_ComboBox.Text) || string.IsNullOrEmpty(Remark_textBox.Text))
            {
                MessageBox.Show("Missing Information","Vehicle",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                VehicleForm vehicle = new VehicleForm()
                {
                    S_Number = S_Number_TextBox.Text,
                    Vehicle_Number = Vehicle_Number_TextBox.Text,
                    Vehicle_model = Vehicle_Model_TextBox.Text,
                    CH_Number = Ch_TextBox.Text,
                    Engine_Number = Engine_Number_TextBox.Text,
                    purchasing_Date = Pur_Date_TimePicker.Value,
                    Value = Value_TextBox.Text,
                    Provide = provide_textBox.Text,
                    Branch = Branch_ComboBox.Text,
                    Remark = Remark_textBox.Text
                };
                var setter = client.Set("Vehicle/" +"Add/" + Vehicle_Number_TextBox.Text, vehicle);
                MessageBox.Show("Vehicle Details Added Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Vehicle Details Added");
                clear();
            }
        }

        //Vehicle GridView code
        public void Vehicle_Grid_View()
        {
            try
            {
                FirebaseResponse response = client.Get("Vehicle/Add/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, VehicleForm> getUser = response.ResultAs<Dictionary<string, VehicleForm>>();

                    if (getUser != null)
                    {
                        foreach (var get in getUser)
                        {
                            Vehicle_GridView.Rows.Add(
                                get.Value.S_Number,
                                get.Value.Vehicle_Number,
                                get.Value.Vehicle_model,
                                get.Value.CH_Number,
                                get.Value.Engine_Number,
                                get.Value.purchasing_Date.ToString(),
                                get.Value.Value,
                                get.Value.Provide,
                                get.Value.Branch,
                                get.Value.Remark
                            );
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data in the database");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }




        private void SearchBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Vehicle_Number_TextBox.Text))
            {
                MessageBox.Show("Please Enter the vehicle Number"); 
            }
            else
            {
                var result = client.Get("Vehicle/" + "Add/" + Vehicle_Number_TextBox.Text);

                // Check if the result is null or if essential properties are null
                if (result == null || result.ResultAs<VehicleForm>() == null || result.ResultAs<VehicleForm>().Vehicle_Number == null)
                {
                    MessageBox.Show("No matching Vehicle Number found in the database");
                    // You may want to clear the textboxes here or handle it as per your requirements
                }
                else
                {
                    VehicleForm inven = result.ResultAs<VehicleForm>();
                    S_Number_TextBox.Text = inven.S_Number;
                    Vehicle_Number_TextBox.Text = inven.Vehicle_Number;
                    Vehicle_Model_TextBox.Text = inven.Vehicle_model;
                    Ch_TextBox.Text = inven.CH_Number;
                    Engine_Number_TextBox.Text = inven.Engine_Number;
                    Pur_Date_TimePicker.Text = inven.purchasing_Date.ToString();
                    Value_TextBox.Text = inven.Value;
                    provide_textBox.Text = inven.Provide;
                    Branch_ComboBox.Text = inven.Branch;
                    Remark_textBox.Text = inven.Remark;
                }
            }
        }

        private void Vehicle_Delete_Button_Click(object sender, EventArgs e)
        {
            if ( Vehicle_Number_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the vehicle Number");
            }
            else
            {
                var setter = client.Delete("Vehicle/" + "Add/" + Vehicle_Number_TextBox.Text);
                MessageBox.Show("Vehicle De deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Vehicle details deleted");
                clear();
            }
        }

        private void Vehicle_Update_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(S_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Model_TextBox.Text) || string.IsNullOrEmpty(Ch_TextBox.Text) || string.IsNullOrEmpty(Engine_Number_TextBox.Text) ||
                string.IsNullOrEmpty(Pur_Date_TimePicker.Text) || string.IsNullOrEmpty(Value_TextBox.Text) || string.IsNullOrEmpty(provide_textBox.Text) || string.IsNullOrEmpty(Branch_ComboBox.Text) || string.IsNullOrEmpty(Remark_textBox.Text))
            {
                MessageBox.Show("Missing Information", "Vehicle", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                VehicleForm vehicle = new VehicleForm()
                {
                    S_Number = S_Number_TextBox.Text,
                    Vehicle_Number = Vehicle_Number_TextBox.Text,
                    Vehicle_model = Vehicle_Model_TextBox.Text,
                    CH_Number = Ch_TextBox.Text,
                    Engine_Number = Engine_Number_TextBox.Text,
                    purchasing_Date = Pur_Date_TimePicker.Value,
                    Value = Value_TextBox.Text,
                    Provide = provide_textBox.Text,
                    Branch = Branch_ComboBox.Text,
                    Remark = Remark_textBox.Text
                };
                var setter = client.Update("Vehicle/" + "Add/" + Vehicle_Number_TextBox.Text, vehicle);
                MessageBox.Show("Vehicle Details Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Vehicle details Updated");
                clear();

            }
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            S_Number_TextBox.Clear();
            Vehicle_Number_TextBox.Clear();
            Vehicle_Model_TextBox.Clear();
            Ch_TextBox.Clear();
            Engine_Number_TextBox.Clear();
            Value_TextBox.Clear();
            provide_textBox.Clear();
            Remark_textBox.Clear();
        }

        private void Vehicle_Refresh_Button_Click(object sender, EventArgs e)
        {
            Vehicle_GridView.DataSource = null;
            Vehicle_GridView.Rows.Clear();
            Vehicle_Grid_View();
        }
        string User = Loginform.User;

        // Sub Function===============================================================================================
        public void Track(string Activity)
        {
            string Date = System.DateTime.Now.ToString("yyyy-MM-dd");
            string timme = System.DateTime.Now.ToString("HH-mm-ss");
           
            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(User, Date,timme,Activity,"Vehicle Details");
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
