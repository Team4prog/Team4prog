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
            btnStartTraining = new Button();
            btnDelete = new Button();
            groupBoxData = new GroupBox();
            lblThrottle = new Label();
            lblAngle = new Label();
            lblFrame = new Label();
            listBoxFrames = new ListBox();
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
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).BeginInit();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeed).BeginInit();
            groupBoxPlayControls.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(323, 91);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(636, 316);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            trackBarFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarFrame.Location = new Point(323, 524);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(882, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenFolder.BackColor = Color.Silver;
            btnOpenFolder.Font = new Font("한컴 고딕", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFolder.Location = new Point(965, 12);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(204, 55);
            btnOpenFolder.TabIndex = 2;
            btnOpenFolder.Text = "폴더 열기";
            btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // btnStartTraining
            // 
            btnStartTraining.BackColor = Color.Silver;
            btnStartTraining.Font = new Font("한컴 고딕", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStartTraining.Location = new Point(150, 696);
            btnStartTraining.Name = "btnStartTraining";
            btnStartTraining.Size = new Size(167, 55);
            btnStartTraining.TabIndex = 3;
            btnStartTraining.Text = "학습 시작";
            btnStartTraining.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(255, 128, 128);
            btnDelete.Font = new Font("한컴 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDelete.Location = new Point(197, 91);
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
            groupBoxData.Location = new Point(323, 413);
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
            // listBoxFrames
            // 
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxFrames.BackColor = Color.Silver;
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.Location = new Point(12, 141);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(305, 268);
            listBoxFrames.TabIndex = 5;
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxLog.BackColor = Color.Silver;
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(12, 422);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(305, 268);
            listBoxLog.TabIndex = 6;
            // 
            // chartPanel
            // 
            chartPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartPanel.BackColor = Color.Black;
            chartPanel.Location = new Point(12, 776);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(1193, 360);
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
            groupBoxPlayControls.Location = new Point(965, 91);
            groupBoxPlayControls.Name = "groupBoxPlayControls";
            groupBoxPlayControls.Size = new Size(240, 316);
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
            txtTubNavigator.Location = new Point(12, 91);
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
            groupBox1.Location = new Point(323, 590);
            groupBox1.Name = "groupBox1";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1231, 1148);
            Controls.Add(groupBox1);
            Controls.Add(txtTubNavigator);
            Controls.Add(listBoxFrames);
            Controls.Add(groupBoxData);
            Controls.Add(chartPanel);
            Controls.Add(btnStartTraining);
            Controls.Add(btnDelete);
            Controls.Add(btnOpenFolder);
            Controls.Add(trackBarFrame);
            Controls.Add(picFrame);
            Controls.Add(groupBoxPlayControls);
            Controls.Add(listBoxLog);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picFrame;
        private TrackBar trackBarFrame;
        private Button btnOpenFolder;
        private Button btnStartTraining;
        private GroupBox groupBoxData;
        private ListBox listBoxFrames;
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
    }
}
