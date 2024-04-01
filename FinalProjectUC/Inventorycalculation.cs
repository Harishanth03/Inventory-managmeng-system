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
using Google.Type;
using Guna.UI2.WinForms;
using static FinalProjectUC.UserTracking;


namespace FinalProjectUC
{
    public partial class UserContrInventoryCalculation : UserControl
    {
        public UserContrInventoryCalculation()
        {
            InitializeComponent();
        }
        //connection
        IFirebaseConfig connction = new FirebaseConfig()
        {
            AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
            BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private void UserContrInventoryCalculation_Load(object sender, EventArgs e)
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
            TotalCostTextBox.ReadOnly = true;


            if(Loginform.Userrole == "User")
            {
                DetailsUpdateBitton.Hide();
                DetailsDeleteButton.Hide();
            }
        }
        public void clear()
        {
            InventoryNumberTextBox.Clear();
            ItemNameTextBox.Clear();
            AvailableQuantityTextBox.Clear();
            OneQTYTextBox.Clear();
            QuantityNeedTextBox.Clear   ();
            TotalCostTextBox.Clear();
            responsivePersonTextBox.Clear();
     
        }
        //InventoryCalculation Add =================================================================================================================================
        private void SaveButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(InvenNumtextBox.Text) || string.IsNullOrEmpty(ITMNameTextBox.Text) || string.IsNullOrEmpty(AvaiQTYTextBox1.Text) ||
                string.IsNullOrEmpty(PriceTextBox1.Text))
            {
                MessageBox.Show("Please Fill your details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Inventorycalculationclass addding = new Inventorycalculationclass()
                {
                    InventoryNumber = InvenNumtextBox.Text,
                    InventoryName = ITMNameTextBox.Text,
                    availableQTY = AvaiQTYTextBox1.Text,
                    oneQTYprice = PriceTextBox1.Text,
                };
                FirebaseResponse response = client.Set("CalculationAdd/" + InvenNumtextBox.Text, addding);
                MessageBox.Show("Inventory Details added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Calculation Details added");
                clear();
;           }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(InventoryNumberTextBox.Text) || string.IsNullOrEmpty(ItemNameTextBox.Text) || string.IsNullOrEmpty(AvailableQuantityTextBox.Text)
                || string.IsNullOrEmpty(OneQTYTextBox.Text) ||
                string.IsNullOrEmpty(responsivePersonTextBox.Text) || string.IsNullOrEmpty(DateTimeBox.Text))
            {
                MessageBox.Show("Missing information", "Information",MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                if(int.TryParse(QuantityNeedTextBox.Text, out int QuantityNeed) && int.TryParse(AvailableQuantityTextBox.Text, out int AvailableQuantity))
                {
                    if (QuantityNeed > AvailableQuantity)
                    {
                        MessageBox.Show("You can as the need quantity more than available quantity");
                    }
                    else
                    {
                        if(int.TryParse(OneQTYTextBox.Text, out int OneQuantityprice))
                        {
                            int totalCost = OneQuantityprice * QuantityNeed;
                            TotalCostTextBox.Text = totalCost.ToString();
                            int updateTotalQantity = AvailableQuantity - QuantityNeed;
                            
                        }
                        else
                        {
                            MessageBox.Show("Invalid price value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid quantity values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SearchBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InventoryNumberTextBox.Text))
            {
                MessageBox.Show("Please Enter the vehicle Number");
            }
            else
            {
                var result = client.Get("CalculationAdd/" + InventoryNumberTextBox.Text);

                // Check if the result is null or if essential properties are null
                if (result == null || result.ResultAs<Inventorycalculationclass>() == null || result.ResultAs<Inventorycalculationclass>().InventoryNumber == null)
                {
                    MessageBox.Show("No matching Vehicle Number found in the database");
                    // You may want to clear the textboxes here or handle it as per your requirements
                }
                else
                {
                    Inventorycalculationclass inven = result.ResultAs<Inventorycalculationclass>();
                    InventoryNumberTextBox.Text = inven.InventoryNumber;
                    ItemNameTextBox.Text = inven.InventoryName;
                    AvailableQuantityTextBox.Text = inven.availableQTY;
                    OneQTYTextBox.Text = inven.oneQTYprice;
                  //  QuantityNeedTextBox.Text = inven.qtyne
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        // Inventory Calculation Update==============================================================================================================================
        private void DetailsUpdateBitton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InvenNumtextBox.Text) || string.IsNullOrEmpty(ITMNameTextBox.Text) || string.IsNullOrEmpty(AvaiQTYTextBox1.Text) ||
                string.IsNullOrEmpty(PriceTextBox1.Text))
            {
                MessageBox.Show("Please Fill your details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Inventorycalculationclass addding = new Inventorycalculationclass()
                {
                    InventoryNumber = InvenNumtextBox.Text,
                    InventoryName = ITMNameTextBox.Text,
                    availableQTY = AvaiQTYTextBox1.Text,
                    oneQTYprice = PriceTextBox1.Text,
                };
                FirebaseResponse response = client.Update("CalculationAdd/" + InvenNumtextBox.Text,addding);
                MessageBox.Show("Inventory Details Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Calculation details Updated");
                clear();
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(InvenNumtextBox.Text))
            {
                MessageBox.Show("Please select the ID");
            }
            else
            {
                var result = client.Get("CalculationAdd/" + InvenNumtextBox.Text);
                Inventorycalculationclass cal = result.ResultAs<Inventorycalculationclass>();
                InvenNumtextBox.Text = cal.InventoryNumber;
                ITMNameTextBox.Text = cal.InventoryName;
                AvaiQTYTextBox1.Text = cal.availableQTY;
                PriceTextBox1.Text = cal.oneQTYprice;

            }
        }
        //Calculation View==========================================================================================================
        public void InventoryCalculation()
        {
            try
            {
                FirebaseResponse response = client.Get("InventoryCalculationSave/Save/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    // Check if the response is a JSON array
                    // ... (your existing code)

                    // Check if the response is a JSON array
                    if (response.Body.StartsWith("["))
                    {
                        // If it's an array, deserialize it as a List
                        List<Inventorycalculationclass> inventoryList = response.ResultAs<List<Inventorycalculationclass>>();

                        foreach (var inventory in inventoryList)
                        {
                            if (inventory != null)
                            {
                                Fuelview.Rows.Add(
                                    inventory.InventoryNumber,
                                    inventory.InventoryName,
                                    inventory.availableQTY,
                                    inventory.oneQTYprice,
                                    inventory.qtyneed,
                                    inventory.totalCost,
                                    inventory.responsibleperson,
                                    inventory.date.ToString()
                                );
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is no data in the database");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Inventory Calculation Save ================================================================================================================================
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InventoryNumberTextBox.Text) || string.IsNullOrEmpty(ItemNameTextBox.Text) || string.IsNullOrEmpty(AvailableQuantityTextBox.Text)
                || string.IsNullOrEmpty(OneQTYTextBox.Text) || string.IsNullOrEmpty(TotalCostTextBox.Text) ||
                string.IsNullOrEmpty(responsivePersonTextBox.Text) || string.IsNullOrEmpty(DateTimeBox.Text))
            {
                MessageBox.Show("Missing information", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                Inventorycalculationclass inven = new Inventorycalculationclass()
                {
                   InventoryNumber = InventoryNumberTextBox.Text,
                   InventoryName = ItemNameTextBox.Text,
                   availableQTY = AvailableQuantityTextBox.Text,
                   oneQTYprice = OneQTYTextBox.Text,
                   qtyneed = QuantityNeedTextBox.Text,
                   totalCost = TotalCostTextBox.Text,
                   responsibleperson = responsivePersonTextBox.Text,
                   date = DateTimeBox.Value,
                };
                var insert = client.Push("InventoryCalculationSave/Save/", inven);

                if (insert.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Successful insertion
                    MessageBox.Show("Inventory calculation Details added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Handle the error
                    MessageBox.Show($"Error inserting tracking data. Status code: {insert.StatusCode}");
                }
               // FirebaseResponse respons = client.Set("InventoryCalculationSave/Save/" + InventoryNumberTextBox.Text, inven);
                
                Track("Calculation Details Saved");

                if (int.TryParse(QuantityNeedTextBox.Text, out int QuantityNeed) && int.TryParse(AvailableQuantityTextBox.Text, out int AvailableQuantity))
                {
                    int UpdateQuantity =   AvailableQuantity - QuantityNeed;

                    Inventorycalculationclass updatecdetails = new Inventorycalculationclass()
                    {
                        InventoryNumber = InventoryNumberTextBox.Text,
                        InventoryName = ItemNameTextBox.Text,
                        availableQTY = UpdateQuantity.ToString(),
                        oneQTYprice = OneQTYTextBox.Text
                    };
                    FirebaseResponse response = client.Update("CalculationAdd/" + InventoryNumberTextBox.Text,updatecdetails);
                    MessageBox.Show("Inventory Details Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                else
                {
                    MessageBox.Show("Unable to Update!");
                }
            }

        }
        // Exit Button===============================================================================================================================================
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //View Button================================================================================================================================================
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            InventoryCalculation();
        }
        // Inventory Calculation delete==============================================================================================================================
        private void DetailsDeleteButton_Click(object sender, EventArgs e)
        {
            if (InvenNumtextBox.Text == "")
            {
                MessageBox.Show("Please enter the correct username");
            }
            else
            {
                var setter = client.Delete("CalculationAdd/" + InvenNumtextBox.Text);
                MessageBox.Show("Inventory Calculation deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Calculation deatils deleted");
                clear();
            }
        }
        string User = Loginform.User;
        //Tracking===============================================================================================================================================

        public void Track(string Activity)
        {
            string Date = System.DateTime.Now.ToString("yyyy-MM-dd");
            string timme = System.DateTime.Now.ToString("HH-mm-ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(User, Date, timme, Activity, "Inventory Calculation");
        }
    }
}
