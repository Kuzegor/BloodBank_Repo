using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankLibrary.Models
{
    public class IssueModel
    {
        public int Id { get; set; }
        public RecipientModel Recipient { get; set; }
        public BloodModel Blood { get; set; }
        public double BloodAmount { get; set; }
        public string Unit { get; set; }
        public decimal PricePaid { get; set; }
        public DoctorModel DoctorInCharge { get; set; }
        public string DateOfIssue { get; set; }

        public DonationTypeModel DonationTypeModel { get; set; }
        public BloodGroupModel BloodGroupModel { get; set; }
    }
}
