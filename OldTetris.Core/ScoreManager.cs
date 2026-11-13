namespace OldTetris.Core
{
    public class ScoreManager // Keeps track of points and cleared lines
    {
        public int Points { get; private set; }
        public int Lines { get; private set; }

        public int Level => Lines / 10 + 1; // level goes up every 10 lines

        // Points for clearing 1, 2, 3 or 4 lines at once (classic Tetris scoring)
        private static readonly int[] pointsPerClear = { 0, 100, 300, 500, 800 };

        public void AddClearedLines(int count)
        {
            if (count <= 0) return;

            Lines += count;
            Points += pointsPerClear[Math.Min(count, 4)];
        }
    }
}