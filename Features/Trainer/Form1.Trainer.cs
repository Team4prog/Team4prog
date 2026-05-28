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
                    MessageBox.Show("모델 타입을 선택하세요.");
                    return;
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

                // Run DonkeyCar training from the selected car folder.
                if (!TryConvertWslUncPath(workingDir, out var distroName, out var linuxWorkingDir))
                {
                    MessageBox.Show("선택한 폴더가 올바른 WSL 경로(\\\\wsl.localhost\\...)가 아닙니다.\nWSL 내부의 mycar 폴더를 선택해주세요.");
                    isTrainingRunning = false;
                    btnTrain.Enabled = true;
                    return;
                }
                string wslArgs = $"-d {distroName} -- bash -ic \"conda activate e2e_env && cd '{linuxWorkingDir}' && python manage.py train --model {modelType}\"";

                var psi = new ProcessStartInfo
                {
                    FileName = "wsl.exe",
                    Arguments = wslArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process();
                process.StartInfo = psi;

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
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

                if (process.ExitCode == 0)
                {
                    MessageBox.Show("학습 완료");
                    AddLog("[학습 완료]");
                }
                else
                {
                    MessageBox.Show($"학습 프로세스가 오류 코드 {process.ExitCode}로 종료되었습니다.");
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
                }
            }
        }

        private static bool TryConvertWslUncPath(string path, out string distroName, out string linuxPath)
        {
            distroName = "";
            linuxPath = "";

            var normalizedPath = path.TrimEnd('\\', '/');
            string relativePath;

            if (normalizedPath.StartsWith(@"\\wsl.localhost\", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = normalizedPath.Substring(@"\\wsl.localhost\".Length);
            }
            else if (normalizedPath.StartsWith(@"\\wsl$\", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = normalizedPath.Substring(@"\\wsl$\".Length);
            }
            else
            {
                return false;
            }

            var parts = relativePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return false;
            }

            distroName = parts[0];
            linuxPath = "/" + string.Join("/", parts.Skip(1));
            return true;
        }
    }
}

