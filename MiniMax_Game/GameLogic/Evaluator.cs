using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMax_Game.GameLogic
{
    public static class Evaluator
    {
        public static int Evaluate(GameNode node, int machinePlayer)
        {
            if (node.IsTerminal)
            {
                int winner = GameEngine.GetWinner(node.Score, node.Bank);
                return winner == machinePlayer ? 1000 : -1000;
            }

            int h = 0;

            int projectedWinner = GameEngine.GetWinner(node.Score, node.Bank);
            h += (projectedWinner == machinePlayer) ? 30 : -30;

            double proximity = (double)node.Number / node.EndOptions;
            if (proximity > 1.0) proximity = 1.0;
            h += (int)(proximity * 20);

            h -= node.Bank * 3;

            h += (node.CurrentPlayer == machinePlayer) ? 5 : -5;

            return h;
        }

    }
}
