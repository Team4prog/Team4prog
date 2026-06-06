using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Team4prog.UI
{
    public partial class Form1
    {
        // Train 버튼 차단 조건: 범위 삭제 등으로 휴지통에 남은 데이터가 있으면 학습 시작 금지
        private bool HasTrashFramesForTrainingBlock()
        {
            return deletedImagePaths.Count > 0 || listBoxLog.Items.Count > 0;
        }

        private void ShowTrashTrainingBlockedMessage()
        {
            string message =
                "휴지통에 삭제 대기 중인 데이터가 남아 있어서 학습을 시작할 수 없습니다.\n\n" +
                $"남은 휴지통 데이터: {deletedImagePaths.Count}개\n\n" +
                "Tub Manager 화면에서 휴지통 항목을 선택한 뒤 좌상단 Delete 버튼으로 " +
                "이미지 파일, JSON, catalog 항목까지 완전히 삭제하거나 Restore로 복구한 후 다시 학습하세요.";

            MessageBox.Show(message, "학습 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AddTrainLog($"[학습 차단] 휴지통 데이터 {deletedImagePaths.Count}개가 남아 있어 학습을 시작하지 않았습니다.");
        }

        // 좌상단 Delete 버튼에서 먼저 호출: listBoxLog에서 선택한 휴지통 항목을 실제 파일 + json + catalog까지 영구 삭제
        private bool DeleteSelectedTrashFramesPermanently()
        {
            if (listBoxLog.SelectedIndices.Count == 0)
                return false;

            var selectedTrashIndices = listBoxLog.SelectedIndices
                .Cast<int>()
                .Where(i => i >= 0 && i < deletedImagePaths.Count)
                .Distinct()
                .OrderByDescending(i => i)
                .ToList();

            if (selectedTrashIndices.Count == 0)
            {
                MessageBox.Show("선택한 휴지통 항목과 실제 삭제 데이터가 일치하지 않습니다.", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            var dr = MessageBox.Show(
                $"휴지통에서 선택한 {selectedTrashIndices.Count}개 데이터를 실제 파일, JSON, catalog에서 완전히 삭제할까요?\n\n이 작업은 복구할 수 없습니다.",
                "휴지통 영구 삭제 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr != DialogResult.Yes)
                return true;

            int successCount = 0;
            int failCount = 0;

            foreach (int trashIndex in selectedTrashIndices)
            {
                string imagePath = deletedImagePaths[trashIndex];

                if (DeleteFrameFilesAndCatalogPermanently(imagePath))
                {
                    RemoveTrashBufferAt(trashIndex);
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }

            MessageBox.Show(
                $"휴지통 영구 삭제 완료\n성공: {successCount}개\n실패: {failCount}개",
                "삭제 결과",
                MessageBoxButtons.OK,
                failCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            return true;
        }

        // 좌상단 Delete 버튼에서 휴지통 선택이 없을 때 호출: 현재 프레임 목록에서 선택된 사진을 실제 파일 + json + catalog까지 영구 삭제
        private bool DeleteSelectedLoadedFramesPermanently()
        {
            var selectedFrameIndices = GetSelectedFrameIndicesForPermanentDelete();

            if (selectedFrameIndices.Count == 0)
                return false;

            var dr = MessageBox.Show(
                $"현재 불러온 프레임 {selectedFrameIndices.Count}개를 실제 파일, JSON, catalog에서 완전히 삭제할까요?\n\n이 작업은 복구할 수 없습니다.",
                "프레임 영구 삭제 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr != DialogResult.Yes)
                return true;

            int successCount = 0;
            int failCount = 0;
            int preferredIndex = selectedFrameIndices.Min();

            // 큰 인덱스부터 지워야 앞쪽 인덱스가 밀리지 않음
            foreach (int idx in selectedFrameIndices.OrderByDescending(i => i))
            {
                if (idx < 0 || idx >= imagePaths.Count)
                    continue;

                string imagePath = imagePaths[idx];

                if (DeleteFrameFilesAndCatalogPermanently(imagePath))
                {
                    RemoveActiveFrameAt(idx);
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }

            RefreshFrameListAfterPermanentDelete(preferredIndex);

            MessageBox.Show(
                $"프레임 영구 삭제 완료\n성공: {successCount}개\n실패: {failCount}개",
                "삭제 결과",
                MessageBoxButtons.OK,
                failCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            return true;
        }

        private List<int> GetSelectedFrameIndicesForPermanentDelete()
        {
            var selected = listBoxFrames.SelectedIndices
                .Cast<int>()
                .Where(i => i >= 0 && i < imagePaths.Count)
                .Distinct()
                .ToList();

            if (selected.Count > 0)
                return selected;

            if (listBoxFrames.SelectedIndex >= 0 && listBoxFrames.SelectedIndex < imagePaths.Count)
                return new List<int> { listBoxFrames.SelectedIndex };

            if (currentIndex >= 0 && currentIndex < imagePaths.Count)
                return new List<int> { currentIndex };

            return new List<int>();
        }

        private bool DeleteFrameFilesAndCatalogPermanently(string imagePath)
        {
            try
            {
                if (picFrame.Image != null && currentIndex >= 0 && currentIndex < imagePaths.Count)
                {
                    string currentPath = imagePaths[currentIndex];
                    if (string.Equals(Path.GetFullPath(currentPath), Path.GetFullPath(imagePath), StringComparison.OrdinalIgnoreCase))
                    {
                        ClearImageDisplay();
                    }
                }

                string imageFileName = Path.GetFileName(imagePath);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                else
                {
                    AddExceptionLog($"[삭제 경고] 이미지 파일이 이미 없습니다: {imageFileName}");
                }

                string jsonPath = Path.ChangeExtension(imagePath, ".json");
                if (File.Exists(jsonPath))
                {
                    File.Delete(jsonPath);
                }

                RemoveImageFromCatalogs(imagePath);
                AddExceptionLog($"[영구 삭제 완료] {imageFileName}");
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("파일 삭제 권한이 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddExceptionLog($"[삭제 실패] 권한 오류 - {Path.GetFileName(imagePath)}: {ex.Message}");
                return false;
            }
            catch (IOException ex)
            {
                MessageBox.Show("파일이 사용 중이거나 접근할 수 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddExceptionLog($"[삭제 실패] I/O 오류 - {Path.GetFileName(imagePath)}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("삭제 중 오류가 발생했습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddExceptionLog($"[삭제 실패] {Path.GetFileName(imagePath)}: {ex.Message}");
                return false;
            }
        }

        private void RemoveTrashBufferAt(int trashIndex)
        {
            if (trashIndex >= 0 && trashIndex < listBoxLog.Items.Count)
                listBoxLog.Items.RemoveAt(trashIndex);

            if (trashIndex >= 0 && trashIndex < deletedImagePaths.Count)
                deletedImagePaths.RemoveAt(trashIndex);

            if (trashIndex >= 0 && trashIndex < deletedAngles.Count)
                deletedAngles.RemoveAt(trashIndex);

            if (trashIndex >= 0 && trashIndex < deletedThrottles.Count)
                deletedThrottles.RemoveAt(trashIndex);

            if (trashIndex >= 0 && trashIndex < deletedIndices.Count)
                deletedIndices.RemoveAt(trashIndex);
        }

        private void RemoveActiveFrameAt(int idx)
        {
            if (idx >= 0 && idx < imagePaths.Count)
                imagePaths.RemoveAt(idx);

            if (idx >= 0 && idx < angles.Count)
                angles.RemoveAt(idx);

            if (idx >= 0 && idx < throttles.Count)
                throttles.RemoveAt(idx);

            if (idx >= 0 && idx < pilotAngles.Count)
                pilotAngles.RemoveAt(idx);

            if (idx >= 0 && idx < pilotThrottles.Count)
                pilotThrottles.RemoveAt(idx);
        }

        private void RefreshFrameListAfterPermanentDelete(int preferredIndex)
        {
            RebuildListBoxFrames();

            trackBarFrame.Minimum = 0;
            trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

            if (imagePaths.Count == 0)
            {
                ClearImageDisplay();
                trackBarFrame.Value = 0;
            }
            else
            {
                int newIdx = Math.Max(0, Math.Min(preferredIndex, imagePaths.Count - 1));
                listBoxFrames.SelectedIndex = newIdx;
                trackBarFrame.Value = newIdx;
            }

            UpdateChart();
        }
    }
}
