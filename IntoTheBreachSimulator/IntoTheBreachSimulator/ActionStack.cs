using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units.EnemyUnits;
using IntoTheBreachSimulator.Units.PlayerUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public class ActionStack
    {
        private Stack<GameAction> mStack;
        public Stack<GameAction> Stack => mStack;

        private List<EnemyAttack> mEnemyAttacks;

        private PlayArea mPlayArea;

        private int mNextId;
        public int NextId => mNextId;

        public int EnemyAttackIndex;

        public ActionStack(PlayArea pPlayArea)
        {
            mPlayArea = pPlayArea;
            EnemyAttackIndex = 0;
            mNextId = 1;
            mStack = new Stack<GameAction>();
            mEnemyAttacks = new List<EnemyAttack>();
        }

        public bool UndoLastAction()
        {
            mPlayArea.RemoveHighlights();
            if (mStack.Count == 0)
                return false;
            GameAction action;
            if (mStack.Peek() is PlaceholderAction placeholder)
            {
                if ((int)placeholder.Action % 2 != 1)
                    return false;
                PlaceholderAction.Actions start = (PlaceholderAction.Actions)((int)placeholder.Action - 1);
                do
                {
                    action = mStack.Pop();
                    if (action is EnemyQueueAttack)
                        RemoveLastEemyAttack();
                    placeholder = action as PlaceholderAction;
                    action.Undo();
                } while (placeholder == null || placeholder.Action != start);
                UpdateUndoText();
                return true;
            }
            return false;
        }

        private void UpdateUndoText()
        {
            if (mStack.Count == 0)
                mPlayArea.UpdateUndoText(null);
            else
            {
                string output = mStack.Peek().ToString();
                output = output.Substring(0, output.Length - 4);
                mPlayArea.UpdateUndoText(output);
            }
        }

        public void EndTurn()
        {
            Add(new PlaceholderAction(PlaceholderAction.Actions.EnemyTurnStart, "Enemy turn start"));
            foreach (PlayerUnit player in mPlayArea.PlayerUnits)
            {
                if (player.Statuses.Contains(UnitStatus.Fire))
                    new UnitDamage(player, DamageType.Status, 1).Do(this);
            }
            foreach (EnemyUnit enemy in mPlayArea.EnemyUnits)
            {
                if (enemy.Statuses.Contains(UnitStatus.Fire))
                    new UnitDamage(enemy, DamageType.Status, 1).Do(this);
            }
            for(int i = EnemyAttackIndex; i < mEnemyAttacks.Count; ++i)
            {
                mEnemyAttacks[i].GetAttack().Do(this);
            }
            new MoveEnemyAttackIndex(mEnemyAttacks.Count).Do(this);
            Add(new PlaceholderAction(PlaceholderAction.Actions.EnemyTurnEnd, "Enemy turn end"));
        }

        public int Add(GameAction pAction)
        {
            mStack.Push(pAction);
            UpdateUndoText();
            return mStack.Count;
        }

        public void AddEnemyAttack(EnemyAttack pAttack)
        {
            mEnemyAttacks.Add(pAttack);
        }

        public void RemoveLastEemyAttack()
        {
            mEnemyAttacks.RemoveAt(mEnemyAttacks.Count - 1);
        }

        public bool HasEnemyAttack()
        {
            return mEnemyAttacks.Count > 0;
        }
    }
}
