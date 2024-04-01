using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Guna.UI2.WinForms;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace FinalProjectUC
{
    public partial class Report : UserControl
    {
        public Report()
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

        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(connction);
                report_Grid_View();
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message + "check your cpnnection");
            }
            Vehicle_Model_TextBox.ReadOnly = true;
            
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Vehicle_Number_TextBox.Text) || string.IsNullOrEmpty(Vehicle_Model_TextBox.Text) || string.IsNullOrEmpty(Branch_ComboBox.Text)
                || string.IsNullOrEmpty(Fuel_Type_ComboBox.Text) || string.IsNullOrEmpty(Driver_TextBox.Text) || string.IsNullOrEmpty(Issuing_Person_TextBox.Text)
                || string.IsNullOrEmpty(Reason_TextBox.Text))
            {
                MessageBox.Show("Missing Information", "Fuel Calculation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                reportgeneration vehicle = new reportgeneration()
                {
                    vehicle_number = Vehicle_Number_TextBox.Text,
                    vehicle_model = Vehicle_Model_TextBox.Text,
                    branch = Branch_ComboBox.Text,
                    driver = Driver_TextBox.Text,
                    issuing_person = Issuing_Person_TextBox.Text,
                    fuel_type = Fuel_Type_ComboBox.Text,
                    issuing_date_and_time = IssuingDate_DateTime_Picker.Text,
                    reason = Reason_TextBox.Text

                };
                var setter = client.Set("Report/add/" + Vehicle_Number_TextBox.Text, vehicle);
                MessageBox.Show("Vehicle Details Added Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("Vehicle issued");
                clear();
            }
        }

        public void clear()
        {
            Vehicle_Number_TextBox.Clear();
            Vehicle_Model_TextBox.Clear();
            Driver_TextBox.Clear();
            Issuing_Person_TextBox.Clear();
            Reason_TextBox.Clear();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Vehicle_Number_TextBox.Text))
            {
                MessageBox.Show("Please Enter the vehicle Number");
            }
            else
            {
                var result = client.Get("Vehicle/" + "Add/" + Vehicle_Number_TextBox.Text);

                // Check if the result is null or if essential properties are null
                if (result == null || result.ResultAs<reportgeneration>() == null || result.ResultAs<reportgeneration>().vehicle_number == null)
                {
                    MessageBox.Show("No matching Vehicle Number found in the database");
                   
                }
                else
                {
                    reportgeneration fuel = result.ResultAs<reportgeneration>();
                    Vehicle_Number_TextBox.Text = fuel.vehicle_number;
                    Vehicle_Model_TextBox.Text = fuel.vehicle_model;
                    Branch_ComboBox.Text = fuel.branch;
                    Driver_TextBox.Text = fuel.driver;
                    Issuing_Person_TextBox.Text = fuel.issuing_person;
                    Fuel_Type_ComboBox.Text = fuel.fuel_type;
                    IssuingDate_DateTime_Picker.Text = fuel.issuing_date_and_time;
                    Reason_TextBox.Text = fuel.reason;
                }
            }
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            clear();
        }

        //Vehicle GridView code
        public void report_Grid_View()
        {
            try
            {
                FirebaseResponse response = client.Get("Report/add/");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, reportgeneration> report = response.ResultAs<Dictionary<string, reportgeneration>>();

                    if (report != null)
                    {
                        foreach (var get in report)
                        {
                            Vehicle_Issuing_Form_DataGridView.Rows.Add(
                                get.Value.vehicle_number,
                                get.Value.vehicle_model,
                                get.Value.branch,
                                get.Value.driver,
                                get.Value.issuing_person,
                                get.Value.fuel_type,
                                get.Value.issuing_date_and_time,
                                get.Value.reason
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

        private void Refresh_Button_Click(object sender, EventArgs e)
        {
            Vehicle_Issuing_Form_DataGridView.DataSource = null;
            Vehicle_Issuing_Form_DataGridView.Rows.Clear();
            report_Grid_View();
        }

        private void Download_PDF_Button_Click(object sender, EventArgs e)
        {
            try
            {
                
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Create a new Document
                    Document document = new Document();
                    PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();

                    // Add a table to the document
                    PdfPTable pdfTable = new PdfPTable(Vehicle_Issuing_Form_DataGridView.Columns.Count);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    // Add headers from the DataGridView to the PDF table
                    foreach (DataGridViewColumn column in Vehicle_Issuing_Form_DataGridView.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                        pdfTable.AddCell(cell);
                    }

                    // Add data from the DataGridView to the PDF table
                    foreach (DataGridViewRow row in Vehicle_Issuing_Form_DataGridView.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            pdfTable.AddCell(cell.Value.ToString());
                        }
                    }

                    // Add the PDF table to the document
                    document.Add(pdfTable);

                    // Close the document
                    document.Close();

                    MessageBox.Show("PDF created successfully!");
                    Track("PDF Downloaded from Vehicle Issue");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            /* if(Vehicle_Issuing_Form_DataGridView.Rows.Count > 0)
             {
                 SaveFileDialog savePDF = new SaveFileDialog();
                 savePDF.Filter = "PDF (*.pdf|*.pdf)";
                 savePDF.FileName = "Report Generating";
                 bool error_message = false;
                 if(savePDF.ShowDialog() == DialogResult.OK)
                 {
                     if(File.Exists(savePDF.FileName))
                     {
                         try
                         {
                             File.Delete(savePDF.FileName);
                         }
                         catch (Exception ex)
                         {
                             error_message = true;
                             MessageBox.Show("Unable to write data in disk"+ex.Message);
                         }

                     }
                     if(!error_message)
                     {
                         try
                         {
                             PdfPTable tdptable = new PdfPTable(Vehicle_Issuing_Form_DataGridView.Columns.Count);
                             tdptable.DefaultCell.Padding = 2;
                             tdptable.WidthPercentage = 100;
                             tdptable.HorizontalAlignment = Element.ALIGN_LEFT;
                             foreach(DataGridViewColumn col in Vehicle_Issuing_Form_DataGridView.Columns)
                             {
                                 PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                                 tdptable.AddCell(cell);
                             }
                             foreach(DataGridViewRow viewRow in Vehicle_Issuing_Form_DataGridView.Rows)
                             {
                                 foreach (DataGridViewCell dcell in viewRow.Cells)
                                 {
                                     tdptable.AddCell(dcell.Value.ToString());
                                 }
                             }

                             using (FileStream fileStream = new FileStream(savePDF.FileName, FileMode.Create))
                             {
                                 Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                 document.Open();
                                 document.Add(tdptable);
                                 document.Close();
                                 fileStream.Close();
                             }

                             MessageBox.Show("Data export successfully", "Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                         }
                         catch
                         { 
                             MessageBox.Show("Error  while exporting the data","Information",MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                     }
                 }
             }
             else
             {
                 MessageBox.Show("No records found", "Information", MessageBoxButtons.OK, MessageBoxIcon.None);
             }*/


        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        string user = Loginform.User;

        public void Track(string Activity)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            string timme = DateTime.Now.ToString("HH-mm-ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(user, Date, timme, Activity, "Vehicle Tracking");
        }
    }
}
