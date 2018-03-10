using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    public class SquareDamage : GameAction
    {
        private Square mSquare;
        private bool mNeedsUndo;

        public SquareDamage(Square pSquare)
        {
            mSquare = pSquare;
        }

        public override void Do(ActionStack pActionStack)
        {
            mNeedsUndo = false;
            base.Do(pActionStack);
            if (mSquare is RegularSquare)
            {
                return;
            }
            if (mSquare is ForestSquare)
            {
                new SquareApplyStatus(mSquare, SquareStatus.Fire).Do(pActionStack);
                Unit unit = mSquare.Unit;
                if (unit != null)
                    new UnitApplyStatus(unit, UnitStatus.Fire).Do(pActionStack);
                return;
            }
        }

        public override void Undo()
        {
            if (mNeedsUndo)
            {
                // TODO: Handle damagabe squares like frozen water.
            }
            base.Undo();
        }

        public override string ToString()
        {
            if (Done)
                return $"Will damage {mSquare}";
            return $"Damaged {mSquare}";
        }
    }
}
