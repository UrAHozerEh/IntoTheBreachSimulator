using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public abstract class Mech : PlayerUnit
    {
        public override int CurrentMove => BaseMove + (Pilot?.MoveBonus ?? 0);
        public override int MaxHealth => BaseHealth + (Pilot?.HealthBonus ?? 0);

        public Mech(Square pLocation, Pilot pPilot, UnitType pType, int pBaseHealth, int pBaseMove) :
            base(pLocation, pPilot, pType, pBaseHealth, pBaseMove)
        {
            mIsMassive = true;
        }
    }
}
