using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    public class PlaceholderAction : GameAction
    { 
        private Actions mAction;
        public Actions Action => mAction;

        private string mDescription;

        public PlaceholderAction(Actions pAction, string pDescription)
        {
            mAction = pAction;
            mDescription = pDescription;
        }

        public static bool IsPlaceholderAction(GameAction pAction, Actions pType)
        {
            if (pAction is PlaceholderAction p)
            {
                if (p.Action == pType)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return mDescription;
        }

        public enum Actions
        {
            AttackStart,
            AttackEnd,
            PlayerTurnStart,
            PlayerTurnEnd,
            EnemyTurnStart,
            EnemyTurnEnd,
            EnemyPrepStart,
            EnemyPrepEnd,
            GameTurnStart,
            GameTurnEnd,
            UnitMoveStart,
            UnitMoveEnd
        }
    }
}
