using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectUC
{
    public partial class Dasgboard : Form
    {
        public Dasgboard()
        {
            InitializeComponent();
        }


        

        public void add_pannel(Control add)
        {
            add.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(add);
        }

        private void Dasgboard_Load(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            add_pannel(inventory);
            if(Loginform.Userrole == "Admin")
            {
                InventoryButton.Visible = true;
                InventoryCalculationButton.Visible = true;  
                guna2Button1.Visible = true;
                Fuel_Calculation_Button.Visible = true;
                UserButton.Visible = true;
                guna2Button2.Visible = true;
                UserTrackingButton.Visible = true;
            }
            else
            {
                InventoryButton.Visible = true;
                InventoryCalculationButton.Visible = true;
                guna2Button1.Visible = true;
                Fuel_Calculation_Button.Visible = true;
                UserButton.Visible = false;
                guna2Button2.Visible = true;
                UserTrackingButton.Visible = false;
            }
            
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            add_pannel(inventory);
        }

        private void UserButton_Click(object sender, EventArgs e)
        {
            User user = new User();
            add_pannel(user);
        }

        private void InventoryCalculationButton_Click(object sender, EventArgs e)
        {
            UserContrInventoryCalculation cal = new UserContrInventoryCalculation();
            add_pannel(cal);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Vehicle_Details vehicle = new Vehicle_Details();
            add_pannel(vehicle);
        }

        private void Fuel_Calculation_Button_Click(object sender, EventArgs e)
        {
            Fuel_Calculation fuel = new Fuel_Calculation();
            add_pannel(fuel);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Report re = new Report();   
            add_pannel(re);
        }

        private void UserTrackingButton_Click(object sender, EventArgs e)
        {
            Usertrackingform track = new Usertrackingform();
            add_pannel(track);
        }


        string user = Loginform.User;

        //Tracking function================================================================================
        public void Track(string Activity)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");  // Fix the date format
            string time = DateTime.Now.ToString("HH:mm:ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(user, date, time, Activity, "Dashboard");

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Loginform form = new Loginform();
            form.Show();
            Track("Logout the System");
        }
    }
}
