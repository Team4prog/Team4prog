using Team4prog.UI.Components;

namespace Team4prog.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            trackBarFrame = new TrackBar();
            btnOpenFolder = new Button();
            btnDelete = new Button();
            groupBoxData = new GroupBox();
            lblThrottle = new Label();
            lblAngle = new Label();
            lblFrame = new Label();
            listBoxLog = new ListBox();
            chartPanel = new Panel();
            btnPrev = new Button();
            btnNext = new Button();
            btnPlayForward = new Button();
            btnPlayBackward = new Button();
            btnStop = new Button();
            nudSpeed = new NumericUpDown();
            groupBoxPlayControls = new GroupBox();
            lblNext = new Label();
            lblPlayForward = new Label();
            lblStop = new Label();
            lblPlayBackward = new Label();
            lblPrev = new Label();
            txtTubNavigator = new Label();
            groupBox1 = new GroupBox();
            cmbThrottleOp = new ComboBox();
            cmbAngleOp = new ComboBox();
            lblRange = new Label();
            btnReload = new Button();
            btnRestore = new Button();
            btnDeleteRange = new Button();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            txtAngleFilter = new TextBox();
            txtThrottleFilter = new TextBox();
            btnSetFilter = new Button();
            btnClearFilter = new Button();
            listBoxFrames = new ListBox();
            panelTubManager = new Panel();
            innerPanel = new Panel();
            label1 = new Label();
            listBoxException = new ListBox();
            picFrame = new PictureBox();
            panelTrainer = new Panel();
            listBoxChartLoss = new ListBox();
            groupBoxPilotManager = new GroupBox();
            btnDeleteModel = new Button();
            cmbModelList = new ComboBox();
            chartLoss = new Panel();
            groupBoxTrainer = new GroupBox();
            btnSelectCarFolder = new Button();
            btnTrain = new Button();
            btnLoadModel = new Button();
            cmbModelType = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).BeginInit();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            groupBoxPlayControls.SuspendLayout();
            groupBox1.SuspendLayout();
            panelTubManager.SuspendLayout();
            innerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            panelTrainer.SuspendLayout();
            groupBoxPilotManager.SuspendLayout();
            groupBoxTrainer.SuspendLayout();
            SuspendLayout();
            // 
            // trackBarFrame
            // 
            trackBarFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarFrame.Location = new Point(322, 825);
            trackBarFrame.Margin = new Padding(2);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(1983, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.BackColor = Color.Silver;
            btnOpenFolder.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFolder.Location = new Point(9, 47);
            btnOpenFolder.Margin = new Padding(2);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(197, 62);
            btnOpenFolder.TabIndex = 2;
            btnOpenFolder.Text = "폴더 열기";
            btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(255, 128, 128);
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDelete.Location = new Point(221, 45);
            btnDelete.Margin = new Padding(2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 64);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // groupBoxData
            // 
            groupBoxData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxData.Controls.Add(lblThrottle);
            groupBoxData.Controls.Add(lblAngle);
            groupBoxData.Controls.Add(lblFrame);
            groupBoxData.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxData.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxData.Location = new Point(322, 740);
            groupBoxData.Margin = new Padding(2);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Padding = new Padding(2);
            groupBoxData.Size = new Size(1981, 81);
            groupBoxData.TabIndex = 4;
            groupBoxData.TabStop = false;
            // 
            // lblThrottle
            // 
            lblThrottle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(1648, 34);
            lblThrottle.Margin = new Padding(2, 0, 2, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(86, 32);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "가속값";
            // 
            // lblAngle
            // 
            lblAngle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(833, 34);
            lblAngle.Margin = new Padding(2, 0, 2, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(62, 32);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "각도";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Location = new Point(43, 34);
            lblFrame.Margin = new Padding(2, 0, 2, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(86, 32);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임";
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            listBoxLog.BackColor = Color.Silver;
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(9, 652);
            listBoxLog.Margin = new Padding(2);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(302, 412);
            listBoxLog.TabIndex = 6;
            // 
            // chartPanel
            // 
            chartPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartPanel.BackColor = Color.Black;
            chartPanel.Location = new Point(9, 1114);
            chartPanel.Margin = new Padding(2);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(2294, 318);
            chartPanel.TabIndex = 15;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.DimGray;
            btnPrev.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(454, 36);
            btnPrev.Margin = new Padding(2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(158, 56);
            btnPrev.TabIndex = 8;
            btnPrev.Text = "◁";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.DimGray;
            btnNext.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(1213, 36);
            btnNext.Margin = new Padding(2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(158, 56);
            btnNext.TabIndex = 9;
            btnNext.Text = "▷";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnPlayForward
            // 
            btnPlayForward.BackColor = Color.DimGray;
            btnPlayForward.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPlayForward.Location = new Point(1032, 36);
            btnPlayForward.Margin = new Padding(2);
            btnPlayForward.Name = "btnPlayForward";
            btnPlayForward.Size = new Size(158, 56);
            btnPlayForward.TabIndex = 11;
            btnPlayForward.Text = "▶▶";
            btnPlayForward.UseVisualStyleBackColor = false;
            // 
            // btnPlayBackward
            // 
            btnPlayBackward.BackColor = Color.DimGray;
            btnPlayBackward.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPlayBackward.Location = new Point(652, 36);
            btnPlayBackward.Margin = new Padding(2);
            btnPlayBackward.Name = "btnPlayBackward";
            btnPlayBackward.Size = new Size(158, 56);
            btnPlayBackward.TabIndex = 10;
            btnPlayBackward.Text = "◀◀";
            btnPlayBackward.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.DimGray;
            btnStop.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStop.Location = new Point(844, 36);
            btnStop.Margin = new Padding(2);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(158, 56);
            btnStop.TabIndex = 12;
            btnStop.Text = "■";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // nudSpeed
            // 
            nudSpeed.BackColor = Color.LightGray;
            nudSpeed.BorderStyle = BorderStyle.None;
            nudSpeed.DecimalPlaces = 2;
            nudSpeed.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeed.Location = new Point(1524, 53);
            nudSpeed.Margin = new Padding(2);
            nudSpeed.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudSpeed.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(129, 35);
            nudSpeed.TabIndex = 14;
            nudSpeed.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // groupBoxPlayControls
            // 
            groupBoxPlayControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxPlayControls.Controls.Add(lblNext);
            groupBoxPlayControls.Controls.Add(lblPlayForward);
            groupBoxPlayControls.Controls.Add(lblStop);
            groupBoxPlayControls.Controls.Add(lblPlayBackward);
            groupBoxPlayControls.Controls.Add(lblPrev);
            groupBoxPlayControls.Controls.Add(nudSpeed);
            groupBoxPlayControls.Controls.Add(btnStop);
            groupBoxPlayControls.Controls.Add(btnPlayForward);
            groupBoxPlayControls.Controls.Add(btnPlayBackward);
            groupBoxPlayControls.Controls.Add(btnNext);
            groupBoxPlayControls.Controls.Add(btnPrev);
            groupBoxPlayControls.Font = new Font("Microsoft Sans Serif", 13.9999981F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBoxPlayControls.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPlayControls.Location = new Point(322, 589);
            groupBoxPlayControls.Margin = new Padding(2);
            groupBoxPlayControls.Name = "groupBoxPlayControls";
            groupBoxPlayControls.Padding = new Padding(2);
            groupBoxPlayControls.Size = new Size(1975, 143);
            groupBoxPlayControls.TabIndex = 9;
            groupBoxPlayControls.TabStop = false;
            // 
            // lblNext
            // 
            lblNext.AutoSize = true;
            lblNext.Location = new Point(1251, 102);
            lblNext.Name = "lblNext";
            lblNext.Size = new Size(85, 32);
            lblNext.TabIndex = 19;
            lblNext.Text = "다음 장";
            // 
            // lblPlayForward
            // 
            lblPlayForward.AutoSize = true;
            lblPlayForward.Location = new Point(1078, 102);
            lblPlayForward.Name = "lblPlayForward";
            lblPlayForward.Size = new Size(56, 32);
            lblPlayForward.TabIndex = 18;
            lblPlayForward.Text = "재생";
            // 
            // lblStop
            // 
            lblStop.AutoSize = true;
            lblStop.Location = new Point(896, 102);
            lblStop.Name = "lblStop";
            lblStop.Size = new Size(56, 32);
            lblStop.TabIndex = 17;
            lblStop.Text = "정지";
            // 
            // lblPlayBackward
            // 
            lblPlayBackward.AutoSize = true;
            lblPlayBackward.Location = new Point(686, 102);
            lblPlayBackward.Name = "lblPlayBackward";
            lblPlayBackward.Size = new Size(77, 32);
            lblPlayBackward.TabIndex = 16;
            lblPlayBackward.Text = "역재생";
            // 
            // lblPrev
            // 
            lblPrev.AutoSize = true;
            lblPrev.Location = new Point(485, 102);
            lblPrev.Name = "lblPrev";
            lblPrev.Size = new Size(85, 32);
            lblPrev.TabIndex = 15;
            lblPrev.Text = "이전 장";
            // 
            // txtTubNavigator
            // 
            txtTubNavigator.BackColor = Color.FromArgb(64, 64, 64);
            txtTubNavigator.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTubNavigator.ForeColor = Color.White;
            txtTubNavigator.Location = new Point(9, 11);
            txtTubNavigator.Margin = new Padding(2, 0, 2, 0);
            txtTubNavigator.Name = "txtTubNavigator";
            txtTubNavigator.Size = new Size(222, 27);
            txtTubNavigator.TabIndex = 10;
            txtTubNavigator.Text = "주행 데이터 탐색기";
            txtTubNavigator.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(cmbThrottleOp);
            groupBox1.Controls.Add(cmbAngleOp);
            groupBox1.Controls.Add(lblRange);
            groupBox1.Controls.Add(btnReload);
            groupBox1.Controls.Add(btnRestore);
            groupBox1.Controls.Add(btnDeleteRange);
            groupBox1.Controls.Add(btnSetRight);
            groupBox1.Controls.Add(btnSetLeft);
            groupBox1.Controls.Add(txtAngleFilter);
            groupBox1.Controls.Add(txtThrottleFilter);
            groupBox1.Controls.Add(btnSetFilter);
            groupBox1.Controls.Add(btnClearFilter);
            groupBox1.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.FromArgb(192, 192, 255);
            groupBox1.Location = new Point(322, 886);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Size(1981, 183);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // cmbThrottleOp
            // 
            cmbThrottleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbThrottleOp.FormattingEnabled = true;
            cmbThrottleOp.Location = new Point(994, 105);
            cmbThrottleOp.Margin = new Padding(2);
            cmbThrottleOp.Name = "cmbThrottleOp";
            cmbThrottleOp.Size = new Size(124, 45);
            cmbThrottleOp.TabIndex = 23;
            // 
            // cmbAngleOp
            // 
            cmbAngleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbAngleOp.FormattingEnabled = true;
            cmbAngleOp.Location = new Point(271, 107);
            cmbAngleOp.Margin = new Padding(2);
            cmbAngleOp.Name = "cmbAngleOp";
            cmbAngleOp.Size = new Size(124, 45);
            cmbAngleOp.TabIndex = 22;
            // 
            // lblRange
            // 
            lblRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblRange.AutoSize = true;
            lblRange.Location = new Point(454, 40);
            lblRange.Margin = new Padding(2, 0, 2, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(95, 37);
            lblRange.TabIndex = 17;
            lblRange.Text = "[0, 0]";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnReload.BackColor = Color.DimGray;
            btnReload.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReload.Location = new Point(1600, 29);
            btnReload.Margin = new Padding(2);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(177, 66);
            btnReload.TabIndex = 16;
            btnReload.Text = "초기화";
            btnReload.UseVisualStyleBackColor = false;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnRestore.BackColor = Color.DimGray;
            btnRestore.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnRestore.Location = new Point(1375, 29);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(221, 66);
            btnRestore.TabIndex = 15;
            btnRestore.Text = "삭제값 되돌리기";
            btnRestore.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRange
            // 
            btnDeleteRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnDeleteRange.BackColor = Color.DimGray;
            btnDeleteRange.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteRange.Location = new Point(1194, 29);
            btnDeleteRange.Margin = new Padding(2);
            btnDeleteRange.Name = "btnDeleteRange";
            btnDeleteRange.Size = new Size(177, 66);
            btnDeleteRange.TabIndex = 14;
            btnDeleteRange.Text = "지정값 삭제";
            btnDeleteRange.UseVisualStyleBackColor = false;
            btnDeleteRange.Click += btnDeleteRange_Click;
            // 
            // btnSetRight
            // 
            btnSetRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetRight.BackColor = Color.DimGray;
            btnSetRight.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetRight.Location = new Point(77, 29);
            btnSetRight.Margin = new Padding(2);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(177, 66);
            btnSetRight.TabIndex = 13;
            btnSetRight.Text = "시작점 지정";
            btnSetRight.UseVisualStyleBackColor = false;
            btnSetRight.Click += btnSetRight_Click;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetLeft.BackColor = Color.DimGray;
            btnSetLeft.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetLeft.Location = new Point(258, 29);
            btnSetLeft.Margin = new Padding(2);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(177, 66);
            btnSetLeft.TabIndex = 12;
            btnSetLeft.Text = "끝점 지정";
            btnSetLeft.UseVisualStyleBackColor = false;
            btnSetLeft.Click += btnSetLeft_Click;
            // 
            // txtAngleFilter
            // 
            txtAngleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtAngleFilter.BackColor = Color.Gray;
            txtAngleFilter.Location = new Point(410, 106);
            txtAngleFilter.Margin = new Padding(2);
            txtAngleFilter.Name = "txtAngleFilter";
            txtAngleFilter.PlaceholderText = "각도";
            txtAngleFilter.Size = new Size(431, 44);
            txtAngleFilter.TabIndex = 19;
            // 
            // txtThrottleFilter
            // 
            txtThrottleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtThrottleFilter.BackColor = Color.Gray;
            txtThrottleFilter.Location = new Point(1138, 106);
            txtThrottleFilter.Margin = new Padding(2);
            txtThrottleFilter.Name = "txtThrottleFilter";
            txtThrottleFilter.PlaceholderText = "가속값";
            txtThrottleFilter.Size = new Size(431, 44);
            txtThrottleFilter.TabIndex = 20;
            // 
            // btnSetFilter
            // 
            btnSetFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetFilter.BackColor = Color.DimGray;
            btnSetFilter.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSetFilter.Location = new Point(77, 98);
            btnSetFilter.Margin = new Padding(2);
            btnSetFilter.Name = "btnSetFilter";
            btnSetFilter.Size = new Size(177, 66);
            btnSetFilter.TabIndex = 18;
            btnSetFilter.Text = "필터 지정";
            btnSetFilter.UseVisualStyleBackColor = false;
            btnSetFilter.Click += btnSetFilter_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnClearFilter.BackColor = Color.DimGray;
            btnClearFilter.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearFilter.Location = new Point(1600, 99);
            btnClearFilter.Margin = new Padding(2);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(177, 66);
            btnClearFilter.TabIndex = 21;
            btnClearFilter.Text = "텍스트박스 삭제";
            btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // listBoxFrames
            // 
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxFrames.BackColor = Color.Silver;
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.IntegralHeight = false;
            listBoxFrames.Location = new Point(7, 114);
            listBoxFrames.Margin = new Padding(0);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(304, 488);
            listBoxFrames.TabIndex = 5;
            // 
            // panelTubManager
            // 
            panelTubManager.AutoScroll = true;
            panelTubManager.Controls.Add(innerPanel);
            panelTubManager.Dock = DockStyle.Fill;
            panelTubManager.Location = new Point(0, 0);
            panelTubManager.Margin = new Padding(2);
            panelTubManager.Name = "panelTubManager";
            panelTubManager.Size = new Size(2328, 1500);
            panelTubManager.TabIndex = 17;
            // 
            // innerPanel
            // 
            innerPanel.Controls.Add(label1);
            innerPanel.Controls.Add(listBoxException);
            innerPanel.Controls.Add(groupBox1);
            innerPanel.Controls.Add(txtTubNavigator);
            innerPanel.Controls.Add(groupBoxData);
            innerPanel.Controls.Add(listBoxFrames);
            innerPanel.Controls.Add(chartPanel);
            innerPanel.Controls.Add(btnDelete);
            innerPanel.Controls.Add(btnOpenFolder);
            innerPanel.Controls.Add(trackBarFrame);
            innerPanel.Controls.Add(picFrame);
            innerPanel.Controls.Add(listBoxLog);
            innerPanel.Controls.Add(groupBoxPlayControls);
            innerPanel.Location = new Point(10, 45);
            innerPanel.Margin = new Padding(2);
            innerPanel.Name = "innerPanel";
            innerPanel.Size = new Size(2318, 1444);
            innerPanel.TabIndex = 24;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label1.ForeColor = Color.FromArgb(192, 192, 255);
            label1.Location = new Point(9, 615);
            label1.Name = "label1";
            label1.Size = new Size(174, 32);
            label1.TabIndex = 17;
            label1.Text = "삭제 로그 관리";
            // 
            // listBoxException
            // 
            listBoxException.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            listBoxException.BackColor = Color.Silver;
            listBoxException.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxException.FormattingEnabled = true;
            listBoxException.Location = new Point(2001, 11);
            listBoxException.Margin = new Padding(2);
            listBoxException.Name = "listBoxException";
            listBoxException.Size = new Size(302, 580);
            listBoxException.TabIndex = 16;
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(322, 11);
            picFrame.Margin = new Padding(2);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(1666, 581);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // panelTrainer
            // 
            panelTrainer.Controls.Add(listBoxChartLoss);
            panelTrainer.Controls.Add(groupBoxPilotManager);
            panelTrainer.Controls.Add(chartLoss);
            panelTrainer.Controls.Add(groupBoxTrainer);
            panelTrainer.Dock = DockStyle.Fill;
            panelTrainer.Location = new Point(0, 0);
            panelTrainer.Margin = new Padding(2);
            panelTrainer.Name = "panelTrainer";
            panelTrainer.Size = new Size(2328, 1500);
            panelTrainer.TabIndex = 18;
            // 
            // listBoxChartLoss
            // 
            listBoxChartLoss.BackColor = Color.Gray;
            listBoxChartLoss.Font = new Font("한컴 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            listBoxChartLoss.FormattingEnabled = true;
            listBoxChartLoss.Location = new Point(1559, 74);
            listBoxChartLoss.Name = "listBoxChartLoss";
            listBoxChartLoss.Size = new Size(748, 1330);
            listBoxChartLoss.TabIndex = 17;
            // 
            // groupBoxPilotManager
            // 
            groupBoxPilotManager.Anchor = AnchorStyles.Left;
            groupBoxPilotManager.Controls.Add(btnDeleteModel);
            groupBoxPilotManager.Controls.Add(cmbModelList);
            groupBoxPilotManager.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxPilotManager.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPilotManager.Location = new Point(10, 694);
            groupBoxPilotManager.Margin = new Padding(2);
            groupBoxPilotManager.Name = "groupBoxPilotManager";
            groupBoxPilotManager.Padding = new Padding(2);
            groupBoxPilotManager.Size = new Size(1521, 122);
            groupBoxPilotManager.TabIndex = 0;
            groupBoxPilotManager.TabStop = false;
            groupBoxPilotManager.Text = "모델 관리 기능";
            // 
            // btnDeleteModel
            // 
            btnDeleteModel.BackColor = Color.DimGray;
            btnDeleteModel.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteModel.Location = new Point(371, 39);
            btnDeleteModel.Margin = new Padding(2);
            btnDeleteModel.Name = "btnDeleteModel";
            btnDeleteModel.Size = new Size(178, 48);
            btnDeleteModel.TabIndex = 1;
            btnDeleteModel.Text = "모델 삭제";
            btnDeleteModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelList
            // 
            cmbModelList.BackColor = Color.DimGray;
            cmbModelList.FormattingEnabled = true;
            cmbModelList.Location = new Point(40, 41);
            cmbModelList.Margin = new Padding(2);
            cmbModelList.Name = "cmbModelList";
            cmbModelList.Size = new Size(299, 46);
            cmbModelList.TabIndex = 0;
            // 
            // chartLoss
            // 
            chartLoss.Anchor = AnchorStyles.Left;
            chartLoss.BackColor = Color.Black;
            chartLoss.Location = new Point(10, 300);
            chartLoss.Margin = new Padding(2);
            chartLoss.Name = "chartLoss";
            chartLoss.Size = new Size(1521, 330);
            chartLoss.TabIndex = 16;
            // 
            // groupBoxTrainer
            // 
            groupBoxTrainer.Anchor = AnchorStyles.Left;
            groupBoxTrainer.Controls.Add(btnSelectCarFolder);
            groupBoxTrainer.Controls.Add(btnTrain);
            groupBoxTrainer.Controls.Add(btnLoadModel);
            groupBoxTrainer.Controls.Add(cmbModelType);
            groupBoxTrainer.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxTrainer.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxTrainer.Location = new Point(12, 109);
            groupBoxTrainer.Margin = new Padding(2);
            groupBoxTrainer.Name = "groupBoxTrainer";
            groupBoxTrainer.Padding = new Padding(2);
            groupBoxTrainer.Size = new Size(1519, 145);
            groupBoxTrainer.TabIndex = 1;
            groupBoxTrainer.TabStop = false;
            groupBoxTrainer.Text = "트레이너";
            // 
            // btnSelectCarFolder
            // 
            btnSelectCarFolder.BackColor = Color.DimGray;
            btnSelectCarFolder.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSelectCarFolder.Location = new Point(21, 31);
            btnSelectCarFolder.Margin = new Padding(2);
            btnSelectCarFolder.Name = "btnSelectCarFolder";
            btnSelectCarFolder.Size = new Size(194, 50);
            btnSelectCarFolder.TabIndex = 20;
            btnSelectCarFolder.Text = "차량 폴더 선택";
            btnSelectCarFolder.UseVisualStyleBackColor = false;
            btnSelectCarFolder.Click += btnSelectCarFolder_Click;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(128, 255, 128);
            btnTrain.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnTrain.ForeColor = Color.Black;
            btnTrain.Location = new Point(805, 43);
            btnTrain.Margin = new Padding(2);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(505, 79);
            btnTrain.TabIndex = 2;
            btnTrain.Text = "학습";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnLoadModel
            // 
            btnLoadModel.BackColor = Color.DimGray;
            btnLoadModel.Font = new Font("한컴 고딕", 13.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoadModel.Location = new Point(21, 85);
            btnLoadModel.Margin = new Padding(2);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(526, 45);
            btnLoadModel.TabIndex = 1;
            btnLoadModel.Text = "모델 선택";
            btnLoadModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelType
            // 
            cmbModelType.BackColor = Color.DimGray;
            cmbModelType.FormattingEnabled = true;
            cmbModelType.Items.AddRange(new object[] { "linear", "categorical" });
            cmbModelType.Location = new Point(233, 31);
            cmbModelType.Margin = new Padding(2);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(314, 40);
            cmbModelType.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(2328, 1500);
            Controls.Add(panelTubManager);
            Controls.Add(panelTrainer);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "DonkeyCar UI";
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).EndInit();
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            groupBoxPlayControls.ResumeLayout(false);
            groupBoxPlayControls.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelTubManager.ResumeLayout(false);
            innerPanel.ResumeLayout(false);
            innerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            panelTrainer.ResumeLayout(false);
            groupBoxPilotManager.ResumeLayout(false);
            groupBoxTrainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TrackBar trackBarFrame;
        private Button btnOpenFolder;
        private GroupBox groupBoxData;
        private ListBox listBoxLog;
        private Label lblAngle;
        private Label lblFrame;
        private Label lblThrottle;
        private Button btnDelete;
        private Button btnPrev;
        private GroupBox groupBoxPlayControls;
        private Button btnStop;
        private Button btnPlayForward;
        private Button btnPlayBackward;
        private Button btnNext;
        private NumericUpDown nudSpeed;
        private Label txtTubNavigator;
        private GroupBox groupBox1;
        private Button btnDeleteRange;
        private Button btnSetRight;
        private Button btnSetLeft;
        private Label lblRange;
        private Button btnReload;
        private Button btnRestore;
        private Panel chartPanel;
        private Button btnClearFilter;
        private Button btnSetFilter;
        private TextBox txtThrottleFilter;
        private TextBox txtAngleFilter;
        private ComboBox cmbAngleOp;
        private ComboBox cmbThrottleOp;
        private AppNavigationBar topBar;
        private Button btnTubManager;
        private Button btnTrainer;
        private ListBox listBoxFrames;
        private Panel panelTubManager;
        private Panel panelTrainer;
        private GroupBox groupBoxTrainer;
        private ComboBox cmbModelType;
        private Button btnLoadModel;
        private Button btnTrain;
        private GroupBox groupBoxPilotManager;
        private Panel chartLoss;
        private Button btnDeleteModel;
        private ComboBox cmbModelList;
        private Button btnSelectCarFolder;
        private Panel innerPanel;
        private PictureBox picFrame;
        private Label lblPrev;
        private Label lblNext;
        private Label lblPlayForward;
        private Label lblStop;
        private Label lblPlayBackward;
        private ListBox listBoxChartLoss;
        private ListBox listBoxException;
        private Label label1;
    }
}
