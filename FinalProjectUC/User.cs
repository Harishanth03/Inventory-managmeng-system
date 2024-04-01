using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Net;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using FireSharp;
using System.Threading.Tasks;

namespace FinalProjectUC
{
    public partial class User : UserControl
    {
        public User()
        {
            InitializeComponent();
            
        }
        private IFirebaseClient firebaseClient;
        //connection
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "YEewY6PO1lM2NR3DkXM2z4BYdhHRHYteh7rj55Ku",
            BasePath = "https://urbancouncil-915ed-default-rtdb.firebaseio.com/"
        };
        //client connnection
        IFirebaseClient client;
        public void cleartext()
        {
            NameTB.Clear();
            UsernameTB.Clear();
            PasswordTB.Clear();
            PhoneNumberTB.Clear();
            //TypeComboBox.Clear();
            AddressTB.Clear();
        }
        private void User_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Problem on the internet");
            }

            RoleLable.Text = Loginform.Userrole;
            Namelable.Text = Loginform.User;
            numberLable.Text = Loginform.phone;
            GetImage();

        }
        
        public void ViewUser()
        {
            try
            {
                FirebaseResponse response = client.Get("User/" + "Register/");
                if (response == null)
                {
                    MessageBox.Show("There is no Data In the Database");
                }
                else
                {
                    Dictionary<string, Register_Login> getUser = response.ResultAs<Dictionary<string, Register_Login>>();
                    foreach (var get in getUser)
                    {
                        UserViews.Rows.Add(
                            get.Value.name,
                            get.Value.gender,
                            get.Value.Username,
                            get.Value.Password,
                            get.Value.DateofBirth,
                            get.Value.phoneNumber,
                            get.Value.Type,
                            get.Value.address
                            );
                    }
                }
            }
            catch
            {
                MessageBox.Show("No data stored");
            }

            
        }

        private void UserAddButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(NameTB.Text) || string.IsNullOrEmpty(GenderTB.Text) || string.IsNullOrEmpty(UsernameTB.Text) || string.IsNullOrEmpty(PasswordTB.Text) || string.IsNullOrEmpty(DOBTB.Text) || string.IsNullOrEmpty(PhoneNumberTB.Text) || string.IsNullOrEmpty(TypeComboBox.Text) || string.IsNullOrEmpty(AddressTB.Text) || AddImagePictureBox.Image == null)
            {
                MessageBox.Show("Please Fill the data","Information",MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            else
            {
                
                Register_Login register_Login = new Register_Login()
                {
                    name = NameTB.Text,
                    gender = GenderTB.Text,
                    Username = UsernameTB.Text,
                    Password = PasswordTB.Text,
                    DateofBirth = DOBTB.Text,
                    Type = TypeComboBox.Text,
                    phoneNumber = int.Parse(PhoneNumberTB.Text),
                    address = AddressTB.Text,
                    Image = ImageIntoBase64(AddImagePictureBox)
                };
                var setter = client.Set("User/" + "Register/" + UsernameTB.Text, register_Login);
                MessageBox.Show("User added","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("User Added");
                cleartext();
            }
            
        }

        private void UserUpdateButton_Click(object sender, EventArgs e)
        {
            if (UsernameTB.Text == "")
            {
                MessageBox.Show("Please enter the correct username");
            }
            else
            {
                Register_Login register_Login = new Register_Login()
                {
                    name = NameTB.Text,
                    gender = GenderTB.Text,
                    Username = UsernameTB.Text,
                    Password = PasswordTB.Text,
                    DateofBirth = DOBTB.Text,
                    Type = TypeComboBox.Text,
                    phoneNumber = int.Parse(PhoneNumberTB.Text),
                    address = AddressTB.Text,
                };
                var setter = client.Update("User/" + "Register/" + UsernameTB.Text, register_Login);
                MessageBox.Show("User Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("User Details Updated by " + user);
                
                cleartext();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(UsernameTB.Text == "")
            {
                MessageBox.Show("Please Enter the username","Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                var result = client.Get("User/"+ "Register/" + UsernameTB.Text);
                Register_Login std = result.ResultAs<Register_Login>();
                if( std.Username != UsernameTB.Text )
                {
                    MessageBox.Show("Please Enter the correct username", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    NameTB.Text = std.name;
                    GenderTB.Text = std.gender;
                    UsernameTB.Text = std.Username;
                    PasswordTB.Text = std.Password;
                    DOBTB.Text = std.DateofBirth.ToString();
                    PhoneNumberTB.Text = std.phoneNumber.ToString();
                    TypeComboBox.Text = std.Type;
                    AddressTB.Text = std.address;
                    
                }
                
            }
            
        }

        private void UserDeleteButton_Click(object sender, EventArgs e)
        {
            if (UsernameTB.Text == "")
            {
                MessageBox.Show("Please enter the correct username");
            }
            else
            {
                var setter = client.Delete("User/" + "Register/" + UsernameTB.Text);
                MessageBox.Show("User deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Track("User Deleted");
                cleartext();
            }
        }

        /*private void UpdateButton_Click(object sender, EventArgs e)
        {
            Addimage img = new Addimage()
            {
                id = IDTextBox.Text,
                name = NameTextBox.Text,
                ph = PNTextBox.Text,
                img = imagetostring(AddImagePictureBox)
            };
            var set = client.Set("User/" + "Addimage/" + IDTextBox.Text, img);
      
        }*/

        private void Refreshbuttton_Click(object sender, EventArgs e)
        {
            UserViews.DataSource = null;
            UserViews.Rows.Clear();
            ViewUser();

        }
       // private string savedImagePath;

        private  void AddImagePictureBox_Click(object sender, EventArgs e)
        {
            
        }
        
        private void PNTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
        string user = Loginform.User;

        //Tracking function================================================================================
        public void Track(string Activity)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");  // Fix the date format
            string time = DateTime.Now.ToString("HH:mm:ss");

            UserTracking tracking = new UserTracking();
            tracking.SystemTracking(user, date, time, Activity, "User Control");

        }
      
        

        private void UserViews_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public static string Username = Loginform.name;
        private void BrowzeImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files(*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png |All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                AddImagePictureBox.Load(openFileDialog.FileName);
            }
           /* Register_Login image = new Register_Login()
            {
                Username = Loginform.User,
                Image = Imageintobase64(AddImagePictureBox)
            };
            var set = client.Set("User/" + "RegisterImage/" + Username, image);
            MessageBox.Show("Image inserted successfully","User Information", MessageBoxButtons.OK,MessageBoxIcon.Information);*/
            
        }


        private Image GetImage()
        {
            var get = client.Get("User/Register/" + Loginform.User);

            if (get.Body == "null") // Check if the response is null
            {
                MessageBox.Show("No data found for the specified username.");
                return null; // Or return a default image
            }

            Register_Login img = get.ResultAs<Register_Login>();
            //Loginform.User = img.Username;

            if (!string.IsNullOrEmpty(img.Image))
            {
                AddImagePictureBox.Image = ImageIntoImage(img.Image);
                return AddImagePictureBox.Image;
            }
            else
            {
                MessageBox.Show("No image found for the specified username.");
                return null; // Or return a default image
            }
        }

        public static string ImageIntoBase64(PictureBox pbox)
        {
            if (pbox.Image == null)
            {
                return null; // Or handle the case when there is no image
            }

            MemoryStream stream = new MemoryStream();
            pbox.Image.Save(stream, pbox.Image.RawFormat);
            return Convert.ToBase64String(stream.ToArray());
        }

        public static Image ImageIntoImage(string str64)
        {
            if (string.IsNullOrEmpty(str64))
            {
                MessageBox.Show("Base64 string is empty or null.");
                return null; // Or return a default image
            }

            try
            {
                byte[] img = Convert.FromBase64String(str64);
                using (MemoryStream stream = new MemoryStream(img))
                {
                    return Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                MessageBox.Show("Error converting base64 string to image: " + ex.Message);
                return null; // Or return a default image
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        { 
            if(string.IsNullOrEmpty(UsernameTB.Text))
            {
                MessageBox.Show("Please enter the user name","Warning",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files(*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png |All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    AddImagePictureBox.Load(openFileDialog.FileName);
                }
                Register_Login image = new Register_Login()
                {
                    Username =UsernameTB.Text,
                    Image = ImageIntoBase64(AddImagePictureBox)
                };
                var set = client.Update("User/" + "RegisterImage/" + Username, image);
                MessageBox.Show("Image inserted successfully","User Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }

        }
    }
}
