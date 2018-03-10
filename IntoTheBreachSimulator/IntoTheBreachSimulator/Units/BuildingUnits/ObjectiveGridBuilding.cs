using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Squares;

namespace IntoTheBreachSimulator.Units.BuildingUnits
{
    public class ObjectiveGridBuilding : GridBuilding
    {
        public override string Name => "Obj";

        public ObjectiveGridBuilding(Square pLocation, int pBaseHealth) : base(pLocation, pBaseHealth)
        {

        }
    }
}
