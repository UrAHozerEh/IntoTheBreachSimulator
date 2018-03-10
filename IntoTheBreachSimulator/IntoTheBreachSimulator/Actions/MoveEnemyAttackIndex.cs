using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class MoveEnemyAttackIndex : GameAction
    {
        private int mNewIndex;
        private int mOldIndex;
        ActionStack mActionStack;

        public MoveEnemyAttackIndex(int pNewIndex)
        {
            mNewIndex = pNewIndex;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            mActionStack = pActionStack;
            mOldIndex = pActionStack.EnemyAttackIndex;
            pActionStack.EnemyAttackIndex = mNewIndex;
        }
        public override void Undo()
        {
            mActionStack.EnemyAttackIndex = mOldIndex;
            base.Undo();
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will move attack index to {mNewIndex}";
            else
                return $"Moved attack index from {mOldIndex} to {mNewIndex}";
        }
    }
}
