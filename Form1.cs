using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using Team4prog.UI.Components;

namespace Team4prog.UI
{
    // Main form shell.
    // Detailed feature handlers are split into partial files under Features/* to keep this file focused on shared state and wiring.
    public partial class Form1 : Form
    {
        private List<string> imagePaths = new List<string>();
        private List<double?> angles = new List<double?>();
        private List<double?> throttles = new List<double?>();
        private List<double?> pilotAngles = new List<double?>();
        private List<double?> pilotThrottles = new List<double?>();
        private string? selectedTrainedModelPath = null;
        private bool showTrainedModelOverlay = false;
        private int currentIndex = -1;
        private System.Windows.Forms.Timer timerPlayback;
        private bool isPlayingForward = false;
        private bool isPlayingBackward = false;
        private double playbackSpeed = 1.0;
        private bool isTrainingRunning = false;

        // Range deletion state for Tub Cleaner.
        private int leftIndex = -1;
        private int rightIndex = -1;
        private List<string> deletedImagePaths = new List<string>();
        private List<double> deletedAngles = new List<double>();
        private List<double> deletedThrottles = new List<double>();

        // Original indices are stored so deleted frames can be restored near their previous positions.
        private List<int> deletedIndices = new List<int>();

        // Last loaded image folder, reused by the Reload button.
        private string? currentFolder = null;

        // Filter backup lists. The active lists are replaced while a filter is applied.
        private List<string> originalImagePaths = new List<string>();
        private List<double?> originalAngles = new List<double?>();
        private List<double?> originalThrottles = new List<double?>();
        private List<double?> originalPilotAngles = new List<double?>();
        private List<double?> originalPilotThrottles = new List<double?>();

        private List<string> filteredImagePaths = new List<string>();
        private List<double?> filteredAngles = new List<double?>();
        private List<double?> filteredThrottles = new List<double?>();
        private List<double?> filteredPilotAngles = new List<double?>();
        private List<double?> filteredPilotThrottles = new List<double?>();
        private string carFolderPath = "";

        public Form1()
        {
            InitializeComponent();
            EnsureTopBar();


            try
            {
                // Enable the navigation bar and connect it to the two main panels.
                if (topBar is AppNavigationBar navBar)
                {
                    navBar.Enabled = true;
                    btnTubManager = navBar.TubManagerButton;
                    btnTrainer = navBar.TrainerButton;

                    navBar.HelpButton.Click += (s, e) =>
                    {
                        var help = new HelpForm();
                        help.TopMost = true;
                        help.Show();
                    };
                }

                if (btnTubManager != null)
                {
                    btnTubManager.Click += (s, e) => ShowTubManager();
                }

                if (btnTrainer != null)
                {
                    btnTrainer.Click += (s, e) => ShowTrainer();
                }

                if (panelTubManager != null)
                {
                    panelTubManager.Dock = DockStyle.Fill;
                    panelTubManager.Parent = this;
                    panelTubManager.Visible = true;
                }

                if (panelTrainer != null)
                {
                    panelTrainer.Dock = DockStyle.Fill;
                    panelTrainer.Parent = this;
                    panelTrainer.Visible = false;
                }

                topBar.BringToFront();
            }
            catch (Exception ex)
            {
                AddExceptionLog($"UI 패널 초기화 오류: {ex.Message}");
            }

            chartPanel.MouseClick += ChartPanel_MouseClick;

            listBoxLog.HorizontalScrollbar = true;
            listBoxLog.ScrollAlwaysVisible = true;

            listBoxChartLoss.HorizontalScrollbar = true;
            listBoxChartLoss.ScrollAlwaysVisible = true;
            listBoxLog.SelectionMode = SelectionMode.MultiExtended;
            listBoxFrames.SelectionMode = SelectionMode.MultiExtended;

            // Keep frames visible without distortion.
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;

            cmbAngleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });
            cmbThrottleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });

            cmbAngleOp.SelectedIndex = -1;
            cmbThrottleOp.SelectedIndex = -1;

            // Validate filter inputs as the user types.
            txtAngleFilter.TextChanged += ValidateInput;
            txtThrottleFilter.TextChanged += ValidateInput;

            // Feature event wiring. Implementations live in the partial files under Features/*.
            btnOpenFolder.Click += btnOpenFolder_Click;
            btnSetFilter.Click += btnSetFilter_Click;
            btnClearFilter.Click += btnClearFilter_Click;
            btnSetLeft.Click += btnSetLeft_Click;
            btnSetRight.Click += btnSetRight_Click;
            btnDeleteRange.Click += btnDeleteRange_Click;
            btnRestore.Click += btnRestore_Click;
            btnReload.Click += btnReload_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnPlayForward.Click += btnPlayForward_Click;
            btnPlayBackward.Click += btnPlayBackward_Click;
            btnStop.Click += btnStop_Click;
            Btn_showtrain.Click += Btn_showtrain_Click;
            nudSpeed.ValueChanged += nudSpeed_ValueChanged;
            listBoxFrames.SelectedIndexChanged += listBoxFrames_SelectedIndexChanged;
            trackBarFrame.Scroll += trackBarFrame_Scroll;

            this.MinimumSize = new Size(1000, 700);

            InitializeChart();

            InitializeLossChart();
            // FixTrainerLayout();

            ApplyResponsiveLayout();

            this.Resize += (s, e) => ApplyResponsiveLayout();

            if (panelTubManager != null)
            {
                panelTubManager.Resize += (s, e) => ApplyResponsiveLayout();
            }

            if (panelTrainer != null)
            {
                panelTrainer.Resize += (s, e) => ApplyResponsiveLayout();
            }

            lblRange.Text = "[0, 0]";

            timerPlayback = new System.Windows.Forms.Timer();
            timerPlayback.Interval = 100;
            timerPlayback.Tick += TimerPlayback_Tick;
        }

        private void EnsureTopBar()
        {
            if (topBar == null)
            {
                topBar = new AppNavigationBar
                {
                    Name = "topBar",
                    Dock = DockStyle.Top,
                    Height = 35,
                    Enabled = true
                };

                Controls.Add(topBar);
            }

            topBar.Visible = true;
            topBar.Enabled = true;
            topBar.BringToFront();
        }

        private void ShowTubManager()
        {
            try
            {
                panelTubManager.Visible = true;
                panelTubManager.BringToFront();

                panelTrainer.Visible = false;

                topBar.BringToFront();
                ApplyResponsiveLayout();
            }
            catch (Exception ex)
            {
                AddExceptionLog($"ShowTubManager 오류: {ex.Message}");
            }
        }

        private void ShowTrainer()
        {
            try
            {
                panelTrainer.Visible = true;
                panelTrainer.BringToFront();

                panelTubManager.Visible = false;

                topBar.BringToFront();
                ApplyResponsiveLayout();
                chartLoss.Invalidate();
            }
            catch (Exception ex)
            {
                AddExceptionLog($"ShowTrainer 오류: {ex.Message}");
            }
        }


        private void ApplyResponsiveLayout()
        {
            try
            {
                //ApplyTubManagerLayout();
                //FixTrainerLayout();
            }
            catch
            {
                // 레이아웃 조정 오류가 나도 UI 실행은 막지 않음.
            }
        }

        /*
        private void ApplyTubManagerLayout()
        {
            if (panelTubManager == null || innerPanel == null)
                return;

            int margin = 10;
            int gap = 8;

            int clientW = panelTubManager.ClientSize.Width;
            int clientH = panelTubManager.ClientSize.Height;

            if (clientW < 900 || clientH < 600)
                return;

            panelTubManager.AutoScroll = true;

            int topY = 42;
            int leftW = 260;
            int rightW = 260;
            int contentW = clientW - margin * 2;

            int centerX = margin + leftW + gap;
            int rightX = contentW - rightW;
            int centerW = rightX - centerX - gap;

            if (centerW < 420)
            {
                leftW = 230;
                rightW = 235;
                centerX = margin + leftW + gap;
                rightX = contentW - rightW;
                centerW = rightX - centerX - gap;
            }

            if (centerW < 360)
                centerW = 360;

            innerPanel.SuspendLayout();

            innerPanel.Location = new Point(0, 0);
            innerPanel.Width = Math.Max(clientW - 20, centerX + centerW + gap + rightW + margin);

            txtTubNavigator.Location = new Point(margin, topY);
            txtTubNavigator.Size = new Size(leftW - 105, 28);

            btnDelete.Location = new Point(margin + leftW - 100, topY - 2);
            btnDelete.Size = new Size(100, 32);

            listBoxFrames.Location = new Point(margin, topY + 38);
            listBoxFrames.Size = new Size(leftW, 270);

            listBoxLog.Location = new Point(margin, topY + 338);
            listBoxLog.Size = new Size(leftW, 250);

            btnOpenFolder.Location = new Point(margin + 5, topY + 600);
            btnOpenFolder.Size = new Size(leftW - 10, 48);

            picFrame.Location = new Point(centerX, topY);
            picFrame.Size = new Size(centerW, 300);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;

            groupBoxPlayControls.Location = new Point(centerX + centerW + gap, topY);
            groupBoxPlayControls.Size = new Size(rightW, 300);
            // ArrangePlayControls();

            int dataY = picFrame.Bottom + 28;
            groupBoxData.Location = new Point(centerX, dataY);
            groupBoxData.Size = new Size(centerW + gap + rightW, 82);

            lblFrame.Location = new Point(50, 32);
            lblAngle.Location = new Point(groupBoxData.Width / 3 + 30, 32);
            lblThrottle.Location = new Point(groupBoxData.Width * 2 / 3 + 20, 32);

            trackBarFrame.Location = new Point(centerX, groupBoxData.Bottom + 18);
            trackBarFrame.Size = new Size(centerW + gap + rightW, 55);

            groupBox1.Location = new Point(centerX, trackBarFrame.Bottom + 30);
            groupBox1.Size = new Size(centerW + gap + rightW, 145);

            //ArrangeTubCleanerControls();

            chartPanel.Location = new Point(centerX, groupBox1.Bottom + 20);
            chartPanel.Size = new Size(centerW + gap + rightW, Math.Max(210, clientH - chartPanel.Location.Y - 30));

            int bottom = chartPanel.Bottom + 30;
            innerPanel.Height = Math.Max(bottom, clientH + 20);

            chartPanel.Invalidate();

            innerPanel.ResumeLayout();
        } */

        /* 
        private void ArrangePlayControls()
        {
            if (groupBoxPlayControls == null)
                return;

            int w = groupBoxPlayControls.ClientSize.Width;
            int margin = 12;
            int gap = 8;

            int usableW = Math.Max(160, w - margin * 2);
            int halfW = Math.Max(70, (usableW - gap) / 2);

            nudSpeed.Location = new Point(Math.Max(margin + 80, w - 105), 40);
            nudSpeed.Size = new Size(90, 32);

            btnPrev.Location = new Point(margin, 95);
            btnPrev.Size = new Size(halfW, 42);

            btnNext.Location = new Point(margin + halfW + gap, 95);
            btnNext.Size = new Size(halfW, 42);

            btnPlayBackward.Location = new Point(margin, 145);
            btnPlayBackward.Size = new Size(halfW, 42);

            btnPlayForward.Location = new Point(margin + halfW + gap, 145);
            btnPlayForward.Size = new Size(halfW, 42);

            btnStop.Location = new Point(margin, 200);
            btnStop.Size = new Size(usableW, 45);
        } */

        /* private void ArrangeTubCleanerControls()
        {
            if (groupBox1 == null)
                return;

            int w = groupBox1.ClientSize.Width;
            int margin = 18;
            int gap = 10;

            int y1 = 38;
            int y2 = 92;

            int btnW = 115;
            int btnH = 40;

            btnSetLeft.Location = new Point(margin, y1);
            btnSetLeft.Size = new Size(btnW, btnH);

            btnSetRight.Location = new Point(btnSetLeft.Right + gap, y1);
            btnSetRight.Size = new Size(btnW, btnH);

            lblRange.Location = new Point(btnSetRight.Right + 25, y1 + 9);

            int rightStart = Math.Max(lblRange.Right + 40, w - (btnW * 3 + gap * 2 + margin));

            btnDeleteRange.Location = new Point(rightStart, y1);
            btnDeleteRange.Size = new Size(btnW, btnH);

            btnRestore.Location = new Point(btnDeleteRange.Right + gap, y1);
            btnRestore.Size = new Size(btnW, btnH);

            btnReload.Location = new Point(btnRestore.Right + gap, y1);
            btnReload.Size = new Size(btnW, btnH);

            btnSetFilter.Location = new Point(margin, y2);
            btnSetFilter.Size = new Size(btnW, btnH);

            cmbAngleOp.Location = new Point(btnSetFilter.Right + gap + 10, y2 + 4);
            cmbAngleOp.Size = new Size(75, 32);

            txtAngleFilter.Location = new Point(cmbAngleOp.Right + 6, y2 + 4);
            txtAngleFilter.Size = new Size(120, 32);

            cmbThrottleOp.Location = new Point(txtAngleFilter.Right + 18, y2 + 4);
            cmbThrottleOp.Size = new Size(75, 32);

            txtThrottleFilter.Location = new Point(cmbThrottleOp.Right + 6, y2 + 4);
            txtThrottleFilter.Size = new Size(135, 32);

            btnClearFilter.Location = new Point(Math.Min(txtThrottleFilter.Right + 18, w - btnW - margin), y2);
            btnClearFilter.Size = new Size(btnW, btnH);
        } */

        // Adds a timestamped entry and keeps the latest log visible.
        private void AddLog(string message)
        {
            try
            {
                // Show only successfully deleted frames here so listBoxLog selection maps to restore data.
                if (!message.Contains("[삭제 완료]"))
                    return;

                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                listBoxLog.Items.Add($"[{timestamp}] {message}");

                if (listBoxLog.Items.Count > 0)
                {
                    listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
                }
            }
            catch
            {
                // Logging should never interrupt the main UI flow.
            }
        }

        private void AddExceptionLog(string message)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                listBoxException.Items.Add($"[{timestamp}] {message}");

                if (listBoxException.Items.Count > 0)
                {
                    listBoxException.TopIndex = listBoxException.Items.Count - 1;
                }
            }
            catch
            {
                // 무시
            }
        }
    }
}
