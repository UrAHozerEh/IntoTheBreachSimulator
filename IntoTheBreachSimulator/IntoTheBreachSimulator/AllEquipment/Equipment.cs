using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.AllEquipment
{
    public abstract class Equipment
    {
        private Unit mUnit;
        public Unit Unit => mUnit;

        private Dictionary<string, EquipmentUpgrade> mUpgrades;
        public Dictionary<string, EquipmentUpgrade> Upgrades => mUpgrades;

        private int mReactorCost;
        public int ReactorCost => mReactorCost;

        public Equipment(Unit pUnit, int pReactorCost, UnitType pEquipmentType)
        {
            mUnit = pUnit;
            mUpgrades = new Dictionary<string, EquipmentUpgrade>();
        }
    }
}
