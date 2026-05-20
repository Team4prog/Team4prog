using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Team4prog.UI
{
    public partial class Form1 : Form
    {
        private List<string> imagePaths = new List<string>();
        private List<double?> angles = new List<double?>();
        private List<double?> throttles = new List<double?>();
        private int currentIndex = -1;

        public Form1()
        {
            InitializeComponent();

            // 이미지가 PictureBox 크기에 맞게 보이도록 설정
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;

            // 이벤트 핸들러 연결
            btnOpenFolder.Click += btnOpenFolder_Click;
            listBoxFrames.SelectedIndexChanged += listBoxFrames_SelectedIndexChanged;
            trackBarFrame.Scroll += trackBarFrame_Scroll;
        }

        // 테스트용 JSON 파일 자동 생성: 이미지 파일명에 맞춰 .json 파일 생성
        private void GenerateTestJson(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                AddLog($"JSON 생성 실패: 폴더가 존재하지 않음: {folderPath}");
                return;
            }

            var jsonFiles = Directory.EnumerateFiles(folderPath, "*.json").ToArray();
            if (jsonFiles.Length > 0)
            {
                AddLog("JSON 파일이 이미 존재하여 생성하지 않습니다.");
                return;
            }

            int created = 0;
            var rnd = new Random();

            foreach (var imgPath in imagePaths)
            {
                try
                {
                    var jsonPath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(imgPath) + ".json");

                    if (File.Exists(jsonPath))
                        continue;

                    // 랜덤 값 생성
                    double angle = Math.Round((rnd.NextDouble() * 2.0) - 1.0, 2); // -1.0 ~ 1.0
                    double throttle = Math.Round(rnd.NextDouble(), 2); // 0.0 ~ 1.0

                    var jobj = new JObject
                    {
                        ["user/angle"] = angle,
                        ["user/throttle"] = throttle
                    };

                    File.WriteAllText(jsonPath, jobj.ToString());
                    created++;
                }
                catch (Exception ex)
                {
                    AddLog($"테스트 JSON 생성 실패 ({Path.GetFileName(imgPath)}): {ex.Message}");
                }
            }

            AddLog($"테스트 JSON 생성 완료: {created}개");
        }

        // 폴더 선택 버튼 클릭
        private void btnOpenFolder_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "이미지 폴더를 선택하세요";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
                AddLog($"폴더 선택: {folder}");
                LoadImages(folder);
            }
        }

        // 지정 폴더에서 이미지 파일을 읽어서 리스트에 추가
        private void LoadImages(string folderPath)
        {
            try
            {
                // 기존 상태 초기화
                listBoxFrames.Items.Clear();
                imagePaths.Clear();
                currentIndex = -1;

                if (!Directory.Exists(folderPath))
                {
                    AddLog($"폴더가 존재하지 않음: {folderPath}");
                    return;
                }

                var files = Directory.EnumerateFiles(folderPath, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // 숫자 기준 정렬: 파일명 앞의 숫자를 추출하여 정렬. 추출 실패한 파일은 자연 정렬로 뒤에 위치.
                try
                {
                    files = files
                        .Select(f => new { Path = f, Num = ExtractLeadingNumber(f) })
                        .OrderBy(x => x.Num.HasValue ? 0 : 1) // 숫자 있는 것 먼저
                        .ThenBy(x => x.Num ?? int.MaxValue)
                        .ThenBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                        .Select(x => x.Path)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    AddLog($"파일 정렬 중 오류 발생: {ex.Message}. 기본 정렬 사용.");
                    files = files.OrderBy(f => f).ToArray();
                }

                if (files.Length == 0)
                {
                    AddLog("이미지 파일이 없습니다.");
                    trackBarFrame.Minimum = 0;
                    trackBarFrame.Maximum = 0;
                    return;
                }

                foreach (var f in files)
                {
                    imagePaths.Add(f);
                    listBoxFrames.Items.Add(Path.GetFileName(f));
                }

                // 트랙바 설정
                trackBarFrame.Minimum = 0;
                trackBarFrame.Maximum = Math.Max(0, imagePaths.Count - 1);
                trackBarFrame.Value = 0;

                // 첫 이미지 선택
                listBoxFrames.SelectedIndex = 0;

                AddLog($"이미지 로드 완료: {imagePaths.Count}개");
                // JSON 파일이 없으면 테스트용 JSON 생성 후 로드
                try
                {
                    GenerateTestJson(folderPath);
                }
                catch (Exception ex)
                {
                    AddLog($"테스트 JSON 생성 오류: {ex.Message}");
                }

                // JSON 데이터 로드 시도 (같은 폴더)
                LoadJsonData(folderPath);
            }
            catch (Exception ex)
            {
                AddLog($"이미지 로드 오류: {ex.Message}");
            }
        }

        // 파일 이름에서 선행 숫자를 추출합니다. 없으면 null 반환
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

        // 폴더의 JSON 파일을 읽어 angle, throttle 리스트에 저장
        private void LoadJsonData(string folderPath)
        {
            try
            {
                angles.Clear();
                throttles.Clear();

                if (!Directory.Exists(folderPath))
                {
                    AddLog($"JSON 폴더가 존재하지 않음: {folderPath}");
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
                    return;
                }

                foreach (var jf in jsonFiles)
                {
                    try
                    {
                        var txt = File.ReadAllText(jf);
                        var jobj = JObject.Parse(txt);

                        // 우선순위: key "user/angle" (슬래시 포함) -> nested user.angle -> fallback "angle"
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
                        // 파싱 실패한 경우 자리 유지 (null)
                        angles.Add(null);
                        throttles.Add(null);
                    }
                }

                AddLog($"JSON 로드 완료: {jsonFiles.Length}개");

                if (jsonFiles.Length != imagePaths.Count)
                {
                    AddLog($"경고: 이미지 개수({imagePaths.Count})와 JSON 개수({jsonFiles.Length})가 다릅니다.");
                }
            }
            catch (Exception ex)
            {
                AddLog($"JSON 로드 오류: {ex.Message}");
            }
        }

        // 인덱스에 해당하는 이미지 표시
        private void ShowImage(int index)
        {
            if (index < 0 || index >= imagePaths.Count)
            {
                AddLog($"잘못된 인덱스: {index}");
                return;
            }

            try
            {
                // 기존 이미지 Dispose 처리
                if (picFrame.Image != null)
                {
                    var old = picFrame.Image;
                    picFrame.Image = null;
                    old.Dispose();
                }

                // 파일을 잠그지 않도록 스트림으로 읽어 복사
                string path = imagePaths[index];
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var img = Image.FromStream(fs))
                {
                    picFrame.Image = new Bitmap(img);
                }

                currentIndex = index;
                lblFrame.Text = $"Frame: {currentIndex}";

                // angle/throttle 표시
                if (angles.Count > index && angles[index].HasValue)
                    lblAngle.Text = $"Angle: {angles[index].Value.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblAngle.Text = "Angle: N/A";

                if (throttles.Count > index && throttles[index].HasValue)
                    lblThrottle.Text = $"Throttle: {throttles[index].Value.ToString("F2", CultureInfo.InvariantCulture)}";
                else
                    lblThrottle.Text = "Throttle: N/A";

                AddLog($"이미지 표시: {Path.GetFileName(imagePaths[index])} (인덱스 {index})");
            }
            catch (Exception ex)
            {
                AddLog($"이미지 표시 오류: {ex.Message}");
            }
        }

        // 로그 추가 및 자동 스크롤
        private void AddLog(string message)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                listBoxLog.Items.Add($"[{timestamp}] {message}");
                // 최신 로그가 보이도록 스크롤
                if (listBoxLog.Items.Count > 0)
                {
                    listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
                }
            }
            catch
            {
                // 로그 실패는 무시
            }
        }

        // 리스트박스 선택 변경 이벤트
        private void listBoxFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0 && listBoxFrames.SelectedIndex < imagePaths.Count)
            {
                int idx = listBoxFrames.SelectedIndex;
                // 트랙바와 동기화
                if (trackBarFrame.Value != idx)
                    trackBarFrame.Value = idx;

                ShowImage(idx);
            }
        }

        // 트랙바 이동 이벤트
        private void trackBarFrame_Scroll(object? sender, EventArgs e)
        {
            int idx = trackBarFrame.Value;
            if (idx >= 0 && idx < imagePaths.Count)
            {
                // 리스트와 동기화 (리스트의 SelectedIndex를 설정하면 SelectedIndexChanged가 실행되어 ShowImage 호출)
                if (listBoxFrames.SelectedIndex != idx)
                    listBoxFrames.SelectedIndex = idx;
                else
                    ShowImage(idx);
            }
        }

        private void listBoxLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
