using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    public class UnitMove : GameAction
    {
        private Unit mUnit;
        private Square mStartSquare;
        private Square mEndSquare;

        public UnitMove(Unit pUnit, Square pEndSquare)
        {
            mUnit = pUnit;
            mStartSquare = mUnit.Location;
            mEndSquare = pEndSquare;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            mUnit.MoveTo(mEndSquare);
            if (mEndSquare.IsFire)
            {
                new UnitApplyStatus(mUnit, UnitStatus.Fire).Do(pActionStack);
            }
            if(mEndSquare.IsAcid)
            {
                new UnitApplyStatus(mUnit, UnitStatus.ACID).Do(pActionStack);
            }
            if(mEndSquare.IsWater)
            {
                if(!mUnit.IsMassive)
                {
                    // Gota kill
                }
                else
                {
                    new UnitRemoveStatus(mUnit, UnitStatus.Fire).Do(pActionStack);
                }
            }
        }

        public override void Undo()
        {
            mUnit.MoveTo(mStartSquare);
            base.Undo();
        }

        public override string ToString()
        {
            if (Done)
                return $"{mUnit.Name} moved from {mStartSquare} to {mEndSquare}";
            else
                return $"{mUnit.Name} will move from {mStartSquare} to {mEndSquare}";
        }
    }
}
