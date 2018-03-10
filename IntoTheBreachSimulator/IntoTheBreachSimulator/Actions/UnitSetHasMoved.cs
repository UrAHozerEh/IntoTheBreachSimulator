using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitSetHasMoved : GameAction
    {
        private Unit mUnit;

        public UnitSetHasMoved(Unit pUnit)
        {
            mUnit = pUnit;
        }

        public override void Do(ActionStack pActionStack)
        {
            mUnit.HasMoved = true;
            base.Do(pActionStack);
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will set {mUnit.Name}'s HasMoved to true";
            else
                return $"Set {mUnit.Name}'s HasMoved to true";
        }

        public override void Undo()
        {
            mUnit.HasMoved = false;
            base.Undo();
        }
    }
}
