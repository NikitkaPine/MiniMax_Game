using System;

namespace MiniMax_Game.GameLogic
{
    /// <summary>
    /// Реализует два алгоритма принятия решений для компьютерного игрока:
    ///   1. Минимакс (Minimax) — полный перебор дерева.
    ///   2. Альфа-бета отсечение (Alpha-Beta) — оптимизированный перебор.
    ///
    /// Оба алгоритма работают как «поиск на N ходов вперёд» (lookahead),
    /// где N = GameEngine.MaxDepth. Дерево строится заново перед каждым ходом машины.
    ///
    /// МИНИМАКС:
    ///   Машина — MAX-игрок (максимизирует свою оценку).
    ///   Человек — MIN-игрок (минимизирует оценку машины).
    ///   Просматриваются ВСЕ узлы дерева — гарантирован оптимальный ход,
    ///   но при большой глубине работает медленнее.
    ///
    /// АЛЬФА-БЕТА:
    ///   Тот же результат, что у минимакса, но с отсечением ветвей,
    ///   которые заведомо не улучшат результат.
    ///   α — лучшее, что MAX уже может гарантировать.
    ///   β — лучшее, что MIN уже может гарантировать.
    ///   Если α >= β — ветка бесполезна и отсекается.
    ///   Обычно сокращает число оцениваемых узлов в 2–10 раз.
    /// </summary>
    public static class MiniMax
    {
        // Счётчики для статистики (сбрасываются перед каждым ходом машины)
        public static int NodesGenerated;  // Количество пройденных узлов
        public static int NodesEvaluated;  // Количество оценённых листьев

        // ════════════════════════════════════════════════════════════════════
        // АЛГОРИТМ 1: МИНИМАКС
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Рекурсивный минимакс.
        ///
        /// Базовый случай: узел — лист (нет детей) → вызываем эвристику.
        ///
        /// Рекурсивный случай:
        ///   - Если ход машины (MAX): возвращаем максимум среди детей.
        ///   - Если ход человека (MIN): возвращаем минимум среди детей.
        ///
        /// ИСПРАВЛЕНИЕ оригинального кода: maxVal инициализировался int.MaxValue
        /// вместо int.MinValue — это означало, что MAX-игрок никогда не обновлял
        /// значение (любое реальное значение было меньше MaxValue, но Math.Max
        /// возвращал бы MaxValue). Теперь исправлено.
        /// </summary>
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