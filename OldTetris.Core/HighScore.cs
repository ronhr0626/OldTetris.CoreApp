namespace OldTetris.Core
{
    // One line in the top 10 list
    public record ScoreEntry(string Name, int Points);

    public class HighScore // Remembers the top 10 scores with names, saved to a file
    {
        private const int maxEntries = 10;     // number of places on the list
        private const int maxNameLength = 12;  // max letters in a name
        private readonly string filePath;
        private readonly List<ScoreEntry> scores = new List<ScoreEntry>();

        public IReadOnlyList<ScoreEntry> Top => scores; // best first

        public HighScore(string filePath)
        {
            this.filePath = filePath;
            Load();
        }

        // True if the score is good enough for the top 10
        public bool Qualifies(int points)
        {
            if (points <= 0) return false;
            return scores.Count < maxEntries || points > scores[^1].Points;
        }

        // Adds a score to the list and saves it
        public void Submit(string name, int points)
        {
            if (!Qualifies(points)) return;
            if (name.Length > maxNameLength) name = name[..maxNameLength]; // never more than 12 letters

            scores.Add(new ScoreEntry(name, points));
            scores.Sort((a, b) => b.Points.CompareTo(a.Points)); // sort highest first

            if (scores.Count > maxEntries)
                scores.RemoveRange(maxEntries, scores.Count - maxEntries); // keep only the top 10

            Save();
        }

        // Reads the list from the file. Each line: Name;Points
        private void Load()
        {
            if (!File.Exists(filePath)) return;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split(';');

                if (parts.Length == 2 && int.TryParse(parts[1], out int points))
                    scores.Add(new ScoreEntry(parts[0], points));
                else if (int.TryParse(line, out int oldPoints))
                    scores.Add(new ScoreEntry("???", oldPoints)); // old file format without names
            }

            scores.Sort((a, b) => b.Points.CompareTo(a.Points));
        }

        // Writes the list to the file. Each line: Name;Points
        private void Save()
        {
            File.WriteAllLines(filePath, scores.Select(s => $"{s.Name};{s.Points}"));
        }
    }
}