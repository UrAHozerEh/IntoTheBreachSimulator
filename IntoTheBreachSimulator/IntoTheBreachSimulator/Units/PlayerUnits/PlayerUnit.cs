using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.AllEquipment;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public abstract class PlayerUnit : Unit
    {
        private UnitType mType;
        public UnitType Type => mType;

        private Pilot mPilot;
        public Pilot Pilot => mPilot;

        public PlayerUnit(Square pLocation, Pilot pPilot, UnitType pType, int pBaseHealth, int pBaseMove) :
            base(pLocation, Team.Player, pBaseHealth, pBaseMove)
        {
            mType = pType;
            mPilot = pPilot;
        }

        public override List<PlayerAttack> PossibleAttacks(int pEquipment = 0)
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();
            if (HasAttacked)
                return attacks;
            if (mFirstSlot is ActiveEquipment activeOne && (pEquipment == 0 || pEquipment == 1))
            {
                attacks.AddRange(activeOne.PossibleAttacks());
            }
            if (mSecondSlot is ActiveEquipment activeTwo && (pEquipment == 0 || pEquipment == 2))
            {
                attacks.AddRange(activeTwo.PossibleAttacks());
            }
            return attacks;
        }

        public override List<Square> MoveSquares()
        {
            if (Statuses.Contains(UnitStatus.Web))
                return new List<Square>();
            if (HasMoved || (HasAttacked && !(mPilot is ChenRong)))
                return new List<Square>();
            List<Square> found = new List<Square>();
            if (HasAttacked)
                FindSquares(ref found, Location, 0, 1);
            else
                FindSquares(ref found, Location, 0, CurrentMove);
            return found;
        }
    }
}
