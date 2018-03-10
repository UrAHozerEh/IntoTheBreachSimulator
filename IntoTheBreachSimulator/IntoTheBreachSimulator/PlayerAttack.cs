using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public class PlayerAttack : IFullAction
    {
        private Unit mUnit;
        public Unit Unit => mUnit;

        private List<GameAction> mActions;

        private Square mTargetSquare;
        public Square TargetSquare => mTargetSquare;

        private string mDescription;

        public PlayerAttack(Unit pUnit, Square pTargetSquare, string pDescription)
        {
            mUnit = pUnit;
            mDescription = pDescription;
            mTargetSquare = pTargetSquare;
            mActions = new List<GameAction>();
            mActions.Add(new PlaceholderAction(PlaceholderAction.Actions.AttackStart, $"{mDescription} start"));
            mActions.Add(new UnitSetHasAttacked(mUnit));
            if (mUnit.WebbedTarget != null)
                mActions.Add(new UnitRemoveWeb(mUnit));
            mActions.Add(new PlaceholderAction(PlaceholderAction.Actions.AttackEnd, $"{mDescription} end"));
        }

        public void AddAction(GameAction pAction)
        {
            mActions.Insert(mActions.Count - 1, pAction);
        }

        public void Do(ActionStack pActionStack)
        {
            foreach (GameAction g in mActions)
                g.Do(pActionStack);
        }

        public override string ToString()
        {
            return mDescription;
        }
    }

    public enum DamageType
    {
        Direct,
        DirectEnemy,
        Bump,
        Status
    }
}
