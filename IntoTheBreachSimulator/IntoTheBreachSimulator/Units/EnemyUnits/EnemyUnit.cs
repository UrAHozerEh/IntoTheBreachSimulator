using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.EnemyUnits
{
    public abstract class EnemyUnit : Unit
    {
        public override int MaxHealth => BaseHealth;
        public override int CurrentMove => BaseMove;

        public EnemyUnit(Square pLocation, int pBaseHealth, int pBaseMove) : 
            base(pLocation, Team.Enemy, pBaseHealth, pBaseMove)
        {

        }

        public abstract PlayerAttack GetFinalAttack(Direction pDirection, int pDistance);
    }
}
