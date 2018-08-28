using System;
using System.Collections.Generic;
using Chess.Models;

namespace Chess.Components
{
    public class Board
    {
        private Piece[,] matrix;
        private Position whiteKing, blackKing;
        private List<Piece> whitePieces, blackPieces;

        private bool checkWhite = false, checkBlack = false;
        private bool checkMateWhite = false, checkMateBlack = false;

        public Board() {
            matrix = new Piece[8, 8];
        }

        // initialize the board (add pieces)
        public void Init()
        {
            whitePieces = AddPieces(true);
            blackPieces = AddPieces(false);
        }

        private List<Piece> AddPieces(bool isWhite)
        {
            List<Piece> pieces = new List<Piece>();

            int colStart = isWhite ? 0 : 6;
            int colEnd = isWhite ? 2 : 8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = colStart; j < colEnd; j++)
                {
                    if ((isWhite && j == colStart) || (!isWhite && j != colStart))
                    {
                        switch (i)
                        {
                            case 0:
                            case 7:
                                {
                                    matrix[i, j] = new Rook(isWhite, i, j, matrix);
                                }
                                break;
                            case 1:
                            case 6:
                                {
                                    matrix[i, j] = new Knight(isWhite, i, j, matrix);
                                }
                                break;
                            case 2:
                            case 5:
                                {
                                    matrix[i, j] = new Bishop(isWhite, i, j, matrix);
                                }
                                break;
                            case 3:
                                {
                                    matrix[i, j] = new Queen(isWhite, i, j, matrix);
                                }
                                break;
                            case 4:
                                {
                                    matrix[i, j] = new King(isWhite, i, j, matrix);

                                    if (isWhite)
                                    {
                                        whiteKing = matrix[i, j].Start;
                                    }
                                    else
                                    {
                                        blackKing = matrix[i, j].Start;
                                    }

                                }
                                break;
                        }
                        pieces.Add(matrix[i, j]);
                    }
                    else
                    {
                        matrix[i, j] = new Pawn(isWhite, i, j, matrix);
                        pieces.Add(matrix[i, j]);
                    }
                }
            }

            return pieces;
        }

        // draw the structure of the board
        public void Draw()
        {
            Console.Clear();
            for (int j = 7; j >= 0; j--)
            {
                Console.Write("   ");
                for (int k = 0; k < 8; k++)  {
                    Console.Write("--- ");
                }
                   
                Console.WriteLine();
                Console.Write((j + 1) + " |");

                for (int i = 0; i < 8; i++)
                {
                    Console.ForegroundColor = IsWhiteColor(i, j) ? ConsoleColor.Yellow : ConsoleColor.Blue;                  
                    DrawPiece(i, j);
                    Console.ResetColor();
                    Console.Write(" |");
                }
                Console.WriteLine();
            }

            Console.Write("   "); 
            for (int k = 0; k < 8; k++) {
                Console.Write("--- ");
            }
               
            Console.WriteLine();
            Console.Write("   ");

            for (int k = 0; k < 8; k++) {
                Console.Write(" " + (k + 1) + "  ");
            }

            Console.WriteLine("\n");
        }

        // draw the piece identifier
        public void DrawPiece(int i, int j)
        {
            char piece = matrix[i, j] != null ? matrix[i, j].Identifier : ' ';
            Console.Write(" " + piece);
        }

        // return the color of the piece in the position
        public bool IsWhiteColor(int i, int j)
        {
            return matrix[i, j] != null ? matrix[i, j].IsWhite : false;
        }

        // read move from command line 
        public bool ReadMove(bool isWhite)
        {
            bool result = false;

            // input must follow the pattern
            try
            {
                string[] coordinates;

                coordinates = Console.ReadLine().Split(',');
                int startX = int.Parse(coordinates[0]) - 1;
                int startY = int.Parse(coordinates[1]) - 1;

                Helper.AskMove(isWhite);
                coordinates = Console.ReadLine().Split(',');

                int endX = int.Parse(coordinates[0]) - 1;
                int endY = int.Parse(coordinates[1]) - 1;

                
                if (Move(startX, startY, endX, endY, isWhite)) {

                    result = true;

                    Draw();

                    if (checkWhite)
                    {
                        Helper.Info("Check on the white King", ConsoleColor.Green);
                    }

                    if (checkBlack)
                    {
                        Helper.Info("Check on the black King", ConsoleColor.Green);
                    }
                }

            }
            catch (System.FormatException)
            {
                Helper.Error("Wrong input format");
            }
            catch (System.IndexOutOfRangeException)
            {
                Helper.Error("Wrong input format");
            }

            return result;
        }

        // check if move is inside the board
        private bool IsInBounds(int startX, int startY, int endX, int endY)
        {
            return (startX >= 0 && startX < 8) && (startY >= 0 && startY < 8) &&
                (endX >= 0 && endX < 8) && (endY >= 0 && endY < 8);
        }


        // move a piece from a position to another
        public bool Move(int startX, int startY, int endX, int endY, bool isWhite)
        {
            bool result = false;

            checkWhite = checkBlack = false;


            if (IsInBounds(startX, startY, endX, endY)) {

                Piece piece = matrix[startX, startY];
                Position end = new Position(endX, endY);

                if (piece != null) { // if start cell is empty
                
                    if (piece.IsWhite == isWhite) { // if player move his own piece

                        if ((startX != endX) || (startY != endY)) { // if end is different than start

                            if (piece.CheckMove(end))
                            {
                                result = true;

                                // track king positions
                                if (piece.Start == whiteKing) {
                                    whiteKing = end;
                                }
                                if (piece.Start == blackKing)
                                {
                                    blackKing = end;
                                }

                                // kill enemy control
                                if (matrix[endX, endY] != null) {

                                    if (matrix[endX, endY].IsWhite) {
                                        whitePieces.Remove(matrix[endX, endY]);
                                    } else {
                                        blackPieces.Remove(matrix[endX, endY]);
                                    }
                                        
                                }
                                    
                                // move the piece
                                piece.Start = end;
                                matrix[endX, endY] = piece;
                                matrix[startX, startY] = null;
                                piece = null;

                                if (VerifyCheck(matrix[endX, endY].IsWhite))
                                {
                                    if (matrix[endX, endY].IsWhite)
                                    {
                                        checkMateWhite = true;
                                    }
                                    else
                                    {
                                        checkMateBlack = true;
                                    }
                                }
                                else
                                {
                                    if (matrix[endX, endY].IsWhite) {
                                        checkBlack = VerifyCheck(!matrix[endX, endY].IsWhite);
                                    } else {
                                        checkWhite = VerifyCheck(!matrix[endX, endY].IsWhite);
                                    }
                                }
                            } else {
                                Helper.Error("Invalid move!");
                            }

                        } else {
                            Helper.Error("Start position and end position must be different!");
                        }
                    } else {
                        Helper.Error("You can't move your opponent's piece!");
                    }
                } else {
                    Helper.Error("Selected position is empty!");
                }
            } else {
                Helper.Error("Selected position does not exists!");
            }
                           
            return result;
        }


        private bool VerifyCheck(bool isWhite)
        {
            bool result = false;

            if (isWhite) 
            {
                foreach (Piece piece in blackPieces) {
                    if (piece.CheckMove(whiteKing)) 
                    {
                        result = true;
                    }
                }
            } else {
                foreach (Piece piece in whitePieces)
                {
                    if (piece.CheckMove(blackKing))
                    {
                        result = true;
                    }
                }
            }
          
            return result;
        }

        public bool CheckWhite
        {
            get
            {
                return checkWhite;
            }
        }

        public bool CheckBlack
        {
            get
            {
                return checkBlack;
            }
        }

        public bool CheckMateWhite
        {
            get
            {
                return checkMateWhite;
            }
        }

        public bool CheckMateBlack
        {
            get
            {
                return checkMateBlack;
            }
        }
    }
}
