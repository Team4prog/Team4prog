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
            try
            {
                string modelType = cmbModelType.Text;
                string comment = txtComment.Text;

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

                string workingDir = carFolderPath;

                // Run DonkeyCar training from the selected car folder.
                string command = $"manage.py train --model {modelType}";

                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = command,
                    WorkingDirectory = workingDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new Process();
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

                process.Start();

                process.BeginOutputReadLine();

                await Task.Run(() => process.WaitForExit());

                MessageBox.Show("학습 완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
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
    }
}

