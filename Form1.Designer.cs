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
            picFrame = new PictureBox();
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
            topBar = new AppNavigationBar();
            listBoxFrames = new ListBox();
            panelTubManager = new Panel();
            innerPanel = new Panel();
            panelTrainer = new Panel();
            groupBoxPilotManager = new GroupBox();
            btnDeleteModel = new Button();
            cmbModelList = new ComboBox();
            chartLoss = new Panel();
            groupBoxTrainer = new GroupBox();
            btnSelectCarFolder = new Button();
            btnTrain = new Button();
            btnLoadModel = new Button();
            cmbModelType = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).BeginInit();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            groupBoxPlayControls.SuspendLayout();
            groupBox1.SuspendLayout();
            panelTubManager.SuspendLayout();
            innerPanel.SuspendLayout();
            panelTrainer.SuspendLayout();
            groupBoxPilotManager.SuspendLayout();
            groupBoxTrainer.SuspendLayout();
            SuspendLayout();
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(242, 29);
            picFrame.Margin = new Padding(2);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(824, 260);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            trackBarFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarFrame.Location = new Point(242, 488);
            trackBarFrame.Margin = new Padding(2);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(1028, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.BackColor = Color.Silver;
            btnOpenFolder.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFolder.Location = new Point(9, 17);
            btnOpenFolder.Margin = new Padding(2);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(136, 41);
            btnOpenFolder.TabIndex = 2;
            btnOpenFolder.Text = "폴더 열기";
            btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(255, 128, 128);
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDelete.Location = new Point(147, 17);
            btnDelete.Margin = new Padding(2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 41);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // groupBoxData
            // 
            groupBoxData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxData.Controls.Add(lblThrottle);
            groupBoxData.Controls.Add(lblAngle);
            groupBoxData.Controls.Add(lblFrame);
            groupBoxData.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxData.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxData.Location = new Point(242, 397);
            groupBoxData.Margin = new Padding(2);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Padding = new Padding(2);
            groupBoxData.Size = new Size(1028, 87);
            groupBoxData.TabIndex = 4;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Driving Data";
            // 
            // lblThrottle
            // 
            lblThrottle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(694, 26);
            lblThrottle.Margin = new Padding(2, 0, 2, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(101, 32);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "Throttle";
            // 
            // lblAngle
            // 
            lblAngle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(358, 26);
            lblAngle.Margin = new Padding(2, 0, 2, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(77, 32);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "Angle";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Location = new Point(40, 26);
            lblFrame.Margin = new Padding(2, 0, 2, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(81, 32);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "Frame";
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            listBoxLog.BackColor = Color.Silver;
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(1070, 29);
            listBoxLog.Margin = new Padding(2);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(217, 364);
            listBoxLog.TabIndex = 6;
            // 
            // chartPanel
            // 
            chartPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartPanel.BackColor = Color.Black;
            chartPanel.Location = new Point(9, 736);
            chartPanel.Margin = new Padding(2);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(1265, 211);
            chartPanel.TabIndex = 15;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.DimGray;
            btnPrev.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPrev.Location = new Point(187, 36);
            btnPrev.Margin = new Padding(2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(81, 39);
            btnPrev.TabIndex = 8;
            btnPrev.Text = "◁";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.DimGray;
            btnNext.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnNext.Location = new Point(532, 36);
            btnNext.Margin = new Padding(2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(81, 39);
            btnNext.TabIndex = 9;
            btnNext.Text = "▷";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnPlayForward
            // 
            btnPlayForward.BackColor = Color.DimGray;
            btnPlayForward.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayForward.Location = new Point(447, 36);
            btnPlayForward.Margin = new Padding(2);
            btnPlayForward.Name = "btnPlayForward";
            btnPlayForward.Size = new Size(81, 39);
            btnPlayForward.TabIndex = 11;
            btnPlayForward.Text = "▶";
            btnPlayForward.UseVisualStyleBackColor = false;
            // 
            // btnPlayBackward
            // 
            btnPlayBackward.BackColor = Color.DimGray;
            btnPlayBackward.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayBackward.Location = new Point(272, 36);
            btnPlayBackward.Margin = new Padding(2);
            btnPlayBackward.Name = "btnPlayBackward";
            btnPlayBackward.Size = new Size(81, 39);
            btnPlayBackward.TabIndex = 10;
            btnPlayBackward.Text = "◀";
            btnPlayBackward.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.DimGray;
            btnStop.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStop.Location = new Point(357, 36);
            btnStop.Margin = new Padding(2);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(81, 39);
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
            nudSpeed.Location = new Point(679, 36);
            nudSpeed.Margin = new Padding(2);
            nudSpeed.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudSpeed.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(102, 35);
            nudSpeed.TabIndex = 14;
            nudSpeed.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // groupBoxPlayControls
            // 
            groupBoxPlayControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxPlayControls.Controls.Add(nudSpeed);
            groupBoxPlayControls.Controls.Add(btnStop);
            groupBoxPlayControls.Controls.Add(btnPlayForward);
            groupBoxPlayControls.Controls.Add(btnPlayBackward);
            groupBoxPlayControls.Controls.Add(btnNext);
            groupBoxPlayControls.Controls.Add(btnPrev);
            groupBoxPlayControls.Font = new Font("Microsoft Sans Serif", 13.9999981F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBoxPlayControls.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPlayControls.Location = new Point(246, 298);
            groupBoxPlayControls.Margin = new Padding(2);
            groupBoxPlayControls.Name = "groupBoxPlayControls";
            groupBoxPlayControls.Padding = new Padding(2);
            groupBoxPlayControls.Size = new Size(820, 95);
            groupBoxPlayControls.TabIndex = 9;
            groupBoxPlayControls.TabStop = false;
            groupBoxPlayControls.Text = "PlayControls";
            // 
            // txtTubNavigator
            // 
            txtTubNavigator.BackColor = Color.FromArgb(64, 64, 64);
            txtTubNavigator.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTubNavigator.ForeColor = Color.White;
            txtTubNavigator.Location = new Point(246, 0);
            txtTubNavigator.Margin = new Padding(2, 0, 2, 0);
            txtTubNavigator.Name = "txtTubNavigator";
            txtTubNavigator.Size = new Size(143, 27);
            txtTubNavigator.TabIndex = 10;
            txtTubNavigator.Text = "Tub Navigator";
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
            groupBox1.Font = new Font("Microsoft Sans Serif", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox1.ForeColor = Color.FromArgb(192, 192, 255);
            groupBox1.Location = new Point(241, 559);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Size(1028, 160);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tub Cleaner";
            // 
            // cmbThrottleOp
            // 
            cmbThrottleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbThrottleOp.FormattingEnabled = true;
            cmbThrottleOp.Location = new Point(522, 89);
            cmbThrottleOp.Margin = new Padding(2);
            cmbThrottleOp.Name = "cmbThrottleOp";
            cmbThrottleOp.Size = new Size(68, 37);
            cmbThrottleOp.TabIndex = 23;
            // 
            // cmbAngleOp
            // 
            cmbAngleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbAngleOp.FormattingEnabled = true;
            cmbAngleOp.Location = new Point(290, 89);
            cmbAngleOp.Margin = new Padding(2);
            cmbAngleOp.Name = "cmbAngleOp";
            cmbAngleOp.Size = new Size(68, 37);
            cmbAngleOp.TabIndex = 22;
            // 
            // lblRange
            // 
            lblRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblRange.AutoSize = true;
            lblRange.Location = new Point(384, 37);
            lblRange.Margin = new Padding(2, 0, 2, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(71, 29);
            lblRange.TabIndex = 17;
            lblRange.Text = "[0, 0]";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnReload.BackColor = Color.DimGray;
            btnReload.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReload.Location = new Point(757, 30);
            btnReload.Margin = new Padding(2);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(129, 48);
            btnReload.TabIndex = 16;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = false;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnRestore.BackColor = Color.DimGray;
            btnRestore.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRestore.Location = new Point(625, 30);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(128, 48);
            btnRestore.TabIndex = 15;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRange
            // 
            btnDeleteRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnDeleteRange.BackColor = Color.DimGray;
            btnDeleteRange.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDeleteRange.Location = new Point(495, 30);
            btnDeleteRange.Margin = new Padding(2);
            btnDeleteRange.Name = "btnDeleteRange";
            btnDeleteRange.Size = new Size(126, 48);
            btnDeleteRange.TabIndex = 14;
            btnDeleteRange.Text = "Delete";
            btnDeleteRange.UseVisualStyleBackColor = false;
            // 
            // btnSetRight
            // 
            btnSetRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetRight.BackColor = Color.DimGray;
            btnSetRight.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSetRight.Location = new Point(77, 30);
            btnSetRight.Margin = new Padding(2);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(142, 48);
            btnSetRight.TabIndex = 13;
            btnSetRight.Text = "Set Right";
            btnSetRight.UseVisualStyleBackColor = false;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetLeft.BackColor = Color.DimGray;
            btnSetLeft.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSetLeft.Location = new Point(223, 30);
            btnSetLeft.Margin = new Padding(2);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(135, 48);
            btnSetLeft.TabIndex = 12;
            btnSetLeft.Text = "Set left";
            btnSetLeft.UseVisualStyleBackColor = false;
            // 
            // txtAngleFilter
            // 
            txtAngleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtAngleFilter.BackColor = Color.Gray;
            txtAngleFilter.Location = new Point(362, 91);
            txtAngleFilter.Margin = new Padding(2);
            txtAngleFilter.Name = "txtAngleFilter";
            txtAngleFilter.PlaceholderText = "Angle";
            txtAngleFilter.Size = new Size(156, 35);
            txtAngleFilter.TabIndex = 19;
            // 
            // txtThrottleFilter
            // 
            txtThrottleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtThrottleFilter.BackColor = Color.Gray;
            txtThrottleFilter.Location = new Point(594, 89);
            txtThrottleFilter.Margin = new Padding(2);
            txtThrottleFilter.Name = "txtThrottleFilter";
            txtThrottleFilter.PlaceholderText = "Throttle";
            txtThrottleFilter.Size = new Size(159, 35);
            txtThrottleFilter.TabIndex = 20;
            // 
            // btnSetFilter
            // 
            btnSetFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetFilter.BackColor = Color.DimGray;
            btnSetFilter.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSetFilter.Location = new Point(148, 85);
            btnSetFilter.Margin = new Padding(2);
            btnSetFilter.Name = "btnSetFilter";
            btnSetFilter.Size = new Size(125, 45);
            btnSetFilter.TabIndex = 18;
            btnSetFilter.Text = "Set Filter";
            btnSetFilter.UseVisualStyleBackColor = false;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnClearFilter.BackColor = Color.DimGray;
            btnClearFilter.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearFilter.Location = new Point(757, 85);
            btnClearFilter.Margin = new Padding(2);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(129, 45);
            btnClearFilter.TabIndex = 21;
            btnClearFilter.Text = "Clear";
            btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // topBar
            // 
            topBar.Dock = DockStyle.Top;
            topBar.Enabled = false;
            topBar.Location = new Point(0, 0);
            topBar.Margin = new Padding(2);
            topBar.Name = "topBar";
            topBar.Size = new Size(1312, 41);
            topBar.TabIndex = 16;
            // 
            // listBoxFrames
            // 
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxFrames.BackColor = Color.Silver;
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.IntegralHeight = false;
            listBoxFrames.Location = new Point(7, 60);
            listBoxFrames.Margin = new Padding(0);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(230, 659);
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
            panelTubManager.Size = new Size(1312, 1006);
            panelTubManager.TabIndex = 17;
            // 
            // innerPanel
            // 
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
            innerPanel.Size = new Size(1289, 959);
            innerPanel.TabIndex = 24;
            // 
            // panelTrainer
            // 
            panelTrainer.Controls.Add(groupBoxPilotManager);
            panelTrainer.Controls.Add(chartLoss);
            panelTrainer.Controls.Add(groupBoxTrainer);
            panelTrainer.Dock = DockStyle.Fill;
            panelTrainer.Location = new Point(0, 0);
            panelTrainer.Margin = new Padding(2);
            panelTrainer.Name = "panelTrainer";
            panelTrainer.Size = new Size(1312, 1006);
            panelTrainer.TabIndex = 18;
            // 
            // groupBoxPilotManager
            // 
            groupBoxPilotManager.Controls.Add(btnDeleteModel);
            groupBoxPilotManager.Controls.Add(cmbModelList);
            groupBoxPilotManager.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxPilotManager.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPilotManager.Location = new Point(9, 625);
            groupBoxPilotManager.Margin = new Padding(2);
            groupBoxPilotManager.Name = "groupBoxPilotManager";
            groupBoxPilotManager.Padding = new Padding(2);
            groupBoxPilotManager.Size = new Size(898, 166);
            groupBoxPilotManager.TabIndex = 0;
            groupBoxPilotManager.TabStop = false;
            groupBoxPilotManager.Text = "Pilot Manager";
            // 
            // btnDeleteModel
            // 
            btnDeleteModel.BackColor = Color.DimGray;
            btnDeleteModel.Location = new Point(364, 41);
            btnDeleteModel.Margin = new Padding(2);
            btnDeleteModel.Name = "btnDeleteModel";
            btnDeleteModel.Size = new Size(178, 26);
            btnDeleteModel.TabIndex = 1;
            btnDeleteModel.Text = "Delete Model";
            btnDeleteModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelList
            // 
            cmbModelList.BackColor = Color.DimGray;
            cmbModelList.FormattingEnabled = true;
            cmbModelList.Location = new Point(40, 41);
            cmbModelList.Margin = new Padding(2);
            cmbModelList.Name = "cmbModelList";
            cmbModelList.Size = new Size(299, 33);
            cmbModelList.TabIndex = 0;
            // 
            // chartLoss
            // 
            chartLoss.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartLoss.BackColor = Color.Black;
            chartLoss.Location = new Point(8, 522);
            chartLoss.Margin = new Padding(2);
            chartLoss.Name = "chartLoss";
            chartLoss.Size = new Size(1293, 254);
            chartLoss.TabIndex = 16;
            // 
            // groupBoxTrainer
            // 
            groupBoxTrainer.Controls.Add(btnSelectCarFolder);
            groupBoxTrainer.Controls.Add(btnTrain);
            groupBoxTrainer.Controls.Add(btnLoadModel);
            groupBoxTrainer.Controls.Add(cmbModelType);
            groupBoxTrainer.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxTrainer.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxTrainer.Location = new Point(8, 206);
            groupBoxTrainer.Margin = new Padding(2);
            groupBoxTrainer.Name = "groupBoxTrainer";
            groupBoxTrainer.Padding = new Padding(2);
            groupBoxTrainer.Size = new Size(907, 106);
            groupBoxTrainer.TabIndex = 1;
            groupBoxTrainer.TabStop = false;
            groupBoxTrainer.Text = "Trainer";
            // 
            // btnSelectCarFolder
            // 
            btnSelectCarFolder.BackColor = Color.DimGray;
            btnSelectCarFolder.Location = new Point(21, 32);
            btnSelectCarFolder.Margin = new Padding(2);
            btnSelectCarFolder.Name = "btnSelectCarFolder";
            btnSelectCarFolder.Size = new Size(194, 28);
            btnSelectCarFolder.TabIndex = 20;
            btnSelectCarFolder.Text = "Select Car Folder";
            btnSelectCarFolder.UseVisualStyleBackColor = false;
            btnSelectCarFolder.Click += btnSelectCarFolder_Click;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(128, 255, 128);
            btnTrain.ForeColor = Color.Black;
            btnTrain.Location = new Point(482, 65);
            btnTrain.Margin = new Padding(2);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(407, 30);
            btnTrain.TabIndex = 2;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnLoadModel
            // 
            btnLoadModel.BackColor = Color.DimGray;
            btnLoadModel.Location = new Point(21, 65);
            btnLoadModel.Margin = new Padding(2);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(394, 30);
            btnLoadModel.TabIndex = 1;
            btnLoadModel.Text = "Choose Model";
            btnLoadModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelType
            // 
            cmbModelType.BackColor = Color.DimGray;
            cmbModelType.FormattingEnabled = true;
            cmbModelType.Items.AddRange(new object[] { "linear", "categorical" });
            cmbModelType.Location = new Point(233, 36);
            cmbModelType.Margin = new Padding(2);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(182, 33);
            cmbModelType.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1312, 1006);
            Controls.Add(topBar);
            Controls.Add(panelTubManager);
            Controls.Add(panelTrainer);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "DonkeyCar UI";
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).EndInit();
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            groupBoxPlayControls.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelTubManager.ResumeLayout(false);
            innerPanel.ResumeLayout(false);
            innerPanel.PerformLayout();
            panelTrainer.ResumeLayout(false);
            groupBoxPilotManager.ResumeLayout(false);
            groupBoxTrainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picFrame;
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
    }
}
