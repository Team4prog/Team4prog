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
    // Tub Cleaner range operations: select a frame range, delete it from the current list, and restore it later.
    public partial class Form1
    {
        private void btnSetLeft_Click(object? sender, EventArgs e)
        {
            try
            {
                int idx = listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex;
                if (idx < 0 || idx >= imagePaths.Count)
                {
                    MessageBox.Show("Select a frame to set the range.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Select a frame to set the range.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // Keep the default display until both range ends are selected.
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
                AddLog($"범위 라벨 업데이트 오류: {ex.Message}");
            }
        }

        private void btnDeleteRange_Click(object? sender, EventArgs e)
        {
            try
            {
                if (leftIndex < 0 || rightIndex < 0)
                {
                    MessageBox.Show("Set the range to delete first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int l = leftIndex, r = rightIndex;
                if (l > r)
                {
                    var t = l; l = r; r = t;
                }

                if (imagePaths.Count == 0)
                {
                    MessageBox.Show("삭제할 이미지가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                l = Math.Max(0, l);
                r = Math.Min(imagePaths.Count - 1, r);

                // Capture deleted data first so Restore can rebuild the same frame entries.
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

                // Delete from end to start so earlier indexes do not shift during removal.
                for (int i = r; i >= l; i--)
                {
                    imagePaths.RemoveAt(i);
                    if (angles.Count > i) angles.RemoveAt(i);
                    if (throttles.Count > i) throttles.RemoveAt(i);
                }

                // Keep the deleted frames in their original order for restoration.
                deletedImagePaths.AddRange(tempPaths);
                deletedAngles.AddRange(tempAngles);
                deletedThrottles.AddRange(tempThrottles);
                deletedIndices.AddRange(tempIndices);

                // Refresh list and trackbar after the active frame collection changes.
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

                // Reset range selection after a successful delete.
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
                    MessageBox.Show("복원할 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Restore frames near their saved indices; append if the list is now shorter.
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

                // Clear restore buffers after they are applied.
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
                    MessageBox.Show("Load a folder first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LoadImages(currentFolder);
                AddLog("[Reload] 폴더 새로고침 완료");
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
    }
}

