namespace OldTetris
{
    // Only job: a small popup window that asks for the player's name
    public class NameDialog : Form
    {
        private const int maxNameLength = 12; // max letters in a name
        private readonly TextBox nameBox = new TextBox();

        private NameDialog(int points)
        {
            Text = "New high score!";
            FormBorderStyle = FormBorderStyle.FixedDialog;  // not resizable
            StartPosition = FormStartPosition.CenterParent; // open in the middle of the game window
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(300, 140);

            Label label = new Label
            {
                Text = $"You scored {points} points!\nEnter your name (max {maxNameLength} letters):",
                Location = new Point(20, 15),
                AutoSize = true
            };

            nameBox.Location = new Point(20, 65);
            nameBox.Width = 260;
            nameBox.MaxLength = maxNameLength; // can't type more than 12 letters

            Button okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK, // closes the dialog when clicked
                Location = new Point(200, 100)
            };

            Controls.Add(label);
            Controls.Add(nameBox);
            Controls.Add(okButton);
            AcceptButton = okButton; // Enter key = OK
        }

        // Shows the dialog and returns the name the player typed
        public static string Ask(IWin32Window owner, int points)
        {
            using NameDialog dialog = new NameDialog(points);
            dialog.ShowDialog(owner);

            string name = dialog.nameBox.Text.Trim().Replace(";", ""); // ";" is used in the save file
            return name.Length == 0 ? "Player" : name;
        }
    }
}