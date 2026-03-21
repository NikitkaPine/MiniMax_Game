using System.Collections.Generic;

namespace MiniMax_Game.GameLogic
{
    /// <summary>
    /// Ядро игровой логики: правила ходов, генерация дерева, определение победителя.
    ///
    /// ПРАВИЛА ИГРЫ:
    ///   1. Начальное число — от 20 до 30 (выбирает человек).
    ///   2. Общий счёт = 0, банк = 0.
    ///   3. Каждый ход: умножить текущее число на 3, 4 или 5.
    ///      - Если РЕЗУЛЬТАТ чётный  → счёт += 1
    ///      - Если РЕЗУЛЬТАТ нечётный → счёт -= 1
    ///      - Если последняя цифра результата = 0 или 5 → банк += 1
    ///   4. Игра заканчивается при числе >= 3000.
    ///   5. Финальный счёт: чётный → вычесть банк; нечётный → прибавить банк.
    ///   6. Финальный счёт чётный → победитель — тот, кто начинал (игрок 1).
    ///      Финальный счёт нечётный → победитель — второй игрок (игрок 2).
    /// </summary>
    public static class GameEngine
    {
        public static readonly int[] Multipliers = { 3, 4, 5 };

        public const int MaxDepth = 7;

        /// <summary>
        /// Применяет один ход (умножение на multiplier) к состоянию игры.
        /// 
        /// Возвращает кортеж с тремя новыми значениями — не меняет входные параметры.
        /// Такой «чистый» подход позволяет безопасно генерировать дерево:
        /// каждый дочерний узел получает собственную копию состояния.
        ///
        /// ВАЖНО: чётность проверяется у РЕЗУЛЬТАТА умножения (newNumber),
        /// а не у исходного числа. Это соответствует условию задачи.
        /// </summary>
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