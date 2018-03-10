using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.AllEquipment;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public class JudoMech : PrimeMech
    {
        public override string Name => "Judo";

        public JudoMech(Square pLocation, Pilot pPilot) : 
            base(pLocation, pPilot, 3, 4)
        {
            mFirstSlot = new ViceFist(this);
            mIsBaseArmored = true;
        }
    }
}
