using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankLibrary.Models
{
    public class BloodModel
    {
        public int Id { get; set; }
        public int DonationType { get; set; }
        public DonorModel Donor { get; set; }
        public int BloodGroup { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public string DateOfCollection { get; set; }
        public DoctorModel DoctorInCharge { get; set; }
        public DonationTypeModel DonationTypeModel { get; set; }
        public BloodGroupModel BloodGroupModel { get; set; }

    }
}
