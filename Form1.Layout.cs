using System;
using System.Drawing;
using System.Windows.Forms;

namespace Team4prog.UI
{
    public partial class Form1
    {
        private const int LayoutMargin = 12;
        private const int TopBarHeight = 35;
        private const int LeftColumnWidth = 305;
        private const int PlayControlsWidth = 240;
        private const int Gap = 6;

        private void ConfigureResponsiveLayout()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(980, 720);

            topBar.Dock = DockStyle.Top;
            topBar.Height = TopBarHeight;
            topBar.BringToFront();

            panelTubManager.Dock = DockStyle.Fill;
            panelTubManager.AutoScroll = true;
            panelTubManager.Padding = new Padding(0);

            panelTrainer.Dock = DockStyle.Fill;
            panelTrainer.AutoScroll = true;
            panelTrainer.Padding = new Padding(0, TopBarHeight, 0, 0);

            innerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            innerPanel.MinimumSize = new Size(960, 660);

            ResetProblematicAnchors();
            ApplyResponsiveLayout();

            Resize += (_, _) => ApplyResponsiveLayout();
            panelTubManager.SizeChanged += (_, _) => ApplyResponsiveLayout();
            panelTrainer.SizeChanged += (_, _) => ApplyResponsiveLayout();
        }

        private void ResetProblematicAnchors()
        {
            btnOpenFolder.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxLog.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            groupBoxPlayControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxData.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            trackBarFrame.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            chartPanel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            foreach (Control control in groupBox1.Controls)
            {
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            foreach (Control control in groupBoxData.Controls)
            {
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            groupBoxConfigEditor.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            groupBoxTrainer.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            chartLoss.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            groupBoxPilotManager.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void ApplyResponsiveLayout()
        {
            if (panelTubManager == null || innerPanel == null)
                return;

            LayoutTubManager();
            LayoutTrainer();
        }

        private void LayoutTubManager()
        {
            int availableWidth = Math.Max(panelTubManager.ClientSize.Width, 980);
            int availableHeight = Math.Max(panelTubManager.ClientSize.Height, 720);
            int contentWidth = Math.Max(960, availableWidth);
            int contentHeight = Math.Max(660, availableHeight - TopBarHeight);

            innerPanel.Location = new Point(0, TopBarHeight);
            innerPanel.Size = new Size(contentWidth, contentHeight);

            int left = LayoutMargin;
            int mainLeft = left + LeftColumnWidth + Gap;
            int rightGap = LayoutMargin;
            int playLeft = contentWidth - rightGap - PlayControlsWidth;
            int mainRight = playLeft - Gap;
            int mainWidth = Math.Max(420, mainRight - mainLeft);
            int chartHeight = Math.Max(120, Math.Min(260, contentHeight / 4));
            int cleanerHeight = 171;
            int trackHeight = 69;
            int dataHeight = 105;

            txtTubNavigator.Location = new Point(left, 8);
            txtTubNavigator.Size = new Size(179, txtTubNavigator.Height);

            btnDelete.Location = new Point(left + LeftColumnWidth - btnDelete.Width, 8);

            groupBoxPlayControls.Location = new Point(playLeft, 8);
            groupBoxPlayControls.Size = new Size(PlayControlsWidth, 347);

            int topContent = 49;
            int chartTop = contentHeight - LayoutMargin - chartHeight;
            int bottomButtonTop = chartTop - Gap - btnOpenFolder.Height;
            int logTop = Math.Max(topContent + 120, bottomButtonTop - 292 - Gap);
            int frameListHeight = Math.Max(120, logTop - Gap - 80);

            listBoxFrames.Location = new Point(left, 80);
            listBoxFrames.Size = new Size(LeftColumnWidth, frameListHeight);

            listBoxLog.Location = new Point(left, logTop);
            listBoxLog.Size = new Size(LeftColumnWidth, Math.Max(120, bottomButtonTop - logTop - Gap));

            btnOpenFolder.Location = new Point(left + LeftColumnWidth - btnOpenFolder.Width, bottomButtonTop);

            int cleanerTop = chartTop - Gap - cleanerHeight;
            int trackTop = cleanerTop - Gap - trackHeight;
            int dataTop = trackTop - Gap - dataHeight;
            int pictureHeight = Math.Max(190, dataTop - Gap - topContent);

            picFrame.Location = new Point(mainLeft, topContent);
            picFrame.Size = new Size(mainWidth, pictureHeight);

            groupBoxData.Location = new Point(mainLeft, dataTop);
            groupBoxData.Size = new Size(mainWidth, dataHeight);
            LayoutDrivingDataLabels();

            trackBarFrame.Location = new Point(mainLeft, trackTop);
            trackBarFrame.Size = new Size(mainWidth, trackHeight);

            groupBox1.Location = new Point(mainLeft, cleanerTop);
            groupBox1.Size = new Size(mainWidth, cleanerHeight);

            chartPanel.Location = new Point(left, chartTop);
            chartPanel.Size = new Size(contentWidth - LayoutMargin * 2, chartHeight);
        }

        private void LayoutDrivingDataLabels()
        {
            int third = Math.Max(1, groupBoxData.ClientSize.Width / 3);
            lblFrame.Location = new Point(24, 35);
            lblAngle.Location = new Point(third + 24, 35);
            lblThrottle.Location = new Point(third * 2 + 24, 35);
        }

        private void LayoutTrainer()
        {
            if (panelTrainer == null)
                return;

            int contentWidth = Math.Max(panelTrainer.ClientSize.Width, 980);
            int width = contentWidth - LayoutMargin * 2;

            groupBoxConfigEditor.Location = new Point(LayoutMargin, TopBarHeight + 34);
            groupBoxConfigEditor.Size = new Size(width, 141);

            groupBoxTrainer.Location = new Point(LayoutMargin, groupBoxConfigEditor.Bottom + 64);
            groupBoxTrainer.Size = new Size(width, 141);

            int half = Math.Max(280, (groupBoxTrainer.ClientSize.Width - 84) / 2);
            btnSelectCarFolder.Location = new Point(28, 43);
            btnLoadModel.Location = new Point(28, 87);
            btnLoadModel.Size = new Size(half, 40);
            cmbModelType.Location = new Point(28 + half + 24, 48);
            cmbModelType.Size = new Size(Math.Min(241, half), 33);
            txtComment.Location = new Point(28 + half + 24, 87);
            txtComment.Size = new Size(Math.Max(220, groupBoxTrainer.ClientSize.Width - txtComment.Left - 28), 32);
            btnTrain.Location = new Point(txtComment.Left, 43);
            btnTrain.Size = new Size(txtComment.Width, 40);

            chartLoss.Location = new Point(LayoutMargin, groupBoxTrainer.Bottom + 55);
            chartLoss.Size = new Size(width, Math.Max(220, Math.Min(338, panelTrainer.ClientSize.Height / 3)));

            groupBoxPilotManager.Location = new Point(LayoutMargin, chartLoss.Bottom + 25);
            groupBoxPilotManager.Size = new Size(width, 221);
        }
    }
}
