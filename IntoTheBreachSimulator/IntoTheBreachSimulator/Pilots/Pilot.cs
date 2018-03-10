using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator.Pilots
{
    public abstract class Pilot
    {
        public int MoveBonus { get; set; }
        public int HealthBonus { get; set; }
        public abstract string Name { get; }
    }
}
