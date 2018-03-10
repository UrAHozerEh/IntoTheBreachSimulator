using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class GravWell : ActiveEquipment
    {
        public GravWell(Unit pUnit) : base(pUnit, 0, UnitType.Science)
        {

        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();
            List<Square> squares = Unit.Location.ArtillerySquares();

            foreach (Square s in squares)
            {
                if (s.Unit == null || !s.Unit.IsForceMovable)
                    continue;
                attacks.Add(GetAttack(s));
            }

            return attacks;
        }

        private PlayerAttack GetAttack(Square pSquare)
        {

            string description = $"{Unit.Name} Grav Well at {pSquare}";
            PlayerAttack attack = new PlayerAttack(Unit, pSquare, description);

            Direction direction = pSquare.GetDirectionTowards(Unit.Location);

            attack.AddAction(new UnitPush(pSquare.Unit, direction));

            return attack;
        }
    }
}
