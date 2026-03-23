using System.Collections.Generic;

namespace MiniMax_Game.GameLogic
{

    public static class GameEngine
    {
        public static readonly int[] Multipliers = { 3, 4, 5 };

        public const int MaxDepth = 3;

        public static readonly int[] EndOptions = { 8000, 10000, 15000 };
        public static (int newNumber, int newScore, int newBank,int newStreak) DoMove(
            int number, int score, int bank, int multiplier,int streak)
        {
            int newNumber = number * multiplier;
            int newScore = score;
            int newBank = bank;

            bool isEven = (newNumber % 2 == 0);
                
            if(isEven) 
                newScore += 1;
            else
                newScore -= 1;

            int lastDigit = newNumber % 10;
            if (lastDigit == 0 || lastDigit == 5)
                newBank += 1;

            int newStreak;
            if (isEven)
            {
                newStreak = (streak >= 0) ? streak + 1 : 1;
            }
            else 
            { 
                newStreak = (streak <= 0) ? streak - 1 : -1;
            }

            if(newStreak == 3)
            {
                newNumber -= 1;
                newStreak = 0;
            }
            else if(newStreak == -3)
            {
                newNumber += 1;
                newStreak = 0;
            }


            return (newNumber, newScore, newBank,newStreak);
        }

        public static void GenerateTree(GameNode gameNode, int maxDepth)
        {
            if (gameNode.IsTerminal || gameNode.Depth >= maxDepth)
                return;

            int nextPlayer = gameNode.CurrentPlayer == 1 ? 2 : 1;

            foreach (int multiplier in Multipliers)
            {
                var (newNumber, newScore, newBank,newStreak) = DoMove(
                    gameNode.Number, gameNode.Score, gameNode.Bank, multiplier,gameNode.StreakChecker);

                GameNode child = new GameNode(
                    number: newNumber,
                    score: newScore,
                    bank: newBank,
                    currentPlayer: nextPlayer,
                    moveWeight: multiplier,
                    depth: gameNode.Depth + 1,
                    parent: gameNode,
                    streak:newStreak,
                    endOptions: gameNode.EndOptions);

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
