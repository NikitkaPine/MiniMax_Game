using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MiniMax_Game.GameLogic;

namespace MiniMax_Game
{

    public partial class Form1 : Form
    {
        private int currentNumber;   
        private int currentScore;    
        private int currentBank;     
        private int currentPlayer;    
        private int humanPlayer;      
        private int computerPlayer;  
        private bool gameActive;      
        private bool useAlphaBeta;     
        private int currentStreak;     
        private int currentEndOption;  

        private List<ExperimentResult> experiments = new List<ExperimentResult>();

        private System.Windows.Forms.Timer computerMoveTimer;

        public Form1()
        {
            InitializeComponent();
            InitTimer();
            UpdateUI();
        }

        private void InitTimer()
        {
            computerMoveTimer = new System.Windows.Forms.Timer();
            computerMoveTimer.Interval = 400;
            computerMoveTimer.Tick += ComputerMoveTimer_Tick;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int startNumber = (int)numStartNumber.Value;

            int endOption = GetSelectedEndOption();

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

            // Reset accumulated node statistics for a fresh game run
            totalNodesGenerated = 0;
            totalNodesEvaluated = 0;

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

        private void btnMul3_Click(object sender, EventArgs e) => HumanMove(3);
        private void btnMul4_Click(object sender, EventArgs e) => HumanMove(4);
        private void btnMul5_Click(object sender, EventArgs e) => HumanMove(5);

        private void HumanMove(int multiplier)
        {
            if (!gameActive || currentPlayer != humanPlayer) return;

            ApplyMove(multiplier, "Human");

            if (!gameActive) return;

            currentPlayer = computerPlayer;
            UpdateUI();
            computerMoveTimer.Start();
        }

        private void ComputerMoveTimer_Tick(object sender, EventArgs e)
        {
            computerMoveTimer.Stop();
            if (!gameActive) return;

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
                EndGame(-1);
                return;
            }

            RecordStats(sw.Elapsed.TotalSeconds, MiniMax.NodesGenerated, MiniMax.NodesEvaluated);

            ApplyMove(bestMove.MoveWeight, "Computer");

            if (!gameActive) return;

            currentPlayer = humanPlayer;
            UpdateUI();
        }

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

    
        private void UpdateUI()
        {
           
            bool setup = !gameActive;
            grpSettings.Enabled = setup;
            btnStart.Enabled = setup;

            bool humanTurn = gameActive && currentPlayer == humanPlayer;
            btnMul3.Enabled = humanTurn;
            btnMul4.Enabled = humanTurn;
            btnMul5.Enabled = humanTurn;

            lblNumber.Text = "Current number: " + (gameActive ? currentNumber.ToString() : "-");
            lblScore.Text = "Score: " + currentScore;
            lblBank.Text = "Bank: " + currentBank;

            if (!gameActive)
                lblTurn.Text = "Configure settings and press Start";
            else if (currentPlayer == humanPlayer)
                lblTurn.Text = "Your turn";
            else
                lblTurn.Text = "Computer thinking...";

            lblTurn.ForeColor = !gameActive ? Color.Gray
                              : currentPlayer == humanPlayer ? Color.FromArgb(0, 190, 110)
                                                                   : Color.FromArgb(220, 120, 30);
        }

       
        private double totalComputerTime;
        private int totalComputerMoves;
        private int totalNodesGenerated;
        private int totalNodesEvaluated;

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

        private class ExperimentResult
        {
            public int Number;
            public string Algorithm;
            public bool HumanWon;
            public int StartNumber;
        }
    }
}
