using IntoTheBreachSimulator.Squares;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public abstract class Directions
    {
        public static Square GetSquareFrom(Square pSquare, Direction pDirection, int pDistance = 1)
        {
            PlayArea playArea = pSquare.PlayArea;
            int size = playArea.Size;
            int row = pSquare.Row;
            int col = pSquare.Column;

            if(pDistance == -1)
            {
                if (GetSquareFrom(pSquare, pDirection) == null)
                    return null;
                Square square = pSquare;
                do
                {
                    if (GetSquareFrom(square, pDirection) == null)
                        break;
                    square = GetSquareFrom(square, pDirection);
                } while (square != null && square.Unit == null);
                return square;
            }

            switch (pDirection)
            {
                case Direction.North:
                    row -= pDistance;
                    if (row < 0)
                        return null;
                    break;
                case Direction.East:
                    col += pDistance;
                    if (col >= size)
                        return null;
                    break;
                case Direction.South:
                    row += pDistance;
                    if (row >= size)
                        return null;
                    break;
                case Direction.West:
                    col -= pDistance;
                    if (col < 0)
                        return null;
                    break;
                default:
                    break;
            }

            return playArea.Squares[row, col];
        }

        public static Direction[] GetDirections()
        {
            return new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };
        }

        public static Direction OppositeDirection(Direction pDirection)
        {
            switch (pDirection)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.North;
            }
        }
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}
