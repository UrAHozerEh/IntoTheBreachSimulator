using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.AllEquipment.PassiveEquipment;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public class GravityMech : ScienceMech
    {
        public GravityMech(Square pLocation, Pilot pPilot) : base(pLocation, pPilot, 3, 4)
        {
            mFirstSlot = new GravWell(this);
            mSecondSlot = new VekHormones(this);
        }

        public override string Name => "Grav";
    }
}
