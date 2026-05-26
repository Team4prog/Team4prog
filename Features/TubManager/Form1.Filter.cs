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
                string? currentImagePath = null;
                if (currentIndex >= 0 && currentIndex < imagePaths.Count)
                    currentImagePath = imagePaths[currentIndex];

                bool useAngle = TryGetFilterCondition(cmbAngleOp, txtAngleFilter, out double angleVal, out bool stopFilter);
                if (stopFilter)
                    return;

                bool useThrottle = TryGetFilterCondition(cmbThrottleOp, txtThrottleFilter, out double throttleVal, out stopFilter);
                if (stopFilter)
                    return;

                if (!useAngle && !useThrottle)
                {
                    MessageBox.Show("필터 조건을 선택하세요. 예: Throttle > 0");
                    return;
                }

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
                    originalCatalogImagePaths = new List<string?>(catalogImagePaths);
                    originalAngles = new List<double?>(angles);
                    originalThrottles = new List<double?>(throttles);
                }

                filteredImagePaths.Clear();
                filteredCatalogImagePaths.Clear();
                filteredAngles.Clear();
                filteredThrottles.Clear();
                int selectedFilteredIndex = -1;
                int nearestFilteredIndex = -1;
                int originalCurrentIndex = string.IsNullOrWhiteSpace(currentImagePath)
                    ? -1
                    : originalImagePaths.FindIndex(path => string.Equals(path, currentImagePath, StringComparison.OrdinalIgnoreCase));

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
                        filteredCatalogImagePaths.Add(originalCatalogImagePaths.Count > i ? originalCatalogImagePaths[i] : null);
                        filteredAngles.Add(originalAngles[i]);
                        filteredThrottles.Add(originalThrottles[i]);

                        int newIndex = filteredImagePaths.Count - 1;
                        if (!string.IsNullOrWhiteSpace(currentImagePath) &&
                            string.Equals(originalImagePaths[i], currentImagePath, StringComparison.OrdinalIgnoreCase))
                        {
                            selectedFilteredIndex = newIndex;
                        }
                        else if (selectedFilteredIndex < 0 && nearestFilteredIndex < 0 && originalCurrentIndex >= 0 && i > originalCurrentIndex)
                        {
                            nearestFilteredIndex = newIndex;
                        }
                    }
                }

                imagePaths = new List<string>(filteredImagePaths);
                catalogImagePaths = new List<string?>(filteredCatalogImagePaths);
                angles = new List<double?>(filteredAngles);
                throttles = new List<double?>(filteredThrottles);

                RebuildListBoxFrames();
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                if (imagePaths.Count > 0)
                {
                    int nextIndex = selectedFilteredIndex >= 0
                        ? selectedFilteredIndex
                        : nearestFilteredIndex >= 0
                            ? nearestFilteredIndex
                            : imagePaths.Count - 1;

                    listBoxFrames.SelectedIndex = nextIndex;
                    trackBarFrame.Value = nextIndex;
                    ShowImage(nextIndex);
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

        private bool TryGetFilterCondition(ComboBox opCombo, TextBox valueBox, out double value, out bool stopFilter)
        {
            value = 0;
            stopFilter = false;

            bool hasOperator = opCombo.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(opCombo.Text);
            bool hasValue = !string.IsNullOrWhiteSpace(valueBox.Text);

            if (!hasOperator && !hasValue)
                return false;

            if (!hasOperator)
            {
                MessageBox.Show("필터 연산자를 선택하세요.");
                stopFilter = true;
                return false;
            }

            if (!hasValue)
                return true;

            if (double.TryParse(valueBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return true;

            if (double.TryParse(valueBox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
                return true;

            MessageBox.Show("필터 값은 숫자로 입력하세요.");
            stopFilter = true;
            return false;
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
            if (double.TryParse(tb.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value) ||
                double.TryParse(tb.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
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
            catalogImagePaths = new List<string?>(originalCatalogImagePaths);
            angles = new List<double?>(originalAngles);
            throttles = new List<double?>(originalThrottles);

            originalImagePaths.Clear();
            originalCatalogImagePaths.Clear();
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
    }
}
