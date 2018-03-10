using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public abstract class BruteMech : Mech
    {
        public BruteMech(Square pLocation, Pilot pPilot, int pBaseHealth, int pBaseMove) :
            base(pLocation, pPilot, UnitType.Brute, pBaseHealth, pBaseMove)
        {
            
        }
    }
}
