using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;

namespace IntoTheBreachSimulator.Units.BuildingUnits
{
    public abstract class Building : Unit
    {
        public override int MaxHealth => BaseHealth;
        public override int CurrentMove => 0;

        public Building(Square pLocation, int pBaseHealth) : base(pLocation, Team.Neutral, pBaseHealth, 0)
        {
            mIsForceMovable = false;
            ApplyStatus(UnitStatus.FireImmune);
        }

        public override List<PlayerAttack> PossibleAttacks(int pEquipment = 0)
        {
            return new List<PlayerAttack>();
        }
    }
}
