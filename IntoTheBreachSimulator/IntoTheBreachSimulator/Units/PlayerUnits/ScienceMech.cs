using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public abstract class ScienceMech : Mech
    {
        public ScienceMech(Square pLocation, Pilot pPilot, int pBaseHealth, int pBaseMove) : 
            base(pLocation, pPilot, UnitType.Science, pBaseHealth, pBaseMove)
        {

        }
    }
}
