using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Interfaces
{
    internal interface IBloodCollectionCaller
    {
        void SelectBlood(BloodModel selectedBlood);
    }
}
