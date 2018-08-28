using System;

namespace Chess.Models
{
    public class Queen : Piece
    {
        public Queen(bool isWhite, int x, int y, Piece[,] matrix) : base(isWhite, x, y, matrix)
        {
            identifier = 'Q';
        }

        public override bool CheckMove(Position end)
        {
            bool result = false;

            int absX = GetMoveAbsValue(end, 'X');
            int absY = GetMoveAbsValue(end, 'Y');


            if (start.X == end.X) // vertical move
            {
                for (int i = 1; ((i < absY) && (result)); i++)
                {
                    if (start.Y < end.Y)
                    {
                        result = !(matrix[start.X, start.Y + i] != null);
                    }
                    else
                    {
                        result = !(matrix[start.X, start.Y - i] != null);
                    }
                }
                   
            }
            else if (start.Y == end.Y) // horizontal move
            {
                for (int i = 1; ((i < absX) && (result)); i++)
                {
                    if (start.Y < end.Y)
                    {
                        result = !(matrix[start.X + i, start.Y] != null);
                    }
                    else
                    {
                        result = !(matrix[start.X - i, start.Y] != null);
                    }
                }
            }
            else if (absX == absY) // diagonal move
            {
                for (int i = 1; ((i < absX) && (result)); i++) 
                {
                    if (start.X < end.X)
                    {
                        if (start.Y < end.Y)
                        {
                            result = !(matrix[start.X + i, start.Y + i] != null);
                        }
                        else 
                        {
                            result = !(matrix[start.X + i, start.Y - i] != null);
                        }
                    }
                    else
                    {
                        if (start.Y < end.Y)
                        {
                            result = !(matrix[start.X - i, start.Y + i] != null);
                        }
                        else
                        {
                            result = !(matrix[start.X - i, start.Y - i] != null);
                        }
                    }
                }
            }

            return result ? IsPositionFreeOfAllies(end) : result;
        }
    }
}
