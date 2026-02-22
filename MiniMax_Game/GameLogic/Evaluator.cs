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

        }

    }
}
