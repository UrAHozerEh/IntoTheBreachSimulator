using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IntoTheBreachSimulator.Squares
{
    public class WaterSquare : Square
    {
        public override bool IsWater => true;
        public override string Name => "Water";

        public WaterSquare(PlayArea pPlayArea, int pRow, int pColumn) : base(pPlayArea, pRow, pColumn, Color.LightBlue)
        {

        }
    }
}
