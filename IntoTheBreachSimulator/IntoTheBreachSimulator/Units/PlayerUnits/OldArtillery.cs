using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Units.PlayerUnits
{
    public class OldArtillery : PlayerUnit
    {
        public override string Name => "Arty";
        public override int MaxHealth => BaseHealth;
        public override int CurrentMove => BaseMove;

        public OldArtillery(Square pLocation) : base(pLocation, null, UnitType.Ranged, 2, 1)
        {
            mFirstSlot = new OldEarthArtillery(this);
        }
    }
}
