using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class OldEarthArtillery : ActiveEquipment
    {
        public OldEarthArtillery(Unit pUnit) : base(pUnit, 0, UnitType.Universal)
        {

        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();
            List<Square> squares = Unit.Location.ArtillerySquares();

            foreach(Square s in squares)
            {
                attacks.Add(GetAttack(s));
            }

            return attacks;
        }

        private PlayerAttack GetAttack(Square pSquare)
        {
            string description = $"{Unit.Name} Old Earth Artillery at {pSquare}";
            PlayerAttack attack = new PlayerAttack(Unit, pSquare, description);

            Direction d = Unit.Location.GetDirectionTowards(pSquare);
            Square other = pSquare.GetSquareTowards(d);

            Unit first = pSquare.Unit;
            Unit second = other?.Unit;

            if (first != null)
                attack.AddAction(new UnitDamage(first, DamageType.Direct, 2));
            attack.AddAction(new SquareDamage(pSquare));
            if(second != null)
                attack.AddAction(new UnitDamage(second, DamageType.Direct, 2));
            if(other != null)
                attack.AddAction(new SquareDamage(other));

            return attack;
        }
    }
}
