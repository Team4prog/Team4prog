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
        private readonly List<double> trainLossValues = new();
        private readonly List<double> valLossValues = new();

        private static readonly Regex LossRegex =
            new Regex(@"(?<!val_)loss:\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ValLossRegex =
            new Regex(@"val_loss:\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private void InitializeLossChart()
        {
            chartLoss.Paint -= ChartLoss_Paint;
            chartLoss.Paint += ChartLoss_Paint;
            chartLoss.Resize += (s, e) => chartLoss.Invalidate();
        }

        private void FixTrainerLayout()
        {
            if (panelTrainer == null || chartLoss == null)
                return;

            panelTrainer.AutoScroll = true;

            groupBoxConfigEditor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxTrainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            chartLoss.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            chartLoss.Location = new Point(10, groupBoxTrainer.Bottom + 20);
            chartLoss.Size = new Size(Math.Max(600, panelTrainer.ClientSize.Width - 26), 320);

            groupBoxPilotManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxPilotManager.Location = new Point(12, chartLoss.Bottom + 25);
            groupBoxPilotManager.Width = Math.Max(600, panelTrainer.ClientSize.Width - 31);
        }

        private void ResetLossChart()
        {
            trainLossValues.Clear();
            valLossValues.Clear();
            chartLoss.Invalidate();
        }

        private void ParseLossLine(string line)
        {
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

            if (changed)
                chartLoss.Invalidate();
        }

        private void ChartLoss_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (chartLoss.Width < 100 || chartLoss.Height < 100)
                return;

            Rectangle plot = new Rectangle(55, 30, chartLoss.Width - 80, chartLoss.Height - 80);

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
                g.DrawString("학습을 시작하면 loss 그래프가 표시됩니다.", titleFont, textBrush, plot.Left + 20, plot.Top + 30);
                return;
            }

            var all = trainLossValues.Concat(valLossValues).ToList();

            double min = all.Min();
            double max = all.Max();

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

        private void DrawLossSeries(Graphics g, Rectangle plot, List<double> values, double min, double max, Color color, string label, int index)
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