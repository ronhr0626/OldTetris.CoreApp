using OldTetris.Core;

namespace OldTetris
{
    // Only job: draw the game on the screen
    public class Renderer
    {
        private readonly int cellSize;
        private static readonly Font font = new Font("Segoe UI", 16);                       // font for the score text
        private static readonly Font titleFont = new Font("Segoe UI", 36, FontStyle.Bold);  // big bold font for the title
        private static readonly Font smallFont = new Font("Segoe UI", 12);                  // small font for the controls
        private static readonly StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far }; // right-aligned text
        
        // Index 0 = empty cell, 1-7 = piece colors (I, O, T, S, Z, J, L)
        private static readonly Brush[] colors =
        {
            Brushes.Black, Brushes.Cyan, Brushes.Yellow, Brushes.Purple,
            Brushes.LimeGreen, Brushes.Red, Brushes.Blue, Brushes.Orange
        };

        // How to play, shown at the bottom right
        private const string controls =
            "How to play:\n" +
            "←  /  A      Move left\n" +
            "→  /  D      Move right\n" +
            "↓  /  S      Move down\n" +
            "↑  /  W      Rotate\n" +
            "Space        Drop\n" +
            "P                Pause\n" +
            "R                Restart";

        public Renderer(int cellSize)
        {
            this.cellSize = cellSize;
        }

        // Draws the board, the title, the scores, the next piece, the top 10, status messages and controls
        public void Draw(Graphics g, TetrisGame game, IReadOnlyList<ScoreEntry> topScores)
        {
            // 1. Draw the board
            for (int x = 0; x < game.Width; x++)
            {
                for (int y = 0; y < game.Height; y++)
                {
                    // new Rectangle(x, y, width, height)
                    //               ↑  ↑    ↑      ↑
                    //             left top width height

                    // Here we draw the box (rectangle) for each cell
                    Rectangle rect = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);
                    g.FillRectangle(colors[game.GetCell(x, y)], rect); // fill with the cell's color (black = empty)
                    g.DrawRectangle(Pens.Gray, rect);                  // draw a gray border around each cell (makes the grid lines)
                }
            }

            int textX = game.Width * cellSize + 20; // left edge of the text area, right of the board
            int best = topScores.Count > 0 ? topScores[0].Points : 0;

            // 2. Draw the title
            g.DrawString("Tetris", titleFont, Brushes.Black, textX, 10);

            // 3. Draw score, high score, lines and level
            g.DrawString($"Score: {game.Points}", font, Brushes.Black, textX, 90);
            g.DrawString($"High score: {Math.Max(best, game.Points)}", font, Brushes.Black, textX, 130);
            g.DrawString($"Lines: {game.Lines}", font, Brushes.Black, textX, 170);
            g.DrawString($"Level: {game.Level}", font, Brushes.Black, textX, 210);

            // 4. Draw the next piece
            g.DrawString("Next:", font, Brushes.Black, textX, 260);

            Tetromino nextPiece = game.NextPiece;
            for (int r = 0; r < nextPiece.Rows; r++)
            {
                for (int c = 0; c < nextPiece.Cols; c++)
                {
                    if (!nextPiece.IsFilled(r, c)) continue;

                    Rectangle rect = new Rectangle(textX + c * cellSize, 300 + r * cellSize, cellSize, cellSize);
                    g.FillRectangle(colors[nextPiece.Color], rect); // fill with the piece's color
                    g.DrawRectangle(Pens.Gray, rect);               // gray border
                }
            }

            // 5. Draw the top 10 list in a second column
            int listX = textX + 240;     // left edge of the list
            const int listWidth = 300;   // the points end at the right edge of this width

            g.DrawString("Top 10", font, Brushes.Black, listX, 90);

            for (int i = 0; i < 10; i++)
            {
                int rowY = 130 + i * 35;

                // Column 1: place number
                g.DrawString($"{i + 1}.", font, Brushes.Black, listX, rowY);

                if (i < topScores.Count)
                {
                    // Column 2: name
                    g.DrawString(topScores[i].Name, font, Brushes.Black, listX + 45, rowY);

                    // Column 3: points, right-aligned so the numbers line up
                    RectangleF pointsArea = new RectangleF(listX, rowY, listWidth, 35);
                    g.DrawString(topScores[i].Points.ToString(), font, Brushes.Black, pointsArea, rightAlign);
                }
                else
                {
                    g.DrawString("-", font, Brushes.Black, listX + 45, rowY); // empty spot
                }
            }

            // 6. Draw status messages
            if (game.IsPaused)
                g.DrawString("PAUSED\nPress P to resume", font, Brushes.Black, textX, 380);

            if (game.IsGameOver)
                g.DrawString("GAME OVER\nPress R to restart", font, Brushes.Red, textX, 380);

            // 7. Draw how to play
            g.DrawString(controls, smallFont, Brushes.Black, textX, 500);
        }
    }
}