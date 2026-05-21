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
            lblRange = new Label();
            btnReload = new Button();
            btnRestore = new Button();
            btnDeleteRange = new Button();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            btnSetFilter = new Button();
            txtAngleFilter = new TextBox();
            txtThrottleFilter = new TextBox();
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
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(323, 91);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(636, 316);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            trackBarFrame.Location = new Point(323, 524);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(882, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
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
            btnStartTraining.Location = new Point(92, 644);
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
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(438, 35);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(101, 32);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "Throttle";
            // 
            // lblAngle
            // 
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
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.Location = new Point(12, 141);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(305, 268);
            listBoxFrames.TabIndex = 5;
            // 
            // listBoxLog
            // 
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(12, 413);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(305, 196);
            listBoxLog.TabIndex = 6;
            listBoxLog.SelectedIndexChanged += listBoxLog_SelectedIndexChanged;
            // 
            // chartPanel
            // 
            chartPanel.BackColor = Color.Black;
            chartPanel.Location = new Point(12, 760);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(1193, 217);
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
            groupBoxPlayControls.Controls.Add(nudSpeed);
            groupBoxPlayControls.Controls.Add(btnStop);
            groupBoxPlayControls.Controls.Add(btnPlayForward);
            groupBoxPlayControls.Controls.Add(btnPlayBackward);
            groupBoxPlayControls.Controls.Add(btnNext);
            groupBoxPlayControls.Controls.Add(btnPrev);
            groupBoxPlayControls.Controls.Add(lblSpeed);
            groupBoxPlayControls.Font = new Font("함초롬바탕 확장", 13.9999981F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBoxPlayControls.ForeColor = Color.White;
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
            groupBox1.Controls.Add(txtThrottleFilter);
            groupBox1.Controls.Add(txtAngleFilter);
            groupBox1.Controls.Add(btnSetFilter);
            groupBox1.Controls.Add(lblRange);
            groupBox1.Controls.Add(btnReload);
            groupBox1.Controls.Add(btnRestore);
            groupBox1.Controls.Add(btnDeleteRange);
            groupBox1.Controls.Add(btnSetRight);
            groupBox1.Controls.Add(btnSetLeft);
            // filter controls
            groupBox1.Controls.Add(txtAngleFilter);
            groupBox1.Controls.Add(txtThrottleFilter);
            groupBox1.Controls.Add(btnSetFilter);
            groupBox1.Controls.Add(btnClearFilter);
            groupBox1.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox1.ForeColor = Color.FromArgb(192, 192, 255);
            groupBox1.Location = new Point(323, 583);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(882, 171);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tub Cleaner";
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Location = new Point(335, 60);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(77, 31);
            lblRange.TabIndex = 17;
            lblRange.Text = "[0, 0]";
            // 
            // txtAngleFilter
            // 
            txtAngleFilter = new TextBox();
            txtAngleFilter.Location = new Point(22, 18);
            txtAngleFilter.Name = "txtAngleFilter";
            txtAngleFilter.Size = new Size(180, 30);
            txtAngleFilter.TabIndex = 18;
            txtAngleFilter.PlaceholderText = "Angle condition (e.g. > 0.2)";
            // 
            // txtThrottleFilter
            // 
            txtThrottleFilter = new TextBox();
            txtThrottleFilter.Location = new Point(210, 18);
            txtThrottleFilter.Name = "txtThrottleFilter";
            txtThrottleFilter.Size = new Size(180, 30);
            txtThrottleFilter.TabIndex = 19;
            txtThrottleFilter.PlaceholderText = "Throttle condition (e.g. < 0.5)";
            // 
            // btnSetFilter
            // 
            btnSetFilter = new Button();
            btnSetFilter.Location = new Point(452, 14);
            btnSetFilter.Name = "btnSetFilter";
            btnSetFilter.Size = new Size(132, 36);
            btnSetFilter.TabIndex = 20;
            btnSetFilter.Text = "Set Filter";
            btnSetFilter.UseVisualStyleBackColor = false;
            btnSetFilter.BackColor = Color.DimGray;
            // 
            // btnClearFilter
            // 
            btnClearFilter = new Button();
            btnClearFilter.Location = new Point(590, 14);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(132, 36);
            btnClearFilter.TabIndex = 21;
            btnClearFilter.Text = "Clear";
            btnClearFilter.UseVisualStyleBackColor = false;
            btnClearFilter.BackColor = Color.DimGray;
            // 
            // btnReload
            // 
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
            btnSetLeft.BackColor = Color.DimGray;
            btnSetLeft.Location = new Point(22, 54);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(137, 46);
            btnSetLeft.TabIndex = 12;
            btnSetLeft.Text = "Set left";
            btnSetLeft.UseVisualStyleBackColor = false;
            // 
            // btnSetFilter
            // 
            btnSetFilter.BackColor = Color.DimGray;
            btnSetFilter.Location = new Point(22, 106);
            btnSetFilter.Name = "btnSetFilter";
            btnSetFilter.Size = new Size(137, 46);
            btnSetFilter.TabIndex = 18;
            btnSetFilter.Text = "Set Filter";
            btnSetFilter.UseVisualStyleBackColor = false;
            // 
            // txtAngleFilter
            // 
            txtAngleFilter.BackColor = Color.Gray;
            txtAngleFilter.Location = new Point(178, 113);
            txtAngleFilter.Name = "txtAngleFilter";
            txtAngleFilter.Size = new Size(341, 39);
            txtAngleFilter.TabIndex = 19;
            // 
            // txtThrottleFilter
            // 
            txtThrottleFilter.BackColor = Color.Gray;
            txtThrottleFilter.Location = new Point(525, 113);
            txtThrottleFilter.Name = "txtThrottleFilter";
            txtThrottleFilter.Size = new Size(351, 39);
            txtThrottleFilter.TabIndex = 20;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1231, 989);
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
    }
}
