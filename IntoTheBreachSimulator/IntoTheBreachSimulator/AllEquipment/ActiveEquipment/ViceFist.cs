using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Units;

namespace IntoTheBreachSimulator.AllEquipment.ActiveEquipment
{
    public class ViceFist : ActiveEquipment
    {
        public ViceFist(Unit pUnit) : base(pUnit, 0, UnitType.Prime)
        {
            EquipmentUpgrade upgrade = new EquipmentUpgrade("AlliesImmune", 1);
            Upgrades.Add(upgrade.Name, upgrade);
            upgrade = new EquipmentUpgrade("BonusDamage", 3);
            Upgrades.Add(upgrade.Name, upgrade);
        }

        public override List<PlayerAttack> PossibleAttacks()
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();

            if (CanAttack(Direction.North))
                attacks.Add(GetAttack(Direction.North));
            if (CanAttack(Direction.South))
                attacks.Add(GetAttack(Direction.South));
            if (CanAttack(Direction.East))
                attacks.Add(GetAttack(Direction.East));
            if (CanAttack(Direction.West))
                attacks.Add(GetAttack(Direction.West));

            return attacks;
        }

        private bool CanAttack(Direction pDirection)
        {
            Square square = Unit.Location;
            Square forward, reverse;
            forward = square.GetSquareTowards(pDirection);
            reverse = square.GetSquareTowards(Directions.OppositeDirection(pDirection));
            if (forward == null || reverse == null)
                return false;
            if (forward.Unit == null || !forward.Unit.IsForceMovable)
                return false;
            if (reverse.Unit != null)
                return false;
            return true;
        }

        private PlayerAttack GetAttack(Direction pDirection)
        {
            Square square = Unit.Location;
            Square target = square.GetSquareTowards(pDirection);
            Unit unit = target.Unit;
            Square destination = square.GetSquareTowards(Directions.OppositeDirection(pDirection));
            string description = $"{Unit.Name} Vice Fist {pDirection}";
            PlayerAttack attack = new PlayerAttack(Unit, target, description);

            attack.AddAction(new UnitMove(unit, destination));
            if (Upgrades["AlliesImmune"].Purchased && unit.Team == Unit.Team)
                return attack;
            int damage = Upgrades["BonusDamage"].Purchased ? 3 : 1;
            attack.AddAction(new UnitDamage(unit, DamageType.Direct, damage));
            attack.AddAction(new SquareDamage(destination));

            return attack;
        }
    }
}
