using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public abstract class ActiveEquipment : Equipment
    {
        public ActiveEquipment(Unit pUnit, int pReactorCost, UnitType pEquipmentType) : 
            base(pUnit, pReactorCost, pEquipmentType)
        {

        }

        public abstract List<PlayerAttack> PossibleAttacks();
    }
}
