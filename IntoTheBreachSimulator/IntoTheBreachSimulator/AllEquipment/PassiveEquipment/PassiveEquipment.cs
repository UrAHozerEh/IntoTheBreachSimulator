using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.AllEquipment.PassiveEquipment
{
    public abstract class PassiveEquipment : Equipment
    {
        public PassiveEquipment(Unit pUnit, int pReactorCost, UnitType pEquipmentType) : 
            base(pUnit, pReactorCost, pEquipmentType)
        {

        }
    }
}
