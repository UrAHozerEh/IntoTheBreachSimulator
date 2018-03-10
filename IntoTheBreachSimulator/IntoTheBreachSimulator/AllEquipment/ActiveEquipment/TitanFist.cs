using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class TitanFist : ActiveEquipment
    {
        public TitanFist(Unit pUnit) : base(pUnit, 0, UnitType.Prime)
        {

        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            throw new NotImplementedException();
        }
    }
}
