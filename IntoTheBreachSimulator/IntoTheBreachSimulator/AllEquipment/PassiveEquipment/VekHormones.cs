using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.PassiveEquipment
{
    public class VekHormones : PassiveEquipment
    {
        public int BonusDamage
        {
            get
            {
                int value = 1;
                foreach (EquipmentUpgrade u in Upgrades.Values)
                    if (u.Purchased)
                        ++value;
                return value;
            }
        }

        public VekHormones(Unit pUnit) : base(pUnit, 1, UnitType.Universal)
        {
            EquipmentUpgrade upgrade = new EquipmentUpgrade("FirstBonusDamage", 1);
            Upgrades.Add(upgrade.Name, upgrade);
            upgrade = new EquipmentUpgrade("SecondBonusDamage", 2);
            Upgrades.Add(upgrade.Name, upgrade);
        }
    }
}
