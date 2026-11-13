namespace OldTetris.Core
{
    public class TetrisGame // Game rules: the falling piece, player moves and game over
    {
        private readonly Board board;

        private Tetromino current; // the falling piece
        private Tetromino next;    // the piece that comes after
        public Tetromino NextPiece => next; // so the renderer can show it
        private int pieceX;        // its column
        private int pieceY;        // its row

        public int Width => board.Width;
        public int Height => board.Height;
        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; private set; }

        private readonly ScoreManager score = new ScoreManager();
        public int Points => score.Points; // current score
        public int Lines => score.Lines;   // total cleared lines
        public int Level => score.Level;   // current level

        // The player can only move pieces when the game is running
        private bool CanAct => !IsGameOver && !IsPaused;

        public TetrisGame(int width, int height)
        {
            board = new Board(width, height);
            current = Tetromino.CreateRandom();
            next = Tetromino.CreateRandom();
            SpawnPiece();
        }

        // Color index of a cell, including the falling piece (0 = empty, 1-7 = piece)
        public int GetCell(int x, int y)
        {
            int r = y - pieceY;
            int c = x - pieceX;

            if (r >= 0 && r < current.Rows && c >= 0 && c < current.Cols && current.IsFilled(r, c))
                return current.Color;

            return board[x, y];
        }

        // Called by the timer: moves the piece down or locks it
        public void Tick()
        {
            if (!CanAct) return;

            if (board.CanPlace(current, pieceX, pieceY + 1))
            {
                pieceY++;
            }
            else
            {
                board.Lock(current, pieceX, pieceY);
                int cleared = board.ClearFullRows(); // remove full rows
                score.AddClearedLines(cleared);      // give points
                current = next;                      // the next piece starts falling
                next = Tetromino.CreateRandom();     // pick a new next piece
                SpawnPiece();
            }
        }

        // Pauses or resumes the game
        public void TogglePause()
        {
            if (!IsGameOver) IsPaused = !IsPaused;
        }

        public void MoveLeft() => TryMove(pieceX - 1, pieceY);
        public void MoveRight() => TryMove(pieceX + 1, pieceY);
        public void MoveDown() => TryMove(pieceX, pieceY + 1);

        public void Rotate()
        {
            if (!CanAct) return;

            Tetromino rotated = current.Rotated();
            if (board.CanPlace(rotated, pieceX, pieceY)) current = rotated;
        }

        public void HardDrop()
        {
            if (!CanAct) return;

            while (board.CanPlace(current, pieceX, pieceY + 1)) pieceY++;
        }

        // Moves the piece only if the new position is free
        private void TryMove(int newX, int newY)
        {
            if (!CanAct) return;

            if (board.CanPlace(current, newX, newY))
            {
                pieceX = newX;
                pieceY = newY;
            }
        }

        // Places a new piece at the top center
        private void SpawnPiece()
        {
            pieceX = Width / 2 - current.Cols / 2;
            pieceY = 0;

            if (!board.CanPlace(current, pieceX, pieceY))
                IsGameOver = true;
        }
    }
}