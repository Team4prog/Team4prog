using System.Drawing;
using System.Windows.Forms;

namespace Team4prog.Components
{
    public class AppNavigationBar : UserControl
    {
        public Button TubManagerButton { get; }
        public Button TrainerButton { get; }
        public Button HelpButton { get; }

        public AppNavigationBar()
        {
            this.Name = "topBar";
            this.Dock = DockStyle.Top;
            this.Height = 40;

            TubManagerButton = new Button()
            {
                Text = "[Tub Manage]",
                Location = new Point(10, 5),
                Size = new Size(120, 30),
                BackColor = Color.Silver
            };

            TrainerButton = new Button()
            {
                Text = "[Trainer]",
                Location = new Point(140, 5),
                Size = new Size(120, 30),
                BackColor = Color.Silver
            };

            HelpButton = new Button()
            {
                Text = "?",
                Size = new Size(30, 30),
                BackColor = Color.FromArgb(192, 192, 255),
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            HelpButton.FlatAppearance.BorderSize = 0;
            HelpButton.Dock = DockStyle.Right;
            HelpButton.Width = 40;

            this.Controls.Add(TubManagerButton);
            this.Controls.Add(TrainerButton);
            this.Controls.Add(HelpButton);
        }
    }
}