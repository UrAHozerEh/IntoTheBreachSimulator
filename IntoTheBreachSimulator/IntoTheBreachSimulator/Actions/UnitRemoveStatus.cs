using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitRemoveStatus : GameAction
    {
        private Unit mUnit;
        private UnitStatus mStatus;
        private bool mNeedsUndo;

        public UnitRemoveStatus(Unit pUnit, UnitStatus pStatus)
        {
            mUnit = pUnit;
            mStatus = pStatus;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            if (!mUnit.Statuses.Contains(mStatus))
            {
                mNeedsUndo = false;
            }
            else
            {
                mUnit.RemoveStatus(mStatus);
                mNeedsUndo = true;
            }
        }

        public override void Undo()
        {
            if (mNeedsUndo)
                mUnit.ApplyStatus(mStatus);
            base.Undo();
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will attempt to remove {mStatus} from {mUnit.Name}";
            else if (mNeedsUndo)
                return $"Removed {mStatus} from {mUnit.Name}";
            else
                return $"Failed to remove {mStatus} from {mUnit.Name}";
        }
    }
}
