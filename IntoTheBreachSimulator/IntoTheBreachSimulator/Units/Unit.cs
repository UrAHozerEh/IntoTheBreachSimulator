using IntoTheBreachSimulator.AllEquipment;
using IntoTheBreachSimulator.Squares;
using IntoTheBreachSimulator.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntoTheBreachSimulator.Units
{
    public abstract class Unit
    {
        public event EventHandler<MoveEventArgs> UnitMoved;
        public event EventHandler UnitUpdated;

        public abstract string Name { get; }

        private Square mLocation;
        public Square Location
        {
            get { return mLocation; }
            set { mLocation = value; }
        }

        private int mBaseHealth;
        public int BaseHealth => mBaseHealth;

        public abstract int MaxHealth { get; }

        protected int mCurrentHealth;
        public int CurrentHealth => mCurrentHealth;

        private bool mHasShield;
        public bool HasShield => mHasShield;

        private Team mTeam;
        public Team Team => mTeam;

        private int mBaseMove;
        public int BaseMove => mBaseMove;

        public abstract int CurrentMove { get; }

        protected bool mHasFlyingMovement;
        public bool HasFlyingMovement => mHasFlyingMovement;

        protected bool mIsMassive;
        public bool IsMassive => mIsMassive;

        protected bool mIsFlying;
        public bool IsFlying => mIsFlying;

        protected bool mIsForceMovable;
        public bool IsForceMovable => mIsForceMovable;

        protected bool mIsBaseArmored;
        public bool IsArmored => mIsBaseArmored || mStatuses.Contains(UnitStatus.Armor);

        private List<UnitStatus> mStatuses;
        public List<UnitStatus> Statuses => mStatuses;

        public bool HasMoved { get; set; }
        public bool HasAttacked { get; set; }

        protected Equipment mFirstSlot;
        public Equipment FirstSlot => mFirstSlot;

        protected Equipment mSecondSlot;
        public Equipment SecondSlot => mSecondSlot;

        public Unit WebbedTarget;

        public Unit(Square pLocation, Team pTeam, int pBaseHealth, int pBaseMove)
        {
            mLocation = pLocation;
            mTeam = pTeam;
            mBaseHealth = pBaseHealth;
            mCurrentHealth = MaxHealth;
            mBaseMove = pBaseMove;
            mHasFlyingMovement = false;
            mHasShield = false;
            mIsFlying = false;
            mIsMassive = false;
            mIsForceMovable = true;
            mIsBaseArmored = false;
            mStatuses = new List<UnitStatus>();
            mLocation.SetUnit(this);
        }

        public virtual List<Square> MoveSquares()
        {
            if (mStatuses.Contains(UnitStatus.Web))
                return new List<Square>();
            if (HasMoved || HasAttacked)
                return new List<Square>();
            List<Square> found = new List<Square>();
            FindSquares(ref found, mLocation, 0, CurrentMove);
            return found;
        }

        public List<UnitMoveTurn> PossibleMoves()
        {
            List<UnitMoveTurn> moves = new List<UnitMoveTurn>();
            foreach (Square s in MoveSquares())
                moves.Add(new UnitMoveTurn(this, s));
            return moves;
        }

        public abstract List<PlayerAttack> PossibleAttacks(int pEquipment = 0);

        protected void FindSquares(ref List<Square> pFoundSquares, Square pCurSquare, int pCurMoves, int pMaxMoves)
        {
            if (pCurMoves == pMaxMoves)
                return;
            List<Square> adjacent = pCurSquare.GetAdjacentSquares();

            foreach (Square s in adjacent)
            {
                if (s.CanUnitMoveTo(this))
                    if (!pFoundSquares.Contains(s))
                        pFoundSquares.Add(s);

                if (HasFlyingMovement || s.CanUnitMovePast(this))
                    FindSquares(ref pFoundSquares, s, pCurMoves + 1, pMaxMoves);
            }
        }

        public virtual int Damage(int pDamage, DamageType pType)
        {
            if (pDamage == 0)
                return 0;
            if (pType == DamageType.Direct || pType == DamageType.DirectEnemy)
            {
                if (pType == DamageType.DirectEnemy && mTeam == Team.Enemy)
                    pDamage += Location.PlayArea.GetVekHormoneDamage();
                if (IsArmored)
                    --pDamage;
                if (mStatuses.Contains(UnitStatus.ACID))
                    pDamage *= 2;
            }
            if (mHasShield)
            {
                mHasShield = false;
                UnitUpdated?.Invoke(this, null);
                return -999;
            }
            mCurrentHealth -= pDamage;
            UnitUpdated?.Invoke(this, null);
            return pDamage;
        }

        public List<IFullAction> PossibleActions()
        {
            List<IFullAction> actions = new List<IFullAction>();
            actions.AddRange(PossibleAttacks());
            actions.AddRange(PossibleMoves());
            return actions;
        }

        public virtual void Heal(int pDamage)
        {
            if (pDamage == 0)
                return;
            mCurrentHealth = Math.Min(CurrentHealth + pDamage, MaxHealth);
            UnitUpdated?.Invoke(this, null);
        }

        public virtual bool ApplyStatus(UnitStatus pStatus)
        {
            if (!CanApplyStatus(pStatus))
                return false;
            mStatuses.Add(pStatus);
            UnitUpdated?.Invoke(this, null);
            return true;
        }

        public virtual bool CanApplyStatus(UnitStatus pStatus)
        {
            if (pStatus != UnitStatus.Web && mStatuses.Contains(pStatus))
                return false;
            switch (pStatus)
            {
                case UnitStatus.Web:
                    if (mStatuses.Contains(UnitStatus.WebImmune))
                        return false;
                    break;
                case UnitStatus.Smoke:
                    if (mStatuses.Contains(UnitStatus.SmokeImmune))
                        return false;
                    break;
                case UnitStatus.Water:
                    if (IsMassive)
                        return false;
                    break;
                case UnitStatus.Fire:
                    if (mStatuses.Contains(UnitStatus.FireImmune))
                        return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        public void RemoveStatus(UnitStatus pStatus)
        {
            if (mStatuses.Contains(pStatus))
            {
                mStatuses.Remove(pStatus);
                UnitUpdated?.Invoke(this, null);
            }
        }

        public void MoveTo(Square pSquare)
        {
            MoveEventArgs args = new MoveEventArgs(this, Location, pSquare);
            Location.SetUnit(null);
            pSquare.SetUnit(this);
            mLocation = pSquare;
            UnitMoved?.Invoke(this, args);
        }

        public void ApplyShield()
        {
            mHasShield = true;
            UnitUpdated?.Invoke(this, null);
        }

        public override string ToString()
        {
            string output = "";
            if (mHasShield)
                output = "($HP)";
            else
                output = "$HP";
            output = output.Replace("$HP", $"{CurrentHealth}/{MaxHealth}");
            string statuses = "";
            foreach (UnitStatus s in mStatuses)
            {
                switch (s)
                {
                    case UnitStatus.Web:
                        statuses += "W";
                        break;
                    case UnitStatus.WebImmune:
                        statuses += "w";
                        break;
                    case UnitStatus.ACID:
                        statuses += "A";
                        break;
                    case UnitStatus.Smoke:
                        statuses += "S";
                        break;
                    case UnitStatus.SmokeImmune:
                        statuses += "s";
                        break;
                    case UnitStatus.Water:
                        statuses += "L";
                        break;
                    case UnitStatus.Armor:
                        statuses += "R";
                        break;
                    case UnitStatus.Fire:
                        statuses += "F";
                        break;
                    case UnitStatus.FireImmune:
                        statuses += "f";
                        break;
                    default:
                        break;
                }
            }
            return Name + "\n" + output + "\n" + statuses;
        }
    }

    public enum Team
    {
        Player,
        Enemy,
        Neutral
    }

    public enum UnitType
    {
        Science,
        Brute,
        Ranged,
        Prime,
        Universal
    }

    public class MoveEventArgs : EventArgs
    {
        private Unit mUnit;
        public Unit Unit => mUnit;

        private Square mStartSquare;
        public Square StartSquare => mStartSquare;

        private Square mEndSquare;
        public Square EndSquare => mEndSquare;

        public MoveEventArgs(Unit pUnit, Square pStartSquare, Square pEndSquare)
        {
            mUnit = pUnit;
            mStartSquare = pStartSquare;
            mEndSquare = pEndSquare;
        }
    }
}
