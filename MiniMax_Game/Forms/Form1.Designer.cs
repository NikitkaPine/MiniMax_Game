// Form1.Designer.cs — English UI, fixed layout, no overlaps
// Layout plan (1000px wide):
//   Left column  x=10..280  (270px): Settings + Start + Stats + Experiments
//   Center       x=290..670 (380px): Game board + Rules
//   Right column x=680..990 (310px): Move log

using System.Drawing;
using System.Windows.Forms;

namespace MiniMax_Game
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // ── Form fields ──────────────────────────────────────────────────────
        private GroupBox grpSettings;
        private Label lblStartNumberLabel;
        private NumericUpDown numStartNumber;
        private GroupBox grpFirstPlayer;
        private RadioButton rbHumanFirst;
        private RadioButton rbComputerFirst;
        private GroupBox grpAlgorithm;
        private RadioButton rbMinimax;
        private RadioButton rbAlphaBeta;
        private Button btnStart;

        private GroupBox grpStats;
        private Label lblStats;
        private GroupBox grpExp;
        private Label lblExpSummary;

        private Label lblNumber;
        private Label lblScore;
        private Label lblBank;
        private Label lblTurn;
        private Button btnMul3;
        private Button btnMul4;
        private Button btnMul5;
        private Button btnNewGame;

        private GroupBox grpLog;
        private ListBox lstMoveLog;

        // ── Helper methods (NOT local functions — must be class-level for C# 5/6) ──
        private GroupBox MakeGroup(string title, int x, int y, int w, int h)
        {
            return new GroupBox
            {
                Text = title,
                Location = new Point(x, y),
                Size = new Size(w, h),
                ForeColor = Color.FromArgb(180, 180, 220),
                BackColor = Color.FromArgb(40, 40, 55),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            };
        }

        private Button MakeMulBtn(string text, int x)
        {
            Button b = new Button
            {
                Text = text,
                Location = new Point(x, 190),
                Size = new Size(112, 64),
                BackColor = Color.FromArgb(50, 90, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false,
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 120, 210);
            return b;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Form ─────────────────────────────────────────────────────────
            this.Text = "MiniMax Number Game";
            this.ClientSize = new Size(1000, 660);
            this.MinimumSize = new Size(1020, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.Font = new Font("Segoe UI", 9.5f);

            // ════════════════════════════════════════════════════════════════
            // LEFT COLUMN  x=10, width=270
            // ════════════════════════════════════════════════════════════════

            // Settings group  y=10, h=260
            grpSettings = MakeGroup("Game Settings", 10, 10, 270, 260);

            lblStartNumberLabel = new Label
            {
                Text = "Starting number (20-30):",
                Location = new Point(10, 28),
                Size = new Size(240, 20),
                ForeColor = Color.FromArgb(200, 200, 230),
                Font = new Font("Segoe UI", 9f),
            };

            numStartNumber = new NumericUpDown
            {
                Minimum = 20,
                Maximum = 30,
                Value = 25,
                Location = new Point(10, 52),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(55, 55, 75),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f),
                TextAlign = HorizontalAlignment.Center,
            };

            // Who goes first  y=90, h=65
            grpFirstPlayer = MakeGroup("Who goes first", 10, 90, 250, 65);
            grpFirstPlayer.Font = new Font("Segoe UI", 9f);

            rbHumanFirst = new RadioButton
            {
                Text = "Human",
                Location = new Point(10, 28),
                Size = new Size(90, 24),
                Checked = true,
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rbComputerFirst = new RadioButton
            {
                Text = "Computer",
                Location = new Point(115, 28),
                Size = new Size(110, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            grpFirstPlayer.Controls.Add(rbHumanFirst);
            grpFirstPlayer.Controls.Add(rbComputerFirst);

            // Algorithm  y=165, h=65
            grpAlgorithm = MakeGroup("AI Algorithm", 10, 165, 250, 65);
            grpAlgorithm.Font = new Font("Segoe UI", 9f);

            rbMinimax = new RadioButton
            {
                Text = "Minimax",
                Location = new Point(10, 28),
                Size = new Size(90, 24),
                Checked = true,
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rbAlphaBeta = new RadioButton
            {
                Text = "Alpha-Beta",
                Location = new Point(115, 28),
                Size = new Size(115, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            grpAlgorithm.Controls.Add(rbMinimax);
            grpAlgorithm.Controls.Add(rbAlphaBeta);

            grpSettings.Controls.Add(lblStartNumberLabel);
            grpSettings.Controls.Add(numStartNumber);
            grpSettings.Controls.Add(grpFirstPlayer);
            grpSettings.Controls.Add(grpAlgorithm);

            // Start button  y=278, h=40
            btnStart = new Button
            {
                Text = "Start Game",
                Location = new Point(10, 278),
                Size = new Size(270, 40),
                BackColor = Color.FromArgb(0, 160, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += btnStart_Click;

            // Stats group  y=328, h=160
            grpStats = MakeGroup("Last Move Stats", 10, 328, 270, 160);

            lblStats = new Label
            {
                Text = "Play at least one\ncomputer move to see stats.",
                Location = new Point(10, 26),
                Size = new Size(248, 125),
                ForeColor = Color.FromArgb(170, 200, 170),
                Font = new Font("Segoe UI", 9f),
            };
            grpStats.Controls.Add(lblStats);

            // Experiments group  y=498, h=150
            grpExp = MakeGroup("Experiment Results", 10, 498, 270, 150);

            lblExpSummary = new Label
            {
                Text = "No games played yet.",
                Location = new Point(10, 26),
                Size = new Size(248, 115),
                ForeColor = Color.FromArgb(200, 220, 255),
                Font = new Font("Segoe UI", 9f),
            };
            grpExp.Controls.Add(lblExpSummary);

            // ════════════════════════════════════════════════════════════════
            // CENTER COLUMN  x=290, width=380
            // ════════════════════════════════════════════════════════════════

            // Game board group  y=10, h=310
            GroupBox grpBoard = MakeGroup("Game Board", 290, 10, 380, 308);

            lblNumber = new Label
            {
                Text = "Current number: -",
                Location = new Point(10, 32),
                Size = new Size(358, 52),
                ForeColor = Color.FromArgb(255, 220, 80),
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            lblScore = new Label
            {
                Text = "Score: 0",
                Location = new Point(10, 90),
                Size = new Size(170, 30),
                ForeColor = Color.FromArgb(180, 220, 255),
                Font = new Font("Segoe UI", 13f),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            lblBank = new Label
            {
                Text = "Bank: 0",
                Location = new Point(198, 90),
                Size = new Size(170, 30),
                ForeColor = Color.FromArgb(255, 180, 120),
                Font = new Font("Segoe UI", 13f),
                TextAlign = ContentAlignment.MiddleRight,
            };

            lblTurn = new Label
            {
                Text = "Configure settings and press Start",
                Location = new Point(10, 128),
                Size = new Size(358, 34),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Multiply buttons  y=190 inside grpBoard, gap=12
            btnMul3 = MakeMulBtn("x3", 10);
            btnMul4 = MakeMulBtn("x4", 134);
            btnMul5 = MakeMulBtn("x5", 258);
            btnMul3.Click += btnMul3_Click;
            btnMul4.Click += btnMul4_Click;
            btnMul5.Click += btnMul5_Click;

            // New game button  y=266
            btnNewGame = new Button
            {
                Text = "New Game",
                Location = new Point(10, 266),
                Size = new Size(358, 32),
                BackColor = Color.FromArgb(80, 55, 25),
                ForeColor = Color.FromArgb(220, 180, 120),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f),
                Cursor = Cursors.Hand,
            };
            btnNewGame.FlatAppearance.BorderSize = 0;
            btnNewGame.Click += btnNewGame_Click;

            grpBoard.Controls.Add(lblNumber);
            grpBoard.Controls.Add(lblScore);
            grpBoard.Controls.Add(lblBank);
            grpBoard.Controls.Add(lblTurn);
            grpBoard.Controls.Add(btnMul3);
            grpBoard.Controls.Add(btnMul4);
            grpBoard.Controls.Add(btnMul5);
            grpBoard.Controls.Add(btnNewGame);

            // Rules group  y=328, h=320
            GroupBox grpRules = MakeGroup("Game Rules", 290, 328, 380, 320);

            Label lblRules = new Label
            {
                Text =
                    "* Choose a starting number between 20 and 30\r\n" +
                    "* Each turn: multiply the current number by 3, 4 or 5\r\n\r\n" +
                    "After each move:\r\n" +
                    "  Result is even   -> score +1\r\n" +
                    "  Result is odd    -> score -1\r\n" +
                    "  Last digit 0 or 5 -> bank +1\r\n\r\n" +
                    "Game ends when number >= 3000\r\n\r\n" +
                    "Final score:\r\n" +
                    "  Score is even   -> final = score - bank\r\n" +
                    "  Score is odd    -> final = score + bank\r\n\r\n" +
                    "Winner:\r\n" +
                    "  Final even  -> Player 1 wins (first mover)\r\n" +
                    "  Final odd   -> Player 2 wins",
                Location = new Point(12, 26),
                Size = new Size(354, 284),
                ForeColor = Color.FromArgb(190, 200, 220),
                Font = new Font("Segoe UI", 9f),
            };
            grpRules.Controls.Add(lblRules);

            // ════════════════════════════════════════════════════════════════
            // RIGHT COLUMN  x=680, width=310
            // ════════════════════════════════════════════════════════════════

            // Move log  y=10, h=638
            grpLog = MakeGroup("Move Log", 680, 10, 310, 638);

            lstMoveLog = new ListBox
            {
                Location = new Point(8, 26),
                Size = new Size(292, 602),
                BackColor = Color.FromArgb(22, 22, 33),
                ForeColor = Color.FromArgb(190, 210, 190),
                Font = new Font("Consolas", 8.5f),
                BorderStyle = BorderStyle.None,
                ScrollAlwaysVisible = true,
                HorizontalScrollbar = true,
            };
            grpLog.Controls.Add(lstMoveLog);

            // ── Add everything to the form ───────────────────────────────────
            this.Controls.Add(grpSettings);
            this.Controls.Add(btnStart);
            this.Controls.Add(grpStats);
            this.Controls.Add(grpExp);
            this.Controls.Add(grpBoard);
            this.Controls.Add(grpRules);
            this.Controls.Add(grpLog);
        }
    }
}