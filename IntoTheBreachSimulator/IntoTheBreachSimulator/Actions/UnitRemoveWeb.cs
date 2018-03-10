using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitRemoveWeb : GameAction
    {
        private bool mNeedsUndo;
        private Unit mTarget;
        private Unit mWebber;

        public UnitRemoveWeb(Unit pWebber)
        {
            mWebber = pWebber;
            mTarget = mWebber.WebbedTarget;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            if(mWebber.WebbedTarget != null)
            {
                mTarget = mWebber.WebbedTarget;
                mNeedsUndo = true;
                mTarget.RemoveStatus(UnitStatus.Web);
                mWebber.WebbedTarget = null;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if (mNeedsUndo)
            {
                mWebber.WebbedTarget = mTarget;
                mTarget.ApplyStatus(UnitStatus.Web);
            }
        }

        public override string ToString()
        {
            if (!Done)
                return $"{mWebber.Name} will break web on {mTarget.Name}";
            if (mNeedsUndo)
                return $"{mWebber.Name} broke web on {mTarget.Name}";
            else
                return $"{mWebber.Name} failed to break web on {mTarget.Name}";
        }
    }
}
