using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    class SiegeMech : RangedMech
    {
        public SiegeMech(Square pLocation, Pilot pPilot) : base(pLocation, pPilot, 2, 3)
        {
            mFirstSlot = new ClusterArtillery(this);
        }

        public override string Name => "Siege";
    }
}
