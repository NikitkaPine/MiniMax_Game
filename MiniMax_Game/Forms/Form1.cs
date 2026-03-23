using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MiniMax_Game.GameLogic;

namespace MiniMax_Game
{
    /// <summary>
    /// Main game form. Manages the entire game flow:
    ///   - Initial setup (who goes first, algorithm, starting number)
    ///   - Displaying current state
    ///   - Human move handling (x3 / x4 / x5 buttons)
    ///   - Triggering the computer move via timer
    ///   - Tracking experiment statistics
    ///   - Determining and  displaying the winner
    /// </summary>
    public partial class Form1 : Form
    {
        // ── Current game state ───────────────────────────────────────────────
        private int currentNumber;    // The active number in the game
        private int currentScore;     // Shared score (can be negative)
        private int currentBank;      // Bank points (applied at game end)
        private int currentPlayer;    // Whose turn it is right now (1 or 2)
        private int humanPlayer;      // Player number assigned to the human
        private int computerPlayer;   // Player number assigned to the computer
        private bool gameActive;       // True while a game is in progress
        private bool useAlphaBeta;     // True = Alpha-Beta, False = plain Minimax
        private int currentStreak;     // Tracks consecutive even/odd moves for the streak rule
        private int currentEndOption;  // The target number to reach (8000, 10000, or 15000)


        // ── Experiment tracking ──────────────────────────────────────────────
        private List<ExperimentResult> experiments = new List<ExperimentResult>();

        // ── Timer — gives the UI a frame to repaint before the AI move ───────
        private System.Windows.Forms.Timer computerMoveTimer;

        public Form1()
        {
            InitializeComponent();
            InitTimer();
            UpdateUI();
        }

        // ────────────────────────────────────────────────────────────────────
        // INITIALISATION
        // ────────────────────────────────────────────────────────────────────

        private void InitTimer()
        {
            // A short delay lets the form repaint (showing "Computer thinking...")
            // before the AI calculation blocks the UI thread.
            computerMoveTimer = new System.Windows.Forms.Timer();
            computerMoveTimer.Interval = 400;
            computerMoveTimer.Tick += ComputerMoveTimer_Tick;
        }

        // ────────────────────────────────────────────────────────────────────
        // START GAME
        // ────────────────────────────────────────────────────────────────────

        private void btnStart_Click(object sender, EventArgs e)
        {
            int startNumber = (int)numStartNumber.Value;

            int endOption = GetSelectedEndOption();
            // The player who moves first is always Player 1.
            // This matters for winner determination (even final = Player 1 wins).

            if (rbHumanFirst.Checked)
            {
                humanPlayer = 1;
                computerPlayer = 2;
            }
            else
            {
                humanPlayer = 2;
                computerPlayer = 1;
            }

            useAlphaBeta = rbAlphaBeta.Checked;
            currentNumber = startNumber;
            currentScore = 0;
            currentBank = 0;
            currentStreak = 0;
            currentEndOption = endOption;
            currentPlayer = 1; // Player 1 always moves first
            gameActive = true;

            lstMoveLog.Items.Clear();
            LogMove("Game started. Number: " + currentNumber +
                    "  |  End at: " + endOption +
                    "  |  First move: " + (currentPlayer == humanPlayer ? "Human" : "Computer"));

            UpdateUI();

            // If the computer goes first, trigger its move via timer
            if (currentPlayer == computerPlayer)
                computerMoveTimer.Start();
        }

        private int GetSelectedEndOption()
        {
            if(rb10000.Checked) return 10000;
            if(rb15000.Checked) return 15000;
            return 8000; // Default
        }

        // ────────────────────────────────────────────────────────────────────
        // HUMAN MOVE
        // ────────────────────────────────────────────────────────────────────

        private void btnMul3_Click(object sender, EventArgs e) => HumanMove(3);
        private void btnMul4_Click(object sender, EventArgs e) => HumanMove(4);
        private void btnMul5_Click(object sender, EventArgs e) => HumanMove(5);

        /// <summary>
        /// Applies the human's chosen multiplier move.
        /// If the game continues, hands control to the computer via timer.
        /// </summary>
        private void HumanMove(int multiplier)
        {
            if (!gameActive || currentPlayer != humanPlayer) return;

            ApplyMove(multiplier, "Human");

            if (!gameActive) return; // Game ended on this move

            currentPlayer = computerPlayer;
            UpdateUI();
            computerMoveTimer.Start();
        }

        // ────────────────────────────────────────────────────────────────────
        // COMPUTER MOVE (fired by timer)
        // ────────────────────────────────────────────────────────────────────

        private void ComputerMoveTimer_Tick(object sender, EventArgs e)
        {
            computerMoveTimer.Stop();
            if (!gameActive) return;

            // Build the root node from the current game state.
            // CurrentPlayer = computerPlayer because it is the computer's turn now.
            GameNode root = new GameNode(
                number: currentNumber,
                score: currentScore,
                bank: currentBank,
                currentPlayer: computerPlayer,
                streak: currentStreak,
                endOptions: currentEndOption);

            Stopwatch sw = Stopwatch.StartNew();
            GameNode bestMove = MiniMax.GetBestMove(root, computerPlayer, useAlphaBeta);
            sw.Stop();

            if (bestMove == null)
            {
                // Should not happen in normal play, but guard against it
                EndGame(-1);
                return;
            }

            RecordStats(sw.Elapsed.TotalSeconds, MiniMax.NodesGenerated, MiniMax.NodesEvaluated);

            ApplyMove(bestMove.MoveWeight, "Computer");

            if (!gameActive) return;

            currentPlayer = humanPlayer;
            UpdateUI();
        }

        // ────────────────────────────────────────────────────────────────────
        // APPLY MOVE (shared logic for both players)
        // ────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Computes the result of multiplying by the given factor,
        /// updates game state, writes to the log, and checks for game end.
        /// </summary>
        private void ApplyMove(int multiplier, string playerName)
        {
            var (newNumber, newScore, newBank, newStreak) =
                GameEngine.DoMove(currentNumber, currentScore, currentBank, multiplier,currentStreak);

            // Build log line
            string logLine = playerName + ": " + currentNumber + " x" + multiplier + " = " + newNumber;
            if (newNumber % 2 == 0) logLine += "  [score +1]";
            else logLine += "  [score -1]";
            int ld = newNumber % 10;
            if (ld == 0 || ld == 5) logLine += "  [bank +1]";

            if(newStreak == 0 && currentStreak == 2)
            {
                logLine += "  [3 even streak: score -1]";
            }
            else if(newStreak == 0 && currentStreak == -2)
            {
                logLine += "  [3 odd streak: score +1]";
            }

            currentNumber = newNumber;
            currentScore = newScore;
            currentBank = newBank;
            currentStreak = newStreak;

            LogMove(logLine);

            if (currentNumber >= currentEndOption)
            {
                int winner = GameEngine.GetWinner(currentScore, currentBank);
                EndGame(winner);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // END GAME
        // ────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Finalises the game, determines the winner, updates experiment log.
        /// </summary>
        private void EndGame(int winnerPlayer)
        {
            gameActive = false;
            computerMoveTimer.Stop();

            int finalScore = (currentScore % 2 == 0)
                ? currentScore - currentBank
                : currentScore + currentBank;

            bool humanWon = (winnerPlayer == humanPlayer);
            string winnerText;

            if (winnerPlayer == -1)
                winnerText = "Draw / error";
            else if (humanWon)
                winnerText = "Human wins!";
            else
                winnerText = "Computer wins!";

            LogMove("--- Game over ---");
            LogMove("Score: " + currentScore + "  Bank: " + currentBank + "  Final: " + finalScore);
            LogMove(winnerText);

            experiments.Add(new ExperimentResult
            {
                Number = experiments.Count + 1,
                Algorithm = useAlphaBeta ? "Alpha-Beta" : "Minimax",
                HumanWon = humanWon,
                StartNumber = currentNumber,
            });
            RefreshStatsTable();

            UpdateUI();
            MessageBox.Show(
                "Game over!\n\n" +
                "Final number : " + currentNumber + "\n" +
                "Score : " + currentScore + "   Bank : " + currentBank + "\n" +
                "Final score  : " + finalScore + "\n\n" +
                winnerText,
                "Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ────────────────────────────────────────────────────────────────────
        // UPDATE UI
        // ────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Synchronises all UI controls with the current game state.
        /// Called after every state change.
        /// </summary>
        private void UpdateUI()
        {
            // Settings panel and Start button are only available before the game
            bool setup = !gameActive;
            grpSettings.Enabled = setup;
            btnStart.Enabled = setup;

            // Multiply buttons are only active on the human's turn
            bool humanTurn = gameActive && currentPlayer == humanPlayer;
            btnMul3.Enabled = humanTurn;
            btnMul4.Enabled = humanTurn;
            btnMul5.Enabled = humanTurn;

            // State indicators
            lblNumber.Text = "Current number: " + (gameActive ? currentNumber.ToString() : "-");
            lblScore.Text = "Score: " + currentScore;
            lblBank.Text = "Bank: " + currentBank;

            if (!gameActive)
                lblTurn.Text = "Configure settings and press Start";
            else if (currentPlayer == humanPlayer)
                lblTurn.Text = "Your turn";
            else
                lblTurn.Text = "Computer thinking...";

            // Colour feedback for current turn
            lblTurn.ForeColor = !gameActive ? Color.Gray
                              : currentPlayer == humanPlayer ? Color.FromArgb(0, 190, 110)
                                                                   : Color.FromArgb(220, 120, 30);
        }

        // ────────────────────────────────────────────────────────────────────
        // STATISTICS
        // ────────────────────────────────────────────────────────────────────

        private double totalComputerTime;
        private int totalComputerMoves;
        private int totalNodesGenerated;
        private int totalNodesEvaluated;

        /// <summary>
        /// Records statistics for one computer move and updates the stats label.
        /// </summary>
        private void RecordStats(double seconds, int generated, int evaluated)
        {
            totalComputerTime += seconds;
            totalComputerMoves += 1;
            totalNodesGenerated += generated;
            totalNodesEvaluated += evaluated;

            double avgMs = totalComputerMoves > 0
                ? (totalComputerTime / totalComputerMoves) * 1000.0
                : 0;

            lblStats.Text =
                "Algorithm : " + (useAlphaBeta ? "Alpha-Beta" : "Minimax") + "\n" +
                "Last move  : " + (seconds * 1000).ToString("F1") + " ms\n" +
                "Nodes generated : " + generated + "\n" +
                "Nodes evaluated : " + evaluated + "\n" +
                "Avg move time   : " + avgMs.ToString("F1") + " ms";
        }

        private void RefreshStatsTable()
        {
            int compWins = 0, humWins = 0;
            foreach (ExperimentResult r in experiments)
            {
                if (r.HumanWon) humWins++; else compWins++;
            }
            lblExpSummary.Text =
                "Games played   : " + experiments.Count + "\n" +
                "Human wins     : " + humWins + "\n" +
                "Computer wins  : " + compWins + "\n" +
                "Total nodes gen: " + totalNodesGenerated + "\n" +
                "Total nodes eval: " + totalNodesEvaluated;
        }

        // ────────────────────────────────────────────────────────────────────
        // HELPERS
        // ────────────────────────────────────────────────────────────────────

        private void LogMove(string text)
        {
            lstMoveLog.Items.Add(text);
            lstMoveLog.TopIndex = lstMoveLog.Items.Count - 1; // Auto-scroll to bottom
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            // Reset game state but keep experiment history
            gameActive = false;
            currentNumber = 0;
            currentScore = 0;
            currentBank = 0;
            computerMoveTimer.Stop();
            lstMoveLog.Items.Clear();
            UpdateUI();
        }

        // ── Experiment record ─────────────────────────────────────────────────
        private class ExperimentResult
        {
            public int Number;
            public string Algorithm;
            public bool HumanWon;
            public int StartNumber;
        }
    }
}