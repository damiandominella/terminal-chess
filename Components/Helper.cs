using System;
namespace Chess.Components
{
    public static class Helper
    {
        public static void GetPiece(bool isWhite)
        {
            Console.ForegroundColor = isWhite ? ConsoleColor.Yellow : ConsoleColor.Blue;
            string player = isWhite ? "WHITE" : "BLACK";
            Console.Write("[" + player + "], select the piece to move (X, Y): ");
            Console.ResetColor();
        }

        public static void AskMove(bool isWhite)
        {
            Console.ForegroundColor = isWhite ? ConsoleColor.Yellow : ConsoleColor.Blue;
            string player = isWhite ? "WHITE" : "BLACK";
            Console.Write("[" + player + "], select the position where to move the piece (X, Y): ");
            Console.ResetColor();
        }

        public static void Info(string message, ConsoleColor color = ConsoleColor.White) 
        {
            Console.ForegroundColor = color;
            Console.WriteLine("[INFO] " + message);
            Console.ResetColor();
        }

        public static void GameEnd(bool isWhite)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string player = isWhite ? "WHITE" : "BLACK";
            Console.WriteLine("[CHECK MATE] " + player + " won the game!");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERROR] " + message);
            Console.ResetColor();
        }
    }
}
