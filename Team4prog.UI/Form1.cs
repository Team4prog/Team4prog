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
    public partial class Form1 : Form
    {
        private List<string> imagePaths = new List<string>();
        private List<double?> angles = new List<double?>();
        private List<double?> throttles = new List<double?>();
        private int currentIndex = -1;
        private System.Windows.Forms.Timer timerPlayback;
        private bool isPlayingForward = false;
        private bool isPlayingBackward = false;
        private double playbackSpeed = 1.0;
        // Tub cleaner variables
        private int leftIndex = -1;
        private int rightIndex = -1;
        private List<string> deletedImagePaths = new List<string>();
        private List<double> deletedAngles = new List<double>();
        private List<double> deletedThrottles = new List<double>();
        // Keep original indices for restore
        private List<int> deletedIndices = new List<int>();
        // remember last loaded folder for Reload
        private string? currentFolder = null;
        // Filtering
        private List<string> originalImagePaths = new List<string>();
        private List<double?> originalAngles = new List<double?>();
        private List<double?> originalThrottles = new List<double?>();

        private List<string> filteredImagePaths = new List<string>();
        private List<double?> filteredAngles = new List<double?>();
        private List<double?> filteredThrottles = new List<double?>();
        private string carFolderPath = "";
        // (panel fields are defined in Designer; will reference designer controls directly)

        public Form1()
        {
            InitializeComponent(); // 폼 초기화

            // UI 패널 초기화 및 이벤트 연결
            try
            {
                // Try to enable topBar if exists
                if (this.Controls.Find("topBar", true).FirstOrDefault() is Control topBar)
                    topBar.Enabled = true;

                if (btnTubManager != null)
                {
                    // avoid accidentally adding duplicate handlers (best-effort)
                    btnTubManager.Click += (s, e) => ShowTubManager();
                }

                if (btnTrainer != null)
                {
                    btnTrainer.Click += (s, e) => ShowTrainer(); // 탭 버튼 이벤트 연결
                }

                // Set initial visibility and docking for panels
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
                    panelTrainer.Visible = false; // 초기에는 Tub Manager 패널만 보이도록 설정
                }
            }
            catch (Exception ex)
            {
                AddLog($"UI 패널 초기화 오류: {ex.Message}");
            }

            chartPanel.MouseClick += ChartPanel_MouseClick; // 차트 클릭 이벤트 연결

            listBoxLog.HorizontalScrollbar = true; //   로그 리스트박스 가로 스크롤 허용
            listBoxLog.ScrollAlwaysVisible = true; // 로그 리스트박스 스크롤바 항상 표시

            // 이미지가 PictureBox 크기에 맞게 보이도록 설정
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;

            //combobox 연산자 추가
            cmbAngleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });
            cmbThrottleOp.Items.AddRange(new string[] { ">", "<", ">=", "<=", "==" });

            cmbAngleOp.SelectedIndex = -1;
            cmbThrottleOp.SelectedIndex = -1;

            // 입력 검증 이벤트 연결
            txtAngleFilter.TextChanged += ValidateInput;
            txtThrottleFilter.TextChanged += ValidateInput;

            // 이벤트 핸들러 연결
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
            nudSpeed.ValueChanged += nudSpeed_ValueChanged;
            listBoxFrames.SelectedIndexChanged += listBoxFrames_SelectedIndexChanged;
            trackBarFrame.Scroll += trackBarFrame_Scroll;

            // 폼 크기 고정
            this.MinimumSize = this.Size;

            // Chart 초기화
            InitializeChart();

            // lblRange 기본값
            lblRange.Text = "[0, 0]";

            // Timer 초기화
            timerPlayback = new System.Windows.Forms.Timer();
            timerPlayback.Interval = 100; // 기본 100ms
            timerPlayback.Tick += TimerPlayback_Tick;

            // Log 초기화

        }

        private void ShowTubManager()
        {
            try
            {
                panelTubManager.Visible = true;
                panelTubManager.BringToFront();

                panelTrainer.Visible = false;
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
            }
            catch (Exception ex)
            {
                AddLog($"ShowTrainer 오류: {ex.Message}");
            }
        }




        private void btnSetFilter_Click(object? sender, EventArgs e)
        {
            try
            {
                ApplyFilter();
            }
            catch (Exception ex)
            {
                AddLog($"필터 적용 오류: {ex.Message}");
            }
        }

        private void btnClearFilter_Click(object? sender, EventArgs e)
        {
            try
            {
                ResetFilter();
            }
            catch (Exception ex)
            {
                AddLog($"필터 해제 오류: {ex.Message}");
            }
        }


        private void ApplyFilter()
        {
            try
            {
                bool useAngle = double.TryParse(txtAngleFilter.Text, out double angleVal);
                bool useThrottle = double.TryParse(txtThrottleFilter.Text, out double throttleVal);

                // 범위 제한
                if (useAngle && (angleVal < -1.0 || angleVal > 1.0))
                {
                    MessageBox.Show("Angle 값은 -1.0 ~ 1.0 사이여야 합니다");
                    return;
                }

                if (useThrottle && (throttleVal < -1.0 || throttleVal > 1.0))
                {
                    MessageBox.Show("Throttle 값은 -1.0 ~ 1.0 사이여야 합니다");
                    return;
                }

                if (originalImagePaths.Count == 0)
                {
                    originalImagePaths = new List<string>(imagePaths);
                    originalAngles = new List<double?>(angles);
                    originalThrottles = new List<double?>(throttles);
                }

                filteredImagePaths.Clear();
                filteredAngles.Clear();
                filteredThrottles.Clear();

                for (int i = 0; i < originalAngles.Count; i++)
                {
                    bool ok = true;

                    if (useAngle)
                    {
                        if (!originalAngles[i].HasValue)
                            ok = false;
                        else
                            ok = Compare(originalAngles[i].Value, cmbAngleOp.Text, angleVal);
                    }

                    if (ok && useThrottle)
                    {
                        if (!originalThrottles[i].HasValue)
                            ok = false;
                        else
                            ok = Compare(originalThrottles[i].Value, cmbThrottleOp.Text, throttleVal);
                    }

                    if (ok)
                    {
                        filteredImagePaths.Add(originalImagePaths[i]);
                        filteredAngles.Add(originalAngles[i]);
                        filteredThrottles.Add(originalThrottles[i]);
                    }
                }

                imagePaths = new List<string>(filteredImagePaths);
                angles = new List<double?>(filteredAngles);
                throttles = new List<double?>(filteredThrottles);

                RebuildListBoxFrames();
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                if (imagePaths.Count > 0)
                {
                    listBoxFrames.SelectedIndex = 0;
                    trackBarFrame.Value = 0;
                    ShowImage(0);
                }
                else
                {
                    ClearImageDisplay();
                }

                AddLog($"[필터 결과] {originalImagePaths.Count} → {imagePaths.Count}");
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"필터 오류: {ex.Message}");
            }
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb == null) return;

            // 빈 값이면 기본색
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.BackColor = SystemColors.Window;
                return;
            }

            // 숫자 검사
            if (double.TryParse(tb.Text, out double value))
            {
                if (value < -1.0 || value > 1.0)
                {
                    tb.BackColor = Color.LightCoral; // 빨간색
                }
                else
                {
                    tb.BackColor = SystemColors.Window; // 정상
                }
            }
            else
            {
                tb.BackColor = Color.LightCoral; // 숫자 아님
            }
        }

        private bool Compare(double value, string op, double target)
        {
            switch (op)
            {
                case ">": return value > target;
                case "<": return value < target;
                case ">=": return value >= target;
                case "<=": return value <= target;
                case "==": return Math.Abs(value - target) < 0.0001;
                default: return true;
            }
        }

        private void ResetFilter()
        {
            if (originalImagePaths.Count == 0)
            {
                AddLog("원본 데이터가 없습니다");
                return;
            }

            imagePaths = new List<string>(originalImagePaths);
            angles = new List<double?>(originalAngles);
            throttles = new List<double?>(originalThrottles);

            originalImagePaths.Clear();
            originalAngles.Clear();
            originalThrottles.Clear();

            RebuildListBoxFrames();
            trackBarFrame.Minimum = 0;
            trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);
            if (imagePaths.Count > 0)
            {
                listBoxFrames.SelectedIndex = 0;
                trackBarFrame.Value = 0;
                ShowImage(0);
            }
            AddLog("[필터 해제]");
            UpdateChart();
        }

        // Chart 초기화 (패널 기반 간단 드로잉)
        private void InitializeChart()
        {
            try
            {
                chartPanel.Paint += ChartPanel_Paint;
                // prepare cache
                cachedChartBitmap = null;
                lastHighlightedIndex = -1;
            }
            catch (Exception ex)
            {
                AddLog($"차트 초기화 오류: {ex.Message}");
            }
        }

        private Bitmap? cachedChartBitmap = null;
        private int lastHighlightedIndex = -1;

        private void UpdateChart()
        {
            try
            {
                // rebuild cached bitmap for static parts (axes, lines, labels)
                int w = Math.Max(10, chartPanel.Width);
                int h = Math.Max(10, chartPanel.Height);
                var bmp = new Bitmap(w, h);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(chartPanel.BackColor);

                    int margin = 40;
                    int plotW = Math.Max(10, w - margin * 2);
                    int plotH = Math.Max(10, h - margin * 2);

                    // background for plot area
                    using (var brush = new SolidBrush(Color.FromArgb(30, 30, 30)))
                    {
                        g.FillRectangle(brush, margin, margin, plotW, plotH);
                    }

                    // axes
                    using (var pen = new Pen(Color.White))
                    {
                        g.DrawLine(pen, margin, margin + plotH, margin + plotW, margin + plotH); // x axis
                        g.DrawLine(pen, margin, margin, margin, margin + plotH); // y axis
                    }

                    int n = Math.Max(1, Math.Max(angles.Count, throttles.Count));

                    if (n > 0)
                    {
                        // compute points
                        PointF[] ptsA = new PointF[n];
                        PointF[] ptsT = new PointF[n];
                        for (int i = 0; i < n; i++)
                        {
                            float x = margin + (n == 1 ? plotW / 2f : (float)i * (plotW - 1) / (n - 1));
                            double a = (i < angles.Count && angles[i].HasValue) ? angles[i].Value : 0.0;
                            double t = (i < throttles.Count && throttles[i].HasValue) ? throttles[i].Value : 0.0;
                            float yA = margin + plotH / 2f - (float)(a * (plotH / 2f));
                            float yT = margin + plotH / 2f - (float)(t * (plotH / 2f));
                            ptsA[i] = new PointF(x, yA);
                            ptsT[i] = new PointF(x, yT);
                        }

                        // draw lines
                        if (n > 1)
                        {
                            using (var penA = new Pen(Color.Blue, 2))
                                g.DrawLines(penA, ptsA);
                            using (var penT = new Pen(Color.Yellow, 2))
                                g.DrawLines(penT, ptsT);
                        }
                        else
                        {
                            // single sample: draw tiny horizontal ticks (no marker) for performance
                            using (var penA = new Pen(Color.Blue, 2))
                                g.DrawLine(penA, ptsA[0].X - 2, ptsA[0].Y, ptsA[0].X + 2, ptsA[0].Y);
                            using (var penT = new Pen(Color.Yellow, 2))
                                g.DrawLine(penT, ptsT[0].X - 2, ptsT[0].Y, ptsT[0].X + 2, ptsT[0].Y);
                        }

                        // draw zero baseline
                        using (var zeroPen = new Pen(Color.WhiteSmoke, 1))
                        {
                            float y0 = margin + plotH / 2f;
                            zeroPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            g.DrawLine(zeroPen, margin, y0, margin + plotW, y0);
                        }

                        // X-axis labels
                        using (var font = new Font("Segoe UI", 8))
                        using (var brush = new SolidBrush(Color.White))
                        {
                            int approx = Math.Max(1, n / 10);
                            for (int i = 0; i < n; i += approx)
                            {
                                float x = margin + (n == 1 ? plotW / 2f : (float)i * (plotW - 1) / (n - 1));
                                var text = i.ToString();
                                var sz = g.MeasureString(text, font);
                                g.DrawString(text, font, brush, x - sz.Width / 2f, margin + plotH + 2);
                            }
                        }
                    }
                }

                // swap cache
                var old = cachedChartBitmap;
                cachedChartBitmap = bmp;
                if (old != null) old.Dispose();

                // reset highlighted index to force full repaint
                lastHighlightedIndex = -1;
                chartPanel.Invalidate();
                AddLog("[그래프 업데이트 완료]");
            }
            catch (Exception ex)
            {
                AddLog($"그래프 업데이트 오류: {ex.Message}");
            }
        }

        private void HighlightCurrentIndex(int index)
        {
            try
            {
                if (cachedChartBitmap == null)
                {
                    chartPanel.Invalidate();
                    return;
                }

                int n = Math.Max(1, Math.Max(angles.Count, throttles.Count));
                if (index < 0 || index >= n)
                {
                    // remove previous strip by invalidating previous area
                    if (lastHighlightedIndex >= 0 && lastHighlightedIndex < n)
                    {
                        var rect = GetStripRect(lastHighlightedIndex);
                        chartPanel.Invalidate(rect);
                        lastHighlightedIndex = -1;
                    }
                    return;
                }

                // invalidate old and new strip areas only
                var oldRect = Rectangle.Empty;
                if (lastHighlightedIndex >= 0)
                    oldRect = GetStripRect(lastHighlightedIndex);
                var newRect = GetStripRect(index);

                if (!oldRect.IsEmpty)
                {
                    chartPanel.Invalidate(oldRect);
                }
                chartPanel.Invalidate(newRect);
                lastHighlightedIndex = index;
            }
            catch (Exception ex)
            {
                AddLog($"하이라이트 오류: {ex.Message}");
            }
        }

        private Rectangle GetStripRect(int idx)
        {
            int w = chartPanel.Width;
            int h = chartPanel.Height;
            int margin = 40;
            int plotW = Math.Max(10, w - margin * 2);
            int plotH = Math.Max(10, h - margin * 2);
            int n = Math.Max(1, Math.Max(angles.Count, throttles.Count));
            float x = margin + (n == 1 ? plotW / 2f : (float)idx * (plotW - 1) / (n - 1));
            int half = 6;
            return new Rectangle((int)(x) - half, margin, half * 2, plotH + 1);
        }

        private void ChartPanel_Paint(object? sender, PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                // draw cached static bitmap if available (axes/lines/labels)
                if (cachedChartBitmap != null)
                {
                    g.DrawImageUnscaled(cachedChartBitmap, 0, 0);
                }
                else
                {
                    // fallback: clear background
                    g.Clear(chartPanel.BackColor);
                }

                // draw only highlight strip (no markers or grid)
                int n = Math.Max(1, Math.Max(angles.Count, throttles.Count));
                int idx = listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex;
                if (cachedChartBitmap != null && idx >= 0 && idx < n)
                {
                    var rect = GetStripRect(idx);
                    using (var sb = new SolidBrush(Color.FromArgb(80, Color.Red)))
                    {
                        g.FillRectangle(sb, rect);
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"Chart paint 오류: {ex.Message}");
            }
        }

        // 그래프 클릭 → 해당 프레임으로 이동
        private void ChartPanel_MouseClick(object? sender, MouseEventArgs e)
        {
            try
            {
                int margin = 40;
                int w = chartPanel.Width;
                int plotW = Math.Max(10, w - margin * 2);

                int n = Math.Max(1, Math.Max(angles.Count, throttles.Count));

                // 클릭 위치가 그래프 영역 밖이면 무시
                if (e.X < margin || e.X > margin + plotW)
                    return;

                // X좌표 → index 변환
                float relativeX = e.X - margin;
                int index = (int)Math.Round(relativeX / (plotW - 1) * (n - 1));

                index = Math.Max(0, Math.Min(index, n - 1));

                // 🔥 UI 동기화
                listBoxFrames.SelectedIndex = index;
                trackBarFrame.Value = index;

                AddLog($"[그래프 클릭 이동] → {index}");
            }
            catch (Exception ex)
            {
                AddLog($"그래프 클릭 오류: {ex.Message}");
            }
        }

        private void btnSetLeft_Click(object? sender, EventArgs e)
        {
            try
            {
                int idx = listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex;
                if (idx < 0 || idx >= imagePaths.Count)
                {
                    MessageBox.Show("범위를 설정할 프레임을 선택하세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                leftIndex = idx;
                UpdateRangeLabel();
                AddLog($"[Set Left] {leftIndex}");
            }
            catch (Exception ex)
            {
                AddLog($"Set Left 오류: {ex.Message}");
            }
        }

        private void btnSetRight_Click(object? sender, EventArgs e)
        {
            try
            {
                int idx = listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex;
                if (idx < 0 || idx >= imagePaths.Count)
                {
                    MessageBox.Show("범위를 설정할 프레임을 선택하세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                rightIndex = idx;
                UpdateRangeLabel();
                AddLog($"[Set Right] {rightIndex}");
            }
            catch (Exception ex)
            {
                AddLog($"Set Right 오류: {ex.Message}");
            }
        }

        private void UpdateRangeLabel()
        {
            try
            {
                // If either side not set, show default [0, 0]
                if (leftIndex < 0 || rightIndex < 0)
                {
                    lblRange.Text = "[0, 0]";
                    return;
                }

                int l = Math.Min(leftIndex, rightIndex);
                int r = Math.Max(leftIndex, rightIndex);
                lblRange.Text = $"[{l}, {r}]";
            }
            catch (Exception ex)
            {
                AddLog($"범위 레이블 업데이트 오류: {ex.Message}");
            }
        }

        private void btnDeleteRange_Click(object? sender, EventArgs e)
        {
            try
            {
                if (leftIndex < 0 || rightIndex < 0)
                {
                    MessageBox.Show("삭제할 범위를 먼저 설정하세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int l = leftIndex, r = rightIndex;
                if (l > r)
                {
                    var t = l; l = r; r = t;
                }

                if (imagePaths.Count == 0)
                {
                    MessageBox.Show("삭제할 이미지가 없습니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                l = Math.Max(0, l);
                r = Math.Min(imagePaths.Count - 1, r);

                // collect deleted items in order
                var tempPaths = new List<string>();
                var tempAngles = new List<double>();
                var tempThrottles = new List<double>();
                var tempIndices = new List<int>();

                for (int i = l; i <= r; i++)
                {
                    tempPaths.Add(imagePaths[i]);
                    double a = (angles.Count > i && angles[i].HasValue) ? angles[i].Value : double.NaN;
                    double tval = (throttles.Count > i && throttles[i].HasValue) ? throttles[i].Value : double.NaN;
                    tempAngles.Add(a);
                    tempThrottles.Add(tval);
                    tempIndices.Add(i);
                }

                // delete from end to start to preserve indices
                for (int i = r; i >= l; i--)
                {
                    imagePaths.RemoveAt(i);
                    if (angles.Count > i) angles.RemoveAt(i);
                    if (throttles.Count > i) throttles.RemoveAt(i);
                }

                // append to deleted lists (keep original order)
                deletedImagePaths.AddRange(tempPaths);
                deletedAngles.AddRange(tempAngles);
                deletedThrottles.AddRange(tempThrottles);
                deletedIndices.AddRange(tempIndices);

                // UI refresh
                RebuildListBoxFrames();
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                int newIdx = Math.Min(l, imagePaths.Count - 1);
                if (imagePaths.Count == 0)
                {
                    ClearImageDisplay();
                    trackBarFrame.Value = 0;
                }
                else
                {
                    listBoxFrames.SelectedIndex = newIdx;
                    trackBarFrame.Value = newIdx;
                }

                AddLog($"[삭제 완료] {l} ~ {r} 프레임 삭제");

                // reset range
                leftIndex = -1; rightIndex = -1;
                UpdateRangeLabel();
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"범위 삭제 오류: {ex.Message}");
            }
        }

        private void btnRestore_Click(object? sender, EventArgs e)
        {
            try
            {
                if (deletedImagePaths.Count == 0)
                {
                    MessageBox.Show("복원할 데이터가 없습니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // restore in order using saved indices; if index > current count, append
                for (int i = 0; i < deletedImagePaths.Count; i++)
                {
                    int idx = (i < deletedIndices.Count) ? deletedIndices[i] : imagePaths.Count;
                    idx = Math.Min(idx, imagePaths.Count);
                    imagePaths.Insert(idx, deletedImagePaths[i]);
                    double a = deletedAngles[i];
                    double t = deletedThrottles[i];
                    if (double.IsNaN(a))
                        angles.Insert(idx, null);
                    else
                        angles.Insert(idx, a);

                    if (double.IsNaN(t))
                        throttles.Insert(idx, null);
                    else
                        throttles.Insert(idx, t);
                }

                int restoredCount = deletedImagePaths.Count;

                // clear deleted lists
                deletedImagePaths.Clear();
                deletedAngles.Clear();
                deletedThrottles.Clear();
                deletedIndices.Clear();

                RebuildListBoxFrames();
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                if (imagePaths.Count > 0)
                {
                    int sel = 0;
                    listBoxFrames.SelectedIndex = sel;
                    trackBarFrame.Value = sel;
                }

                AddLog($"[복구 완료] {restoredCount}개 복원");
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"복구 오류: {ex.Message}");
            }
        }

        private void btnReload_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentFolder))
                {
                    MessageBox.Show("먼저 폴더를 로드하세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LoadImages(currentFolder);
                AddLog("[Reload] 폴더 재로드 완료");
            }
            catch (Exception ex)
            {
                AddLog($"Reload 오류: {ex.Message}");
            }
        }

        private void RebuildListBoxFrames()
        {
            try
            {
                listBoxFrames.Items.Clear();
                foreach (var p in imagePaths)
                    listBoxFrames.Items.Add(Path.GetFileName(p));
            }
            catch (Exception ex)
            {
                AddLog($"리스트 재구성 오류: {ex.Message}");
            }
        }

        private void nudSpeed_ValueChanged(object? sender, EventArgs e)
        {
            try
            {
                playbackSpeed = (double)nudSpeed.Value;
                UpdateTimerIntervalFromSpeed();
                lblSpeed.Text = $"{playbackSpeed:F2}x";
            }
            catch (Exception ex)
            {
                AddLog($"속도 설정 오류: {ex.Message}");
            }
        }

        private void UpdateTimerIntervalFromSpeed()
        {
            try
            {
                var interval = (int)Math.Max(10, 100.0 / playbackSpeed);
                timerPlayback.Interval = interval;
            }
            catch (Exception ex)
            {
                AddLog($"타이머 간격 설정 오류: {ex.Message}");
            }
        }

        private void btnPrev_Click(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0) return;
                int idx = Math.Max(0, (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex));
                idx = Math.Max(0, idx - 1);
                listBoxFrames.SelectedIndex = idx;
                trackBarFrame.Value = idx;
            }
            catch (Exception ex)
            {
                AddLog($"이전 프레임 이동 오류: {ex.Message}");
            }
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0) return;
                int idx = Math.Min(imagePaths.Count - 1, (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex));
                idx = Math.Min(imagePaths.Count - 1, idx + 1);
                listBoxFrames.SelectedIndex = idx;
                trackBarFrame.Value = idx;
            }
            catch (Exception ex)
            {
                AddLog($"다음 프레임 이동 오류: {ex.Message}");
            }
        }

        private void btnPlayForward_Click(object? sender, EventArgs e)
        {
            if (imagePaths.Count == 0)
            {
                AddLog("재생 불가: 이미지가 없습니다.");
                return;
            }
            isPlayingForward = true;
            isPlayingBackward = false;
            UpdateTimerIntervalFromSpeed();
            timerPlayback.Start();
            AddLog("[재생 시작 >>]");
        }

        private void btnPlayBackward_Click(object? sender, EventArgs e)
        {
            if (imagePaths.Count == 0)
            {
                AddLog("재생 불가: 이미지가 없습니다.");
                return;
            }
            isPlayingForward = false;
            isPlayingBackward = true;
            UpdateTimerIntervalFromSpeed();
            timerPlayback.Start();
            AddLog("[역재생 시작 <<]");
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            timerPlayback.Stop();
            isPlayingForward = false;
            isPlayingBackward = false;
            AddLog("[정지]");
        }

        private void TimerPlayback_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0)
                    return;

                int idx = (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex);
                if (idx < 0) idx = 0;

                if (isPlayingForward)
                {
                    if (idx < imagePaths.Count - 1)
                        idx++;
                    else
                    {
                        // 끝 도달 시 재생 중지
                        btnStop_Click(null, EventArgs.Empty);
                        return;
                    }
                }
                else if (isPlayingBackward)
                {
                    if (idx > 0)
                        idx--;
                    else
                    {
                        btnStop_Click(null, EventArgs.Empty);
                        return;
                    }
                }

                // UI 동기화
                if (idx >= 0 && idx < imagePaths.Count)
                {
                    // Setting SelectedIndex triggers ShowImage via event
                    if (listBoxFrames.SelectedIndex != idx)
                        listBoxFrames.SelectedIndex = idx;
                    trackBarFrame.Value = idx;
                }
            }
            catch (Exception ex)
            {
                AddLog($"재생 중 오류: {ex.Message}");
                btnStop_Click(null, EventArgs.Empty);
            }
        }

        // 삭제 버튼 클릭
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (listBoxFrames.SelectedIndex < 0 || listBoxFrames.SelectedIndex >= imagePaths.Count)
                {
                    MessageBox.Show("삭제할 항목을 선택하세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show("정말 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                int idx = listBoxFrames.SelectedIndex;
                string imagePath = imagePaths[idx];
                string imageFileName = Path.GetFileName(imagePath);

                // 이미지 파일 먼저 삭제
                try
                {
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    AddLog($"[삭제 실패] {imageFileName}: {ex.Message}");
                    return;
                }

                // 동일 이름의 JSON 파일 삭제 시도
                string jsonPath = Path.ChangeExtension(imagePath, ".json");
                if (File.Exists(jsonPath))
                {
                    try
                    {
                        File.Delete(jsonPath);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"[삭제 실패] {Path.GetFileName(jsonPath)}: {ex.Message}");
                        // JSON 삭제 실패하더라도 이미지가 이미 삭제되었으므로 계속 진행
                    }
                }

                // UI 및 내부 리스트 동기화
                try
                {
                    // Remove from imagePaths and listBox
                    imagePaths.RemoveAt(idx);
                    listBoxFrames.Items.RemoveAt(idx);

                    // Remove from angles/throttles if available
                    if (angles.Count > idx)
                        angles.RemoveAt(idx);
                    if (throttles.Count > idx)
                        throttles.RemoveAt(idx);

                    AddLog($"[삭제 완료] {imageFileName}");

                    // 트랙바 최대값 갱신
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                    if (imagePaths.Count == 0)
                    {
                        // 모든 항목 삭제됨
                        ClearImageDisplay();
                        trackBarFrame.Value = 0;
                    }
                    else
                    {
                        int newIdx = Math.Min(idx, imagePaths.Count - 1);
                        // 선택 변경을 통해 ShowImage 호출
                        listBoxFrames.SelectedIndex = newIdx;
                        trackBarFrame.Value = newIdx;
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[삭제 실패] 내부 동기화 오류: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                AddLog($"[삭제 실패] 알수없는 오류: {ex.Message}");
            }
        }

        private void ClearImageDisplay()
        {
            try
            {
                if (picFrame.Image != null)
                {
                    var old = picFrame.Image;
                    picFrame.Image = null;
                    old.Dispose();
                }

                currentIndex = -1;
                lblFrame.Text = "Frame: N/A";
                lblAngle.Text = "Angle: N/A";
                lblThrottle.Text = "Throttle: N/A";
                AddLog("이미지 화면 초기화");
            }
            catch (Exception ex)
            {
                AddLog($"이미지 초기화 오류: {ex.Message}");
            }
        }

        // GenerateTestJson is intentionally disabled. Kept for reference but does nothing to avoid creating test JSON files.
        private void GenerateTestJson(string folderPath)
        {
            // Intentionally left blank to prevent automatic test JSON creation.
        }


        // 폴더 선택 버튼 클릭
        private void btnOpenFolder_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "이미지 폴더를 선택하세요";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                AddLog($"폴더 선택: {folder}");
                LoadImages(folder);
            }
        }

        // 지정 폴더에서 이미지 파일을 읽어서 리스트에 추가
        private void LoadImages(string folderPath)
        {
            try
            {
                // 기존 상태 초기화
                listBoxFrames.Items.Clear();
                imagePaths.Clear();
                currentIndex = -1;
                // tub cleaner reset
                leftIndex = -1;
                rightIndex = -1;
                deletedImagePaths.Clear();
                deletedAngles.Clear();
                deletedThrottles.Clear();
                deletedIndices.Clear();
                UpdateRangeLabel();
                // remember folder for reload
                currentFolder = folderPath;
                // also clear angle/throttle until loaded
                angles.Clear();
                throttles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    AddLog($"폴더가 존재하지 않음: {folderPath}");
                    return;
                }

                var files = Directory.EnumerateFiles(folderPath, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // 숫자 기준 정렬: 파일명 앞의 숫자를 추출하여 정렬. 추출 실패한 파일은 자연 정렬로 뒤에 위치.
                try
                {
                    files = files
                        .Select(f => new { Path = f, Num = ExtractLeadingNumber(f) })
                        .OrderBy(x => x.Num.HasValue ? 0 : 1) // 숫자 있는 것 먼저
                        .ThenBy(x => x.Num ?? int.MaxValue)
                        .ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                        .Select(x => x.Path)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    AddLog($"파일 정렬 중 오류 발생: {ex.Message}. 기본 정렬 사용.");
                    files = files.OrderBy(f => f).ToArray();
                }

                if (files.Length == 0)
                {
                    AddLog("이미지 파일이 없습니다.");
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = 0;
                    return;
                }

                foreach (var f in files)
                {
                    imagePaths.Add(f);
                    listBoxFrames.Items.Add(Path.GetFileName(f));
                }

                // 트랙바 설정
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);
                trackBarFrame.Value = 0;

                // 첫 이미지 선택
                listBoxFrames.SelectedIndex = 0;

                AddLog($"이미지 로드 완료: {imagePaths.Count}개");
                // 테스트용 JSON 자동 생성은 비활성화됨 (실제 JSON 파일만 사용)
                // JSON 데이터 로드 시도 (같은 폴더)
                LoadJsonData(folderPath);
                // 차트 업데이트
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"이미지 로드 오류: {ex.Message}");
            }
        }

        // 파일 이름에서 선행 숫자를 추출합니다. 없으면 null 반환
        private int? ExtractLeadingNumber(string filePath)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrEmpty(fileName))
                    return null;

                var m = Regex.Match(fileName, "^(\\d+)", RegexOptions.Compiled);
                if (m.Success && int.TryParse(m.Groups[1].Value, out var n))
                    return n;
                return null;
            }
            catch (Exception ex)
            {
                AddLog($"숫자 추출 오류 ({Path.GetFileName(filePath)}): {ex.Message}");
                return null;
            }
        }

        // 폴더의 JSON 파일을 읽어 angle, throttle 리스트에 저장
        private void LoadJsonData(string folderPath)
        {
            try
            {
                angles.Clear();
                throttles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    AddLog($"JSON 폴더가 존재하지 않음: {folderPath}");
                    return;
                }

                var jsonFiles = Directory.EnumerateFiles(folderPath, "*.json").ToArray();

                try
                {
                    jsonFiles = jsonFiles
                        .Select(f => new { Path = f, Num = ExtractLeadingNumber(f) })
                        .OrderBy(x => x.Num.HasValue ? 0 : 1)
                        .ThenBy(x => x.Num ?? int.MaxValue)
                        .ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                        .Select(x => x.Path)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    AddLog($"JSON 파일 정렬 중 오류: {ex.Message}");
                    jsonFiles = jsonFiles.OrderBy(f => f).ToArray();
                }

                if (jsonFiles.Length == 0)
                {
                    AddLog("JSON 파일이 없습니다.");
                    return;
                }

                foreach (var jf in jsonFiles)
                {
                    try
                    {
                        var txt = File.ReadAllText(jf);
                        var jobj = JObject.Parse(txt);

                        // 우선순위: key "user/angle" (슬래시 포함) -> nested user.angle -> fallback "angle"
                        JToken? angleToken = null;
                        JToken? throttleToken = null;

                        if (jobj.TryGetValue("user/angle", out var atok))
                            angleToken = atok;
                        else
                            angleToken = jobj.SelectToken("user.angle") ?? jobj["angle"];

                        if (jobj.TryGetValue("user/throttle", out var ttok))
                            throttleToken = ttok;
                        else
                            throttleToken = jobj.SelectToken("user.throttle") ?? jobj["throttle"];

                        double? ang = null;
                        double? thr = null;

                        if (angleToken != null)
                        {
                            if (double.TryParse(angleToken.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var a))
                                ang = a;
                        }

                        if (throttleToken != null)
                        {
                            if (double.TryParse(throttleToken.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var t))
                                thr = t;
                        }

                        angles.Add(ang);
                        throttles.Add(thr);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"JSON 파싱 오류 ({Path.GetFileName(jf)}): {ex.Message}");
                        // 파싱 실패한 경우 자리 유지 (null)
                        angles.Add(null);
                        throttles.Add(null);
                    }
                }

                AddLog($"JSON 로드 완료: {jsonFiles.Length}개");

                if (jsonFiles.Length != imagePaths.Count)
                {
                    AddLog($"경고: 이미지 개수({imagePaths.Count})와 JSON 개수({jsonFiles.Length})가 다릅니다.");
                }
            }
            catch (Exception ex)
            {
                AddLog($"JSON 로드 오류: {ex.Message}");
            }
        }

        // 인덱스에 해당하는 이미지 표시
        private void ShowImage(int index)
        {
            if (index < 0 || index >= imagePaths.Count)
            {
                AddLog($"잘못된 인덱스: {index}");
                return;
            }

            try
            {
                // 기존 이미지 Dispose 처리
                if (picFrame.Image != null)
                {
                    var old = picFrame.Image;
                    picFrame.Image = null;
                    old.Dispose();
                }

                // 파일을 잠그지 않도록 스트림으로 읽어 복사
                string path = imagePaths[index];
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var img = Image.FromStream(fs))
                {
                    picFrame.Image = new Bitmap(img);
                }

                currentIndex = index;
                lblFrame.Text = $"Frame: {currentIndex}";

                // angle/throttle 표시
                if (angles.Count > index && angles[index].HasValue)
                    lblAngle.Text = $"Angle: {angles[index].Value.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblAngle.Text = "Angle: N/A";

                if (throttles.Count > index && throttles[index].HasValue)
                    lblThrottle.Text = $"Throttle: {throttles[index].Value.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblThrottle.Text = "Throttle: N/A";

                AddLog($"이미지 표시: {Path.GetFileName(imagePaths[index])} (인덱스 {index})");
                // 하이라이트 업데이트
                HighlightCurrentIndex(index);
            }
            catch (Exception ex)
            {
                AddLog($"이미지 표시 오류: {ex.Message}");
            }
        }

        // 로그 추가 및 자동 스크롤
        private void AddLog(string message)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                listBoxLog.Items.Add($"[{timestamp}] {message}");
                // 최신 로그가 보이도록 스크롤
                if (listBoxLog.Items.Count > 0)
                {
                    listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
                }
            }
            catch
            {
                // 로그 실패는 무시
            }
        }

        // 리스트박스 선택 변경 이벤트
        private void listBoxFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0 && listBoxFrames.SelectedIndex < imagePaths.Count)
            {
                int idx = listBoxFrames.SelectedIndex;
                // 트랙바와 동기화
                if (trackBarFrame.Value != idx)
                    trackBarFrame.Value = idx;

                ShowImage(idx);
            }
        }

        // 트랙바 이동 이벤트
        private void trackBarFrame_Scroll(object? sender, EventArgs e)
        {
            int idx = trackBarFrame.Value;
            if (idx >= 0 && idx < imagePaths.Count)
            {
                // 리스트와 동기화 (리스트의 SelectedIndex를 설정하면 SelectedIndexChanged가 실행되어 ShowImage 호출)
                if (listBoxFrames.SelectedIndex != idx)
                    listBoxFrames.SelectedIndex = idx;
                else
                    ShowImage(idx);

                // only update highlight strip to avoid full redraw
                HighlightCurrentIndex(idx);
            }
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            await RunTraining();
        }



        private async Task RunTraining()
        {
            try
            {
                string modelType = cmbModelType.Text;   // linear 등
                string comment = txtComment.Text;

                if (string.IsNullOrEmpty(modelType))
                {
                    MessageBox.Show("모델 타입 선택해라");
                    return;
                }

                if (string.IsNullOrEmpty(carFolderPath))
                {
                    MessageBox.Show("먼저 폴더 선택해라");
                    return;
                }

                string workingDir = carFolderPath;

                string command = $"manage.py train --model {modelType}";

                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = command,
                    WorkingDirectory = workingDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new Process();
                process.StartInfo = psi;

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            listBoxLog.Items.Add(e.Data);
                            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
                        }));
                    }
                };

                process.Start();

                process.BeginOutputReadLine();

                await Task.Run(() => process.WaitForExit());

                MessageBox.Show("학습 완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러: " + ex.Message);
            }
        }

        private void btnSelectCarFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "mycar 폴더 선택해라";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    carFolderPath = fbd.SelectedPath;
                    MessageBox.Show("선택됨: " + carFolderPath);
                }
            }
        }
    }
}
