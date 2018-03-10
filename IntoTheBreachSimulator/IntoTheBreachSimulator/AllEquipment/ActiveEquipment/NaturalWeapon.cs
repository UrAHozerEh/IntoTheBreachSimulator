using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class NaturalWeapon : ActiveEquipment
    {
        public NaturalWeapon(Unit pUnit) : base(pUnit, 0, UnitType.Universal)
        {

        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            return Unit.PossibleAttacks();
        }
    }
}
