using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class ClusterArtillery : ActiveEquipment
    {
        public ClusterArtillery(Unit pUnit) : base(pUnit, 1, UnitType.Ranged)
        {
            EquipmentUpgrade upgrade = new EquipmentUpgrade("BuildingsImmune", 2);
            Upgrades.Add(upgrade.Name, upgrade);
            upgrade = new EquipmentUpgrade("BonusDamage", 2);
            Upgrades.Add(upgrade.Name, upgrade);
        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();
            Square square = Unit.Location;

            foreach (Square s in square.ArtillerySquares())
            {
                attacks.Add(GetAttack(s));
            }

            return attacks;
        }

        private PlayerAttack GetAttack(Square pSquare)
        {
            string description = $"{Unit.Name} Cluster Artillery at {pSquare}";
            PlayerAttack attack = new PlayerAttack(Unit, pSquare, description);
            Unit target;
            Square other;
            int damage = Upgrades["BonusDamage"].Purchased ? 2 : 1;
            foreach (Direction d in Directions.GetDirections())
            {
                other = pSquare.GetSquareTowards(d);

                if (other != null)
                {
                    target = other.Unit;
                    if (target != null)
                    {
                        attack.AddAction(new UnitDamage(target, DamageType.Direct, damage));
                        if (target.IsForceMovable)
                            attack.AddAction(new UnitPush(target, d));
                    }
                    attack.AddAction(new SquareDamage(other));
                }
            }
            return attack;
        }
    }
}
