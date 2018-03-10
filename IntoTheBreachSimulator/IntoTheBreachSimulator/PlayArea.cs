using IntoTheBreachSimulator.Actions;
using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.AllEquipment.PassiveEquipment;
using IntoTheBreachSimulator.Pilots;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using IntoTheBreachSimulator.Units.BuildingUnits;
using IntoTheBreachSimulator.Units.EnemyUnits;
using IntoTheBreachSimulator.Units.PlayerUnits;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IntoTheBreachSimulator
{
    public class PlayArea : Grid
    {
        private ActionStack mActionStackNew;
        public ActionStack ActionStackNew => mActionStackNew;

        private int mSize;
        public int Size
        {
            get { return mSize; }
        }

        private Square[,] mSquares;
        public Square[,] Squares
        {
            get { return mSquares; }
        }

        private List<PlayerUnit> mPlayerUnits;
        public List<PlayerUnit> PlayerUnits => mPlayerUnits;
        private List<EnemyUnit> mEnemyUnits;
        public List<EnemyUnit> EnemyUnits => mEnemyUnits;
        private List<GridBuilding> mGridBuildings;

        private List<Square> mHighlightedSquares;
        private List<Square> mEmergingSquares;

        private Button mUndoButton;

        private Page mPage;

        private int mNumCalls;

        public PlayArea(Page pPage, int pSize)
        {
            mPage = pPage;
            Grid bottomButtons = new Grid();
            mUndoButton = new Button()
            {
                Text = "Undo Turn"
            };
            mUndoButton.Pressed += BackPressed;
            bottomButtons.Children.AddHorizontal(mUndoButton);

            Button button = new Button()
            {
                Text = "End Turn"
            };
            button.Pressed += EndTurnPressed;
            bottomButtons.Children.AddHorizontal(button);

            button = new Button()
            {
                Text = "Solve Turn"
            };
            button.Pressed += SolveTurn; ;
            bottomButtons.Children.AddHorizontal(button);

            Padding = 0;
            Margin = 0;
            RowSpacing = 3;
            ColumnSpacing = 3;
            BackgroundColor = Color.Black;
            mSize = pSize;
            mPlayerUnits = new List<PlayerUnit>();
            mEnemyUnits = new List<EnemyUnit>();
            mGridBuildings = new List<GridBuilding>();
            mHighlightedSquares = new List<Square>();
            mEmergingSquares = new List<Square>();
            mSquares = new Square[mSize, mSize];
            mActionStackNew = new ActionStack(this);
            string board = "rrfmmrrr,rrrrmfrr,rffmrrrr,frrrrrrm,rrrrrrfr,rrrrrrrr,fffrrrrm,rrrrrrmm";
            string[] rows = board.Split(',');
            string r;
            char s;
            Square square;
            for (int row = 0; row < mSize; ++row)
            {
                r = rows[row];
                for (int col = 0; col < mSize; ++col)
                {
                    s = r[col];
                    switch (s)
                    {
                        case 'f':
                            square = new ForestSquare(this, row, col);
                            break;
                        case 'm':
                            square = new RegularSquare(this, row, col);
                            new Mountain(square);
                            break;
                        default:
                            square = new RegularSquare(this, row, col);
                            break;
                    }

                    mSquares[row, col] = square;
                    Children.Add(square, col, row);
                }
            }
            mPlayerUnits.Add(new GravityMech(mSquares[3, 2], null));
            mPlayerUnits.Add(new SiegeMech(mSquares[4, 2], null));
            mPlayerUnits.Add(new JudoMech(mSquares[3, 4], new ChenRong()));
            mPlayerUnits.Add(new OldArtillery(mSquares[3, 3]));
            mPlayerUnits[3].Damage(1, DamageType.Direct);

            mEnemyUnits.Add(new Scorpion(mSquares[5, 2]));
            mEnemyUnits[0].Damage(1, DamageType.Direct);
            mEnemyUnits.Add(new Scorpion(mSquares[2, 5]));
            mEnemyUnits.Add(new Firefly(mSquares[5, 4]));
            mEnemyUnits.Add(new Scorpion(mSquares[3, 5]));

            mGridBuildings.Add(new GridBuilding(mSquares[1, 1], 1));
            mGridBuildings.Add(new ObjectiveGridBuilding(mSquares[1, 3], 1));
            mGridBuildings.Add(new GridBuilding(mSquares[2, 4], 1));
            mGridBuildings.Add(new GridBuilding(mSquares[2, 7], 1));
            mGridBuildings.Add(new GridBuilding(mSquares[4, 1], 2));
            mGridBuildings.Add(new GridBuilding(mSquares[4, 7], 2));
            mGridBuildings.Add(new GridBuilding(mSquares[5, 1], 2));
            mGridBuildings.Add(new GridBuilding(mSquares[5, 7], 2));

            ToggleEmerging(mSquares[5, 3]);
            ToggleEmerging(mSquares[7, 4]);

            mSquares[6, 1].ApplyStatus(SquareStatus.Fire);

            //other = new Mountain(mSquares[3, 4]);
            //other = new EnemyUnit(mSquares[4, 6], 3);
            //mEnemyUnits[1].MoveSquares().ForEach(sq => sq.BackgroundColor = Color.Red);
            //List<PlayerAttack> attacks = mPlayerUnits[3].PossibleAttacks();
            //attacks.ForEach(a => a.TargetSquare.BackgroundColor = Color.Blue);
            //attacks[0].Do(mActionStack);
            //attacks = unit.PossibleAttacks();
            //attacks[0].Do(mActionStack);
            //UndoLastAttack();
            //attacks.ForEach(a => { if (a.TargetSquare.Equals(mSquares[3, 3])) a.Do(mActionStack); });

            Children.Add(bottomButtons, 0, mSize, mSize, mSize + 1);
        }

        private void SolveTurn(object sender, EventArgs e)
        {
            int buildingDamage = GetGridBuildingDamage();
            int enemyCount = GetEnemyCount();
            int enemyDamage = GetEnemyDamage();
            mNumCalls = 0;
            var best = FindBestActions(new Stack<IFullAction>(), null, null);
        }

        private Tuple<Stack<IFullAction>, BoardState> FindBestActions(Stack<IFullAction> pCurrentActions, Stack<IFullAction> pBestActions, BoardState pBestBoardState)
        {
            ++mNumCalls;
            EndTurn();
            BoardState initial = new BoardState(this);
            UndoLastAction();
            if (pBestBoardState == null || initial.IsBetterThan(pBestBoardState))
            {
                pBestActions = new Stack<IFullAction>(pCurrentActions);
                pBestBoardState = initial;
            }
            List<IFullAction> possibleActions = PossiblePlayerActions();
            foreach (IFullAction action in possibleActions)
            {
                action.Do(mActionStackNew);
                pCurrentActions.Push(action);
                var current = FindBestActions(pCurrentActions, pBestActions, pBestBoardState);
                if (current.Item2.IsBetterThan(pBestBoardState))
                {
                    pBestActions = new Stack<IFullAction>(current.Item1);
                    pBestBoardState = current.Item2;
                }
                pCurrentActions.Pop();
                UndoLastAction();
            }

            return new Tuple<Stack<IFullAction>, BoardState>(pBestActions, pBestBoardState);
        }

        public int GetPlayerHasMoves()
        {
            int count = 0;
            foreach (PlayerUnit p in mPlayerUnits)
            {
                if (p.PossibleMoves().Count > 0)
                    ++count;
            }
            return count;
        }

        public int GetPlayerHasAttacks()
        {
            int count = 0;
            foreach (PlayerUnit p in mPlayerUnits)
            {
                if (p.PossibleAttacks().Count > 0)
                    ++count;
            }
            return count;
        }

        public int GetVekHormoneDamage()
        {
            int damage = 0;
            foreach (PlayerUnit unit in mPlayerUnits)
            {
                if (unit.FirstSlot is VekHormones firstSlot)
                {
                    damage += firstSlot.BonusDamage;
                }
                if (unit.SecondSlot is VekHormones secondSlot)
                {
                    damage += secondSlot.BonusDamage;
                }
            }
            return damage;
        }

        public void UpdateUndoText(string pText)
        {
            if (string.IsNullOrWhiteSpace(pText))
                mUndoButton.Text = "No Moves to Undo";
            else
                mUndoButton.Text = "Undo " + pText;
        }

        private void EndTurnPressed(object sender, EventArgs e)
        {
            string before = BoardState();
            EndTurn();
            string after = BoardState();
            mPage.DisplayAlert("End Turn", before + "\n\n" + after, "OK");
        }

        private void BackPressed(object sender, EventArgs e)
        {
            UndoLastAction();
        }

        public int GetGridBuildingDamage()
        {
            int count = 0;
            foreach (Building b in mGridBuildings)
            {
                count += b.MaxHealth - b.CurrentHealth;
            }
            return count;
        }

        public int GetPlayerDamage()
        {
            int count = 0;
            foreach (PlayerUnit p in mPlayerUnits)
            {
                count += p.MaxHealth - p.CurrentHealth;
            }
            return count;
        }

        public int GetEnemyDamage()
        {
            int count = 0;
            foreach (EnemyUnit e in mEnemyUnits)
            {
                count += e.MaxHealth - e.CurrentHealth;
            }
            return count;
        }

        public int GetEnemyCount()
        {
            int count = 0;
            foreach (EnemyUnit unit in mEnemyUnits)
            {
                if ((unit.Statuses.Contains(UnitStatus.Fire) || unit.Location.EmergingSquare) && unit.CurrentHealth == 1)
                    continue;
                if (unit.CurrentHealth <= 0)
                    continue;
                ++count;
            }

            foreach (Square s in mEmergingSquares)
            {
                if (s.Unit == null || s.Unit.CurrentHealth <= 0)
                    ++count;
            }
            return count;
        }

        public string BoardState()
        {
            string output = $"Grid Damage: {GetGridBuildingDamage()}, Player Damage: {GetPlayerDamage()},";
            output += $"Enemy Damage: {GetEnemyDamage()}, Enemy Count: {GetEnemyCount()}";
            return output;
        }

        public void HighlightUnitAttack(Unit pUnit, int pEquipment)
        {
            if (pUnit == null)
                return;
            RemoveHighlights();
            pUnit.Location.HighlightType = HighlightType.Selected;
            mHighlightedSquares.Add(pUnit.Location);
            List<PlayerAttack> attacks = pUnit.PossibleAttacks(pEquipment);
            foreach (PlayerAttack p in attacks)
            {
                p.TargetSquare.SetHighlight(HighlightType.Attack, p);
                mHighlightedSquares.Add(p.TargetSquare);
            }
        }

        public void HighlightUnitMove(Unit pUnit)
        {
            if (pUnit == null)
                return;
            RemoveHighlights();
            pUnit.Location.HighlightType = HighlightType.Selected;
            mHighlightedSquares.Add(pUnit.Location);
            List<UnitMoveTurn> moves = pUnit.PossibleMoves();
            foreach (UnitMoveTurn move in moves)
            {
                move.EndSquare.SetHighlight(HighlightType.Move, move);
                mHighlightedSquares.Add(move.EndSquare);
            }
        }

        public List<IFullAction> PossiblePlayerActions()
        {
            List<IFullAction> actions = new List<IFullAction>();
            foreach (PlayerUnit unit in mPlayerUnits)
            {
                actions.AddRange(unit.PossibleActions());
            }
            return actions;
        }

        public void RemoveHighlights()
        {
            foreach (Square s in mHighlightedSquares)
                s.HighlightType = HighlightType.None;
            mHighlightedSquares = new List<Square>();
        }

        //public bool UndoLastAttack()
        //{
        //    RemoveHighlights();
        //    if (mActionStack.Count == 0)
        //        return false;
        //    GameAction action = mActionStack.Peek();
        //    if (!PlaceholderAction.IsPlaceholderAction(action, PlaceholderAction.Actions.AttackEnd))
        //    {
        //        return false;
        //    }
        //    do
        //    {
        //        action = mActionStack.Pop();
        //        action.Undo();
        //    } while (!PlaceholderAction.IsPlaceholderAction(action, PlaceholderAction.Actions.AttackStart));

        //    return true;
        //}

        //public bool UndoLastMove()
        //{
        //    RemoveHighlights();
        //    if (mActionStack.Count == 0)
        //        return false;
        //    GameAction action = mActionStack.Peek();
        //    if (!PlaceholderAction.IsPlaceholderAction(action, PlaceholderAction.Actions.UnitMoveEnd))
        //    {
        //        return false;
        //    }
        //    do
        //    {
        //        action = mActionStack.Pop();
        //        action.Undo();
        //    } while (!PlaceholderAction.IsPlaceholderAction(action, PlaceholderAction.Actions.UnitMoveStart));

        //    return true;
        //}

        public bool UndoLastAction()
        {
            return mActionStackNew.UndoLastAction();
        }

        public void EndTurn()
        {
            mActionStackNew.EndTurn();
        }

        public void ToggleEmerging(Square pSquare)
        {
            if (mEmergingSquares.Contains(pSquare))
            {
                mEmergingSquares.Remove(pSquare);
                pSquare.EmergingSquare = false;
            }
            else
            {
                mEmergingSquares.Add(pSquare);
                pSquare.EmergingSquare = true;
            }
        }

        public void ClearEmerging()
        {
            mEmergingSquares = new List<Square>();
        }
    }
}
