using System.Drawing;
using System.Windows.Forms;

namespace Team4prog.UI.Components
{
    // Top navigation bar used by Form1 to switch between Tub Manager and Trainer panels.
    public class AppNavigationBar : UserControl
    {
        public Button TubManagerButton { get; }
        public Button TrainerButton { get; }
        public Button HelpButton { get; }

        public AppNavigationBar()
        {
            Dock = DockStyle.Top;
            Height = 35;
            Enabled = false;

            TubManagerButton = CreateButton("btnTubManager", "[Tub Manage]", new Point(10, -1), 9F, 0);
            TrainerButton = CreateButton("btnTrainer", "[Trainer]", new Point(140, 0), 10F, 1);
            HelpButton = new Button
            {
                Text = "?",
                Size = new Size(30, 30),
                BackColor = Color.FromArgb(192, 192, 255),
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Width - 40, 2),
                Name = "btnHelp",
                TabIndex = 2,
                UseVisualStyleBackColor = false
            };

            Controls.Add(HelpButton);
            Controls.Add(TrainerButton);
            Controls.Add(TubManagerButton);
        }

        private static Button CreateButton(string name, string text, Point location, float fontSize, int tabIndex)
        {
            return new Button
            {
                BackColor = Color.Silver,
                Font = new Font("Impact", fontSize, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = location,
                Name = name,
                Size = new Size(124, 36),
                TabIndex = tabIndex,
                Text = text,
                UseVisualStyleBackColor = false
            };
        }
    }
}
