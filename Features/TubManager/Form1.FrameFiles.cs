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
    // Tub Manager frame IO: folder selection, image/JSON loading, frame display, and list/trackbar sync.
    public partial class Form1
    {
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (listBoxFrames.SelectedIndex < 0 || listBoxFrames.SelectedIndex >= imagePaths.Count)
                {
                    MessageBox.Show("Select an item to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show("정말 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                dr = MessageBox.Show("이미지와 연결된 JSON 파일이 실제로 삭제됩니다. 계속하시겠습니까?", "삭제 재확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                    return;

                int idx = listBoxFrames.SelectedIndex;
                string imagePath = imagePaths[idx];
                string imageFileName = Path.GetFileName(imagePath);

                // Delete the image file first; if this fails, leave UI state unchanged.
                try
                {
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    else
                    {
                        AddLog($"[삭제 경고] 이미지 파일이 이미 없습니다: {imageFileName}");
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("파일 삭제 권한이 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddLog($"[삭제 실패] 권한 오류 - {imageFileName}: {ex.Message}");
                    return;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("파일이 사용 중이거나 접근할 수 없습니다.\n" + ex.Message, "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddLog($"[삭제 실패] I/O 오류 - {imageFileName}: {ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    AddLog($"[삭제 실패] {imageFileName}: {ex.Message}");
                    return;
                }

                // Delete the paired JSON file when it exists.
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
                        // Continue because the image file was already deleted successfully.
                    }
                }

                // Remove the deleted frame from in-memory lists and visible controls.
                try
                {
                    imagePaths.RemoveAt(idx);
                    listBoxFrames.Items.RemoveAt(idx);

                    if (angles.Count > idx)
                        angles.RemoveAt(idx);
                    if (throttles.Count > idx)
                        throttles.RemoveAt(idx);

                    AddLog($"[삭제 완료] {imageFileName}");

                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);

                    if (imagePaths.Count == 0)
                    {
                        ClearImageDisplay();
                        trackBarFrame.Value = 0;
                    }
                    else
                    {
                        int newIdx = Math.Min(idx, imagePaths.Count - 1);
                        // Updating selection reuses the normal ShowImage path.
                        listBoxFrames.SelectedIndex = newIdx;
                        trackBarFrame.Value = newIdx;
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[삭제 실패] 목록 동기화 오류: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                AddLog($"[삭제 실패] 알 수 없는 오류: {ex.Message}");
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
                AddLog("Image display cleared.");
            }
            catch (Exception ex)
            {
                AddLog($"이미지 초기화 오류: {ex.Message}");
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
            dlg.Description = "이미지 폴더를 선택하세요.";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                AddLog($"폴더 선택: {folder}");
                LoadImages(folder);
            }
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
                deletedImagePaths.Clear();
                deletedAngles.Clear();
                deletedThrottles.Clear();
                deletedIndices.Clear();
                UpdateRangeLabel();

                // Clear driving values until JSON metadata is loaded.
                angles.Clear();
                throttles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show("선택한 폴더가 존재하지 않습니다.", "로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddLog($"폴더가 존재하지 않습니다: {folderPath}");
                    return;
                }

                currentFolder = folderPath;

                var files = Directory.EnumerateFiles(folderPath, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
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
                    AddLog($"파일 정렬 중 오류 발생: {ex.Message}. 기본 정렬을 사용합니다.");
                    files = files.OrderBy(f => f).ToArray();
                }

                if (files.Length == 0)
                {
                    MessageBox.Show("선택한 폴더에 jpg/png 이미지가 없습니다.", "로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AddLog("이미지 파일이 없습니다.");
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

                AddLog($"이미지 로드 완료: {imagePaths.Count}개");
                // Load matching driving metadata from JSON files in the same folder.
                LoadJsonData(folderPath);
                UpdateChart();
            }
            catch (Exception ex)
            {
                AddLog($"이미지 로드 오류: {ex.Message}");
            }
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
                AddLog($"숫자 추출 오류 ({Path.GetFileName(filePath)}): {ex.Message}");
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

                if (!Directory.Exists(folderPath))
                {
                    AddLog($"JSON 폴더가 존재하지 않습니다: {folderPath}");
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
                    while (angles.Count < imagePaths.Count)
                        angles.Add(null);
                    while (throttles.Count < imagePaths.Count)
                        throttles.Add(null);
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
                        // Keep list alignment even when one JSON file cannot be parsed.
                        angles.Add(null);
                        throttles.Add(null);
                    }
                }

                AddLog($"JSON 로드 완료: {jsonFiles.Length}개");

                if (jsonFiles.Length != imagePaths.Count)
                {
                    AddLog($"경고: 이미지 개수({imagePaths.Count})와 JSON 개수({jsonFiles.Length})가 다릅니다.");
                }

                // Keep metadata lists aligned with imagePaths so frame navigation can stay index-safe.
                while (angles.Count < imagePaths.Count)
                    angles.Add(null);
                while (throttles.Count < imagePaths.Count)
                    throttles.Add(null);

            }
            catch (Exception ex)
            {
                AddLog($"JSON 로드 오류: {ex.Message}");
            }
        }

        // Display the selected image and its matching angle/throttle values.
        private void ShowImage(int index)
        {
            if (index < 0 || index >= imagePaths.Count)
            {
                AddLog($"잘못된 인덱스: {index}");
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
                    AddLog($"이미지 파일이 없습니다: {path}");
                    return;
                }

                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var img = Image.FromStream(fs))
                {
                    picFrame.Image = new Bitmap(img);
                }

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

                AddLog($"이미지 표시: {Path.GetFileName(imagePaths[index])} (인덱스 {index})");
                HighlightCurrentIndex(index);
            }
            catch (FileNotFoundException ex)
            {
                AddLog($"이미지 파일 없음: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                AddLog($"이미지 접근 권한 오류: {ex.Message}");
            }
            catch (IOException ex)
            {
                AddLog($"이미지 I/O 오류: {ex.Message}");
            }
            catch (OutOfMemoryException ex)
            {
                AddLog($"이미지 형식 오류 또는 메모리 부족: {ex.Message}");
            }
            catch (Exception ex)
            {
                AddLog($"이미지 표시 오류: {ex.Message}");
            }
        }

        // Selecting a list item updates the trackbar and displayed frame.
        private void listBoxFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0 && listBoxFrames.SelectedIndex < imagePaths.Count)
            {
                int idx = listBoxFrames.SelectedIndex;
                if (trackBarFrame.Value != idx)
                    trackBarFrame.Value = idx;

                ShowImage(idx);
            }
        }

        // Moving the trackbar updates the list selection and displayed frame.
        private void trackBarFrame_Scroll(object? sender, EventArgs e)
        {
            int idx = trackBarFrame.Value;
            if (idx >= 0 && idx < imagePaths.Count)
            {
                // Changing SelectedIndex calls ShowImage through the list selection event.
                if (listBoxFrames.SelectedIndex != idx)
                    listBoxFrames.SelectedIndex = idx;
                else
                    ShowImage(idx);

                HighlightCurrentIndex(idx);
            }
        }
    }
}

