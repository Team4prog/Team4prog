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

                if (leftIndex >= 0 && idx < leftIndex)
                {
                    MessageBox.Show("우측 지정값이 좌측 지정값보다 작아서 지정할 수 없습니다.", "범위 지정 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                leftIndex = idx;
                rightIndex = -1;
                UpdateRangeLabel();
                AddLog($"[Set Left] {leftIndex}");
            }
            catch (Exception ex)
            {
                AddLog($"Set Left 오류: {ex.Message}");
            }
        }

        private void UpdateRangeLabel()
        {
            try
            {
                if (leftIndex < 0 || rightIndex < 0)
                {
                    int l = leftIndex >= 0 ? leftIndex : 0;
                    int r = rightIndex >= 0 ? rightIndex : 0;
                    lblRange.Text = $"[{l}, {r}]";
                    return;
                }

                lblRange.Text = $"[{leftIndex}, {rightIndex}]";
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
                    MessageBox.Show("삭제할 시작점과 끝점을 먼저 지정하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int l = leftIndex, r = rightIndex;
                if (rightIndex < leftIndex)
                {
                    MessageBox.Show("우측 지정값이 좌측 지정값보다 작아서 삭제할 수 없습니다.", "범위 지정 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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
                    double a = (angles.Count > i && angles[i] is double angle) ? angle : double.NaN;
                    double tval = (throttles.Count > i && throttles[i] is double throttle) ? throttle : double.NaN;
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
                    if (pilotAngles.Count > i) pilotAngles.RemoveAt(i);
                    if (pilotThrottles.Count > i) pilotThrottles.RemoveAt(i);
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

                for (int i = 0; i < tempPaths.Count; i++)
                {
                    AddLog($"[삭제 완료] {Path.GetFileName(tempPaths[i])}");
                }

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

                if (listBoxLog.SelectedIndices.Count == 0)
                {
                    MessageBox.Show("복원할 삭제 로그를 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRestoreIndices = listBoxLog.SelectedIndices
                    .Cast<int>()
                    .Where(i => i >= 0 && i < deletedImagePaths.Count)
                    .Distinct()
                    .OrderBy(i => deletedIndices.Count > i ? deletedIndices[i] : imagePaths.Count)
                    .ThenBy(i => i)
                    .ToList();

                if (selectedRestoreIndices.Count == 0)
                {
                    MessageBox.Show("선택한 삭제 로그와 일치하는 복원 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int firstRestoredIndex = -1;

                // Restore only the deleted frames selected in listBoxLog.
                foreach (int restoreIndex in selectedRestoreIndices)
                {
                    int idx = (restoreIndex < deletedIndices.Count) ? deletedIndices[restoreIndex] : imagePaths.Count;
                    idx = Math.Min(idx, imagePaths.Count);
                    imagePaths.Insert(idx, deletedImagePaths[restoreIndex]);
                    double a = deletedAngles[restoreIndex];
                    double t = deletedThrottles[restoreIndex];
                    if (double.IsNaN(a))
                        angles.Insert(idx, null);
                    else
                        angles.Insert(idx, a);

                    if (double.IsNaN(t))
                        throttles.Insert(idx, null);
                    else
                        throttles.Insert(idx, t);

                    pilotAngles.Insert(idx, null);
                    pilotThrottles.Insert(idx, null);

                    if (firstRestoredIndex < 0)
                        firstRestoredIndex = idx;
                }

                int restoredCount = selectedRestoreIndices.Count;

                // Remove restored entries from the visible delete log and restore buffers.
                foreach (int restoreIndex in selectedRestoreIndices.OrderByDescending(i => i))
                {
                    if (restoreIndex < listBoxLog.Items.Count)
                        listBoxLog.Items.RemoveAt(restoreIndex);
                    deletedImagePaths.RemoveAt(restoreIndex);
                    deletedAngles.RemoveAt(restoreIndex);
                    deletedThrottles.RemoveAt(restoreIndex);
                    deletedIndices.RemoveAt(restoreIndex);
                }

                RebuildListBoxFrames();
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                if (imagePaths.Count > 0)
                {
                    int sel = Math.Max(0, Math.Min(firstRestoredIndex, imagePaths.Count - 1));
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

