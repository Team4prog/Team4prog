using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Team4prog.UI
{
    // Trainer view actions: select a DonkeyCar folder and run the training command.
    public partial class Form1
    {
        private Process? trainingProcess = null;
        private bool trainingStopRequested = false;

        private enum TrainingPathMode
        {
            LocalWindows,
            WslUnc,
            WslLinux
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            if (isTrainingRunning)
            {
                StopTrainingProcess();
                return;
            }

            await RunTraining();
        }

        private void StopTrainingProcess()
        {
            try
            {
                trainingStopRequested = true;

                if (trainingProcess != null && !trainingProcess.HasExited)
                {
                    trainingProcess.Kill(entireProcessTree: true);
                    AddTrainLog("[학습 중지 요청]");
                }
            }
            catch (Exception ex)
            {
                AddTrainLog("[학습 중지 오류] " + ex.Message);
            }
        }

        private async Task RunTraining()
        {
            if (isTrainingRunning)
            {
                MessageBox.Show("이미 학습 프로세스가 실행 중입니다.");
                return;
            }

            if (HasTrashFramesForTrainingBlock())
            {
                ShowTrashTrainingBlockedMessage();
                return;
            }

            try
            {
                string modelType = cmbModelType.Text;

                if (string.IsNullOrEmpty(modelType))
                {
                    modelType = "linear";
                    cmbModelType.SelectedItem = modelType;
                    AddTrainLog("[학습] 모델 타입이 비어 있어 linear로 설정했습니다.");
                }

                if (string.IsNullOrEmpty(carFolderPath))
                {
                    MessageBox.Show("먼저 폴더를 선택하세요.");
                    return;
                }

                string workingDir = NormalizeUserPath(carFolderPath);
                TrainingPathMode pathMode = GetTrainingPathMode(workingDir);

                if (!DirectoryExistsForTrainingPath(workingDir, pathMode))
                {
                    MessageBox.Show("선택한 폴더가 존재하지 않습니다.\n\n" + workingDir);
                    return;
                }

                if (!TrainingFileExists(workingDir, pathMode, "manage.py"))
                {
                    MessageBox.Show("선택한 폴더에서 manage.py를 찾을 수 없습니다.\n\n" + workingDir);
                    return;
                }

                ResetLossChart();
                // FixTrainerLayout();

                string modelArgument = ResolveTrainingModelArgument(workingDir, modelType, pathMode);
                var modelBackups = BackupExistingLocalModelFiles(workingDir, pathMode);
                DateTime trainingStartedAt = DateTime.Now;
                var psi = BuildTrainingStartInfo(workingDir, modelType, modelArgument, pathMode);

                using var process = new Process();
                process.StartInfo = psi;
                var trainingOutput = new List<string>();

                trainingProcess = process;
                trainingStopRequested = false;

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        trainingOutput.Add(e.Data);
                        this.Invoke(new Action(() =>
                        {
                            AddTrainLog(e.Data);   
                            ParseLossLine(e.Data);
                        }));
                    }
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        trainingOutput.Add(e.Data);
                        this.Invoke(new Action(() =>
                        {
                            AddTrainLog("[학습 오류] " + e.Data);
                            ParseLossLine(e.Data);
                        }));
                    }
                };

                isTrainingRunning = true;
                btnTrain.Enabled = true;
                btnTrain.Text = "Stop";
                btnTrain.BackColor = Color.FromArgb(255, 128, 128);
                AddTrainLog("[학습 시작]");

                if (!process.Start())
                {
                    MessageBox.Show("학습 프로세스를 시작하지 못했습니다.");
                    return;
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await Task.Run(() => process.WaitForExit());

                string? logPath = WriteTrainingLog(trainingOutput);
                EnsureTrainingModelOutput(modelArgument, workingDir, pathMode, trainingStartedAt);
                RestoreLocalModelBackups(modelBackups, modelArgument);

                if (trainingStopRequested)
                {
                    MessageBox.Show("학습이 중단되었지만 모델이 생성되었습니다.");
                    LoadModelList();
                    AddTrainLog("[학습 중지됨]");
                }
                else if (process.ExitCode == 0)
                {
                    MessageBox.Show("학습 완료");
                    AddTrainLog("[학습 완료]");
                    LoadModelList();
                }
                else
                {
                    string lastLines = string.Join(Environment.NewLine, trainingOutput.TakeLast(8));
                    MessageBox.Show(
                        $"학습 프로세스가 오류 코드 {process.ExitCode}로 종료되었습니다." +
                        (string.IsNullOrWhiteSpace(lastLines) ? "" : Environment.NewLine + Environment.NewLine + lastLines) +
                        (string.IsNullOrWhiteSpace(logPath) ? "" : Environment.NewLine + Environment.NewLine + "전체 로그: " + logPath),
                        "학습 실패",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    AddTrainLog($"[학습 실패] ExitCode={process.ExitCode}");
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show("python 또는 wsl 실행 파일을 찾을 수 없습니다. Python, WSL, 가상환경 설정을 확인하세요.\n" + ex.Message);
                AddTrainLog($"실행 파일 오류: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("학습 프로세스 실행 상태가 올바르지 않습니다.\n" + ex.Message);
                AddTrainLog($"학습 프로세스 상태 오류: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
                AddTrainLog($"학습 실행 오류: {ex.Message}");
            }
            finally
            {
                isTrainingRunning = false;
                trainingProcess = null;
                trainingStopRequested = false;

                btnTrain.Enabled = true;
                btnTrain.Text = "학습";
                btnTrain.BackColor = Color.FromArgb(128, 255, 128);
            }
        }

        private void AddTrainLog(string message)
        {
            if (listBoxChartLoss == null) return;

            listBoxChartLoss.Items.Add(message);
            listBoxChartLoss.TopIndex = listBoxChartLoss.Items.Count - 1;
        }

        private void btnSelectCarFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "mycar 폴더를 선택하세요.";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    carFolderPath = NormalizeUserPath(fbd.SelectedPath);
                    MessageBox.Show("선택됨: " + carFolderPath);
                    LoadModelList();
                }
            }
        }

        private ProcessStartInfo BuildTrainingStartInfo(string workingDir, string modelType, string modelArgument, TrainingPathMode pathMode)
        {
            string normalizedWorkingDir = NormalizeUserPath(workingDir);
            string trainScript = TrainingFileExists(normalizedWorkingDir, pathMode, "train.py") ? "train.py" : "manage.py";

            if (pathMode == TrainingPathMode.WslUnc || pathMode == TrainingPathMode.WslLinux)
            {
                string distro = GetPreferredWslDistro(normalizedWorkingDir);
                string linuxWorkingDir = ConvertPathForWsl(normalizedWorkingDir, distro);
                string wslModelArgument = ConvertPathForWsl(modelArgument, distro);

                string scriptCmd = trainScript == "train.py"
                    ? $"python train.py --tubs data --model {BashQuote(wslModelArgument)} --type {BashQuote(modelType)}"
                    : $"python manage.py train --model {BashQuote(wslModelArgument)}";

                string bshCommand =
                    "source ~/.bashrc >/dev/null 2>&1; " +
                    "source ~/miniconda3/etc/profile.d/conda.sh >/dev/null 2>&1 || " +
                    "source ~/anaconda3/etc/profile.d/conda.sh >/dev/null 2>&1 || true; " +
                    $"conda activate e2e_env && cd {BashQuote(linuxWorkingDir)} && mkdir -p models && " +
                    $"export MODEL_PATH={BashQuote(wslModelArgument)} DONKEY_MODEL_PATH={BashQuote(wslModelArgument)} DONKEYCAR_MODEL_PATH={BashQuote(wslModelArgument)} && {scriptCmd}";

                var psi = new ProcessStartInfo
                {
                    FileName = "wsl.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                psi.ArgumentList.Add("-d");
                psi.ArgumentList.Add(distro);
                psi.ArgumentList.Add("--");
                psi.ArgumentList.Add("bash");
                psi.ArgumentList.Add("-ic");
                psi.ArgumentList.Add(bshCommand);

                AddTrainLog("[학습 명령 (WSL)] " + FormatProcessStartInfo(psi));
                return psi;
            }

            string pythonExe = Environment.GetEnvironmentVariable("DONKEYCAR_PYTHON") ?? "python";
            var localPsi = new ProcessStartInfo
            {
                FileName = pythonExe,
                WorkingDirectory = normalizedWorkingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            localPsi.Environment["MODEL_PATH"] = modelArgument;
            localPsi.Environment["DONKEY_MODEL_PATH"] = modelArgument;
            localPsi.Environment["DONKEYCAR_MODEL_PATH"] = modelArgument;

            if (trainScript == "train.py")
            {
                localPsi.ArgumentList.Add("train.py");
                localPsi.ArgumentList.Add("--tubs");
                localPsi.ArgumentList.Add("data");
                localPsi.ArgumentList.Add("--model");
                localPsi.ArgumentList.Add(modelArgument);
                localPsi.ArgumentList.Add("--type");
                localPsi.ArgumentList.Add(modelType);
            }
            else
            {
                localPsi.ArgumentList.Add("manage.py");
                localPsi.ArgumentList.Add("train");
                localPsi.ArgumentList.Add("--model");
                localPsi.ArgumentList.Add(modelArgument);
            }

            AddTrainLog("[학습 명령 (Local)] " + FormatProcessStartInfo(localPsi));
            return localPsi;
        }

        private static string FormatProcessStartInfo(ProcessStartInfo psi)
        {
            var parts = new List<string> { psi.FileName };

            if (psi.ArgumentList.Count > 0)
                parts.AddRange(psi.ArgumentList.Select(QuoteArgument));
            else if (!string.IsNullOrWhiteSpace(psi.Arguments))
                parts.Add(psi.Arguments);

            return string.Join(" ", parts);
        }

        private string? WriteTrainingLog(List<string> trainingOutput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(carFolderPath))
                    return null;

                string normalizedPath = NormalizeUserPath(carFolderPath);
                string logFolder;

                if (IsLinuxAbsolutePath(normalizedPath))
                {
                    logFolder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Team4progTrainingLogs");
                }
                else
                {
                    logFolder = Path.Combine(normalizedPath, "logs");
                }

                Directory.CreateDirectory(logFolder);
                string logFilePath = Path.Combine(logFolder, $"training_{DateTime.Now:yyyyMMdd_HHmmss}.log");
                File.WriteAllLines(logFilePath, trainingOutput);
                AddTrainLog("[학습 로그] " + logFilePath);
                return logFilePath;
            }
            catch (Exception ex)
            {
                AddTrainLog($"학습 로그 저장 오류: {ex.Message}");
                return null;
            }
        }

        private string ResolveTrainingModelArgument(string workingDir, string modelType, TrainingPathMode pathMode)
        {
            string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{modelType}.h5";

            if (pathMode == TrainingPathMode.WslLinux)
            {
                string linuxModelFilePath = CombineLinuxPath(workingDir, "models", fileName);
                AddTrainLog("[학습 모델 자동 생성] " + fileName)    ;
                return linuxModelFilePath;
            }

            string modelsDirectoryPath = Path.Combine(workingDir, "models");
            Directory.CreateDirectory(modelsDirectoryPath);
            string createdModelFilePath = Path.Combine(modelsDirectoryPath, fileName);
            AddTrainLog("[학습 모델 자동 생성] " + fileName);
            return createdModelFilePath;
        }

        private Dictionary<string, string> BackupExistingLocalModelFiles(string workingDir, TrainingPathMode pathMode)
        {
            var backups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                if (pathMode == TrainingPathMode.WslLinux)
                    return backups;

                string modelsDirectoryPath = Path.Combine(NormalizeUserPath(workingDir), "models");
                if (!Directory.Exists(modelsDirectoryPath))
                    return backups;

                string backupDirectoryPath = Path.Combine(Path.GetTempPath(), "Team4progModelBackups", Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(backupDirectoryPath);

                foreach (string modelFilePath in Directory.EnumerateFiles(modelsDirectoryPath, "*.*", SearchOption.TopDirectoryOnly).Where(IsModelFile))
                {
                    string backupFilePath = Path.Combine(backupDirectoryPath, Path.GetFileName(modelFilePath));
                    File.Copy(modelFilePath, backupFilePath, overwrite: true);
                    backups[modelFilePath] = backupFilePath;
                }
            }
            catch (Exception ex)
            {
                AddTrainLog("[모델 백업 오류] " + ex.Message);
            }

            return backups;
        }

        private void RestoreLocalModelBackups(Dictionary<string, string> backups, string expectedModelPath)
        {
            foreach (var backup in backups)
            {
                try
                {
                    if (string.Equals(NormalizeUserPath(backup.Key), NormalizeUserPath(expectedModelPath), StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (File.Exists(backup.Value))
                        File.Copy(backup.Value, backup.Key, overwrite: true);
                }
                catch (Exception ex)
                {
                    AddTrainLog("[모델 복원 오류] " + ex.Message);
                }
            }
        }

        private void EnsureTrainingModelOutput(string expectedModelPath, string workingDir, TrainingPathMode pathMode, DateTime trainingStartedAt)
        {
            try
            {
                string normalizedExpectedModelPath = NormalizeUserPath(expectedModelPath);
                if (pathMode == TrainingPathMode.WslLinux)
                {
                    EnsureWslTrainingModelOutput(normalizedExpectedModelPath, workingDir, trainingStartedAt);
                    return;
                }

                if (File.Exists(normalizedExpectedModelPath))
                    return;

                string modelsDirectoryPath = Path.Combine(NormalizeUserPath(workingDir), "models");
                if (!Directory.Exists(modelsDirectoryPath))
                    return;

                DateTime threshold = trainingStartedAt.AddSeconds(-2);
                string? latestModelPath = Directory
                    .EnumerateFiles(modelsDirectoryPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(IsTrainableModelFile)
                    .Where(path => !string.Equals(NormalizeUserPath(path), normalizedExpectedModelPath, StringComparison.OrdinalIgnoreCase))
                    .Where(path => File.GetLastWriteTime(path) >= threshold)
                    .OrderByDescending(File.GetLastWriteTime)
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(latestModelPath))
                    return;

                File.Copy(latestModelPath, normalizedExpectedModelPath, overwrite: true);
                CopyModelSidecar(latestModelPath, normalizedExpectedModelPath, ".tflite");
                AddTrainLog("[학습 모델 이름 보정] " + Path.GetFileName(normalizedExpectedModelPath));
            }
            catch (Exception ex)
            {
                AddTrainLog("[학습 모델 이름 보정 오류] " + ex.Message);
            }
        }

        private void CopyModelSidecar(string sourceModelPath, string expectedModelPath, string extension)
        {
            string sourceSidecarPath = Path.ChangeExtension(sourceModelPath, extension);
            if (!File.Exists(sourceSidecarPath))
                return;

            string expectedSidecarPath = Path.ChangeExtension(expectedModelPath, extension);
            File.Copy(sourceSidecarPath, expectedSidecarPath, overwrite: true);
        }

        private void EnsureWslTrainingModelOutput(string expectedModelPath, string workingDir, DateTime trainingStartedAt)
        {
            string distro = GetPreferredWslDistro(workingDir);
            string linuxWorkingDir = ConvertPathForWsl(workingDir, distro);
            string linuxExpectedModelPath = ConvertPathForWsl(expectedModelPath, distro);
            string linuxExpectedSidecarPath = ChangeLinuxExtension(linuxExpectedModelPath, ".tflite");
            long startedAtUnixTime = new DateTimeOffset(trainingStartedAt.AddSeconds(-2)).ToUnixTimeSeconds();
            string linuxModelsDirectoryPath = CombineLinuxPath(linuxWorkingDir, "models");
            string command =
                $"if [ ! -f {BashQuote(linuxExpectedModelPath)} ]; then " +
                $"latest=$(find {BashQuote(linuxModelsDirectoryPath)} -maxdepth 1 -type f \\( -iname '*.h5' -o -iname '*.keras' \\) -newermt @{startedAtUnixTime} -printf '%T@ %p\\n' 2>/dev/null | sort -nr | head -n 1 | cut -d' ' -f2-); " +
                $"if [ -n \"$latest\" ]; then cp \"$latest\" {BashQuote(linuxExpectedModelPath)}; " +
                $"sidecar=\"${{latest%.*}}.tflite\"; " +
                $"[ -f \"$sidecar\" ] && cp \"$sidecar\" {BashQuote(linuxExpectedSidecarPath)}; fi; fi";

            if (RunWslCommand(distro, command, out string output) != 0 && !string.IsNullOrWhiteSpace(output))
                AddTrainLog("[WSL 모델 이름 보정 오류] " + output.Trim());
        }

        private static string ChangeLinuxExtension(string linuxPath, string extension)
        {
            int slashIndex = linuxPath.LastIndexOf('/');
            int dotIndex = linuxPath.LastIndexOf('.');
            if (dotIndex > slashIndex)
                return linuxPath.Substring(0, dotIndex) + extension;

            return linuxPath + extension;
        }

        private string? GetSelectedTrainingModelPath()
        {
            string? selectedModelFilePath = cmbModelList.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedModelFilePath))
                return null;

            string normalizedSelectedModelPath = NormalizeUserPath(selectedModelFilePath);
            string ext = Path.GetExtension(normalizedSelectedModelPath);

            if (ext.Equals(".h5", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".keras", StringComparison.OrdinalIgnoreCase))
            {
                return normalizedSelectedModelPath;
            }

            AddTrainLog($"[학습] {ext} 모델은 학습 저장 대상으로 쓰지 않고 새 .h5 모델을 생성합니다.");
            return null;
        }

        private static TrainingPathMode GetTrainingPathMode(string path)
        {
            if (IsLinuxAbsolutePath(path))
                return TrainingPathMode.WslLinux;

            if (TryConvertWslUncPath(path, out _, out _))
                return TrainingPathMode.WslUnc;

            return TrainingPathMode.LocalWindows;
        }

        private static bool DirectoryExistsForTrainingPath(string path, TrainingPathMode mode)
        {
            string normalizedPath = NormalizeUserPath(path);

            if (mode == TrainingPathMode.WslLinux)
            {
                string distro = GetPreferredWslDistro(normalizedPath);
                return TestWslPath(distro, normalizedPath, isDirectory: true);
            }

            return Directory.Exists(normalizedPath);
        }

        private static bool TrainingFileExists(string workingDir, TrainingPathMode mode, string fileName)
        {
            string normalizedWorkingDir = NormalizeUserPath(workingDir);

            if (mode == TrainingPathMode.WslLinux)
            {
                string distro = GetPreferredWslDistro(normalizedWorkingDir);
                string linuxFilePath = CombineLinuxPath(normalizedWorkingDir, fileName);
                return TestWslPath(distro, linuxFilePath, isDirectory: false);
            }

            return File.Exists(Path.Combine(normalizedWorkingDir, fileName));
        }

        private static bool TestWslPath(string distro, string linuxPath, bool isDirectory)
        {
            string flag = isDirectory ? "-d" : "-f";
            string command = $"test {flag} {BashQuote(linuxPath)}";
            return RunWslCommand(distro, command, out _) == 0;
        }

        private static string ConvertPathForWsl(string path, string distro)
        {
            string normalizedPath = NormalizeUserPath(path);

            if (TryConvertWslUncPath(normalizedPath, out string linuxPathFromUnc, out _))
                return linuxPathFromUnc;

            if (IsLinuxAbsolutePath(normalizedPath))
                return normalizedPath.Replace('\\', '/');

            if (normalizedPath.Length >= 3 &&
                char.IsLetter(normalizedPath[0]) &&
                normalizedPath[1] == ':' &&
                (normalizedPath[2] == '\\' || normalizedPath[2] == '/'))
            {
                char drive = char.ToLowerInvariant(normalizedPath[0]);
                string rest = normalizedPath.Substring(2).Replace('\\', '/').TrimStart('/');
                return $"/mnt/{drive}/{rest}";
            }

            return normalizedPath.Replace('\\', '/');
        }

        private static bool TryConvertWslUncPath(string path, out string linuxPath, out string? distroName)
        {
            linuxPath = path;
            distroName = null;
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string normalized = path.Replace('/', '\\');
            const string localhostPrefix = @"\\wsl.localhost\";
            const string dollarPrefix = @"\\wsl$\";

            string remainder;
            if (normalized.StartsWith(localhostPrefix, StringComparison.OrdinalIgnoreCase))
                remainder = normalized.Substring(localhostPrefix.Length);
            else if (normalized.StartsWith(dollarPrefix, StringComparison.OrdinalIgnoreCase))
                remainder = normalized.Substring(dollarPrefix.Length);
            else
                return false;

            int slashIndex = remainder.IndexOf('\\');
            if (slashIndex < 0 || slashIndex == remainder.Length - 1)
                return false;

            distroName = remainder.Substring(0, slashIndex);
            string pathPart = remainder.Substring(slashIndex + 1).Replace('\\', '/');
            linuxPath = "/" + pathPart.TrimStart('/');
            return true;
        }

        private static string NormalizeUserPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string trimmed = path.Trim().Trim('"');
            return Environment.ExpandEnvironmentVariables(trimmed);
        }

        private static bool IsLinuxAbsolutePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string normalized = path.Replace('\\', '/');
            return normalized.StartsWith("/", StringComparison.Ordinal) &&
                   !normalized.StartsWith("//", StringComparison.Ordinal);
        }

        private static string CombineLinuxPath(params string[] parts)
        {
            return string.Join("/", parts
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .Select((part, index) => index == 0 ? part.TrimEnd('/') : part.Trim('/')));
        }

        private static string GetPreferredWslDistro(string path = "")
        {
            string? envDistro = Environment.GetEnvironmentVariable("DONKEYCAR_WSL_DISTRO");
            if (!string.IsNullOrWhiteSpace(envDistro))
                return envDistro.Trim();

            if (TryConvertWslUncPath(path, out _, out string? distroFromPath) && !string.IsNullOrWhiteSpace(distroFromPath))
                return distroFromPath;

            string firstDistro = GetFirstInstalledWslDistro();
            return string.IsNullOrWhiteSpace(firstDistro) ? "Ubuntu-22.04" : firstDistro;
        }

        private static string GetFirstInstalledWslDistro()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "wsl.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                psi.ArgumentList.Add("-l");
                psi.ArgumentList.Add("-q");

                using var process = Process.Start(psi);
                if (process == null)
                    return string.Empty;

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(3000);

                string cleaned = output.Replace("\0", "").Trim();
                string? first = cleaned
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .FirstOrDefault(line => !string.IsNullOrWhiteSpace(line));

                return first ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static int RunWslCommand(string distro, string command, out string output)
        {
            output = string.Empty;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "wsl.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                psi.ArgumentList.Add("-d");
                psi.ArgumentList.Add(distro);
                psi.ArgumentList.Add("--");
                psi.ArgumentList.Add("bash");
                psi.ArgumentList.Add("-lc");
                psi.ArgumentList.Add(command);

                using var process = Process.Start(psi);
                if (process == null)
                    return -1;

                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(5000);
                output = stdout + stderr;
                return process.ExitCode;
            }
            catch (Exception ex)
            {
                output = ex.Message;
                return -1;
            }
        }

        private static string BashQuote(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "''";

            return "'" + value.Replace("'", "'\\''") + "'";
        }

        private static string QuoteArgument(string value)
        {
            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        private void btnLoadModel_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dialog = new OpenFileDialog();
                dialog.Title = "모델 파일 선택";
                dialog.Filter = "Model files (*.h5;*.keras;*.tflite;*.onnx;*.pt;*.pkl)|*.h5;*.keras;*.tflite;*.onnx;*.pt;*.pkl|All files (*.*)|*.*";

                string initialDirectory = GetModelSearchFolder();
                dialog.InitialDirectory = IsLinuxAbsolutePath(initialDirectory)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    : initialDirectory;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                string selectedModelFilePath = NormalizeUserPath(dialog.FileName);
                AddModelToCombo(selectedModelFilePath);
                cmbModelList.SelectedItem = selectedModelFilePath;
                AddTrainLog("[모델 선택] " + Path.GetFileName(selectedModelFilePath));
            }
            catch (Exception ex)
            {
                AddTrainLog($"모델 선택 오류: {ex.Message}");
            }
        }

        private void btnDeleteModel_Click(object? sender, EventArgs e)
        {
            try
            {
                string? selectedModelFilePath = cmbModelList.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedModelFilePath))
                {
                    MessageBox.Show("삭제할 모델을 선택하세요.");
                    return;
                }

                string normalizedSelectedModelPath = NormalizeUserPath(selectedModelFilePath);

                if (IsLinuxAbsolutePath(normalizedSelectedModelPath))
                {
                    string distro = GetPreferredWslDistro(carFolderPath);
                    if (!TestWslPath(distro, normalizedSelectedModelPath, isDirectory: false))
                    {
                        MessageBox.Show("삭제할 모델 파일을 찾을 수 없습니다.");
                        return;
                    }
                }
                else if (!File.Exists(normalizedSelectedModelPath))
                {
                    MessageBox.Show("삭제할 모델 파일을 찾을 수 없습니다.");
                    return;
                }

                var result = MessageBox.Show(
                    $"{Path.GetFileName(normalizedSelectedModelPath)} 파일을 삭제할까요?",
                    "모델 삭제",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                if (IsLinuxAbsolutePath(normalizedSelectedModelPath))
                {
                    string distro = GetPreferredWslDistro(carFolderPath);
                    RunWslCommand(distro, $"rm -f {BashQuote(normalizedSelectedModelPath)}", out _);
                }
                else
                {
                    File.Delete(normalizedSelectedModelPath);
                }

                AddTrainLog("[모델 삭제] " + Path.GetFileName(normalizedSelectedModelPath));
                LoadModelList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("모델 삭제 실패: " + ex.Message);
                AddTrainLog($"모델 삭제 오류: {ex.Message}");
            }
        }

        private void LoadModelList()
        {
            try
            {
                cmbModelList.Items.Clear();

                string searchFolder = GetModelSearchFolder();
                var models = EnumerateModelFiles(searchFolder)
                    .Where(IsModelFile)
                    .OrderBy(path => IsTrainableModelFile(path) ? 0 : 1)
                    .ThenByDescending(GetModelLastWriteTimeSafe)
                    .ToArray();

                foreach (var modelFile in models)
                    AddModelToCombo(modelFile);

                if (cmbModelList.Items.Count > 0)
                    cmbModelList.SelectedIndex = 0;

                AddTrainLog($"[모델 목록] {cmbModelList.Items.Count}개");
            }
            catch (Exception ex)
            {
                AddTrainLog($"모델 목록 로드 오류: {ex.Message}");
            }
        }

        private string GetModelSearchFolder()
        {
            if (string.IsNullOrWhiteSpace(carFolderPath))
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string normalizedCarFolderPath = NormalizeUserPath(carFolderPath);

            if (IsLinuxAbsolutePath(normalizedCarFolderPath))
            {
                string linuxModelsFolder = CombineLinuxPath(normalizedCarFolderPath, "models");
                string distro = GetPreferredWslDistro(normalizedCarFolderPath);
                return TestWslPath(distro, linuxModelsFolder, isDirectory: true)
                    ? linuxModelsFolder
                    : normalizedCarFolderPath;
            }

            string modelsFolder = Path.Combine(normalizedCarFolderPath, "models");
            return Directory.Exists(modelsFolder) ? modelsFolder : normalizedCarFolderPath;
        }

        private IEnumerable<string> EnumerateModelFiles(string folderPath)
        {
            string normalizedFolderPath = NormalizeUserPath(folderPath);

            if (IsLinuxAbsolutePath(normalizedFolderPath))
            {
                string distro = GetPreferredWslDistro(carFolderPath);
                string command =
                    $"find {BashQuote(normalizedFolderPath)} -maxdepth 1 -type f " +
                    "\\( -iname '*.h5' -o -iname '*.keras' -o -iname '*.tflite' -o -iname '*.onnx' -o -iname '*.pt' -o -iname '*.pkl' \\) -print";

                if (RunWslCommand(distro, command, out string output) != 0)
                    return Enumerable.Empty<string>();

                return output
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrWhiteSpace(line));
            }

            if (!Directory.Exists(normalizedFolderPath))
                return Enumerable.Empty<string>();

            return Directory.EnumerateFiles(normalizedFolderPath, "*.*", SearchOption.TopDirectoryOnly);
        }

        private DateTime GetModelLastWriteTimeSafe(string filePath)
        {
            try
            {
                if (IsLinuxAbsolutePath(filePath))
                {
                    string distro = GetPreferredWslDistro(carFolderPath);
                    string command = $"stat -c %Y {BashQuote(filePath)} 2>/dev/null || echo 0";
                    if (RunWslCommand(distro, command, out string output) == 0 &&
                        long.TryParse(output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out long unixTime))
                    {
                        return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                    }
                }
                else if (File.Exists(filePath))
                {
                    return File.GetLastWriteTime(filePath);
                }
            }
            catch
            {
                // ignored
            }

            return DateTime.MinValue;
        }

        private void AddModelToCombo(string modelFilePath)
        {
            string normalizedModelFilePath = NormalizeUserPath(modelFilePath);
            if (!cmbModelList.Items.Cast<object>().Any(item => string.Equals(item.ToString(), normalizedModelFilePath, StringComparison.OrdinalIgnoreCase)))
                cmbModelList.Items.Add(normalizedModelFilePath);
        }

        private static bool IsModelFile(string path)
        {
            string ext = Path.GetExtension(path);
            return IsTrainableModelFile(path) ||
                   ext.Equals(".tflite", StringComparison.OrdinalIgnoreCase) ||
                   ext.Equals(".onnx", StringComparison.OrdinalIgnoreCase) ||
                   ext.Equals(".pt", StringComparison.OrdinalIgnoreCase) ||
                   ext.Equals(".pkl", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsTrainableModelFile(string path)
        {
            string ext = Path.GetExtension(path);
            return ext.Equals(".h5", StringComparison.OrdinalIgnoreCase) ||
                   ext.Equals(".keras", StringComparison.OrdinalIgnoreCase);
        }
    }
}
