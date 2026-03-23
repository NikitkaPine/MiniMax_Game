using System;

namespace MiniMax_Game.GameLogic
{

    public static class MiniMax
    {

        public static int NodesGenerated;
        public static int NodesEvaluated;

        public static int Minimax(GameNode node, int machinePlayer)
        {
            NodesGenerated++;

            if (node.Children.Count == 0)
            {
                NodesEvaluated++;
                node.HeuristicValue = Evaluator.Evaluate(node, machinePlayer);
                return node.HeuristicValue;
            }

            if (node.CurrentPlayer == machinePlayer)
            {

                int maxVal = int.MinValue; 
                foreach (GameNode child in node.Children)
                {
                    int val = Minimax(child, machinePlayer);
                    if (val > maxVal) maxVal = val;
                }
                node.HeuristicValue = maxVal;
                return maxVal;
            }
            else
            {
                int minVal = int.MaxValue;
                foreach (GameNode child in node.Children)
                {
                    int val = Minimax(child, machinePlayer);
                    if (val < minVal) minVal = val;
                }
                node.HeuristicValue = minVal;
                return minVal;
            }
        }

        public static int AlphaBeta(
            GameNode node, int machinePlayer,
            int alpha = int.MinValue, int beta = int.MaxValue)
        {
            NodesGenerated++;

            if (node.Children.Count == 0)
            {
                NodesEvaluated++;
                node.HeuristicValue = Evaluator.Evaluate(node, machinePlayer);
                return node.HeuristicValue;
            }

            if (node.CurrentPlayer == machinePlayer)
            {
                // MAX-узел
                int maxVal = int.MinValue;
                foreach (GameNode child in node.Children)
                {
                    int val = AlphaBeta(child, machinePlayer, alpha, beta);
                    if (val > maxVal) maxVal = val;
                    if (val > alpha) alpha = val; 

                    if (alpha >= beta) break;
                }
                node.HeuristicValue = maxVal;
                return maxVal;
            }
            else
            {
                int minVal = int.MaxValue;
                foreach (GameNode child in node.Children)
                {
                    int val = AlphaBeta(child, machinePlayer, alpha, beta);
                    if (val < minVal) minVal = val;
                    if (val < beta) beta = val;  

                    if (alpha >= beta) break;
                }
                node.HeuristicValue = minVal;
                return minVal;
            }
        }

        public static GameNode GetBestMove(
            GameNode root, int computerPlayer, bool useAlphaBeta)
        {
            NodesGenerated = 0;
            NodesEvaluated = 0;

            GameEngine.GenerateTree(root, GameEngine.MaxDepth);

            GameNode bestChild = null;
            int bestValue = int.MinValue;

            foreach (GameNode child in root.Children)
            {
                int val = useAlphaBeta
                    ? AlphaBeta(child, computerPlayer)
                    : Minimax(child, computerPlayer);

                if (val > bestValue)
                {
                    bestValue = val;
                    bestChild = child;
                }
            }

            return bestChild;
        }
    }
}
