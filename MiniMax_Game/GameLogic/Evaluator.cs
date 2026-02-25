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

            // Промежуточный узел — эвристика
            int h = 0;

            // Фактор 1: прогноз победителя если бы игра закончилась сейчас
            int projectedWinner = GameEngine.GetWinner(node.Score, node.Bank);
            h += (projectedWinner == machinePlayer) ? 30 : -30;

            // Фактор 2: близость к концу (0.0 → 1.0)
            double proximity = (double)node.Number / 3000.0;
            if (proximity > 1.0) proximity = 1.0;
            h += (int)(proximity * 20);

            // Фактор 3: большой банк = неопределённость = риск
            h -= node.Bank * 3;

            // Фактор 4: контроль следующего хода
            h += (node.CurrentPlayer == machinePlayer) ? 5 : -5;

            return h;
        }

    }
}
