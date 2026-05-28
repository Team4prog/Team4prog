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

namespace Team4prog.UI
{
    // Main form shell.
    // Detailed feature handlers are split into partial files under Features/* to keep this file focused on shared state and wiring.
    public partial class Form1 : Form
    {
        private List<string> imagePaths = new List<string>();
        private List<string?> catalogImagePaths = new List<string?>();
        private List<double?> angles = new List<double?>();
        private List<double?> throttles = new List<double?>();
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
        private List<string?> deletedCatalogImagePaths = new List<string?>();
        private List<double> deletedAngles = new List<double>();
        private List<double> deletedThrottles = new List<double>();

        // Original indices are stored so deleted frames can be restored near their previous positions.
        private List<int> deletedIndices = new List<int>();

        // Last loaded image folder, reused by the Reload button.
        private string? currentFolder = null;

        // Filter backup lists. The active lists are replaced while a filter is applied.
        private List<string> originalImagePaths = new List<string>();
        private List<string?> originalCatalogImagePaths = new List<string?>();
        private List<double?> originalAngles = new List<double?>();
        private List<double?> originalThrottles = new List<double?>();

        private List<string> filteredImagePaths = new List<string>();
        private List<string?> filteredCatalogImagePaths = new List<string?>();
        private List<double?> filteredAngles = new List<double?>();
        private List<double?> filteredThrottles = new List<double?>();
        private string carFolderPath = "";
        private string? currentCatalogPath = null;

        public Form1()
        {
            InitializeComponent();
            InitializeLossChart();

            try
            {
                // Enable the navigation bar and connect it to the two main panels.
                if (this.Controls.Find("topBar", true).FirstOrDefault() is Control topBar)
                    topBar.Enabled = true;

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
            }
            catch (Exception ex)
            {
                AddLog($"UI 패널 초기화 오류: {ex.Message}");
            }

            ConfigureResponsiveLayout();

            chartPanel.MouseClick += ChartPanel_MouseClick;

            listBoxLog.HorizontalScrollbar = true;
            listBoxLog.ScrollAlwaysVisible = true;

            // Keep frames visible without distortion.
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;

            cmbAngleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });
            cmbThrottleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });

            cmbAngleOp.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbThrottleOp.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.Items.Clear();
            cmbModelType.Items.AddRange(new object[] { "linear", "inferred", "tensorrt_linear", "tflite_linear" });
            cmbAngleOp.SelectedIndex = -1;
            cmbThrottleOp.SelectedIndex = 0;
            if (cmbModelType.Items.Count > 0)
                cmbModelType.SelectedIndex = 0;
            txtThrottleFilter.Text = "0";

            // Validate filter inputs as the user types.
            txtAngleFilter.TextChanged += ValidateInput;
            txtThrottleFilter.TextChanged += ValidateInput;

            // Feature event wiring. Implementations live in the partial files under Features/*.
            btnOpenFolder.Click += btnOpenFolder_Click;
            btnDelete.Click += btnDelete_Click;
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
            btnLoadModel.Click += btnLoadModel_Click;
            btnDeleteModel.Click += btnDeleteModel_Click;
            btnUpdateComment.Click += btnUpdateComment_Click;
            nudSpeed.ValueChanged += nudSpeed_ValueChanged;
            listBoxFrames.SelectedIndexChanged += listBoxFrames_SelectedIndexChanged;
            trackBarFrame.Scroll += trackBarFrame_Scroll;

            InitializeChart();


            // 추가ㅏㅏㅏㅏㅏㅏㅏ
            InitializeLossChart();
            FixTrainerLayout();

            if (panelTrainer != null)
            {
                panelTrainer.Resize += (s, e) => FixTrainerLayout();
            }
            // End of additional

            lblRange.Text = "[0, 0]";

            timerPlayback = new System.Windows.Forms.Timer();
            timerPlayback.Interval = 100;
            timerPlayback.Tick += TimerPlayback_Tick;
        }

        private void ShowTubManager()
        {
            try
            {
                panelTubManager.Visible = true;
                panelTubManager.BringToFront();

                panelTrainer.Visible = false;
                topBar.BringToFront();
            }
            catch (Exception ex)
            {
                AddLog($"ShowTubManager 오류: {ex.Message}");
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
            }
            catch (Exception ex)
            {
                AddLog($"ShowTrainer 오류: {ex.Message}");
            }
        }

        // Adds a timestamped entry and keeps the latest log visible.
        private void AddLog(string message)
        {
            try
            {
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
    }
}
