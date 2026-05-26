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
    // Trainer view actions: select a DonkeyCar folder and run the training command.
    public partial class Form1
    {
        private async void btnTrain_Click(object sender, EventArgs e)
        {
            await RunTraining();
        }



        private async Task RunTraining()
        {
            if (isTrainingRunning)
            {
                MessageBox.Show("이미 학습 프로세스가 실행 중입니다.");
                return;
            }

            try
            {
                string modelType = cmbModelType.Text;

                if (string.IsNullOrEmpty(modelType))
                {
                    modelType = "linear";
                    cmbModelType.SelectedItem = modelType;
                    AddLog("[학습] 모델 타입이 비어 있어 linear로 설정했습니다.");
                }

                if (string.IsNullOrEmpty(carFolderPath))
                {
                    MessageBox.Show("먼저 폴더를 선택하세요.");
                    return;
                }

                if (!Directory.Exists(carFolderPath))
                {
                    MessageBox.Show("선택한 폴더가 존재하지 않습니다.");
                    return;
                }

                string workingDir = carFolderPath;
                string managePyPath = Path.Combine(workingDir, "manage.py");

                if (!File.Exists(managePyPath))
                {
                    MessageBox.Show("선택한 폴더에서 manage.py를 찾을 수 없습니다.");
                    return;
                }

                string? selectedModelPath = GetSelectedTrainingModelPath();
                var psi = BuildTrainingStartInfo(workingDir, modelType, selectedModelPath);

                using var process = new Process();
                process.StartInfo = psi;
                var trainingOutput = new List<string>();

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        trainingOutput.Add(e.Data);
                        this.Invoke(new Action(() =>
                        {
                            listBoxLog.Items.Add(e.Data);
                            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
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
                            AddLog("[학습 오류] " + e.Data);
                        }));
                    }
                };

                isTrainingRunning = true;
                btnTrain.Enabled = false;
                AddLog("[학습 시작]");

                if (!process.Start())
                {
                    MessageBox.Show("학습 프로세스를 시작하지 못했습니다.");
                    return;
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await Task.Run(() => process.WaitForExit());

                string? logPath = WriteTrainingLog(trainingOutput);

                if (process.ExitCode == 0)
                {
                    MessageBox.Show("학습 완료");
                    AddLog("[학습 완료]");
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
                    AddLog($"[학습 실패] ExitCode={process.ExitCode}");
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show("python 실행 파일을 찾을 수 없습니다. Python 또는 가상환경 설정을 확인하세요.\n" + ex.Message);
                AddLog($"Python 실행 오류: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("학습 프로세스 실행 상태가 올바르지 않습니다.\n" + ex.Message);
                AddLog($"학습 프로세스 상태 오류: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
                AddLog($"학습 실행 오류: {ex.Message}");
            }
            finally
            {
                isTrainingRunning = false;
                btnTrain.Enabled = true;
            }
        }

        private void btnSelectCarFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "mycar 폴더를 선택하세요.";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    carFolderPath = fbd.SelectedPath;
                    MessageBox.Show("선택됨: " + carFolderPath);
                    LoadModelList();
                }
            }
        }

        private ProcessStartInfo BuildTrainingStartInfo(string workingDir, string modelType, string? selectedModelPath)
        {
            string modelArgument = ResolveTrainingModelArgument(workingDir, modelType, selectedModelPath);
            string trainScript = File.Exists(Path.Combine(workingDir, "train.py")) ? "train.py" : "manage.py";

            bool isWslPath = TryConvertWslUncPath(workingDir, out string wslWorkingDir, out string? wslDistro);
            string? wslPython = Environment.GetEnvironmentVariable("DONKEYCAR_WSL_PYTHON");
            if (isWslPath || !string.IsNullOrWhiteSpace(wslPython))
            {
                string wslModelArgument = modelArgument;
                if (TryConvertWslUncPath(modelArgument, out string convertedModelPath, out _))
                    wslModelArgument = convertedModelPath;

                string pythonExecutable = string.IsNullOrWhiteSpace(wslPython)
                    ? "~/miniconda3/envs/e2e_env/bin/python"
                    : wslPython;

                var psi = new ProcessStartInfo
                {
                    FileName = "wsl.exe",
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                if (!string.IsNullOrWhiteSpace(wslDistro))
                {
                    psi.ArgumentList.Add("--distribution");
                    psi.ArgumentList.Add(wslDistro);
                }

                psi.ArgumentList.Add("--cd");
                psi.ArgumentList.Add(wslWorkingDir);
                psi.ArgumentList.Add(pythonExecutable);

                if (trainScript == "train.py")
                {
                    psi.ArgumentList.Add("train.py");
                    psi.ArgumentList.Add("--tubs");
                    psi.ArgumentList.Add("data");
                    psi.ArgumentList.Add("--model");
                    psi.ArgumentList.Add(wslModelArgument);
                    psi.ArgumentList.Add("--type");
                    psi.ArgumentList.Add(modelType);
                }
                else
                {
                    psi.ArgumentList.Add("manage.py");
                    psi.ArgumentList.Add("train");
                    psi.ArgumentList.Add("--model");
                    psi.ArgumentList.Add(wslModelArgument);
                }

                AddLog("[학습 명령] " + FormatProcessStartInfo(psi));
                return psi;
            }

            string pythonExe = Environment.GetEnvironmentVariable("DONKEYCAR_PYTHON") ?? "python";
            var localPsi = new ProcessStartInfo
            {
                FileName = pythonExe,
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

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

            AddLog("[학습 명령] " + FormatProcessStartInfo(localPsi));
            return localPsi;
        }

        private static string FormatProcessStartInfo(ProcessStartInfo psi)
        {
            var parts = new List<string> { psi.FileName };
            parts.AddRange(psi.ArgumentList.Select(QuoteArgument));
            return string.Join(" ", parts);
        }

        private string? WriteTrainingLog(List<string> trainingOutput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(carFolderPath))
                    return null;

                string logFolder = Path.Combine(carFolderPath, "logs");
                Directory.CreateDirectory(logFolder);
                string logPath = Path.Combine(logFolder, $"training_{DateTime.Now:yyyyMMdd_HHmmss}.log");
                File.WriteAllLines(logPath, trainingOutput);
                AddLog("[학습 로그] " + logPath);
                return logPath;
            }
            catch (Exception ex)
            {
                AddLog($"학습 로그 저장 오류: {ex.Message}");
                return null;
            }
        }

        private string ResolveTrainingModelArgument(string workingDir, string modelType, string? selectedModelPath)
        {
            if (!string.IsNullOrWhiteSpace(selectedModelPath))
            {
                AddLog("[학습 모델] " + Path.GetFileName(selectedModelPath));
                return selectedModelPath;
            }

            string modelsDir = Path.Combine(workingDir, "models");
            Directory.CreateDirectory(modelsDir);
            string fileName = $"pilot_{modelType}_{DateTime.Now:yyyyMMdd_HHmmss}.h5";
            string modelPath = Path.Combine(modelsDir, fileName);
            AddLog("[학습 모델 자동 생성] " + fileName);
            return modelPath;
        }

        private string? GetSelectedTrainingModelPath()
        {
            string? selectedModelPath = cmbModelList.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedModelPath))
                return null;

            string ext = Path.GetExtension(selectedModelPath);
            if (ext.Equals(".h5", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".keras", StringComparison.OrdinalIgnoreCase))
            {
                return selectedModelPath;
            }

            AddLog($"[학습] {ext} 모델은 학습 저장 대상으로 쓰지 않고 새 .h5 모델을 생성합니다.");
            return null;
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
                dialog.InitialDirectory = GetModelSearchFolder();

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                AddModelToCombo(dialog.FileName);
                cmbModelList.SelectedItem = dialog.FileName;
                AddLog("[모델 선택] " + Path.GetFileName(dialog.FileName));
            }
            catch (Exception ex)
            {
                AddLog($"모델 선택 오류: {ex.Message}");
            }
        }

        private void btnDeleteModel_Click(object? sender, EventArgs e)
        {
            try
            {
                string? modelPath = cmbModelList.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(modelPath) || !File.Exists(modelPath))
                {
                    MessageBox.Show("삭제할 모델을 선택하세요.");
                    return;
                }

                var result = MessageBox.Show(
                    $"{Path.GetFileName(modelPath)} 파일을 삭제할까요?",
                    "모델 삭제",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                File.Delete(modelPath);
                AddLog("[모델 삭제] " + Path.GetFileName(modelPath));
                LoadModelList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("모델 삭제 실패: " + ex.Message);
                AddLog($"모델 삭제 오류: {ex.Message}");
            }
        }

        private void btnUpdateComment_Click(object? sender, EventArgs e)
        {
            try
            {
                string? modelPath = cmbModelList.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(modelPath) || !File.Exists(modelPath))
                {
                    MessageBox.Show("코멘트를 저장할 모델을 선택하세요.");
                    return;
                }

                string commentPath = modelPath + ".comment.txt";
                File.WriteAllText(commentPath, txtComment.Text ?? string.Empty);
                AddLog("[모델 코멘트 저장] " + Path.GetFileName(commentPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("코멘트 저장 실패: " + ex.Message);
                AddLog($"모델 코멘트 저장 오류: {ex.Message}");
            }
        }

        private void LoadModelList()
        {
            try
            {
                cmbModelList.Items.Clear();

                string searchFolder = GetModelSearchFolder();
                if (!Directory.Exists(searchFolder))
                    return;

                var models = Directory.EnumerateFiles(searchFolder, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(IsModelFile)
                    .OrderBy(path => IsTrainableModelFile(path) ? 0 : 1)
                    .ThenByDescending(File.GetLastWriteTime)
                    .ToArray();

                foreach (var model in models)
                    AddModelToCombo(model);

                if (cmbModelList.Items.Count > 0)
                    cmbModelList.SelectedIndex = 0;

                AddLog($"[모델 목록] {cmbModelList.Items.Count}개");
            }
            catch (Exception ex)
            {
                AddLog($"모델 목록 로드 오류: {ex.Message}");
            }
        }

        private string GetModelSearchFolder()
        {
            if (string.IsNullOrWhiteSpace(carFolderPath))
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string modelsFolder = Path.Combine(carFolderPath, "models");
            return Directory.Exists(modelsFolder) ? modelsFolder : carFolderPath;
        }

        private void AddModelToCombo(string modelPath)
        {
            if (!cmbModelList.Items.Cast<object>().Any(item => string.Equals(item.ToString(), modelPath, StringComparison.OrdinalIgnoreCase)))
                cmbModelList.Items.Add(modelPath);
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
