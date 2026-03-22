using System.Collections.Generic;

namespace MiniMax_Game.GameLogic
{

    public static class GameEngine
    {
        public static readonly int[] Multipliers = { 3, 4, 5 };

        public const int MaxDepth = 7;

        public static (int newNumber, int newScore, int newBank) DoMove(
            int number, int score, int bank, int multiplier)
        {
            int newNumber = number * multiplier;
            int newScore = score;
            int newBank = bank;

            if (newNumber % 2 == 0)
                newScore += 1;
            else
                newScore -= 1;

            int lastDigit = newNumber % 10;
            if (lastDigit == 0 || lastDigit == 5)
                newBank += 1;

            return (newNumber, newScore, newBank);
        }

        public static void GenerateTree(GameNode gameNode, int maxDepth)
        {
            if (gameNode.IsTerminal || gameNode.Depth >= maxDepth)
                return;

            int nextPlayer = gameNode.CurrentPlayer == 1 ? 2 : 1;

            foreach (int multiplier in Multipliers)
            {
                var (newNumber, newScore, newBank) = DoMove(
                    gameNode.Number, gameNode.Score, gameNode.Bank, multiplier);

                GameNode child = new GameNode(
                    number: newNumber,
                    score: newScore,
                    bank: newBank,
                    currentPlayer: nextPlayer,
                    moveWeight: multiplier,
                    depth: gameNode.Depth + 1,
                    parent: gameNode);

                gameNode.Children.Add(child);

                GenerateTree(child, maxDepth);
            }
        }

        public static int GetWinner(int score, int bank)
        {
            int finalScore = (score % 2 == 0) ? score - bank : score + bank;
            return (finalScore % 2 == 0) ? 1 : 2;
        }
    }
}
