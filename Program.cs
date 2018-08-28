using System;
using Chess.Components;

namespace Chess
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Board board = new Board();

            board.Init();
            board.Draw();
            

            while (!board.CheckMateWhite && !board.CheckMateBlack)
            {
                do
                {
                    Helper.GetPiece(true);
                } while (!board.ReadMove(true));

                Console.WriteLine();

                do
                {
                    Helper.GetPiece(false);
                } while (!board.ReadMove(false));

                Console.WriteLine();
            }

            Helper.GameEnd(board.CheckMateBlack);
            Console.ReadKey();
        }
    }
}
