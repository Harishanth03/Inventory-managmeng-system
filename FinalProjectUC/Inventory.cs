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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FinalProjectUC
{
    public partial class Inventory : UserControl
    {
        public Inventory()
        {
            InitializeComponent();
       
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
            BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
        };
        //client connnection
        IFirebaseClient client;
        public void clear()
        {
            IDTextBox.Clear();
            NameTextBox.Clear();
            FolioNumberTextBox.Clear();
            LedgerBalanceTextbox.Clear();
            ActualBalanceOnTextBox.Clear();
            SurplusTextBox.Clear();
            DeficidencyTextBox.Clear();
            RemarkTextBox.Clear();
            POBtextBox.Clear();
            PODtextBox.Clear();
            AvailableRoomTextBox.Clear();
        }


        private  void User_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Problem on the internet");
            }
            //INventory_Grid_View();
            //Inventory_Grid_View();



        }

        //Data Grid view coding
        /*public void Viewinven()
         {

             try
             {
                 FirebaseResponse response = client.Get("Inventory/" + "Add/");
                 if (response == null)
                 {
                     MessageBox.Show("There is no Data In the Database");
                 }
                 else
                 {
                     Dictionary<string, InventoryClass> getUser = response.ResultAs<Dictionary<string, InventoryClass>>();
                     foreach (var get in getUser)
                     {
                         InventoryView.Rows.Add(
                             get.Value.ID,
                             get.Value.Name,
                             get.Value.FBFolioNumber,
                             get.Value.ledgerbalance,
                             get.Value.actualbalance,
                             get.Value.surplus,
                             get.Value.defi,
                             get.Value.remark,
                             get.Value.purposeOfBuying,
                             get.Value.purposeOfDistribte,
                             get.Value.availableroom
                             );
                     }
                 }
             }
             catch
             {
                 MessageBox.Show("No data stored");
             }


         }*/

       /* public void INventory_Grid_View()
        {
            try
            {
                FirebaseResponse response = client.Get("Inventory/" + "Add/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, InventoryClass> Inventory = response.ResultAs<Dictionary<string, InventoryClass>>();

                    if (Inventory != null)
                    {
                        foreach (var get in Inventory)
                        {
                            InventoryView.Rows.Add(
                             get.Value.ID,
                             get.Value.Name,
                             get.Value.FBFolioNumber,
                             get.Value.ledgerbalance,
                             get.Value.actualbalance,
                             get.Value.surplus,
                             get.Value.defi,
                             get.Value.remark,
                             get.Value.purposeOfBuying,
                             get.Value.purposeOfDistribte,
                             get.Value.availableroom
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
        }*/


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            clear();
        }
        //Vehicle View
        public void Vehicle_Grid_View()
        {
            try
            {
                FirebaseResponse response = client.Get("Inventory/Add/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, InventoryClass> getUser = response.ResultAs<Dictionary<string, InventoryClass>>();

                    if (getUser != null)
                    {
                        foreach (var get in getUser)
                        {
                            Vehicle_GridView.Rows.Add(
                                get.Value.ID,
                                get.Value.Name,
                                get.Value.FBFolioNumber,
                                get.Value.ledgerbalance,
                                get.Value.actualbalance,
                                get.Value.surplus,
                                get.Value.defi,
                                get.Value.remark,
                                get.Value.purposeOfBuying,
                                get.Value.purposeOfDistribte,
                                get.Value.availableroom
                                ) ;
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



        private void Addbutton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(IDTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(FolioNumberTextBox.Text)
                || string.IsNullOrEmpty(LedgerBalanceTextbox.Text) || string.IsNullOrEmpty(ActualBalanceOnTextBox.Text) || string.IsNullOrEmpty(SurplusTextBox.Text)
                || string.IsNullOrEmpty(DeficidencyTextBox.Text) || string.IsNullOrEmpty(RemarkTextBox.Text) || string.IsNullOrEmpty(POBtextBox.Text)
                || string.IsNullOrEmpty(PODtextBox.Text) || string.IsNullOrEmpty(AvailableRoomTextBox.Text))
            {
                MessageBox.Show("Please Fill the data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                InventoryClass inventory = new InventoryClass()
                {
                    ID = IDTextBox.Text,
                    Name = NameTextBox.Text,
                    FBFolioNumber = FolioNumberTextBox.Text,
                    ledgerbalance = LedgerBalanceTextbox.Text,
                    actualbalance = ActualBalanceOnTextBox.Text,
                    surplus = SurplusTextBox.Text,
                    defi = DeficidencyTextBox.Text,
                    remark = RemarkTextBox.Text,
                    purposeOfBuying = POBtextBox.Text,
                    purposeOfDistribte = PODtextBox.Text,
                    availableroom = AvailableRoomTextBox.Text,
                };
                var setter = client.Set("Inventory/" +"Add/" + IDTextBox.Text, inventory);
                MessageBox.Show("Inventory Details Added Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Inventory details added");
                clear();
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IDTextBox.Text))
            {
                MessageBox.Show("Please Enter the ID");
            }
            else
            {
                var result = client.Get("Inventory/" + "Add/" + IDTextBox.Text);
        
                // Check if the result is null or if essential properties are null
                if (result == null || result.ResultAs<InventoryClass>() == null || result.ResultAs<InventoryClass>().ID == null)
                {
                    MessageBox.Show("No matching ID found in the database");
                    // You may want to clear the textboxes here or handle it as per your requirements
                    clear();
                }
                else
                {
                    InventoryClass inven = result.ResultAs<InventoryClass>();
                    IDTextBox.Text = inven.ID;
                    NameTextBox.Text = inven.Name;
                    FolioNumberTextBox.Text = inven.FBFolioNumber;
                    LedgerBalanceTextbox.Text = inven.ledgerbalance;
                    ActualBalanceOnTextBox.Text = inven.actualbalance;
                    SurplusTextBox.Text = inven.surplus;
                    DeficidencyTextBox.Text = inven.defi;
                    RemarkTextBox.Text = inven.remark;
                    POBtextBox.Text = inven.purposeOfBuying;
                    PODtextBox.Text = inven.purposeOfDistribte;
                    AvailableRoomTextBox.Text = inven.availableroom;
                }
            }
            
        }

        private void InventoryUpdateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IDTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(FolioNumberTextBox.Text)
               || string.IsNullOrEmpty(LedgerBalanceTextbox.Text) || string.IsNullOrEmpty(ActualBalanceOnTextBox.Text) || string.IsNullOrEmpty(SurplusTextBox.Text)
               || string.IsNullOrEmpty(DeficidencyTextBox.Text) || string.IsNullOrEmpty(RemarkTextBox.Text) || string.IsNullOrEmpty(POBtextBox.Text)
               || string.IsNullOrEmpty(PODtextBox.Text) || string.IsNullOrEmpty(AvailableRoomTextBox.Text))
            {
                MessageBox.Show("Please Fill the data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                InventoryClass inventory = new InventoryClass()
                {
                    ID = IDTextBox.Text,
                    Name = NameTextBox.Text,
                    FBFolioNumber = FolioNumberTextBox.Text,
                    ledgerbalance = LedgerBalanceTextbox.Text,
                    actualbalance = ActualBalanceOnTextBox.Text,
                    surplus = SurplusTextBox.Text,
                    defi = DeficidencyTextBox.Text,
                    remark = RemarkTextBox.Text,
                    purposeOfBuying = POBtextBox.Text,
                    purposeOfDistribte = PODtextBox.Text,
                    availableroom = AvailableRoomTextBox.Text,
                };
                var setter = client.Update("Inventory/" + "Add/" + IDTextBox.Text, inventory);
                MessageBox.Show("Inventory Details updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Inventory Details Updated");
                clear();
            }
        }

        private void InventoryDeleteButton_Click(object sender, EventArgs e)
        {
            if (IDTextBox.Text == "")
            {
                MessageBox.Show("Please enter the correct username");
            }
            else
            {
                var setter = client.Delete("Inventory/" + "Add/" + IDTextBox.Text);
                MessageBox.Show("Inventory deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Inventory Details Deleted");
                clear();
            }
        }
        int key = 0;
        private void InventoryView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            key = Convert.ToInt32(Vehicle_GridView.SelectedRows[0].Cells[0].Value.ToString());
            IDTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[0].Value.ToString();
            NameTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[1].Value.ToString();
            FolioNumberTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[2].Value.ToString();
            LedgerBalanceTextbox.Text = Vehicle_GridView.SelectedRows[0].Cells[3].Value.ToString();
            ActualBalanceOnTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[4].Value.ToString();
            SurplusTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[5].Value.ToString();
            DeficidencyTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[6].Value.ToString();
            RemarkTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[7].Value.ToString();
            POBtextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[8].Value.ToString();
            PODtextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[9].Value.ToString();
            AvailableRoomTextBox.Text = Vehicle_GridView.SelectedRows[0].Cells[10].Value.ToString();
        }

        private void Refresh_Button_Click(object sender, EventArgs e)
        {
            Vehicle_GridView.DataSource = null;
            Vehicle_GridView.Rows.Clear();
            Vehicle_Grid_View();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        string user = Loginform.User;
        public void Track(string Activity)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");  // Fix the date format
            string time = DateTime.Now.ToString("HH:mm:ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(user, date, time, Activity, "Inventory");

        }

        /*8private void InventoryRefreshButton_Click(object sender, EventArgs e)
        {

        }*/
    }
}
