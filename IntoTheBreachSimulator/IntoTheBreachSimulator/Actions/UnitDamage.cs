using IntoTheBreachSimulator.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Actions
{
    class UnitDamage : GameAction
    {
        private Unit mUnit;
        private DamageType mType;
        private int mDamage;
        private int mActualDamage;

        public UnitDamage(Unit pUnit, DamageType pType, int pDamage)
        {
            mUnit = pUnit;
            mType = pType;
            mDamage = pDamage;
            mActualDamage = -1;
        }

        public override void Do(ActionStack pActionStack)
        {
            base.Do(pActionStack);
            mActualDamage = mUnit.Damage(mDamage, mType);
        }

        public override void Undo()
        {
            if (mActualDamage == -1)
                return;
            else if (mActualDamage == -999)
                mUnit.ApplyShield();
            else
                mUnit.Heal(mActualDamage);
            base.Undo();
        }

        public override string ToString()
        {
            if (!Done)
                return $"Will deal {mDamage} damage to {mUnit.Name}";
            else if (mActualDamage == -999)
                return $"{mDamage} blocked by shield on {mUnit.Name}";
            else
                return $"{mActualDamage} dealt to {mUnit.Name}";
        }
    }
}
