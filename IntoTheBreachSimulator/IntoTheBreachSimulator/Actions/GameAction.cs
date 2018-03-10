using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    public abstract class GameAction
    {
        private int mId;
        public int Id => mId;

        private bool mDone;
        public bool Done => mDone;

        public GameAction()
        {
            mId = -1;
        }

        public virtual void Do(ActionStack pActionStack)
        {
            mId = pActionStack.Add(this);
            mDone = true;
        }

        public bool Equals(GameAction other)
        {
            if (other is GameAction o)
            {
                return mId == o.Id;
            }
            return false;
        }

        public virtual void Undo()
        {
            mDone = false;
        }

        public abstract override string ToString();
    }
}
