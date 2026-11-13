namespace OldTetris.Core
{
    public class Board // The game board: stores locked pieces and checks collisions
    {
        public int Width { get; }
        public int Height { get; }

        private readonly int[,] grid;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new int[width, height];
        }

        // Color index of a locked cell (0 = empty)
        public int this[int x, int y] => grid[x, y];

        // Returns true if the piece fits at (px, py) without hitting walls or blocks
        public bool CanPlace(Tetromino piece, int px, int py)
        {
            for (int r = 0; r < piece.Rows; r++)
            {
                for (int c = 0; c < piece.Cols; c++)
                {
                    if (!piece.IsFilled(r, c)) continue;

                    int x = px + c;
                    int y = py + r;

                    if (x < 0 || x >= Width || y >= Height) return false;
                    if (y >= 0 && grid[x, y] != 0) return false;
                }
            }
            return true;
        }

        // Copies the piece into the grid so it stays there
        public void Lock(Tetromino piece, int px, int py)
        {
            for (int r = 0; r < piece.Rows; r++)
                for (int c = 0; c < piece.Cols; c++)
                    if (piece.IsFilled(r, c))
                        grid[px + c, py + r] = piece.Color;
        }

        // Removes all full rows and moves the rows above down. Returns how many rows were cleared.
        public int ClearFullRows()
        {
            int cleared = 0;

            for (int y = Height - 1; y >= 0; y--)
            {
                if (IsRowFull(y))
                {
                    // Move every row above this one down by one
                    for (int row = y; row > 0; row--)
                        for (int x = 0; x < Width; x++)
                            grid[x, row] = grid[x, row - 1];

                    // Empty the top row
                    for (int x = 0; x < Width; x++)
                        grid[x, 0] = 0;

                    cleared++;
                    y++; // check the same row again, since a new row moved into it
                }
            }

            return cleared;
        }

        // Returns true if every cell in the row is filled
        private bool IsRowFull(int y)
        {
            for (int x = 0; x < Width; x++)
                if (grid[x, y] == 0) return false;

            return true;
        }
    }
}