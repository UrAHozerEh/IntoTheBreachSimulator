using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    public class UnitPush : GameAction
    {
        private Unit mUnit;
        public Unit Unit => mUnit;

        private Direction mDirection;

        private Square mStartSquare;
        public Square StartSquare => mStartSquare;

        private Square mEndSquare;
        public Square EndSquare => mEndSquare;

        private Unit mBumpedUnit;
        public Unit BumpedUnit => mBumpedUnit;

        public UnitPush(Unit pUnit, Direction pDirection)
        {
            mUnit = pUnit;
            mDirection = pDirection;
            mStartSquare = mUnit.Location;
            mEndSquare = Directions.GetSquareFrom(mStartSquare, pDirection);
            if (mEndSquare != null && mEndSquare.Unit != null)
            {
                mBumpedUnit = mEndSquare.Unit;
                mEndSquare = mStartSquare;
            }
            else if (mEndSquare == null)
            {
                mBumpedUnit = null;
                mEndSquare = mStartSquare;
            }
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            if (mBumpedUnit != null)
            {
                new UnitDamage(mUnit, DamageType.Bump, 1).Do(pActionStack);
                new UnitDamage(mBumpedUnit, DamageType.Bump, 1).Do(pActionStack);
            }
            else
            {
                new UnitMove(mUnit, mEndSquare).Do(pActionStack);
                if (mUnit.WebbedTarget != null)
                    new UnitRemoveWeb(mUnit).Do(pActionStack);
            }
        }

        public override void Undo()
        {
            base.Undo();
        }

        public override string ToString()
        {
            if (Done)
                return $"Will push {mUnit.Name} to the {mDirection}";
            else
                return $"Pushed {mUnit.Name} to the {mDirection}";
        }
    }
}
