using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class SquareApplyStatus : GameAction
    {
        private Square mSquare;
        private SquareStatus mStatus;
        private bool mNeedsUndo;

        public SquareApplyStatus(Square pSquare, SquareStatus pStatus)
        {
            mSquare = pSquare;
            mStatus = pStatus;
        }

        public override void Do(ActionStack pActionStack)
        {
            if (!mSquare.CanApplyStatus(mStatus))
            {
                mNeedsUndo = false;
            }
            else
            {
                mSquare.ApplyStatus(mStatus);
                mNeedsUndo = true;
            }
            base.Do(pActionStack);
        }

        public override void Undo()
        {
            if (mNeedsUndo)
                mSquare.RemoveStatus(mStatus);
            base.Undo();
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will attempt to apply {mStatus} to {mSquare}";
            if (mNeedsUndo)
                return $"Applied {mStatus} to {mSquare}";
            else
                return $"Failed to apply {mStatus} to {mSquare}";
        }
    }
}
