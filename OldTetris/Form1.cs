using OldTetris.Core;

namespace OldTetris
{
    public partial class Form1 : Form
    {
        private const int gridWidth = 10;   // number of columns
        private const int gridHeight = 24;  // number of rows
        private const int cellSize = 30;    // size of each cell in pixels

        private TetrisGame game = new TetrisGame(gridWidth, gridHeight);          // game logic (Core), replaced on restart
        private readonly HighScore highScore = new HighScore("highscore.txt");    // top 10 scores, saved next to the .exe
        private readonly Renderer renderer = new Renderer(cellSize);              // drawing
        private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer(); // makes pieces fall

        private bool scoreSaved = false; // makes sure each game is only saved once

        public Form1()
        {
            InitializeComponent();

            Text = "Tetris";
            // Game area inside the window 920, 720
            // (whole window is ~16 px wider and ~39 px taller)
            ClientSize = new Size(920, 720);
            DoubleBuffered = true; // prevents flickering

            timer.Interval = 500;     // fall speed in milliseconds
            timer.Tick += UpdateGame; // run UpdateGame every tick
            timer.Start();

            Paint += (sender, e) => renderer.Draw(e.Graphics, game, highScore.Top); // let the renderer draw
        }

        // Runs on every timer tick: moves the piece down one step
        private void UpdateGame(object? sender, EventArgs e)
        {
            game.Tick();
            timer.Interval = Math.Max(100, 500 - (game.Level - 1) * 40); // faster each level (min 100 ms)

            if (game.IsGameOver)
                SaveScore();

            Invalidate(); // tells the window to redraw
        }

        // Asks for a name and saves the score to the top 10 (only once per game)
        private void SaveScore()
        {
            if (scoreSaved) return;
            scoreSaved = true; // set first, so it can't run twice while the dialog is open

            if (!highScore.Qualifies(game.Points)) return; // not good enough for the list

            timer.Stop();                                   // pause the game while typing
            string name = NameDialog.Ask(this, game.Points);
            highScore.Submit(name, game.Points);
            timer.Start();
        }

        // Starts a brand new game
        private void RestartGame()
        {
            SaveScore(); // keep the score even if you restart early
            game = new TetrisGame(gridWidth, gridHeight);
            scoreSaved = false;
            timer.Interval = 500;
        }

        // Catches key presses before WinForms uses the arrow keys for something else
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.A:
                    game.MoveLeft();   // move left
                    break;

                case Keys.Right:
                case Keys.D:
                    game.MoveRight();  // move right
                    break;

                case Keys.Down:
                case Keys.S:
                    game.MoveDown();   // fall faster
                    break;

                case Keys.Up:
                case Keys.W:
                    game.Rotate();     // rotate
                    break;

                case Keys.Space:
                    game.HardDrop();   // drop to the bottom
                    break;

                case Keys.P:
                    game.TogglePause(); // pause / resume
                    break;

                case Keys.R:
                    RestartGame();     // start over
                    break;

                default:
                    return base.ProcessCmdKey(ref msg, keyData); // let other keys work as normal
            }

            Invalidate();  // redraw after the key press
            return true;   // tell WinForms the key is handled
        }
    }
}