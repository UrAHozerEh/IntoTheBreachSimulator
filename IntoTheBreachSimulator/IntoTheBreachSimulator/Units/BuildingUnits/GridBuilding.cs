using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.BuildingUnits
{
    public class GridBuilding : Building
    {
        public override string Name => "Grid";

        public GridBuilding(Square pLocation, int pBaseHealth) : base(pLocation, pBaseHealth)
        {

        }
    }
}
