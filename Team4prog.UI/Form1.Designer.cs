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
            groupBoxData = new GroupBox();
            lblThrottle = new Label();
            lblAngle = new Label();
            lblFrame = new Label();
            listBoxFrames = new ListBox();
            listBoxLog = new ListBox();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).BeginInit();
            groupBoxData.SuspendLayout();
            SuspendLayout();
            // 
            // picFrame
            // 
            picFrame.BackColor = Color.White;
            picFrame.Location = new Point(31, 85);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(835, 412);
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            trackBarFrame.Location = new Point(31, 614);
            trackBarFrame.Name = "trackBarFrame";
            trackBarFrame.Size = new Size(835, 69);
            trackBarFrame.TabIndex = 1;
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.BackColor = Color.FromArgb(255, 128, 128);
            btnOpenFolder.Font = new Font("한컴 고딕", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFolder.Location = new Point(31, 24);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(204, 55);
            btnOpenFolder.TabIndex = 2;
            btnOpenFolder.Text = "폴더 열기";
            btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // btnStartTraining
            // 
            btnStartTraining.BackColor = Color.FromArgb(255, 128, 128);
            btnStartTraining.Font = new Font("한컴 고딕", 16F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStartTraining.Location = new Point(31, 689);
            btnStartTraining.Name = "btnStartTraining";
            btnStartTraining.Size = new Size(204, 55);
            btnStartTraining.TabIndex = 3;
            btnStartTraining.Text = "학습 시작";
            btnStartTraining.UseVisualStyleBackColor = false;
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(lblThrottle);
            groupBoxData.Controls.Add(lblAngle);
            groupBoxData.Controls.Add(lblFrame);
            groupBoxData.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxData.Location = new Point(31, 503);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(835, 105);
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
            listBoxFrames.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBoxFrames.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFrames.FormattingEnabled = true;
            listBoxFrames.Location = new Point(872, 85);
            listBoxFrames.Name = "listBoxFrames";
            listBoxFrames.Size = new Size(306, 412);
            listBoxFrames.TabIndex = 5;
            // 
            // listBoxLog
            // 
            listBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBoxLog.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLog.FormattingEnabled = true;
            listBoxLog.Location = new Point(872, 503);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(306, 196);
            listBoxLog.TabIndex = 6;
            listBoxLog.SelectedIndexChanged += listBoxLog_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(1190, 756);
            Controls.Add(listBoxLog);
            Controls.Add(listBoxFrames);
            Controls.Add(groupBoxData);
            Controls.Add(btnStartTraining);
            Controls.Add(btnOpenFolder);
            Controls.Add(trackBarFrame);
            Controls.Add(picFrame);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarFrame).EndInit();
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
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
    }
}
