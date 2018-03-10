using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Units;
using IntoTheBreachSimulator.Units.PlayerUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitSetHasAttacked : GameAction
    {
        private Unit mUnit;

        private bool mBonusMove;
        private bool mPrevHasAttacked;

        public UnitSetHasAttacked(Unit pUnit)
        {
            mUnit = pUnit;
        }

        public override void Do(ActionStack pActionStack)
        {
            mPrevHasAttacked = mUnit.HasAttacked;
            mUnit.HasAttacked = true;
            mBonusMove = false;
            if (mUnit.HasMoved && mUnit is PlayerUnit player)
            {
                if (player.Pilot is ChenRong)
                {
                    mBonusMove = true;
                    mUnit.HasMoved = false;
                }
            }
            base.Do(pActionStack);
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will set {mUnit.Name}'s HasAttacked to true";
            else
                return $"Set {mUnit.Name}'s HasAttacked to true";
        }

        public override void Undo()
        {
            mUnit.HasAttacked = mPrevHasAttacked;
            if(mBonusMove)
            {
                mUnit.HasMoved = true;
            }
            base.Undo();
        }
    }
}
