using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Team4prog.UI.Features.Catalog;
namespace Team4prog.UI
{
    // Tub Manager frame IO: folder selection, image/JSON loading, frame display, and list/trackbar sync.
    public partial class Form1
    {
        private bool isReverseBadgeVisible = false;
        private bool isPicFrameOverlayPaintHooked = false;

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (listBoxLog.SelectedIndices.Count > 0)
                {
                    PermanentlyDeleteSelectedTrashFrames();
                    return;
                }

                var selectedFrameIndices = listBoxFrames.SelectedIndices
                    .Cast<int>()
                    .Where(i => i >= 0 && i < imagePaths.Count)
                    .Distinct()
                    .OrderByDescending(i => i)
                    .ToList();

                if (selectedFrameIndices.Count == 0)
                {
                    MessageBox.Show("Select an item to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show($"선택한 {selectedFrameIndices.Count}개 프레임을 영구 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                dr = MessageBox.Show("이미지, 연결된 JSON 파일, catalog 항목이 실제로 삭제됩니다. 계속하시겠습니까?", "삭제 재확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                    return;

                int firstDeletedIndex = selectedFrameIndices.Min();
                int deletedCount = 0;

                foreach (int idx in selectedFrameIndices)
                {
                    string imagePath = imagePaths[idx];
                    string imageFileName = Path.GetFileName(imagePath);

                    if (!DeleteFrameFilesAndCatalog(imagePath, showMessageOnFailure: selectedFrameIndices.Count == 1))
                        continue;

                    imagePaths.RemoveAt(idx);
                    if (listBoxFrames.Items.Count > idx)
                        listBoxFrames.Items.RemoveAt(idx);
                    RemoveFrameFromFilterBackups(imagePath);
                    if (angles.Count > idx)
                        angles.RemoveAt(idx);
                    if (throttles.Count > idx)
                        throttles.RemoveAt(idx);
                    if (pilotAngles.Count > idx)
                        pilotAngles.RemoveAt(idx);
                    if (pilotThrottles.Count > idx)
                        pilotThrottles.RemoveAt(idx);

                    AddExceptionLog($"[영구 삭제 완료] {imageFileName}");
                    deletedCount++;
                }

                if (deletedCount == 0)
                    return;

                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                if (imagePaths.Count == 0)
                {
                    ClearImageDisplay();
                    trackBarFrame.Value = 0;
                }
                else
                {
                    int newIdx = Math.Min(firstDeletedIndex, imagePaths.Count - 1);
                    listBoxFrames.SelectedIndex = newIdx;
                    trackBarFrame.Value = newIdx;
                }
            }
            catch (Exception ex)
            {
                AddLog($"[삭제 실패] 알 수 없는 오류: {ex.Message}");
            }
        }

        private void PermanentlyDeleteSelectedTrashFrames()
        {
            var selectedTrashIndices = listBoxLog.SelectedIndices
                .Cast<int>()
                .Where(i => i >= 0 && i < deletedImagePaths.Count)
                .Distinct()
                .OrderByDescending(i => i)
                .ToList();

            if (selectedTrashIndices.Count == 0)
            {
                MessageBox.Show("영구 삭제할 휴지통 항목을 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dr = MessageBox.Show(
                $"휴지통에서 선택한 {selectedTrashIndices.Count}개 항목을 영구 삭제하시겠습니까?",
                "확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            dr = MessageBox.Show(
                "이미지, 연결된 JSON 파일, catalog 항목이 실제로 삭제됩니다. 계속하시겠습니까?",
                "삭제 재확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (dr != DialogResult.Yes)
                return;

            int deletedCount = 0;
            foreach (int trashIndex in selectedTrashIndices)
            {
                string imagePath = deletedImagePaths[trashIndex];
                if (!DeleteFrameFilesAndCatalog(imagePath, showMessageOnFailure: false))
                    continue;

                if (trashIndex < listBoxLog.Items.Count)
                    listBoxLog.Items.RemoveAt(trashIndex);
                deletedImagePaths.RemoveAt(trashIndex);
                deletedAngles.RemoveAt(trashIndex);
                deletedThrottles.RemoveAt(trashIndex);
                deletedIndices.RemoveAt(trashIndex);
                deletedCount++;
            }

            AddExceptionLog($"[휴지통 영구 삭제 완료] {deletedCount}개");
        }

        private bool DeleteFrameFilesAndCatalog(string imagePath, bool showMessageOnFailure)
        {
            string imageFileName = Path.GetFileName(imagePath);

            try
            {
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                else
                {
                    AddExceptionLog($"[삭제 경고] 이미지 파일이 이미 없습니다: {imageFileName}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                if (showMessageOnFailure)
                    MessageBox.Show("파일 삭제 권한이 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddExceptionLog($"[삭제 실패] 권한 오류 - {imageFileName}: {ex.Message}");
                return false;
            }
            catch (IOException ex)
            {
                if (showMessageOnFailure)
                    MessageBox.Show("파일이 사용 중이거나 접근할 수 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddExceptionLog($"[삭제 실패] I/O 오류 - {imageFileName}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                AddExceptionLog($"[삭제 실패] {imageFileName}: {ex.Message}");
                return false;
            }

            string jsonPath = Path.ChangeExtension(imagePath, ".json");
            if (File.Exists(jsonPath))
            {
                try
                {
                    File.Delete(jsonPath);
                }
                catch (Exception ex)
                {
                    AddExceptionLog($"[삭제 실패] {Path.GetFileName(jsonPath)}: {ex.Message}");
                }
            }

            RemoveImageFromCatalogs(imagePath);
            return true;
        }

        private bool HasTrashFrames()
        {
            return deletedImagePaths.Count > 0 || listBoxLog.Items.Count > 0;
        }

        private void ClearTrashFrames()
        {
            deletedImagePaths.Clear();
            deletedAngles.Clear();
            deletedThrottles.Clear();
            deletedIndices.Clear();
            listBoxLog.Items.Clear();
        }

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int virtualKey);

        private static bool IsLeftControlKeyDown()
        {
            return (GetKeyState((int)Keys.LControlKey) & 0x8000) != 0;
        }

        private void listBoxFrames_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int index = listBoxFrames.IndexFromPoint(e.Location);
            if (index < 0 || index >= listBoxFrames.Items.Count)
                return;

            if (!IsLeftControlKeyDown())
            {
                isPlainDraggingFrameSelection = true;
                lastPlainDraggedFrameIndex = -1;
                BeginInvoke(new Action(() => SelectSingleFrameOnly(index)));
                return;
            }

            isCtrlDraggingFrameSelection = true;
            ctrlDragStartFrameIndex = index;
            lastCtrlDraggedFrameIndex = -1;
            BeginInvoke(new Action(() => SelectFrameDragRange(index)));
        }

        private void listBoxFrames_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isPlainDraggingFrameSelection)
            {
                if (e.Button != MouseButtons.Left || IsLeftControlKeyDown())
                {
                    isPlainDraggingFrameSelection = false;
                    lastPlainDraggedFrameIndex = -1;
                    return;
                }

                int plainIndex = listBoxFrames.IndexFromPoint(e.Location);
                if (plainIndex >= 0 && plainIndex < listBoxFrames.Items.Count && plainIndex != lastPlainDraggedFrameIndex)
                    BeginInvoke(new Action(() => SelectSingleFrameOnly(plainIndex)));

                return;
            }

            if (!isCtrlDraggingFrameSelection)
                return;

            if (e.Button != MouseButtons.Left || !IsLeftControlKeyDown())
            {
                isCtrlDraggingFrameSelection = false;
                lastCtrlDraggedFrameIndex = -1;
                ctrlDragStartFrameIndex = -1;
                return;
            }

            int index = listBoxFrames.IndexFromPoint(e.Location);
            if (index < 0 || index >= listBoxFrames.Items.Count || index == lastCtrlDraggedFrameIndex)
                return;

            BeginInvoke(new Action(() => SelectFrameDragRange(index)));
        }

        private void listBoxFrames_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isCtrlDraggingFrameSelection)
            {
                int mouseIndex = listBoxFrames.IndexFromPoint(e.Location);
                int finalIndex = mouseIndex >= 0 && mouseIndex < listBoxFrames.Items.Count
                    ? mouseIndex
                    : lastCtrlDraggedFrameIndex >= 0 ? lastCtrlDraggedFrameIndex : ctrlDragStartFrameIndex;
                BeginInvoke(new Action(() =>
                {
                    SelectFrameDragRange(finalIndex);
                    ApplySelectedFrameRangeToDeleteRange();
                    ClearFrameDragSelectionState();
                }));
                return;
            }

            if (isPlainDraggingFrameSelection)
            {
                int finalIndex = listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : lastPlainDraggedFrameIndex;
                BeginInvoke(new Action(() =>
                {
                    SelectSingleFrameOnly(finalIndex);
                    ClearFrameDragSelectionState();
                }));
                return;
            }

            ClearFrameDragSelectionState();
        }

        private void ClearFrameDragSelectionState()
        {
            isPlainDraggingFrameSelection = false;
            isCtrlDraggingFrameSelection = false;
            lastPlainDraggedFrameIndex = -1;
            lastCtrlDraggedFrameIndex = -1;
            ctrlDragStartFrameIndex = -1;
        }

        private void SelectSingleFrameOnly(int index)
        {
            if (index < 0 || index >= listBoxFrames.Items.Count)
                return;

            isApplyingFrameDragSelection = true;
            try
            {
                listBoxFrames.BeginUpdate();
                listBoxFrames.ClearSelected();
                listBoxFrames.SetSelected(index, true);
            }
            finally
            {
                listBoxFrames.EndUpdate();
                isApplyingFrameDragSelection = false;
            }

            lastPlainDraggedFrameIndex = index;
        }

        private void SelectFrameDragRange(int index)
        {
            if (index < 0 || index >= listBoxFrames.Items.Count)
                return;

            int start = ctrlDragStartFrameIndex >= 0 ? ctrlDragStartFrameIndex : index;
            int l = Math.Min(start, index);
            int r = Math.Max(start, index);

            isApplyingFrameDragSelection = true;
            try
            {
                listBoxFrames.BeginUpdate();
                listBoxFrames.ClearSelected();

                for (int i = l; i <= r; i++)
                {
                    listBoxFrames.SetSelected(i, true);
                }
            }
            finally
            {
                listBoxFrames.EndUpdate();
                isApplyingFrameDragSelection = false;
            }

            lastCtrlDraggedFrameIndex = index;
        }

        private void ApplySelectedFrameRangeToDeleteRange()
        {
            var selectedIndices = listBoxFrames.SelectedIndices
                .Cast<int>()
                .Where(i => i >= 0 && i < imagePaths.Count)
                .ToList();

            if (selectedIndices.Count == 0)
                return;

            leftIndex = selectedIndices.Min();
            rightIndex = selectedIndices.Max();
            UpdateRangeLabel();
        }

        private void listBoxLog_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int index = listBoxLog.IndexFromPoint(e.Location);
            if (index < 0 || index >= listBoxLog.Items.Count)
                return;

            if (!IsLeftControlKeyDown())
            {
                isPlainDraggingTrashSelection = true;
                lastPlainDraggedTrashIndex = -1;
                BeginInvoke(new Action(() => SelectSingleTrashOnly(index)));
                return;
            }

            isCtrlDraggingTrashSelection = true;
            ctrlDragStartTrashIndex = index;
            lastCtrlDraggedTrashIndex = -1;
            BeginInvoke(new Action(() => SelectTrashDragRange(index)));
        }

        private void listBoxLog_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isPlainDraggingTrashSelection)
            {
                if (e.Button != MouseButtons.Left || IsLeftControlKeyDown())
                {
                    isPlainDraggingTrashSelection = false;
                    lastPlainDraggedTrashIndex = -1;
                    return;
                }

                int plainIndex = listBoxLog.IndexFromPoint(e.Location);
                if (plainIndex >= 0 && plainIndex < listBoxLog.Items.Count && plainIndex != lastPlainDraggedTrashIndex)
                    BeginInvoke(new Action(() => SelectSingleTrashOnly(plainIndex)));

                return;
            }

            if (!isCtrlDraggingTrashSelection)
                return;

            if (e.Button != MouseButtons.Left || !IsLeftControlKeyDown())
            {
                isCtrlDraggingTrashSelection = false;
                lastCtrlDraggedTrashIndex = -1;
                ctrlDragStartTrashIndex = -1;
                return;
            }

            int index = listBoxLog.IndexFromPoint(e.Location);
            if (index < 0 || index >= listBoxLog.Items.Count || index == lastCtrlDraggedTrashIndex)
                return;

            BeginInvoke(new Action(() => SelectTrashDragRange(index)));
        }

        private void listBoxLog_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isCtrlDraggingTrashSelection)
            {
                int mouseIndex = listBoxLog.IndexFromPoint(e.Location);
                int finalIndex = mouseIndex >= 0 && mouseIndex < listBoxLog.Items.Count
                    ? mouseIndex
                    : lastCtrlDraggedTrashIndex >= 0 ? lastCtrlDraggedTrashIndex : ctrlDragStartTrashIndex;
                BeginInvoke(new Action(() =>
                {
                    SelectTrashDragRange(finalIndex);
                    ClearTrashDragSelectionState();
                }));
                return;
            }

            if (isPlainDraggingTrashSelection)
            {
                int finalIndex = listBoxLog.SelectedIndex >= 0 ? listBoxLog.SelectedIndex : lastPlainDraggedTrashIndex;
                BeginInvoke(new Action(() =>
                {
                    SelectSingleTrashOnly(finalIndex);
                    ClearTrashDragSelectionState();
                }));
                return;
            }

            ClearTrashDragSelectionState();
        }

        private void ClearTrashDragSelectionState()
        {
            isPlainDraggingTrashSelection = false;
            isCtrlDraggingTrashSelection = false;
            lastPlainDraggedTrashIndex = -1;
            lastCtrlDraggedTrashIndex = -1;
            ctrlDragStartTrashIndex = -1;
        }

        private void SelectSingleTrashOnly(int index)
        {
            if (index < 0 || index >= listBoxLog.Items.Count)
                return;

            isApplyingTrashDragSelection = true;
            try
            {
                listBoxLog.BeginUpdate();
                listBoxLog.ClearSelected();
                listBoxLog.SetSelected(index, true);
            }
            finally
            {
                listBoxLog.EndUpdate();
                isApplyingTrashDragSelection = false;
            }

            lastPlainDraggedTrashIndex = index;
        }

        private void SelectTrashDragRange(int index)
        {
            if (index < 0 || index >= listBoxLog.Items.Count)
                return;

            int start = ctrlDragStartTrashIndex >= 0 ? ctrlDragStartTrashIndex : index;
            int l = Math.Min(start, index);
            int r = Math.Max(start, index);

            isApplyingTrashDragSelection = true;
            try
            {
                listBoxLog.BeginUpdate();
                listBoxLog.ClearSelected();

                for (int i = l; i <= r; i++)
                {
                    listBoxLog.SetSelected(i, true);
                }
            }
            finally
            {
                listBoxLog.EndUpdate();
                isApplyingTrashDragSelection = false;
            }

            lastCtrlDraggedTrashIndex = index;
        }

        private void RemoveFrameFromFilterBackups(string imagePath)
        {
            RemoveFrameFromListsByPath(imagePath, originalImagePaths, originalAngles, originalThrottles, originalPilotAngles, originalPilotThrottles);
            RemoveFrameFromListsByPath(imagePath, filteredImagePaths, filteredAngles, filteredThrottles, filteredPilotAngles, filteredPilotThrottles);
        }

        private static void RemoveFrameFromListsByPath(
            string imagePath,
            List<string> paths,
            List<double?> angleValues,
            List<double?> throttleValues,
            List<double?> pilotAngleValues,
            List<double?> pilotThrottleValues)
        {
            int idx = paths.FindIndex(path => string.Equals(path, imagePath, StringComparison.OrdinalIgnoreCase));
            if (idx < 0)
                return;

            paths.RemoveAt(idx);
            if (angleValues.Count > idx) angleValues.RemoveAt(idx);
            if (throttleValues.Count > idx) throttleValues.RemoveAt(idx);
            if (pilotAngleValues.Count > idx) pilotAngleValues.RemoveAt(idx);
            if (pilotThrottleValues.Count > idx) pilotThrottleValues.RemoveAt(idx);
        }

        private void RemoveImageFromCatalogs(string imagePath)
        {
            try
            {
                var catalogPaths = FindCatalogFilesForImage(imagePath).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                foreach (var catalogPath in catalogPaths)
                {
                    RemoveImageFromCatalog(catalogPath, imagePath);
                }
            }
            catch (Exception ex)
            {
                AddExceptionLog($"catalog 삭제 오류: {ex.Message}");
            }
        }

        private IEnumerable<string> FindCatalogFilesForImage(string imagePath)
        {
            var candidates = new List<string>();

            void AddCatalogsFrom(string? folder)
            {
                if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                    return;

                candidates.AddRange(Directory.EnumerateFiles(folder, "catalog_*.catalog", SearchOption.TopDirectoryOnly));

                var dataFolder = Path.Combine(folder, "data");
                if (Directory.Exists(dataFolder))
                    candidates.AddRange(Directory.EnumerateFiles(dataFolder, "catalog_*.catalog", SearchOption.TopDirectoryOnly));
            }

            AddCatalogsFrom(currentFolder);

            var imageFolder = Path.GetDirectoryName(imagePath);
            AddCatalogsFrom(imageFolder);

            var parentFolder = string.IsNullOrWhiteSpace(imageFolder) ? null : Directory.GetParent(imageFolder)?.FullName;
            AddCatalogsFrom(parentFolder);

            if (!string.IsNullOrWhiteSpace(parentFolder))
                AddCatalogsFrom(Path.Combine(parentFolder, "data"));

            return candidates;
        }

        private void RemoveImageFromCatalog(string catalogPath, string imagePath)
        {
            try
            {
                if (!File.Exists(catalogPath))
                    return;

                string catalogFolder = Path.GetDirectoryName(catalogPath) ?? "";
                string imagesDir = Path.Combine(catalogFolder, "images");
                string targetFullPath = Path.GetFullPath(imagePath);
                string targetFileName = Path.GetFileName(imagePath);

                var keptLines = new List<string>();
                int removed = 0;

                foreach (var line in File.ReadLines(catalogPath))
                {
                    if (CatalogLineMatchesImage(line, catalogFolder, imagesDir, targetFullPath, targetFileName))
                    {
                        removed++;
                        continue;
                    }

                    keptLines.Add(line);
                }

                if (removed == 0)
                    return;

                File.WriteAllLines(catalogPath, keptLines);
                AddExceptionLog($"[catalog 삭제] {Path.GetFileName(catalogPath)}에서 {removed}개 항목 제거");
            }
            catch (Exception ex)
            {
                AddExceptionLog($"[catalog 삭제 실패] {Path.GetFileName(catalogPath)}: {ex.Message}");
            }
        }

        private static bool CatalogLineMatchesImage(string line, string catalogFolder, string imagesDir, string targetFullPath, string targetFileName)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            try
            {
                var jobj = JObject.Parse(line);
                string? imagePathFromCatalog = jobj["cam/image_array"]?.ToString();
                if (string.IsNullOrWhiteSpace(imagePathFromCatalog))
                    return false;

                var resolved = ResolveCatalogImagePath(catalogFolder, imagesDir, imagePathFromCatalog);
                if (!string.IsNullOrWhiteSpace(resolved) &&
                    string.Equals(Path.GetFullPath(resolved), targetFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return string.Equals(Path.GetFileName(imagePathFromCatalog), targetFileName, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
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
                isReverseBadgeVisible = false;
                picFrame.Invalidate();
                lblFrame.Text = "Frame: N/A";
                lblAngle.Text = "Angle: N/A";
                lblThrottle.Text = "Throttle: N/A";
                AddExceptionLog("Image display cleared.");
            }
            catch (Exception ex)
            {
                AddExceptionLog($"이미지 초기화 오류: {ex.Message}");
            }
        }

        // Kept as a no-op so old callers do not recreate test JSON files unexpectedly.
        private void GenerateTestJson(string folderPath)
        {
            // Intentionally left blank to prevent automatic test JSON creation.
        }


        // Open a folder picker and load image/JSON data from the selected folder.
        private void btnOpenFolder_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "이미지(data)폴더를 선택하세요.";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                AddExceptionLog($"폴더 선택: {folder}");
                LoadImages(folder);
            }
        }

        private void Btn_showtrain_Click(object? sender, EventArgs e)
        {
            try
            {
                string initialDirectory = GetTubManagerModelSearchFolder();
                using var dialog = new OpenFileDialog();
                dialog.Title = "학습된 모델 선택";
                dialog.Filter = "Model files (*.h5;*.keras;*.tflite;*.onnx;*.pt;*.pkl)|*.h5;*.keras;*.tflite;*.onnx;*.pt;*.pkl|All files (*.*)|*.*";
                if (Directory.Exists(initialDirectory))
                    dialog.InitialDirectory = initialDirectory;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                selectedTrainedModelPath = NormalizeUserPath(dialog.FileName);
                showTrainedModelOverlay = true;
                Btn_showtrain.Text = "모델 표시";
                AddExceptionLog("[학습된 모델 선택] " + Path.GetFileName(selectedTrainedModelPath));

                if (!pilotAngles.Any(value => value.HasValue))
                    AddExceptionLog("[학습된 모델 표시] pilot 예측값이 없어 user angle/throttle 값으로 화살표를 표시합니다.");

                if (currentIndex >= 0 && currentIndex < imagePaths.Count)
                    ShowImage(currentIndex);
            }
            catch (Exception ex)
            {
                AddExceptionLog("[학습된 모델 선택 오류] " + ex.Message);
            }
        }

        private string GetTubManagerModelSearchFolder()
        {
            if (string.IsNullOrWhiteSpace(currentFolder))
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string normalizedFolder = NormalizeUserPath(currentFolder);
            string directModels = Path.Combine(normalizedFolder, "models");
            if (Directory.Exists(directModels))
                return directModels;

            DirectoryInfo? parent = Directory.GetParent(normalizedFolder);
            if (parent != null)
            {
                string siblingModels = Path.Combine(parent.FullName, "models");
                if (Directory.Exists(siblingModels))
                    return siblingModels;
            }

            return normalizedFolder;
        }

        // Reset current state and load all image frames from the folder.
        private void LoadImages(string folderPath)
        {
            try
            {
                // Clear previous session state before loading a new folder.
                listBoxFrames.Items.Clear();
                imagePaths.Clear();
                currentIndex = -1;
                leftIndex = -1;
                rightIndex = -1;
                ClearTrashFrames();
                UpdateRangeLabel();

                // Clear driving values until JSON metadata is loaded.
                angles.Clear();
                throttles.Clear();
                pilotAngles.Clear();
                pilotThrottles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show("선택한 폴더가 존재하지 않습니다.", "로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddExceptionLog($"폴더가 존재하지 않습니다: {folderPath}");
                    return;
                }

                currentFolder = folderPath;

                // Prefer DonkeyCar JSON Lines catalog when present (donkeyui style).
                var catalogPath = Path.Combine(folderPath, "catalog_0.catalog");
                if (File.Exists(catalogPath))
                {
                    if (LoadFromCatalogJsonl(folderPath, catalogPath))
                    {
                        AddExceptionLog("catalog_0.catalog 로드 완료");
                        return;
                    }
                    AddExceptionLog("catalog_0.catalog 로드 실패: 이미지 스캔 방식으로 fallback 합니다.");
                }

                // Fallback: scan image files. Prefer "images/" subfolder when present to avoid picking up unrelated PNGs.
                var imagesDir = Path.Combine(folderPath, "images");
                var scanRoot = Directory.Exists(imagesDir) ? imagesDir : folderPath;

                var files = Directory.EnumerateFiles(scanRoot, "*.*")
                    .Where(f =>
                        f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    // Avoid non-frame images (icons, thumbnails) when scanning the whole folder.
                    .Where(f => Directory.Exists(imagesDir) || Path.GetFileName(f).Contains("_cam-image_array_", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // Sort by leading numeric filename first, then by name for stable ordering.
                try
                {
                    files = files
                        .Select(f => new { Path = f, Num = ExtractLeadingNumber(f) })
                        .OrderBy(x => x.Num.HasValue ? 0 : 1)
                        .ThenBy(x => x.Num ?? int.MaxValue)
                        .ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                        .Select(x => x.Path)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    AddExceptionLog($"파일 정렬 중 오류 발생: {ex.Message}. 기본 정렬을 사용합니다.");
                    files = files.OrderBy(f => f).ToArray();
                }

                if (files.Length == 0)
                {
                    MessageBox.Show("선택한 폴더에 jpg/png 이미지가 없습니다.", "로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddExceptionLog("이미지 파일이 없습니다.");
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = 0;
                    ClearImageDisplay();
                    return;
                }

                foreach (var f in files)
                {
                    imagePaths.Add(f);
                    listBoxFrames.Items.Add(Path.GetFileName(f));
                }

                // Configure navigation controls for the loaded frame count.
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);
                trackBarFrame.Value = 0;

                listBoxFrames.SelectedIndex = 0;

                AddExceptionLog($"이미지 로드 완료: {imagePaths.Count}개");
                // Load matching driving metadata from JSON files only when the folder looks like per-frame JSON layout.
                // (Some folders contain unrelated JSON files like database.json.)
                if (Directory.EnumerateFiles(folderPath, "*.json").Any(f => Path.GetFileName(f).EndsWith(".json", StringComparison.OrdinalIgnoreCase) &&
                                                                         Path.GetFileName(f).Contains("_", StringComparison.OrdinalIgnoreCase) == false))
                {
                    // Heuristic: if JSON files are not frame-indexed, skip parsing to avoid noisy errors.
                    AddExceptionLog("JSON 메타데이터 파싱 스킵(프레임 JSON 레이아웃 아님)");
                }
                else
                {
                    LoadJsonData(folderPath);
                }
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddExceptionLog($"이미지 로드 오류: {ex.Message}");
            }
        }

        // Returns true if catalog load succeeded and produced at least one frame.
        private bool LoadFromCatalogJsonl(string folderPath, string catalogPath)
        {
            try
            {
                var frames = new List<FrameData>();
                foreach (var f in CatalogJsonlReader.ReadFrames(catalogPath))
                    frames.Add(f);

                if (frames.Count == 0)
                    return false;

                // Catalog order is usually already correct, but enforce index ordering for stability.
                frames.Sort((a, b) => a.Index.CompareTo(b.Index));

                var imagesDir = Path.Combine(folderPath, "images");
                int skippedMissing = 0;

                foreach (var frame in frames)
                {
                    if (string.IsNullOrWhiteSpace(frame.ImagePath))
                        continue;

                    // cam/image_array is typically a relative path like "images/123_cam-image_array_.jpg".
                    var abs = ResolveCatalogImagePath(folderPath, imagesDir, frame.ImagePath);
                    if (abs == null || !File.Exists(abs))
                    {
                        skippedMissing++;
                        continue;
                    }

                    imagePaths.Add(abs);
                    listBoxFrames.Items.Add(Path.GetFileName(abs));
                    angles.Add(frame.Angle);
                    throttles.Add(frame.Throttle);
                    pilotAngles.Add(frame.PilotAngle);
                    pilotThrottles.Add(frame.PilotThrottle);
                }

                if (imagePaths.Count == 0)
                    return false;

                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);
                trackBarFrame.Value = 0;
                listBoxFrames.SelectedIndex = 0;

                if (skippedMissing > 0)
                    AddExceptionLog($"catalog 경고: 이미지 누락으로 {skippedMissing}개 프레임 스킵");

                UpdateChart();
                return true;
            }
            catch (Exception ex)
            {
                AddExceptionLog($"catalog 파싱 오류: {ex.Message}");
                return false;
            }
        }

        private static string? ResolveCatalogImagePath(string folderPath, string imagesDir, string imagePathFromCatalog)
        {
            // If already absolute, use as-is.
            if (Path.IsPathRooted(imagePathFromCatalog))
                return imagePathFromCatalog;

            // Common case: "images/xxx.jpg" relative to tub folder.
            var combined = Path.Combine(folderPath, imagePathFromCatalog);
            if (File.Exists(combined))
                return combined;

            // Sometimes the catalog stores only filename; try under images/ as well.
            var fileName = Path.GetFileName(imagePathFromCatalog);
            if (!string.IsNullOrEmpty(fileName))
            {
                var underImages = Path.Combine(imagesDir, fileName);
                if (File.Exists(underImages))
                    return underImages;
            }

            return combined;
        }

        // Extract the leading number from a filename for natural tub-frame sorting.
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
                AddExceptionLog($"숫자 추출 오류 ({Path.GetFileName(filePath)}): {ex.Message}");
                return null;
            }
        }

        // Read angle/throttle values from JSON files and align them with the frame list order.
        private void LoadJsonData(string folderPath)
        {
            try
            {
                angles.Clear();
                throttles.Clear();
                pilotAngles.Clear();
                pilotThrottles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    AddExceptionLog($"JSON 폴더가 존재하지 않습니다: {folderPath}");
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
                    AddExceptionLog($"JSON 파일 정렬 중 오류: {ex.Message}");
                    jsonFiles = jsonFiles.OrderBy(f => f).ToArray();
                }

                if (jsonFiles.Length == 0)
                {
                    AddExceptionLog("JSON 파일이 없습니다.");
                    while (angles.Count < imagePaths.Count)
                        angles.Add(null);
                    while (throttles.Count < imagePaths.Count)
                        throttles.Add(null);
                    while (pilotAngles.Count < imagePaths.Count)
                        pilotAngles.Add(null);
                    while (pilotThrottles.Count < imagePaths.Count)
                        pilotThrottles.Add(null);
                    return;
                }

                foreach (var jf in jsonFiles.Take(imagePaths.Count))
                {
                    try
                    {
                        var txt = File.ReadAllText(jf);
                        var jobj = JObject.Parse(txt);

                        // Support DonkeyCar flat keys first, then nested user.angle/user.throttle, then simple fallback keys.
                        JToken? angleToken = null;
                        JToken? throttleToken = null;
                        JToken? pilotAngleToken = null;
                        JToken? pilotThrottleToken = null;

                        if (jobj.TryGetValue("user/angle", out var atok))
                            angleToken = atok;
                        else
                            angleToken = jobj.SelectToken("user.angle") ?? jobj["angle"];

                        if (jobj.TryGetValue("user/throttle", out var ttok))
                            throttleToken = ttok;
                        else
                            throttleToken = jobj.SelectToken("user.throttle") ?? jobj["throttle"];

                        if (jobj.TryGetValue("pilot/angle", out var patok))
                            pilotAngleToken = patok;
                        else
                            pilotAngleToken = jobj.SelectToken("pilot.angle") ?? jobj["pilot_angle"];

                        if (jobj.TryGetValue("pilot/throttle", out var pttok))
                            pilotThrottleToken = pttok;
                        else
                            pilotThrottleToken = jobj.SelectToken("pilot.throttle") ?? jobj["pilot_throttle"];

                        double? ang = null;
                        double? thr = null;
                        double? pilotAng = null;
                        double? pilotThr = null;

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

                        if (pilotAngleToken != null)
                        {
                            if (double.TryParse(pilotAngleToken.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var pa))
                                pilotAng = pa;
                        }

                        if (pilotThrottleToken != null)
                        {
                            if (double.TryParse(pilotThrottleToken.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var pt))
                                pilotThr = pt;
                        }

                        angles.Add(ang);
                        throttles.Add(thr);
                        pilotAngles.Add(pilotAng);
                        pilotThrottles.Add(pilotThr);
                    }
                    catch (Exception ex)
                    {
                        AddExceptionLog($"JSON 파싱 오류 ({Path.GetFileName(jf)}): {ex.Message}");
                        // Keep list alignment even when one JSON file cannot be parsed.
                        angles.Add(null);
                        throttles.Add(null);
                        pilotAngles.Add(null);
                        pilotThrottles.Add(null);
                    }
                }

                AddExceptionLog($"JSON 로드 완료: {jsonFiles.Length}개");

                if (jsonFiles.Length != imagePaths.Count)
                {
                    AddExceptionLog($"경고: 이미지 개수({imagePaths.Count})와 JSON 개수({jsonFiles.Length})가 다릅니다.");
                }

                // Keep metadata lists aligned with imagePaths so frame navigation can stay index-safe.
                while (angles.Count < imagePaths.Count)
                    angles.Add(null);
                while (throttles.Count < imagePaths.Count)
                    throttles.Add(null);
                while (pilotAngles.Count < imagePaths.Count)
                    pilotAngles.Add(null);
                while (pilotThrottles.Count < imagePaths.Count)
                    pilotThrottles.Add(null);

            }
            catch (Exception ex)
            {
                AddExceptionLog($"JSON 로드 오류: {ex.Message}");
            }
        }

        // Display the selected image and its matching angle/throttle values.
        private void ShowImage(int index)
        {
            if (index < 0 || index >= imagePaths.Count)
            {
                AddExceptionLog($"잘못된 인덱스: {index}");
                return;
            }

            try
            {
                // Dispose the previous bitmap to avoid locking files and leaking GDI handles.
                if (picFrame.Image != null)
                {
                    var old = picFrame.Image;
                    picFrame.Image = null;
                    old.Dispose();
                }

                // Load via stream and copy to a bitmap so the source file is not kept locked.
                string path = imagePaths[index];
                if (!File.Exists(path))
                {
                    AddExceptionLog($"이미지 파일이 없습니다: {path}");
                    return;
                }

                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var img = Image.FromStream(fs))
                {
                    picFrame.Image = CreateFramePreviewBitmap(img, index);
                }

                EnsurePicFrameOverlayPaintHooked();

                currentIndex = index;
                lblFrame.Text = $"Frame: {currentIndex}";

                // Display driving values when JSON metadata exists for this frame.
                if (angles.Count > index && angles[index] is double angle)
                    lblAngle.Text = $"Angle: {angle.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblAngle.Text = "Angle: N/A";

                if (throttles.Count > index && throttles[index] is double throttle)
                    lblThrottle.Text = $"Throttle: {throttle.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblThrottle.Text = "Throttle: N/A";

                isReverseBadgeVisible = throttles.Count > index && throttles[index] is double throttleValue && throttleValue < 0;
                picFrame.Invalidate();

                HighlightCurrentIndex(index);
            }
            catch (FileNotFoundException ex)
            {
                AddExceptionLog($"이미지 파일 없음: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                AddExceptionLog($"이미지 접근 권한 오류: {ex.Message}");
            }
            catch (IOException ex)
            {
                AddExceptionLog($"이미지 I/O 오류: {ex.Message}");
            }
            catch (OutOfMemoryException ex)
            {
                AddExceptionLog($"이미지 형식 오류 또는 메모리 부족: {ex.Message}");
            }
            catch (Exception ex)
            {
                AddExceptionLog($"이미지 표시 오류: {ex.Message}");
            }
        }

        private Bitmap CreateFramePreviewBitmap(Image sourceImage, int index)
        {
            var preview = new Bitmap(sourceImage);

            if (angles.Count > index && angles[index] is double angle)
                DrawDrivingPathOverlay(preview, angle);

            DrawTrainedModelArrowOverlay(preview, index);
            return preview;
        }

        private void DrawDrivingPathOverlay(Bitmap frame, double angle)
        {
            if (frame.Width <= 0 || frame.Height <= 0)
                return;

            float startX = frame.Width * 0.50f;
            float startY = frame.Height * 0.98f;
            float endY = frame.Height * 0.30f;

            double clampedAngle = Math.Max(-1.0, Math.Min(1.0, angle));
            float maxSteerOffset = frame.Width * 0.32f;
            float endX = startX + (float)(clampedAngle * maxSteerOffset);

            using var g = Graphics.FromImage(frame);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float lineWidth = Math.Max(4f, frame.Width / 120f);
            using var centerPen = new Pen(Color.Lime, lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            g.DrawLine(centerPen, startX, startY, startX, endY);
        }

        private void DrawTrainedModelArrowOverlay(Bitmap frame, int index)
        {
            if (!showTrainedModelOverlay || string.IsNullOrWhiteSpace(selectedTrainedModelPath))
                return;

            double? modelAngle = GetModelOverlayValue(pilotAngles, angles, index);
            double? modelThrottle = GetModelOverlayValue(pilotThrottles, throttles, index);
            if (!modelAngle.HasValue)
                return;

            double clampedAngle = Math.Max(-1.0, Math.Min(1.0, modelAngle.Value));
            bool isReverse = modelThrottle.HasValue && modelThrottle.Value < 0;

            float startX = frame.Width * 0.50f;
            float startY = isReverse ? frame.Height * 0.48f : frame.Height * 0.78f;
            float endY = isReverse ? frame.Height * 0.70f : frame.Height * 0.56f;
            float endX = startX + (float)(clampedAngle * frame.Width * 0.10f);

            using var g = Graphics.FromImage(frame);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float lineWidth = Math.Max(3f, frame.Width / 220f);
            using var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(lineWidth * 0.9f, lineWidth * 1.2f, true);
            using var arrowPen = new Pen(Color.Orange, lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                CustomEndCap = arrowCap
            };

            g.DrawLine(arrowPen, startX, startY, endX, endY);
        }

        private static double? GetModelOverlayValue(List<double?> primaryValues, List<double?> fallbackValues, int index)
        {
            if (primaryValues.Count > index && primaryValues[index].HasValue)
                return primaryValues[index];

            if (fallbackValues.Count > index && fallbackValues[index].HasValue)
                return fallbackValues[index];

            return null;
        }

        private void DrawSteeringWheelOverlay(Graphics g, RectangleF bounds, double angle)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            float wheelSize = Math.Min(bounds.Width, bounds.Height);
            float centerX = bounds.X + bounds.Width / 2f;
            float centerY = bounds.Y + bounds.Height / 2f;
            float radius = wheelSize / 2f;

            var state = g.Save();
            g.TranslateTransform(centerX, centerY);
            g.RotateTransform((float)(angle * 90.0));

            using var shadowBrush = new SolidBrush(Color.FromArgb(55, 0, 0, 0));
            g.FillEllipse(shadowBrush, -radius - 3, -radius + 2, wheelSize + 6, wheelSize + 6);

            using var innerBrush = new SolidBrush(Color.FromArgb(235, 245, 248, 250));
            g.FillEllipse(innerBrush, -radius, -radius, wheelSize, wheelSize);

            using var rimPen = new Pen(Color.FromArgb(45, 55, 65), Math.Max(7f, wheelSize / 12f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            using var gripPen = new Pen(Color.FromArgb(35, 45, 55), Math.Max(6f, wheelSize / 14f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            using var spokePen = new Pen(Color.FromArgb(0, 145, 210), Math.Max(4f, wheelSize / 18f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            using var markerPen = new Pen(Color.FromArgb(230, 30, 45, 55), Math.Max(3f, wheelSize / 28f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            using var hubBrush = new SolidBrush(Color.FromArgb(245, 30, 38, 45));

            g.DrawEllipse(rimPen, -radius, -radius, wheelSize, wheelSize);
            g.DrawLine(gripPen, -radius + wheelSize * 0.16f, -wheelSize * 0.04f, radius - wheelSize * 0.16f, -wheelSize * 0.04f);
            g.DrawLine(spokePen, 0, 0, -radius + wheelSize * 0.23f, -wheelSize * 0.04f);
            g.DrawLine(spokePen, 0, 0, radius - wheelSize * 0.23f, -wheelSize * 0.04f);
            g.DrawLine(spokePen, 0, 0, 0, radius - wheelSize * 0.20f);
            g.DrawLine(markerPen, 0, -radius + wheelSize * 0.08f, 0, -radius + wheelSize * 0.25f);
            g.FillEllipse(hubBrush, -wheelSize * 0.15f, -wheelSize * 0.15f, wheelSize * 0.30f, wheelSize * 0.30f);

            g.Restore(state);
        }

        private void EnsurePicFrameOverlayPaintHooked()
        {
            if (isPicFrameOverlayPaintHooked)
                return;

            picFrame.Paint += PicFrame_Paint;
            isPicFrameOverlayPaintHooked = true;
        }

        private void PicFrame_Paint(object? sender, PaintEventArgs e)
        {
            if (picFrame.Image == null)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (currentIndex >= 0 && angles.Count > currentIndex && angles[currentIndex] is double angle)
                DrawSteeringWheelOverlay(e.Graphics, GetSteeringWheelBounds(), Math.Max(-1.0, Math.Min(1.0, angle)));

            if (isReverseBadgeVisible)
                DrawReverseBadge(e.Graphics, GetReverseBadgeBounds());
        }

        private RectangleF GetSteeringWheelBounds()
        {
            Image? image = picFrame.Image;
            if (image == null)
                return RectangleF.Empty;

            Rectangle imageRect = GetZoomedImageBounds(picFrame.ClientSize, image.Size);
            float margin = Math.Max(14f, Math.Min(picFrame.ClientSize.Width, picFrame.ClientSize.Height) * 0.035f);
            float rightBlankLeft = imageRect.Right + margin;
            float rightBlankWidth = picFrame.ClientSize.Width - rightBlankLeft - margin;
            float wheelSize = Math.Max(70f, Math.Min(130f, Math.Min(rightBlankWidth, imageRect.Height * 0.30f)));

            float x = rightBlankWidth >= wheelSize
                ? rightBlankLeft + (rightBlankWidth - wheelSize) / 2f
                : picFrame.ClientSize.Width - wheelSize - margin;
            float y = imageRect.Top + imageRect.Height * 0.52f;

            if (y + wheelSize > picFrame.ClientSize.Height - margin)
                y = picFrame.ClientSize.Height - wheelSize - margin;

            return new RectangleF(x, y, wheelSize, wheelSize);
        }

        private RectangleF GetReverseBadgeBounds()
        {
            string text = "후진";
            Image? image = picFrame.Image;
            if (image == null)
                return RectangleF.Empty;

            Rectangle imageRect = GetZoomedImageBounds(picFrame.ClientSize, image.Size);
            float fontSize = Math.Max(28f, Math.Min(picFrame.ClientSize.Width, picFrame.ClientSize.Height) * 0.08f);

            using var font = new Font("Malgun Gothic", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            Size textSize = TextRenderer.MeasureText(text, font);
            float paddingX = fontSize * 0.45f;
            float paddingY = fontSize * 0.25f;
            float badgeWidth = textSize.Width + paddingX * 2f;
            float badgeHeight = textSize.Height + paddingY * 2f;
            float margin = Math.Max(14f, fontSize * 0.35f);
            float rightBlankLeft = imageRect.Right + margin;
            float x = rightBlankLeft + badgeWidth <= picFrame.ClientSize.Width - margin
                ? rightBlankLeft
                : picFrame.ClientSize.Width - badgeWidth - margin;
            float y = Math.Max(14f, fontSize * 0.35f);

            return new RectangleF(x, y, badgeWidth, badgeHeight);
        }

        private void DrawReverseBadge(Graphics g, RectangleF rect)
        {
            string text = "후진";
            float fontSize = Math.Max(28f, Math.Min(picFrame.ClientSize.Width, picFrame.ClientSize.Height) * 0.08f);
            float paddingX = fontSize * 0.45f;
            float paddingY = fontSize * 0.25f;

            using var backgroundBrush = new SolidBrush(Color.FromArgb(225, 220, 40, 40));
            using var borderPen = new Pen(Color.White, Math.Max(2f, fontSize / 12f));
            using var textBrush = new SolidBrush(Color.White);
            using var font = new Font("Malgun Gothic", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            g.FillRectangle(backgroundBrush, rect);
            g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);
            g.DrawString(text, font, textBrush, rect.X + paddingX, rect.Y + paddingY);
        }

        private static Rectangle GetZoomedImageBounds(Size clientSize, Size imageSize)
        {
            if (clientSize.Width <= 0 || clientSize.Height <= 0 || imageSize.Width <= 0 || imageSize.Height <= 0)
                return Rectangle.Empty;

            double ratio = Math.Min(
                clientSize.Width / (double)imageSize.Width,
                clientSize.Height / (double)imageSize.Height);

            int width = (int)Math.Round(imageSize.Width * ratio);
            int height = (int)Math.Round(imageSize.Height * ratio);
            int x = (clientSize.Width - width) / 2;
            int y = (clientSize.Height - height) / 2;
            return new Rectangle(x, y, width, height);
        }

        // Selecting a list item updates the trackbar and displayed frame.
        private void listBoxFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isPlainDraggingFrameSelection && !isApplyingFrameDragSelection && listBoxFrames.SelectedIndex >= 0)
            {
                int plainIndex = listBoxFrames.SelectedIndex;
                BeginInvoke(new Action(() => SelectSingleFrameOnly(plainIndex)));
            }

            if (listBoxFrames.SelectedIndex >= 0 && listBoxFrames.SelectedIndex < imagePaths.Count)
            {
                int idx = listBoxFrames.SelectedIndex;
                if (trackBarFrame.Value != idx)
                    trackBarFrame.Value = idx;

                ShowImage(idx);
            }
        }

        private void listBoxLog_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (isPlainDraggingTrashSelection && !isApplyingTrashDragSelection && listBoxLog.SelectedIndex >= 0)
            {
                int plainIndex = listBoxLog.SelectedIndex;
                BeginInvoke(new Action(() => SelectSingleTrashOnly(plainIndex)));
            }

        }

        // Moving the trackbar updates the list selection and displayed frame.
        private void trackBarFrame_Scroll(object? sender, EventArgs e)
        {
            int idx = trackBarFrame.Value;
            if (idx >= 0 && idx < imagePaths.Count)
            {
                SetPlaybackFrame(idx);
            }
        }
    }
}
