using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectUC
{
    internal class Register_Login
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DateofBirth { get; set; }
        public string Type { get; set; }
        public int phoneNumber { get; set; }
        public string address { get; set; }
        public string Image {  get; set; }
    }

    internal class Addimage
    {
        public string img { get; set; }
        public string name { get; set; }
        public string ph { get; set; }
        public string id {  get; set; }
    }
}
