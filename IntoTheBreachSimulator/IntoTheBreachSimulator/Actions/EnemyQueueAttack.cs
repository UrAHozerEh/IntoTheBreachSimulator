using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class EnemyQueueAttack : GameAction
    {
        private EnemyAttack mAttack;

        public EnemyQueueAttack(EnemyAttack pAttack)
        {
            mAttack = pAttack;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            pActionStack.AddEnemyAttack(mAttack);
        }

        public override string ToString()
        {
            if (Done)
                return $"Will add {mAttack} to queue";
            else
                return $"Added {mAttack} to queue";
        }
    }
}
