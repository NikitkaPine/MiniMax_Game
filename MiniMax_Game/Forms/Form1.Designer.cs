// Form1.Designer.cs — English UI, fixed layout
// LEFT COLUMN layout (top to bottom, all inside grpSettings):
//   y=28  : lblStartNumberLabel
//   y=52  : numStartNumber
//   y=90  : grpFirstPlayer   (h=65)
//   y=165 : grpAlgorithm     (h=65)
//   y=240 : grpEndOption     (h=70)
//   grpSettings total h=325
//   y=340 : btnStart         (h=40, on form directly)
//   y=390 : grpStats         (h=155)
//   y=555 : grpExp           (h=100)

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
        private GroupBox grpEndOption;
        private RadioButton rb8000;
        private RadioButton rb10000;
        private RadioButton rb15000;
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

            // ── Form ──────────────────────────────────────────────────────────
            this.Text = "MiniMax Number Game";
            this.ClientSize = new Size(1000, 680);
            this.MinimumSize = new Size(1020, 720);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.Font = new Font("Segoe UI", 9.5f);

            // ══════════════════════════════════════════════════════════════════
            // LEFT COLUMN — grpSettings (x=10, y=10, w=270, h=325)
            // All sub-controls sit INSIDE grpSettings, positions are relative
            // ══════════════════════════════════════════════════════════════════
            grpSettings = MakeGroup("Game Settings", 10, 10, 270, 325);

            // Starting number label  (relative y=24)
            lblStartNumberLabel = new Label
            {
                Text = "Starting number (11-19):",
                Location = new Point(10, 24),
                Size = new Size(240, 20),
                ForeColor = Color.FromArgb(200, 200, 230),
                Font = new Font("Segoe UI", 9f),
            };

            // NumericUpDown  (relative y=48)
            numStartNumber = new NumericUpDown
            {
                Minimum = 11,
                Maximum = 19,
                Value = 15,
                Location = new Point(10, 48),
                Size = new Size(80, 26),
                BackColor = Color.FromArgb(55, 55, 75),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f),
                TextAlign = HorizontalAlignment.Center,
            };

            // Who goes first  (relative y=86, h=62)
            grpFirstPlayer = MakeGroup("Who goes first", 10, 86, 248, 62);
            grpFirstPlayer.Font = new Font("Segoe UI", 9f);

            rbHumanFirst = new RadioButton
            {
                Text = "Human",
                Location = new Point(10, 26),
                Size = new Size(90, 24),
                Checked = true,
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rbComputerFirst = new RadioButton
            {
                Text = "Computer",
                Location = new Point(115, 26),
                Size = new Size(110, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            grpFirstPlayer.Controls.Add(rbHumanFirst);
            grpFirstPlayer.Controls.Add(rbComputerFirst);

            // AI Algorithm  (relative y=158, h=62)
            grpAlgorithm = MakeGroup("AI Algorithm", 10, 158, 248, 62);
            grpAlgorithm.Font = new Font("Segoe UI", 9f);

            rbMinimax = new RadioButton
            {
                Text = "Minimax",
                Location = new Point(10, 26),
                Size = new Size(90, 24),
                Checked = true,
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rbAlphaBeta = new RadioButton
            {
                Text = "Alpha-Beta",
                Location = new Point(115, 26),
                Size = new Size(115, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            grpAlgorithm.Controls.Add(rbMinimax);
            grpAlgorithm.Controls.Add(rbAlphaBeta);

            // Game ends at  (relative y=230, h=80)
            grpEndOption = MakeGroup("Game ends at", 10, 230, 248, 80);
            grpEndOption.Font = new Font("Segoe UI", 9f);

            rb8000 = new RadioButton
            {
                Text = "8 000",
                Location = new Point(8, 28),
                Size = new Size(70, 24),
                Checked = true,
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rb10000 = new RadioButton
            {
                Text = "10 000",
                Location = new Point(86, 28),
                Size = new Size(76, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            rb15000 = new RadioButton
            {
                Text = "15 000",
                Location = new Point(170, 28),
                Size = new Size(76, 24),
                ForeColor = Color.FromArgb(200, 200, 230),
            };
            grpEndOption.Controls.Add(rb8000);
            grpEndOption.Controls.Add(rb10000);
            grpEndOption.Controls.Add(rb15000);

            // Add all sub-controls into grpSettings
            grpSettings.Controls.Add(lblStartNumberLabel);
            grpSettings.Controls.Add(numStartNumber);
            grpSettings.Controls.Add(grpFirstPlayer);
            grpSettings.Controls.Add(grpAlgorithm);
            grpSettings.Controls.Add(grpEndOption);

            // Start button — on the FORM directly, below grpSettings
            // grpSettings bottom = 10 + 325 = 335; button at y=343
            btnStart = new Button
            {
                Text = "Start Game",
                Location = new Point(10, 343),
                Size = new Size(270, 40),
                BackColor = Color.FromArgb(0, 160, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += btnStart_Click;

            // Last Move Stats  y=393, h=150
            grpStats = MakeGroup("Last Move Stats", 10, 393, 270, 150);

            lblStats = new Label
            {
                Text = "Play at least one\ncomputer move to see stats.",
                Location = new Point(10, 26),
                Size = new Size(248, 115),
                ForeColor = Color.FromArgb(170, 200, 170),
                Font = new Font("Segoe UI", 9f),
            };
            grpStats.Controls.Add(lblStats);

            // Experiment Results  y=553, h=120
            grpExp = MakeGroup("Experiment Results", 10, 553, 270, 120);

            lblExpSummary = new Label
            {
                Text = "No games played yet.",
                Location = new Point(10, 26),
                Size = new Size(248, 86),
                ForeColor = Color.FromArgb(200, 220, 255),
                Font = new Font("Segoe UI", 9f),
            };
            grpExp.Controls.Add(lblExpSummary);

            // ══════════════════════════════════════════════════════════════════
            // CENTER COLUMN  x=290, width=380
            // ══════════════════════════════════════════════════════════════════

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

            btnMul3 = MakeMulBtn("x3", 10);
            btnMul4 = MakeMulBtn("x4", 134);
            btnMul5 = MakeMulBtn("x5", 258);
            btnMul3.Click += btnMul3_Click;
            btnMul4.Click += btnMul4_Click;
            btnMul5.Click += btnMul5_Click;

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

            // Game Rules  y=328, h=345
            GroupBox grpRules = MakeGroup("Game Rules", 290, 328, 380, 345);

            Label lblRules = new Label
            {
                Text =
                    "* Starting number: 11 to 19 (exclusive)\r\n" +
                    "* Each turn: multiply current number by 3, 4 or 5\r\n\r\n" +
                    "After each move:\r\n" +
                    "  Result even       -> score +1\r\n" +
                    "  Result odd        -> score -1\r\n" +
                    "  Last digit 0 or 5 -> bank +1\r\n" +
                    "  3 even in a row   -> score -1 (extra)\r\n" +
                    "  3 odd  in a row   -> score +1 (extra)\r\n\r\n" +
                    "Game ends when number >= chosen threshold\r\n" +
                    "(8 000 / 10 000 / 15 000)\r\n\r\n" +
                    "Final score:\r\n" +
                    "  Score even -> final = score - bank\r\n" +
                    "  Score odd  -> final = score + bank\r\n\r\n" +
                    "Winner:\r\n" +
                    "  Final even -> Player 1 wins (first mover)\r\n" +
                    "  Final odd  -> Player 2 wins",
                Location = new Point(12, 26),
                Size = new Size(354, 308),
                ForeColor = Color.FromArgb(190, 200, 220),
                Font = new Font("Segoe UI", 9f),
            };
            grpRules.Controls.Add(lblRules);

            // ══════════════════════════════════════════════════════════════════
            // RIGHT COLUMN  x=680, width=310
            // ══════════════════════════════════════════════════════════════════

            grpLog = MakeGroup("Move Log", 680, 10, 310, 658);

            lstMoveLog = new ListBox
            {
                Location = new Point(8, 26),
                Size = new Size(292, 622),
                BackColor = Color.FromArgb(22, 22, 33),
                ForeColor = Color.FromArgb(190, 210, 190),
                Font = new Font("Consolas", 8.5f),
                BorderStyle = BorderStyle.None,
                ScrollAlwaysVisible = true,
                HorizontalScrollbar = true,
            };
            grpLog.Controls.Add(lstMoveLog);

            // ── Add to form ───────────────────────────────────────────────────
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