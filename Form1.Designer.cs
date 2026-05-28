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
            lblSpeed = new Label();
            nudSpeed = new NumericUpDown();
            groupBoxPlayControls = new GroupBox();
            txtTubNavigator = new TextBox();
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
            btnTrainer = topBar.TrainerButton;
            btnTubManager = topBar.TubManagerButton;
            listBoxFrames = new ListBox();
            panelTubManager = new Panel();
            panelTrainer = new Panel();
            groupBoxPilotManager = new GroupBox();
            btnUpdateComment = new Button();
            btnDeleteModel = new Button();
            cmbModelList = new ComboBox();
            chartLoss = new Panel();
            groupBoxTrainer = new GroupBox();
            btnSelectCarFolder = new Button();
            txtComment = new TextBox();
            btnTrain = new Button();
            btnLoadModel = new Button();
            cmbModelType = new ComboBox();
            groupBoxConfigEditor = new GroupBox();
            flowconfig = new FlowLayoutPanel();
            btnAddConfig = new Button();
            btnSaveConfig = new Button();
            innerPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).BeginInit();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            groupBoxPlayControls.SuspendLayout();
            groupBox1.SuspendLayout();
            panelTubManager.SuspendLayout();
            panelTrainer.SuspendLayout();
            groupBoxPilotManager.SuspendLayout();
            groupBoxTrainer.SuspendLayout();
            groupBoxConfigEditor.SuspendLayout();
            flowconfig.SuspendLayout();
            innerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(323, 39);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(636, 347);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            trackBarFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarFrame.Location = new Point(321, 534);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(882, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.BackColor = Color.Silver;
            btnOpenFolder.Font = new Font("한컴 고딕", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFolder.Location = new Point(111, 736);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(204, 55);
            btnOpenFolder.TabIndex = 2;
            btnOpenFolder.Text = "폴더 열기";
            btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(255, 128, 128);
            btnDelete.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDelete.Location = new Point(197, 39);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 35);
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
            groupBoxData.Location = new Point(323, 407);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(882, 105);
            groupBoxData.TabIndex = 4;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Driving Data";
            // 
            // lblThrottle
            // 
            lblThrottle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(438, 35);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(101, 32);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "Throttle";
            // 
            // lblAngle
            // 
            lblAngle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(233, 35);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(77, 32);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "Angle";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Location = new Point(53, 35);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(81, 32);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "Frame";
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxLog.BackColor = Color.Silver;
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(12, 428);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(305, 292);
            listBoxLog.TabIndex = 6;
            // 
            // chartPanel
            // 
            chartPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartPanel.BackColor = Color.Black;
            chartPanel.Location = new Point(12, 814);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(1199, 281);
            chartPanel.TabIndex = 15;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.DimGray;
            btnPrev.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPrev.Location = new Point(12, 119);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(108, 52);
            btnPrev.TabIndex = 8;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.DimGray;
            btnNext.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnNext.Location = new Point(126, 119);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(108, 52);
            btnNext.TabIndex = 9;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnPlayForward
            // 
            btnPlayForward.BackColor = Color.DimGray;
            btnPlayForward.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayForward.Location = new Point(126, 177);
            btnPlayForward.Name = "btnPlayForward";
            btnPlayForward.Size = new Size(108, 52);
            btnPlayForward.TabIndex = 11;
            btnPlayForward.Text = ">>";
            btnPlayForward.UseVisualStyleBackColor = false;
            // 
            // btnPlayBackward
            // 
            btnPlayBackward.BackColor = Color.DimGray;
            btnPlayBackward.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayBackward.Location = new Point(12, 177);
            btnPlayBackward.Name = "btnPlayBackward";
            btnPlayBackward.Size = new Size(108, 52);
            btnPlayBackward.TabIndex = 10;
            btnPlayBackward.Text = "<<";
            btnPlayBackward.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.DimGray;
            btnStop.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStop.Location = new Point(12, 235);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(222, 52);
            btnStop.TabIndex = 12;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(12, 62);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(95, 36);
            lblSpeed.TabIndex = 13;
            lblSpeed.Text = "1.00x";
            // 
            // nudSpeed
            // 
            nudSpeed.DecimalPlaces = 2;
            nudSpeed.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeed.Location = new Point(126, 54);
            nudSpeed.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            nudSpeed.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            nudSpeed.Name = "nudSpeed";
            nudSpeed.Size = new Size(108, 44);
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
            groupBoxPlayControls.Controls.Add(lblSpeed);
            groupBoxPlayControls.Font = new Font("함초롬바탕 확장", 13.9999981F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBoxPlayControls.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPlayControls.Location = new Point(971, 39);
            groupBoxPlayControls.Name = "groupBoxPlayControls";
            groupBoxPlayControls.Size = new Size(240, 347);
            groupBoxPlayControls.TabIndex = 9;
            groupBoxPlayControls.TabStop = false;
            groupBoxPlayControls.Text = "PlayControls";
            // 
            // txtTubNavigator
            // 
            txtTubNavigator.BackColor = Color.FromArgb(64, 64, 64);
            txtTubNavigator.BorderStyle = BorderStyle.None;
            txtTubNavigator.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTubNavigator.ForeColor = Color.White;
            txtTubNavigator.Location = new Point(12, 39);
            txtTubNavigator.Name = "txtTubNavigator";
            txtTubNavigator.Size = new Size(179, 27);
            txtTubNavigator.TabIndex = 10;
            txtTubNavigator.Text = "Tub Navigator";
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
            groupBox1.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox1.ForeColor = Color.FromArgb(192, 192, 255);
            groupBox1.Location = new Point(321, 620);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Size(882, 171);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tub Cleaner";
            // 
            // cmbThrottleOp
            // 
            cmbThrottleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbThrottleOp.FormattingEnabled = true;
            cmbThrottleOp.Location = new Point(421, 118);
            cmbThrottleOp.Name = "cmbThrottleOp";
            cmbThrottleOp.Size = new Size(89, 39);
            cmbThrottleOp.TabIndex = 23;
            // 
            // cmbAngleOp
            // 
            cmbAngleOp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            cmbAngleOp.FormattingEnabled = true;
            cmbAngleOp.Location = new Point(178, 118);
            cmbAngleOp.Name = "cmbAngleOp";
            cmbAngleOp.Size = new Size(89, 39);
            cmbAngleOp.TabIndex = 22;
            // 
            // lblRange
            // 
            lblRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblRange.AutoSize = true;
            lblRange.Location = new Point(329, 60);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(77, 31);
            lblRange.TabIndex = 17;
            lblRange.Text = "[0, 0]";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnReload.BackColor = Color.DimGray;
            btnReload.Location = new Point(732, 51);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(136, 49);
            btnReload.TabIndex = 16;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = false;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnRestore.BackColor = Color.DimGray;
            btnRestore.Location = new Point(590, 51);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(136, 49);
            btnRestore.TabIndex = 15;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRange
            // 
            btnDeleteRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnDeleteRange.BackColor = Color.DimGray;
            btnDeleteRange.Location = new Point(452, 51);
            btnDeleteRange.Name = "btnDeleteRange";
            btnDeleteRange.Size = new Size(132, 49);
            btnDeleteRange.TabIndex = 14;
            btnDeleteRange.Text = "Delete";
            btnDeleteRange.UseVisualStyleBackColor = false;
            // 
            // btnSetRight
            // 
            btnSetRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetRight.BackColor = Color.DimGray;
            btnSetRight.Location = new Point(178, 54);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(132, 46);
            btnSetRight.TabIndex = 13;
            btnSetRight.Text = "Set Right";
            btnSetRight.UseVisualStyleBackColor = false;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetLeft.BackColor = Color.DimGray;
            btnSetLeft.Location = new Point(22, 54);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(137, 46);
            btnSetLeft.TabIndex = 12;
            btnSetLeft.Text = "Set left";
            btnSetLeft.UseVisualStyleBackColor = false;
            // 
            // txtAngleFilter
            // 
            txtAngleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtAngleFilter.BackColor = Color.Gray;
            txtAngleFilter.Location = new Point(273, 118);
            txtAngleFilter.Name = "txtAngleFilter";
            txtAngleFilter.PlaceholderText = "Angle";
            txtAngleFilter.Size = new Size(133, 39);
            txtAngleFilter.TabIndex = 19;
            // 
            // txtThrottleFilter
            // 
            txtThrottleFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtThrottleFilter.BackColor = Color.Gray;
            txtThrottleFilter.Location = new Point(516, 118);
            txtThrottleFilter.Name = "txtThrottleFilter";
            txtThrottleFilter.PlaceholderText = "Throttle";
            txtThrottleFilter.Size = new Size(159, 39);
            txtThrottleFilter.TabIndex = 20;
            // 
            // btnSetFilter
            // 
            btnSetFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSetFilter.BackColor = Color.DimGray;
            btnSetFilter.Location = new Point(22, 113);
            btnSetFilter.Name = "btnSetFilter";
            btnSetFilter.Size = new Size(137, 46);
            btnSetFilter.TabIndex = 18;
            btnSetFilter.Text = "Set Filter";
            btnSetFilter.UseVisualStyleBackColor = false;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnClearFilter.BackColor = Color.DimGray;
            btnClearFilter.Location = new Point(690, 113);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(136, 44);
            btnClearFilter.TabIndex = 21;
            btnClearFilter.Text = "Clear";
            btnClearFilter.UseVisualStyleBackColor = false;
            // 
            // topBar
            // 
            topBar.Location = new Point(0, 0);
            topBar.Name = "topBar";
            topBar.Size = new Size(1231, 35);
            topBar.TabIndex = 16;
            // 
            // listBoxFrames
            // 
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxFrames.BackColor = Color.Silver;
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.IntegralHeight = false;
            listBoxFrames.Location = new Point(12, 80);
            listBoxFrames.Margin = new Padding(0);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(305, 316);
            listBoxFrames.TabIndex = 5;
            // 
            // panelTubManager
            // 
            panelTubManager.AutoScroll = true;
            panelTubManager.Controls.Add(innerPanel);
            panelTubManager.Dock = DockStyle.Fill;
            panelTubManager.Location = new Point(0, 0);
            panelTubManager.Name = "panelTubManager";
            panelTubManager.Size = new Size(1231, 1114);
            panelTubManager.TabIndex = 17;
            // 
            // panelTrainer
            // 
            panelTrainer.Controls.Add(groupBoxPilotManager);
            panelTrainer.Controls.Add(chartLoss);
            panelTrainer.Controls.Add(groupBoxTrainer);
            panelTrainer.Controls.Add(groupBoxConfigEditor);
            panelTrainer.Dock = DockStyle.Fill;
            panelTrainer.Location = new Point(0, 0);
            panelTrainer.Name = "panelTrainer";
            panelTrainer.Size = new Size(1231, 1114);
            panelTrainer.TabIndex = 18;
            // 
            // groupBoxPilotManager
            // 
            groupBoxPilotManager.Controls.Add(btnUpdateComment);
            groupBoxPilotManager.Controls.Add(btnDeleteModel);
            groupBoxPilotManager.Controls.Add(cmbModelList);
            groupBoxPilotManager.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxPilotManager.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxPilotManager.Location = new Point(12, 833);
            groupBoxPilotManager.Name = "groupBoxPilotManager";
            groupBoxPilotManager.Size = new Size(1198, 221);
            groupBoxPilotManager.TabIndex = 0;
            groupBoxPilotManager.TabStop = false;
            groupBoxPilotManager.Text = "Pilot Manager";
            // 
            // btnUpdateComment
            // 
            btnUpdateComment.BackColor = Color.DimGray;
            btnUpdateComment.Location = new Point(729, 53);
            btnUpdateComment.Name = "btnUpdateComment";
            btnUpdateComment.Size = new Size(267, 34);
            btnUpdateComment.TabIndex = 2;
            btnUpdateComment.Text = "UpdateComment";
            btnUpdateComment.UseVisualStyleBackColor = false;
            // 
            // btnDeleteModel
            // 
            btnDeleteModel.BackColor = Color.DimGray;
            btnDeleteModel.Location = new Point(486, 55);
            btnDeleteModel.Name = "btnDeleteModel";
            btnDeleteModel.Size = new Size(237, 34);
            btnDeleteModel.TabIndex = 1;
            btnDeleteModel.Text = "Delete Model";
            btnDeleteModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelList
            // 
            cmbModelList.BackColor = Color.DimGray;
            cmbModelList.FormattingEnabled = true;
            cmbModelList.Location = new Point(54, 55);
            cmbModelList.Name = "cmbModelList";
            cmbModelList.Size = new Size(397, 33);
            cmbModelList.TabIndex = 0;
            // 
            // chartLoss
            // 
            chartLoss.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartLoss.BackColor = Color.Black;
            chartLoss.Location = new Point(10, 470);
            chartLoss.Name = "chartLoss";
            chartLoss.Size = new Size(1205, 338);
            chartLoss.TabIndex = 16;
            // 
            // groupBoxTrainer
            // 
            groupBoxTrainer.Controls.Add(btnSelectCarFolder);
            groupBoxTrainer.Controls.Add(txtComment);
            groupBoxTrainer.Controls.Add(btnTrain);
            groupBoxTrainer.Controls.Add(btnLoadModel);
            groupBoxTrainer.Controls.Add(cmbModelType);
            groupBoxTrainer.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxTrainer.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxTrainer.Location = new Point(10, 274);
            groupBoxTrainer.Name = "groupBoxTrainer";
            groupBoxTrainer.Size = new Size(1209, 141);
            groupBoxTrainer.TabIndex = 1;
            groupBoxTrainer.TabStop = false;
            groupBoxTrainer.Text = "Trainer";
            // 
            // btnSelectCarFolder
            // 
            btnSelectCarFolder.BackColor = Color.DimGray;
            btnSelectCarFolder.Location = new Point(28, 43);
            btnSelectCarFolder.Name = "btnSelectCarFolder";
            btnSelectCarFolder.Size = new Size(259, 37);
            btnSelectCarFolder.TabIndex = 20;
            btnSelectCarFolder.Text = "Select Car Folder";
            btnSelectCarFolder.UseVisualStyleBackColor = false;
            btnSelectCarFolder.Click += btnSelectCarFolder_Click;
            // 
            // txtComment
            // 
            txtComment.BackColor = Color.Snow;
            txtComment.Location = new Point(642, 48);
            txtComment.Name = "txtComment";
            txtComment.PlaceholderText = "Comment";
            txtComment.Size = new Size(539, 32);
            txtComment.TabIndex = 19;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(128, 255, 128);
            btnTrain.ForeColor = Color.Black;
            btnTrain.Location = new Point(642, 87);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(543, 40);
            btnTrain.TabIndex = 2;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnLoadModel
            // 
            btnLoadModel.BackColor = Color.DimGray;
            btnLoadModel.Location = new Point(28, 87);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(525, 40);
            btnLoadModel.TabIndex = 1;
            btnLoadModel.Text = "Choose Model";
            btnLoadModel.UseVisualStyleBackColor = false;
            // 
            // cmbModelType
            // 
            cmbModelType.BackColor = Color.DimGray;
            cmbModelType.FormattingEnabled = true;
            cmbModelType.Items.AddRange(new object[] { "linear", "categorical" });
            cmbModelType.Location = new Point(311, 48);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(241, 33);
            cmbModelType.TabIndex = 0;
            // 
            // groupBoxConfigEditor
            // 
            groupBoxConfigEditor.Controls.Add(flowconfig);
            groupBoxConfigEditor.Font = new Font("Verdana", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxConfigEditor.ForeColor = Color.FromArgb(192, 192, 255);
            groupBoxConfigEditor.Location = new Point(10, 69);
            groupBoxConfigEditor.Margin = new Padding(0);
            groupBoxConfigEditor.Name = "groupBoxConfigEditor";
            groupBoxConfigEditor.Padding = new Padding(6);
            groupBoxConfigEditor.Size = new Size(1214, 141);
            groupBoxConfigEditor.TabIndex = 0;
            groupBoxConfigEditor.TabStop = false;
            groupBoxConfigEditor.Text = "Config Editor";
            // 
            // flowconfig
            // 
            flowconfig.AutoScroll = true;
            flowconfig.Controls.Add(btnAddConfig);
            flowconfig.Controls.Add(btnSaveConfig);
            flowconfig.Dock = DockStyle.Fill;
            flowconfig.Location = new Point(6, 31);
            flowconfig.Name = "flowconfig";
            flowconfig.Size = new Size(1202, 104);
            flowconfig.TabIndex = 1;
            // 
            // btnAddConfig
            // 
            btnAddConfig.BackColor = Color.DimGray;
            btnAddConfig.Location = new Point(3, 3);
            btnAddConfig.Name = "btnAddConfig";
            btnAddConfig.Size = new Size(58, 49);
            btnAddConfig.TabIndex = 0;
            btnAddConfig.Text = "+";
            btnAddConfig.UseVisualStyleBackColor = false;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.BackColor = Color.DimGray;
            btnSaveConfig.Location = new Point(67, 3);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(254, 49);
            btnSaveConfig.TabIndex = 1;
            btnSaveConfig.Text = "Save myconfig";
            btnSaveConfig.UseVisualStyleBackColor = false;
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
            innerPanel.Location = new Point(0, 0);
            innerPanel.Name = "innerPanel";
            innerPanel.Size = new Size(1231, 1111);
            innerPanel.TabIndex = 24;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1231, 1114);
            Controls.Add(topBar);
            Controls.Add(panelTubManager);
            Controls.Add(panelTrainer);
            Name = "Form1";
            Text = "DonkeyCar UI";
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).EndInit();
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).EndInit();
            groupBoxPlayControls.ResumeLayout(false);
            groupBoxPlayControls.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelTubManager.ResumeLayout(false);
            panelTrainer.ResumeLayout(false);
            groupBoxPilotManager.ResumeLayout(false);
            groupBoxTrainer.ResumeLayout(false);
            groupBoxTrainer.PerformLayout();
            groupBoxConfigEditor.ResumeLayout(false);
            flowconfig.ResumeLayout(false);
            innerPanel.ResumeLayout(false);
            innerPanel.PerformLayout();
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
        private Label lblSpeed;
        private NumericUpDown nudSpeed;
        private TextBox txtTubNavigator;
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
        private GroupBox groupBoxConfigEditor;
        private FlowLayoutPanel flowconfig;
        private Button btnAddConfig;
        private GroupBox groupBoxTrainer;
        private ComboBox cmbModelType;
        private Button btnSaveConfig;
        private Button btnLoadModel;
        private Button btnTrain;
        private TextBox txtComment;
        private GroupBox groupBoxPilotManager;
        private Panel chartLoss;
        private Button btnUpdateComment;
        private Button btnDeleteModel;
        private ComboBox cmbModelList;
        private Button btnSelectCarFolder;
        private Panel innerPanel;
    }
}
