using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace FinalProjectUC
{
    public partial class Fuel_Calculation : UserControl
    {
        public Fuel_Calculation()
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

        private void Fuel_Calculation_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(connction);
                fuel_Grid_View();
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message + "check your cpnnection");
            }
            Total_Cost_TextBox.ReadOnly = true;
           // Vehicle_Number_TextBox.ReadOnly = true; 
        }

        private void Calculation_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Voucher_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Number_TextBox.Text) || string.IsNullOrEmpty(Fuel_Type_ComboBox.Text) ||
                string.IsNullOrEmpty(Per_Liter_Cost_TextBox.Text) || string.IsNullOrEmpty(Total_Liters_Used_TextBox.Text) )
            {
                MessageBox.Show("Missing Information", "Fuel Calculation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var result = client.Get("Vehicle/" + "Add/" + Vehicle_Number_TextBox.Text);
                // Check if the result is null or if essential properties are null
                if (result == null || result.ResultAs<VehicleForm>() == null || result.ResultAs<VehicleForm>().Vehicle_Number == null)
                {
                    MessageBox.Show("No matching Vehicle Number found in the database");
                }
                else
                {
                    Fuel_Calculation_OOP fuelcalculation = new Fuel_Calculation_OOP();
                    if (int.TryParse(Per_Liter_Cost_TextBox.Text, out int per_Liter_cost) && int.TryParse(Total_Liters_Used_TextBox.Text, out int total_liters))
                    {
                        int totalcoast = fuelcalculation.fuel_calcilation(per_Liter_cost, total_liters);
                        Total_Cost_TextBox.Text = totalcoast.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid price value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Voucher_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Number_TextBox.Text) || string.IsNullOrEmpty(Fuel_Type_ComboBox.Text) ||
                string.IsNullOrEmpty(Per_Liter_Cost_TextBox.Text) || string.IsNullOrEmpty(Total_Liters_Used_TextBox.Text) || string.IsNullOrEmpty(Total_Cost_TextBox.Text))
            {
                MessageBox.Show("Missing Information", "Fuel Calculation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Fuel_Calculation_OOP fuelsave = new Fuel_Calculation_OOP()
                {
                    voucher_number = Voucher_Number_TextBox.Text,
                    vehicle_number = Vehicle_Number_TextBox.Text,
                    fuel_type = Fuel_Type_ComboBox.Text,
                    per_liter_cost = int.Parse(Per_Liter_Cost_TextBox.Text),
                    total_liter = int.Parse(Total_Liters_Used_TextBox.Text),
                    DateTime = guna2DateTimePicker1.Text,
                    total_cost = int.Parse(Total_Cost_TextBox.Text)
                };
                FirebaseResponse respons = client.Set("SaveFuel/Save/" + Vehicle_Number_TextBox.Text,fuelsave);
                MessageBox.Show("Fuel Details added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Fuel Details Added");
                clear();

            }
        }
        //clear function
        public void clear()
        {
            Voucher_Number_TextBox.Clear();
            Vehicle_Number_TextBox.Clear();
            Per_Liter_Cost_TextBox.Clear();
            Total_Liters_Used_TextBox.Clear();
            Total_Cost_TextBox.Clear() ;
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            clear();
        }

        //Vehicle GridView code
        public void fuel_Grid_View()
        {
            try
            {
                FirebaseResponse response = client.Get("SaveFuel/Save/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, Fuel_Calculation_OOP> getUser = response.ResultAs<Dictionary<string, Fuel_Calculation_OOP>>();

                    if (getUser != null)
                    {
                        foreach (var get in getUser)
                        {
                            Fuelview.Rows.Add(
                                get.Value.voucher_number,
                                get.Value.vehicle_number,
                                get.Value.fuel_type,
                                get.Value.per_liter_cost,
                                get.Value.total_liter,
                                get.Value.DateTime.ToString(),
                                get.Value.total_cost

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

        private void Delete_Button_Click(object sender, EventArgs e)
        {
            if (Vehicle_Number_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the vehicle Number");
            }
            else
            {
                var setter = client.Delete("SaveFuel/Save/" + Vehicle_Number_TextBox.Text);
                MessageBox.Show("Vehicle Details deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Fuel Details Deleted");
                clear();
            }
        }

       /* private void SearchBTN_Click(object sender, EventArgs e)
        {
            
        }*/

        private void Refresh_Button_Click(object sender, EventArgs e)
        {
            Fuelview.DataSource = null;
            Fuelview.Rows.Clear();
            fuel_Grid_View();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        string User = Loginform.User;
        public void Track(string Activity)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            string timme = DateTime.Now.ToString("HH-mm-ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(User, Date, timme, Activity, "Vehicle Details");
        }
    }
}
