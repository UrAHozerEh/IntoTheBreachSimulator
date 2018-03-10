using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitApplyWeb : GameAction
    {
        private bool mNeedsUndo;
        private Unit mTarget;
        private Unit mWebber;
        private Unit mOldTarget;

        public UnitApplyWeb(Unit pWebber, Unit pTarget)
        {
            mWebber = pWebber;
            mTarget = pTarget;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            mNeedsUndo = mTarget.ApplyStatus(UnitStatus.Web);
            if (mNeedsUndo)
            {
                mOldTarget = mWebber.WebbedTarget;
                mWebber.WebbedTarget = mTarget;
            }
        }

        public override void Undo()
        {
            base.Undo();
            if(mNeedsUndo)
            {
                mWebber.WebbedTarget = mOldTarget;
                mTarget.RemoveStatus(UnitStatus.Web);
            }
        }

        public override string ToString()
        {
            if (!Done)
                return $"{mWebber.Name} will attempt to web {mTarget.Name}";
            if (mNeedsUndo)
                return $"{mWebber.Name} webbed {mTarget.Name}";
            else
                return $"{mWebber.Name} failed to web {mTarget.Name}";
        }
    }
}
