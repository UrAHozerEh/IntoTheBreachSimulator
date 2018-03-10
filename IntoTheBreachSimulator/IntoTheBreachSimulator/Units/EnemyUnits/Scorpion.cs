using System;
using System.Collections.Generic;
using System.Text;
using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;

namespace IntoTheBreachSimulator.Units.EnemyUnits
{
    public class Scorpion : EnemyUnit
    {
        public override string Name => "Scorp";

        public Scorpion(Square pLocation) : base(pLocation, 3, 3)
        {
            mFirstSlot = new NaturalWeapon(this);
        }
        // TODO: Handle multiple webbers, fix multiple enemy attacks, add ability to run queued enemy attacks.
        public override List<PlayerAttack> PossibleAttacks(int pEquipment = 0)
        {
            List<PlayerAttack> attacks = new List<PlayerAttack>();
            if (HasAttacked)
                return attacks;
            foreach (Direction d in Directions.GetDirections())
            {
                Square target = Location.GetSquareTowards(d, 1);
                if (target == null)
                    continue;
                attacks.Add(GetInitialAttack(target, d));
            }
            return attacks;
        }

        private PlayerAttack GetInitialAttack(Square pTarget, Direction pDirection)
        {
            string description = $"{this.Name} webs and prepares to attack at {pTarget}";
            PlayerAttack attack = new PlayerAttack(this, pTarget, description);
            if (pTarget.Unit != null)
                attack.AddAction(new UnitApplyWeb(this, pTarget.Unit));
            attack.AddAction(new EnemyQueueAttack(new EnemyAttack(this, pDirection, 1)));
            return attack;
        }

        public override PlayerAttack GetFinalAttack(Direction pDirection, int pDistance)
        {
            Square target = Location.GetSquareTowards(pDirection, pDistance);
            if (target == null)
                return null;
            string description = $"{this.Name} hits towards {target}";
            PlayerAttack attack = new PlayerAttack(this, target, description);
            if (target.Unit != null)
                attack.AddAction(new UnitDamage(target.Unit, DamageType.DirectEnemy, 1));
            attack.AddAction(new SquareDamage(target));
            return attack;
        }
    }
}
