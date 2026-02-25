using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMax_Game.GameLogic
{
    public static class MiniMax
    {
        public static int NodesGenerated;
        public static int NodesEvaluated;

        public static int Minimax(GameNode node, int machinePlayer)
        {
            NodesGenerated++;

            if(node.Children.Count == 0)
            {
                NodesEvaluated++;
                node.HeuristicValue = Evaluator.Evaluate(node, machinePlayer);
                return node.HeuristicValue;
            }

            if(node.CurrentPlayer == machinePlayer)
            {
                int maxVal = int.MaxValue;
                foreach(GameNode child in node.Children)
                {
                    int val = Minimax(child, machinePlayer);
                    maxVal = Math.Max(maxVal, val);
                }
                node.HeuristicValue = maxVal;
                return maxVal;
            }
            else
            {
                int minVal = int.MaxValue;
                foreach (var child in node.Children)
                {
                    int val = Minimax(child, machinePlayer);
                    minVal = Math.Min(minVal, val);
                }
                node.HeuristicValue = minVal;
                return minVal;
            }
        }

        public static GameNode GetBestMove(GameNode root, int computerPlayer)
        {
            NodesGenerated = 0;
            NodesEvaluated = 0;

            GameEngine.GenerateTree(root, GameEngine.MaxDepth);

            GameNode bestChild = null;
            int bestValue = int.MinValue;

            foreach (var child in root.Children)
            {
                int val = Minimax(child, computerPlayer);

                if (val > bestValue)
                {
                    bestValue = val;
                    bestChild = child;
                }
            }
            Console.WriteLine("Hello_World");

            return bestChild;
        }
    }
}
