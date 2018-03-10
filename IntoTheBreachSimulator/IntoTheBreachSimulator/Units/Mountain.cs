using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;

namespace IntoTheBreachSimulator.Units
{
    public class Mountain : Unit
    {
        public override int CurrentMove => 0;
        public override int MaxHealth => BaseHealth;
        public override string Name => "Mtn";

        public Mountain(Square pLocation) : base(pLocation, Team.Neutral, 2, 0)
        {
            mIsForceMovable = false;
        }

        public override List<Square> MoveSquares()
        {
            return new List<Square>();
        }

        public override int Damage(int pDamage, DamageType pType)
        {
            pDamage = Math.Min(pDamage, 1);
            return base.Damage(pDamage, pType);
        }

        public override bool ApplyStatus(UnitStatus pUnitStatus)
        {
            return false;
        }

        public override List<PlayerAttack> PossibleAttacks(int pEquipment = 0)
        {
            return new List<PlayerAttack>();
        }
    }
}
