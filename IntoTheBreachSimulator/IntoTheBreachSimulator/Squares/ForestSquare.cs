using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IntoTheBreachSimulator.Squares
{
    public class ForestSquare : Square
    {
        public override string Name => "Forest";

        public ForestSquare(PlayArea pPlayArea, int pRow, int pColumn) : base(pPlayArea, pRow, pColumn, Color.DarkGreen)
        {
        }
    }
}
