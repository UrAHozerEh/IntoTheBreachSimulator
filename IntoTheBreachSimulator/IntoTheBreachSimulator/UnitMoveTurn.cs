using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public class UnitMoveTurn : IFullAction
    {
        private Unit mUnit;
        public Unit Unit => mUnit;

        private List<GameAction> mActions;

        private Square mEndSquare;
        public Square EndSquare => mEndSquare;

        private Square mStartSquare;
        public Square StartSquare => mStartSquare;

        private string mDescription;

        public UnitMoveTurn(Unit pUnit, Square pEndSquare)
        {
            mUnit = pUnit;
            mStartSquare = mUnit.Location;
            mEndSquare = pEndSquare;
            mDescription = $"{mUnit.Name} moved from {mStartSquare} to {mEndSquare}";
            mActions = new List<GameAction>();
            mActions.Add(new PlaceholderAction(PlaceholderAction.Actions.UnitMoveStart, $"{mDescription} start"));
            mActions.Add(new UnitMove(mUnit, pEndSquare));
            mActions.Add(new UnitSetHasMoved(mUnit));
            mActions.Add(new PlaceholderAction(PlaceholderAction.Actions.UnitMoveEnd, $"{mDescription} end"));
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
}
