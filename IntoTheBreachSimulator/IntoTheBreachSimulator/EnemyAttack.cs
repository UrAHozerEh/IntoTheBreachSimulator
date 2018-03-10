using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Units;
using IntoTheBreachSimulator.Units.EnemyUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public class EnemyAttack
    {
        private EnemyUnit mUnit;
        private Direction mDirection;
        private int mDistance;

        public EnemyAttack(EnemyUnit pUnit, Direction pDirection, int pDistance = 1)
        {
            mUnit = pUnit;
            mDirection = pDirection;
            mDistance = pDistance;
        }

        public PlayerAttack GetAttack()
        {
            return mUnit.GetFinalAttack(mDirection, mDistance);
        }
    }
}
