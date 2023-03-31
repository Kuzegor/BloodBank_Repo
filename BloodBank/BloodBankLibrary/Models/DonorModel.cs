using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankLibrary.Models
{
    public class DonorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int BloodGroup { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        private Uri photoUri;
        public Uri PhotoUri
        {
            get
            {
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                if (Photo != null)
                {
                    photoUri = new Uri(strWorkPath + Photo);
                    return photoUri;
                }
                else
                {
                    photoUri = new Uri(strWorkPath + @"\Images\ph.jpg");
                    return photoUri;
                }
            }
        }

        public BloodGroupModel BloodGroupModel { get; set; }
    }
}
