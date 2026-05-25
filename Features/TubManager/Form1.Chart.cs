using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Team4prog.UI
{
    // Tub Manager chart rendering and chart-click navigation.
    public partial class Form1
    {
        // Register paint handling and prepare the cached chart bitmap.
        private void InitializeChart()
        {
            try
            {
                chartPanel.Paint += ChartPanel_Paint;
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
                // Rebuild static chart parts once, then paint only the moving highlight strip.
                int w = Math.Max(10, chartPanel.Width);
                int h = Math.Max(10, chartPanel.Height);
                var bmp = new Bitmap(w, h);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(chartPanel.BackColor);

                    int margin = 40;
                    int plotW = Math.Max(10, w - margin * 2);
                    int plotH = Math.Max(10, h - margin * 2);

                    // Draw the plot background and axes.
                    using (var brush = new SolidBrush(Color.FromArgb(30, 30, 30)))
                    {
                        g.FillRectangle(brush, margin, margin, plotW, plotH);
                    }

                    using (var pen = new Pen(Color.White))
                    {
                        g.DrawLine(pen, margin, margin + plotH, margin + plotW, margin + plotH); // x axis
                        g.DrawLine(pen, margin, margin, margin, margin + plotH); // y axis
                    }

                    int n = Math.Max(1, imagePaths.Count);

                    if (n > 0)
                    {
                        // Convert normalized angle/throttle values into panel coordinates.
                        PointF[] ptsA = new PointF[n];
                        PointF[] ptsT = new PointF[n];
                        for (int i = 0; i < n; i++)
                        {
                            float x = margin + (n == 1 ? plotW / 2f : (float)i * (plotW - 1) / (n - 1));
                            double a = (i < angles.Count && angles[i] is double angle) ? angle : 0.0;
                            double t = (i < throttles.Count && throttles[i] is double throttle) ? throttle : 0.0;
                            float yA = margin + plotH / 2f - (float)(a * (plotH / 2f));
                            float yT = margin + plotH / 2f - (float)(t * (plotH / 2f));
                            ptsA[i] = new PointF(x, yA);
                            ptsT[i] = new PointF(x, yT);
                        }

                        // Draw angle and throttle traces.
                        if (n > 1)
                        {
                            using (var penA = new Pen(Color.Blue, 2))
                                g.DrawLines(penA, ptsA);
                            using (var penT = new Pen(Color.Yellow, 2))
                                g.DrawLines(penT, ptsT);
                        }
                        else
                        {
                            // A single sample cannot make a line, so draw a short tick instead.
                            using (var penA = new Pen(Color.Blue, 2))
                                g.DrawLine(penA, ptsA[0].X - 2, ptsA[0].Y, ptsA[0].X + 2, ptsA[0].Y);
                            using (var penT = new Pen(Color.Yellow, 2))
                                g.DrawLine(penT, ptsT[0].X - 2, ptsT[0].Y, ptsT[0].X + 2, ptsT[0].Y);
                        }

                        // Zero baseline helps show positive/negative steering and throttle changes.
                        using (var zeroPen = new Pen(Color.WhiteSmoke, 1))
                        {
                            float y0 = margin + plotH / 2f;
                            zeroPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            g.DrawLine(zeroPen, margin, y0, margin + plotW, y0);
                        }

                        // Draw sparse frame-number labels to avoid overcrowding the chart.
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

                // Swap the bitmap cache after the new image is ready.
                var old = cachedChartBitmap;
                cachedChartBitmap = bmp;
                if (old != null) old.Dispose();

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

                int n = Math.Max(1, imagePaths.Count);
                if (index < 0 || index >= n)
                {
                    // Remove the previous strip by invalidating only its old area.
                    if (lastHighlightedIndex >= 0 && lastHighlightedIndex < n)
                    {
                        var rect = GetStripRect(lastHighlightedIndex);
                        chartPanel.Invalidate(rect);
                        lastHighlightedIndex = -1;
                    }
                    return;
                }

                // Repaint only the old and new highlight strips for smoother scrubbing.
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
            int n = Math.Max(1, imagePaths.Count);
            float x = margin + (n == 1 ? plotW / 2f : (float)idx * (plotW - 1) / (n - 1));
            int half = 6;
            return new Rectangle((int)(x) - half, margin, half * 2, plotH + 1);
        }

        private void ChartPanel_Paint(object? sender, PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                // Draw cached static chart content first.
                if (cachedChartBitmap != null)
                {
                    g.DrawImageUnscaled(cachedChartBitmap, 0, 0);
                }
                else
                {
                    g.Clear(chartPanel.BackColor);
                }

                // Draw only the current frame highlight on top of the cached chart.
                int n = Math.Max(1, imagePaths.Count);
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

        // Move the selected frame to the chart position clicked by the user.
        private void ChartPanel_MouseClick(object? sender, MouseEventArgs e)
        {
            try
            {
                int margin = 40;
                int w = chartPanel.Width;
                int plotW = Math.Max(10, w - margin * 2);

                int n = Math.Max(1, imagePaths.Count);

                // Ignore clicks outside the plotted x-axis range.
                if (e.X < margin || e.X > margin + plotW)
                    return;

                // Convert the clicked x-coordinate into a frame index.
                float relativeX = e.X - margin;
                int index = (int)Math.Round(relativeX / (plotW - 1) * (n - 1));

                index = Math.Max(0, Math.Min(index, n - 1));

                // Selecting the list item triggers the existing image display path.
                listBoxFrames.SelectedIndex = index;
                trackBarFrame.Value = index;

                AddLog($"[그래프 클릭 이동] {index}");
            }
            catch (Exception ex)
            {
                AddLog($"그래프 클릭 오류: {ex.Message}");
            }
        }
    }
}

