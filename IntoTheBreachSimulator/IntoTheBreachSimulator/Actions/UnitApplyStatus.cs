using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitApplyStatus : GameAction
    {
        private Unit mUnit;
        private UnitStatus mStatus;
        private bool mNeedsUndo;

        public UnitApplyStatus(Unit pUnit, UnitStatus pStatus)
        {
            mUnit = pUnit;
            mStatus = pStatus;
        }

        public override void Do(ActionStack pActionStack)
        {
            if (mUnit.CanApplyStatus(mStatus))
            {
                mUnit.ApplyStatus(mStatus);
                mNeedsUndo = true;
            }
            else
            {
                mNeedsUndo = false;
            }
            base.Do(pActionStack);
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will attempt to apply {mStatus} to {mUnit.Name}";
            else if (mNeedsUndo)
                return $"Applied {mStatus} to {mUnit.Name}";
            else
                return $"Failed to apply {mStatus} to {mUnit.Name}";
        }

        public override void Undo()
        {
            if (mNeedsUndo)
                mUnit.RemoveStatus(mStatus);
            base.Undo();
        }
    }
}
