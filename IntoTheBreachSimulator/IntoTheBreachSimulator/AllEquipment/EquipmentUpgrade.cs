using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.AllEquipment
{
    public class EquipmentUpgrade
    {
        private string mName;
        public string Name => mName;

        private int mReactorCost;
        public int ReactorCost => mReactorCost;

        public bool Purchased { get; set; }

        public EquipmentUpgrade(string pName, int pReactorCost, bool pPurchased = false)
        {
            mName = pName;
            mReactorCost = pReactorCost;
            Purchased = pPurchased;
        }
    }
}
