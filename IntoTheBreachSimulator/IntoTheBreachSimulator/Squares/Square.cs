using IntoTheBreachSimulator.AllEquipment.ActiveEquipment;
using IntoTheBreachSimulator.Statuses;
using IntoTheBreachSimulator.Units;
using IntoTheBreachSimulator.Units.BuildingUnits;
using IntoTheBreachSimulator.Units.PlayerUnits;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IntoTheBreachSimulator.Squares
{
    public abstract class Square : Grid, IEquatable<Square>
    {
        private PlayArea mPlayArea;
        public PlayArea PlayArea => mPlayArea;

        public abstract string Name { get; }

        private int mRow;
        public int Row => mRow;

        private int mColumn;
        public int Column => mColumn;

        private Unit mUnit;
        public Unit Unit => mUnit;

        public virtual bool IsFire => mStatuses.Contains(SquareStatus.Fire);
        public virtual bool IsAcid => mStatuses.Contains(SquareStatus.ACID);
        public virtual bool IsSmoke => mStatuses.Contains(SquareStatus.Smoke);
        public virtual bool IsWater => false;

        private Label mLabel;

        private HashSet<SquareStatus> mStatuses;
        public HashSet<SquareStatus> Statuses => mStatuses;

        private Color mDefaultColor;

        private Grid mStatus;
        private Grid mContent;

        public Color HighlightColor
        {
            get { return BackgroundColor; }
            set { BackgroundColor = value; }
        }

        public Color StatusColor
        {
            get { return mStatus.BackgroundColor; }
            set { mStatus.BackgroundColor = value; }
        }

        public Color SquareColor
        {
            get { return mContent.BackgroundColor; }
            set { mContent.BackgroundColor = value; }
        }

        private HighlightType mHighlightType;
        public HighlightType HighlightType
        {
            get { return mHighlightType; }
            set
            {
                if (value == mHighlightType)
                    return;
                if(value == HighlightType.None)
                {
                    mHighlightedAttack = null;
                    mHighlightedMove = null;
                    mHighlitedEquipmentSlot = 0;
                }
                mHighlightType = value;
                UpdateColors();
            }
        }

        private PlayerAttack mHighlightedAttack;
        private int mHighlitedEquipmentSlot;

        private UnitMoveTurn mHighlightedMove;

        private bool mEmergingSquare;
        public bool EmergingSquare
        {
            get { return mEmergingSquare; }
            set
            {
                mEmergingSquare = value;
                UnitUpdated();
            }
        }

        public Square(PlayArea pPlayArea, int pRow, int pColumn, Color pDefaultColor)
        {
            Padding = 3;
            mEmergingSquare = false;
            mDefaultColor = pDefaultColor;
            BackgroundColor = mDefaultColor;
            mStatus = new Grid
            {
                BackgroundColor = mDefaultColor,
                Padding = 3
            };
            mContent = new Grid
            {
                BackgroundColor = mDefaultColor,
                Padding = 3
            };
            mHighlightType = HighlightType.None;
            mStatuses = new HashSet<SquareStatus>();
            mLabel = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            mContent.Children.Add(mLabel);
            mStatus.Children.Add(mContent);
            Children.Add(mStatus);
            mPlayArea = pPlayArea;
            mRow = pRow;
            mColumn = pColumn;

            TapGestureRecognizer singleTap = new TapGestureRecognizer();
            singleTap.Tapped += SingleTap;
            singleTap.NumberOfTapsRequired = 1;
            GestureRecognizers.Add(singleTap);

            TapGestureRecognizer doubleTap = new TapGestureRecognizer();
            doubleTap.Tapped += DoubleTap;
            doubleTap.NumberOfTapsRequired = 2;
            GestureRecognizers.Add(doubleTap);
        }

        private void DoubleTap(object sender, EventArgs e)
        {
            if(HighlightType == HighlightType.None)
            {
                mPlayArea.ToggleEmerging(this);
            }
        }

        private void SingleTap(object sender, EventArgs e)
        {
            if (HighlightType == HighlightType.Attack && mHighlightedAttack != null)
            {
                mHighlightedAttack.Do(mPlayArea.ActionStackNew);
                mPlayArea.RemoveHighlights();
                return;
            }
            else if (HighlightType == HighlightType.Move && mHighlightedMove != null)
            {
                mHighlightedMove.Do(mPlayArea.ActionStackNew);
                mPlayArea.RemoveHighlights();
                return;
            }
            else if (mUnit != null)
            {
                if (HighlightType == HighlightType.None)
                {
                    mPlayArea.HighlightUnitMove(mUnit);
                    mHighlitedEquipmentSlot = 0;
                }
                else
                {
                    if (mHighlitedEquipmentSlot == 0)
                    {
                        if (mUnit.FirstSlot is ActiveEquipment)
                        {
                            mPlayArea.HighlightUnitAttack(mUnit, 1);
                            mHighlitedEquipmentSlot = 1;
                        }
                        else if(mUnit.SecondSlot is ActiveEquipment)
                        {
                            mPlayArea.HighlightUnitAttack(mUnit, 2);
                            mHighlitedEquipmentSlot = 2;
                        }
                        else
                        {
                            mPlayArea.RemoveHighlights();
                        }
                    }
                    else if(mHighlitedEquipmentSlot == 1)
                    {
                        if (mUnit.SecondSlot is ActiveEquipment)
                        {
                            mPlayArea.HighlightUnitAttack(mUnit, 2);
                            mHighlitedEquipmentSlot = 2;
                        }
                        else
                        {
                            mPlayArea.RemoveHighlights();
                        }
                    }
                    else if(mHighlitedEquipmentSlot == 2)
                    {
                        mPlayArea.RemoveHighlights();
                    }
                }
            }
            else
                mPlayArea.RemoveHighlights();
        }

        public void SetHighlight(HighlightType pType, object pObject)
        {

            if (pType == HighlightType.Attack)
            {
                if (pObject is PlayerAttack attack)
                {
                    HighlightType = pType;
                    mHighlightedAttack = attack;
                    mHighlightedMove = null;
                    mHighlitedEquipmentSlot = 0;
                }
            }
            if (pType == HighlightType.Move)
            {
                if (pObject is UnitMoveTurn move)
                {
                    HighlightType = pType;
                    mHighlightedMove = move;
                    mHighlightedAttack = null;
                    mHighlitedEquipmentSlot = 0;
                }
            }
            HighlightType = pType;
        }

        public virtual bool CanApplyStatus(SquareStatus status)
        {
            if (mStatuses.Contains(status))
                return false;
            return true;
        }

        public virtual void ApplyStatus(SquareStatus pStatus)
        {
            if (!CanApplyStatus(pStatus))
                return;
            mStatuses.Add(pStatus);
            UpdateColors();
        }

        public virtual void RemoveStatus(SquareStatus pStatus)
        {
            if (mStatuses.Contains(pStatus))
                mStatuses.Remove(pStatus);
            UpdateColors();
        }

        public void SetUnit(Unit pUnit)
        {
            if (mUnit != null)
                mUnit.UnitUpdated -= UnitUpdated;
            mUnit = pUnit;
            if (mUnit != null)
                mUnit.UnitUpdated += UnitUpdated;
            UnitUpdated();
        }

        public void UpdateColors()
        {
            if (mUnit is Mountain)
                SquareColor = Color.Brown;
            else if (mUnit is Building)
                SquareColor = Color.Gray;
            else
                SquareColor = mDefaultColor;

            if (mStatuses.Contains(SquareStatus.Fire))
                StatusColor = Color.Red;
            else if (mStatuses.Contains(SquareStatus.ACID))
                StatusColor = Color.GreenYellow;
            else if (mStatuses.Contains(SquareStatus.Smoke))
                StatusColor = Color.DarkGray;
            else
                StatusColor = SquareColor;

            switch (HighlightType)
            {
                case HighlightType.Selected:
                    HighlightColor = Color.Yellow;
                    break;
                case HighlightType.Move:
                    HighlightColor = Color.LightGreen;
                    break;
                case HighlightType.Attack:
                    HighlightColor = Color.Orange;
                    break;
                default:
                    HighlightColor = SquareColor;
                    break;
            }
        }

        private void UnitUpdated(object sender = null, EventArgs e = null)
        {
            if (mUnit == null)
                mLabel.Text = mEmergingSquare ? "EMERGING!" : "";
            else
                mLabel.Text = mUnit.ToString() + (mEmergingSquare ? "\nEMERGING!" : "");
            UpdateColors();
        }

        public bool CanUnitMoveTo(Unit pUnit)
        {
            return mUnit == null;
        }

        public bool CanUnitMovePast(Unit pUnit)
        {
            if (mUnit == null || mUnit.HasFlyingMovement)
                return true;
            return mUnit.Team == pUnit.Team;
        }

        public List<Square> ArtillerySquares()
        {
            List<Square> squares = new List<Square>();

            Square other;
            foreach (Direction d in Directions.GetDirections())
            {
                for (int i = 2; i < mPlayArea.Size; ++i)
                {
                    other = GetSquareTowards(d, i);
                    if (other == null)
                        break;
                    squares.Add(other);
                }
            }

            return squares;
        }

        public bool Equals(Square other)
        {
            if (other == null)
                return false;
            return GetHashCode().Equals(other.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is Square other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 160684126;
            hashCode = hashCode * -1521134295 + mRow.GetHashCode();
            hashCode = hashCode * -1521134295 + mColumn.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Square square1, Square square2)
        {
            return EqualityComparer<Square>.Default.Equals(square1, square2);
        }

        public static bool operator !=(Square square1, Square square2)
        {
            return !(square1 == square2);
        }

        public List<Square> GetAdjacentSquares()
        {
            List<Square> squares = new List<Square>();
            if (mRow != 0)
                squares.Add(mPlayArea.Squares[mRow - 1, mColumn]);
            if (mRow != mPlayArea.Size - 1)
                squares.Add(mPlayArea.Squares[mRow + 1, mColumn]);

            if (mColumn != 0)
                squares.Add(mPlayArea.Squares[mRow, mColumn - 1]);
            if (mColumn != mPlayArea.Size - 1)
                squares.Add(mPlayArea.Squares[mRow, mColumn + 1]);

            return squares;
        }

        public Square GetSquareTowards(Direction pDirection, int pDistance = 1)
        {
            return Directions.GetSquareFrom(this, pDirection, pDistance);
        }

        public Direction GetDirectionTowards(Square pSquare)
        {
            if (pSquare.Row < mRow && pSquare.Column == mColumn)
                return Direction.North;
            if (pSquare.Row > mRow && pSquare.Column == mColumn)
                return Direction.South;
            if (pSquare.Column < mColumn && pSquare.Row == mRow)
                return Direction.West;
            if (pSquare.Column > mColumn && pSquare.Row == mRow)
                return Direction.East;

            throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            return $"{Name} at {mRow},{mColumn}";
        }
    }

    public enum HighlightType
    {
        Selected,
        Move,
        Attack,
        None
    }
}
