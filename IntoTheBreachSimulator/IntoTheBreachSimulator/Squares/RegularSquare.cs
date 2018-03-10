using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IntoTheBreachSimulator.Squares
{
    public class RegularSquare : Square
    {
        public override string Name => "Square";

        public RegularSquare(PlayArea pPlayArea, int pRow, int pColumn) : base(pPlayArea, pRow, pColumn, Color.White)
        {

        }
    }
}
