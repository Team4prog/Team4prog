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
    // Tub Manager filtering: validates angle/throttle input and swaps the active frame lists with filtered results.
    public partial class Form1
    {
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

                // DonkeyCar driving values are normalized to the -1.0 ~ 1.0 range.
                if (useAngle && (angleVal < -1.0 || angleVal > 1.0))
                {
                    MessageBox.Show("Angle must be between -1.0 and 1.0.");
                    return;
                }

                if (useThrottle && (throttleVal < -1.0 || throttleVal > 1.0))
                {
                    MessageBox.Show("Throttle must be between -1.0 and 1.0.");
                    return;
                }

                if (originalImagePaths.Count == 0)
                {
                    originalImagePaths = new List<string>(imagePaths);
                    originalAngles = new List<double?>(angles);
                    originalThrottles = new List<double?>(throttles);
                    originalPilotAngles = new List<double?>(pilotAngles);
                    originalPilotThrottles = new List<double?>(pilotThrottles);
                }

                filteredImagePaths.Clear();
                filteredAngles.Clear();
                filteredThrottles.Clear();
                filteredPilotAngles.Clear();
                filteredPilotThrottles.Clear();

                for (int i = 0; i < originalAngles.Count; i++)
                {
                    bool ok = true;

                    if (useAngle)
                    {
                        if (!originalAngles[i].HasValue)
                            ok = false;
                        else if (originalAngles[i] is double angle)
                            ok = Compare(angle, cmbAngleOp.Text, angleVal);
                    }

                    if (ok && useThrottle)
                    {
                        if (!originalThrottles[i].HasValue)
                            ok = false;
                        else if (originalThrottles[i] is double throttle)
                            ok = Compare(throttle, cmbThrottleOp.Text, throttleVal);
                    }

                    if (ok)
                    {
                        filteredImagePaths.Add(originalImagePaths[i]);
                        filteredAngles.Add(originalAngles[i]);
                        filteredThrottles.Add(originalThrottles[i]);
                        filteredPilotAngles.Add(originalPilotAngles.Count > i ? originalPilotAngles[i] : null);
                        filteredPilotThrottles.Add(originalPilotThrottles.Count > i ? originalPilotThrottles[i] : null);
                    }
                }

                imagePaths = new List<string>(filteredImagePaths);
                angles = new List<double?>(filteredAngles);
                throttles = new List<double?>(filteredThrottles);
                pilotAngles = new List<double?>(filteredPilotAngles);
                pilotThrottles = new List<double?>(filteredPilotThrottles);

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

                AddLog($"[필터 결과] {originalImagePaths.Count}개 -> {imagePaths.Count}개");
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"필터 오류: {ex.Message}");
            }
        }

        private void ValidateInput(object? sender, EventArgs e)
        {
            if (sender is not TextBox tb)
                return;

            // Empty input means "do not filter by this field".
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.BackColor = SystemColors.Window;
                return;
            }

            // Highlight invalid numbers before the user applies the filter.
            if (double.TryParse(tb.Text, out double value))
            {
                if (value < -1.0 || value > 1.0)
                {
                    tb.BackColor = Color.LightCoral;
                }
                else
                {
                    tb.BackColor = SystemColors.Window;
                }
            }
            else
            {
                tb.BackColor = Color.LightCoral;
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
                AddLog("원본 데이터가 없습니다.");
                return;
            }

            imagePaths = new List<string>(originalImagePaths);
            angles = new List<double?>(originalAngles);
            throttles = new List<double?>(originalThrottles);
            pilotAngles = new List<double?>(originalPilotAngles);
            pilotThrottles = new List<double?>(originalPilotThrottles);

            originalImagePaths.Clear();
            originalAngles.Clear();
            originalThrottles.Clear();
            originalPilotAngles.Clear();
            originalPilotThrottles.Clear();

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
    }
}

