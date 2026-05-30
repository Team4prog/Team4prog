using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Team4prog.UI
{
    public partial class Form1
    {
        private readonly List<double> trainLossValues = new List<double>();
        private readonly List<double> valLossValues = new List<double>();

        private static readonly Regex LossRegex =
            new Regex(@"(?<![\w/])loss:\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ValLossRegex =
            new Regex(@"(?<![\w/])val_loss:\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private void InitializeLossChart()
        {
            if (chartLoss == null)
                return;

            chartLoss.Visible = true;
            chartLoss.Paint -= ChartLoss_Paint;
            chartLoss.Paint += ChartLoss_Paint;
            chartLoss.Resize += (s, e) => chartLoss.Invalidate();
            chartLoss.Invalidate();
        }

        private void FixTrainerLayout()
        {
            if (panelTrainer == null ||
                chartLoss == null ||
                groupBoxTrainer == null ||
                groupBoxPilotManager == null)
            {
                return;
            }

            try
            {
                panelTrainer.SuspendLayout();
                panelTrainer.AutoScroll = true;

                int margin = 10;
                int gap = 14;
                int clientW = panelTrainer.ClientSize.Width;
                int clientH = panelTrainer.ClientSize.Height;

                if (clientW < 700)
                    clientW = 700;

                int scrollBarSpace = panelTrainer.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth + 4 : 0;
                int fullW = Math.Max(650, clientW - margin * 2 - scrollBarSpace);

                int topY = 70;

                groupBoxTrainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                groupBoxTrainer.Location = new Point(margin, topY);
                groupBoxTrainer.Size = new Size(fullW, 128);

                ArrangeTrainerControls();

                int chartY = groupBoxTrainer.Bottom + gap + 6;
                int pilotH = 135;
                int pilotGap = 18;

                int availableChartH = clientH - chartY - pilotGap - pilotH - 35;

                int chartH;
                if (availableChartH >= 230)
                    chartH = Math.Min(320, availableChartH);
                else
                    chartH = 240;

                chartLoss.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                chartLoss.Location = new Point(margin, chartY);
                chartLoss.Size = new Size(fullW, chartH);
                chartLoss.Visible = true;

                groupBoxPilotManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                groupBoxPilotManager.Location = new Point(margin, chartLoss.Bottom + pilotGap);
                groupBoxPilotManager.Size = new Size(fullW, pilotH);

                ArrangePilotManagerControls();

                int bottom = groupBoxPilotManager.Bottom + 30;
                panelTrainer.AutoScrollMinSize = new Size(0, Math.Max(bottom, clientH + 1));

                chartLoss.Invalidate();
            }
            finally
            {
                panelTrainer.ResumeLayout();
            }
        }

        private void ArrangeTrainerControls()
        {
            if (groupBoxTrainer == null)
                return;

            int w = groupBoxTrainer.ClientSize.Width;
            int pad = 22;
            int gap = 10;
            int h = 36;

            int leftW = Math.Min(520, Math.Max(380, (w - pad * 2 - gap) / 2));
            int rightX = pad + leftW + gap + 30;
            int rightW = w - rightX - pad;

            if (rightW < 300)
            {
                leftW = Math.Max(320, w / 2 - 35);
                rightX = pad + leftW + gap;
                rightW = Math.Max(260, w - rightX - pad);
            }

            int selectW = Math.Min(260, Math.Max(190, leftW / 2));
            int comboW = Math.Max(120, leftW - selectW - gap);

            btnSelectCarFolder.Location = new Point(pad, 36);
            btnSelectCarFolder.Size = new Size(selectW, h);

            cmbModelType.Location = new Point(btnSelectCarFolder.Right + gap, 39);
            cmbModelType.Size = new Size(comboW, 32);

            btnLoadModel.Location = new Point(pad, 79);
            btnLoadModel.Size = new Size(leftW, h);

            btnTrain.Location = new Point(rightX, 39);
            btnTrain.Size = new Size(rightW, h + 2);
        }

        private void ArrangePilotManagerControls()
        {
            if (groupBoxPilotManager == null)
                return;

            int w = groupBoxPilotManager.ClientSize.Width;
            int pad = 45;
            int gap = 10;
            int y = 52;
            int h = 34;

            int comboW = Math.Min(410, Math.Max(260, w / 3));
            int btnW = Math.Min(240, Math.Max(160, w - comboW - pad * 2 - gap - 20));

            cmbModelList.Location = new Point(pad, y);
            cmbModelList.Size = new Size(comboW, h);

            btnDeleteModel.Location = new Point(cmbModelList.Right + gap + 20, y);
            btnDeleteModel.Size = new Size(btnW, h);
        }

        private void ResetLossChart()
        {
            trainLossValues.Clear();
            valLossValues.Clear();

            if (chartLoss != null)
                chartLoss.Invalidate();
        }

        private void ParseLossLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            bool changed = false;

            Match lossMatch = LossRegex.Match(line);
            if (lossMatch.Success &&
                double.TryParse(lossMatch.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double loss))
            {
                trainLossValues.Add(loss);
                changed = true;
            }

            Match valMatch = ValLossRegex.Match(line);
            if (valMatch.Success &&
                double.TryParse(valMatch.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double valLoss))
            {
                valLossValues.Add(valLoss);
                changed = true;
            }

            if (changed && chartLoss != null)
                chartLoss.Invalidate();
        }

        private void ChartLoss_Paint(object? sender, PaintEventArgs e)
        {
            if (chartLoss == null)
                return;

            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (chartLoss.Width < 100 || chartLoss.Height < 100)
                return;

            Rectangle plot = new Rectangle(55, 30, chartLoss.Width - 80, chartLoss.Height - 75);

            using Pen axisPen = new Pen(Color.White, 1);
            using Brush textBrush = new SolidBrush(Color.White);
            using Font titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            using Font labelFont = new Font("Segoe UI", 8);

            g.DrawString("Training Loss Chart", titleFont, textBrush, 10, 5);

            g.DrawLine(axisPen, plot.Left, plot.Bottom, plot.Right, plot.Bottom);
            g.DrawLine(axisPen, plot.Left, plot.Top, plot.Left, plot.Bottom);

            int count = Math.Max(trainLossValues.Count, valLossValues.Count);

            if (count == 0)
            {
                g.DrawString("학습을 시작하면 loss 그래프가 표시됩니다.",
                    titleFont,
                    textBrush,
                    plot.Left + 20,
                    plot.Top + 30);
                return;
            }

            List<double> allValues = trainLossValues.Concat(valLossValues).ToList();

            double min = allValues.Min();
            double max = allValues.Max();

            if (Math.Abs(max - min) < 0.000001)
            {
                max += 0.01;
                min -= 0.01;
            }

            DrawLossSeries(g, plot, trainLossValues, min, max, Color.Lime, "loss", 0);
            DrawLossSeries(g, plot, valLossValues, min, max, Color.Orange, "val_loss", 1);

            g.DrawString(max.ToString("F4"), labelFont, textBrush, 5, plot.Top - 5);
            g.DrawString(min.ToString("F4"), labelFont, textBrush, 5, plot.Bottom - 12);
            g.DrawString($"Epoch: {count}", labelFont, textBrush, plot.Right - 80, plot.Bottom + 10);
        }

        private void DrawLossSeries(
            Graphics g,
            Rectangle plot,
            List<double> values,
            double min,
            double max,
            Color color,
            string label,
            int index)
        {
            if (values.Count == 0)
                return;

            using Pen pen = new Pen(color, 2);
            using Brush brush = new SolidBrush(color);
            using Font font = new Font("Segoe UI", 9, FontStyle.Bold);

            PointF[] points = new PointF[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                float x = values.Count == 1
                    ? plot.Left + plot.Width / 2f
                    : plot.Left + i * plot.Width / (float)(values.Count - 1);

                float y = plot.Bottom - (float)((values[i] - min) / (max - min) * plot.Height);

                points[i] = new PointF(x, y);
            }

            if (points.Length >= 2)
                g.DrawLines(pen, points);
            else
                g.FillEllipse(brush, points[0].X - 4, points[0].Y - 4, 8, 8);

            g.DrawString(label, font, brush, plot.Left + 10 + index * 90, plot.Bottom + 10);
        }
    }
}
