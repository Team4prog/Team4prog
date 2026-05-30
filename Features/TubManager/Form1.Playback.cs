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
    // Frame playback controls: step, play forward, play backward, stop, and timer ticks.
    public partial class Form1
    {
        private void nudSpeed_ValueChanged(object? sender, EventArgs e)
        {
            try
            {
                playbackSpeed = (double)nudSpeed.Value;
                UpdateTimerIntervalFromSpeed();
                lblSpeed.Text = $"{playbackSpeed:F2}x";
            }
            catch (Exception ex)
            {
                AddLog($"속도 설정 오류: {ex.Message}");
            }
        }

        private void UpdateTimerIntervalFromSpeed()
        {
            try
            {
                var interval = (int)Math.Max(10, 100.0 / playbackSpeed);
                timerPlayback.Interval = interval;
            }
            catch (Exception ex)
            {
                AddLog($"타이머 간격 설정 오류: {ex.Message}");
            }
        }

        private void btnPrev_Click(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0) return;
                int idx = Math.Max(0, (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex));
                idx = Math.Max(0, idx - 1);
                listBoxFrames.SelectedIndex = idx;
                trackBarFrame.Value = idx;
            }
            catch (Exception ex)
            {
                AddLog($"이전 프레임 이동 오류: {ex.Message}");
            }
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0) return;
                int idx = Math.Min(imagePaths.Count - 1, (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex));
                idx = Math.Min(imagePaths.Count - 1, idx + 1);
                listBoxFrames.SelectedIndex = idx;
                trackBarFrame.Value = idx;
            }
            catch (Exception ex)
            {
                AddLog($"다음 프레임 이동 오류: {ex.Message}");
            }
        }

        private void btnPlayForward_Click(object? sender, EventArgs e)
        {
            if (imagePaths.Count == 0)
            {
                AddLog("재생 불가: 이미지가 없습니다.");
                return;
            }
            isPlayingForward = true;
            isPlayingBackward = false;
            UpdateTimerIntervalFromSpeed();
            timerPlayback.Start();
            AddLog("[재생 시작 >>]");
        }

        private void btnPlayBackward_Click(object? sender, EventArgs e)
        {
            if (imagePaths.Count == 0)
            {
                AddLog("재생 불가: 이미지가 없습니다.");
                return;
            }
            isPlayingForward = false;
            isPlayingBackward = true;
            UpdateTimerIntervalFromSpeed();
            timerPlayback.Start();
            AddLog("[역재생 시작 <<]");
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            timerPlayback.Stop();
            isPlayingForward = false;
            isPlayingBackward = false;
            AddLog("[정지]");
        }

        private void TimerPlayback_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (imagePaths.Count == 0)
                    return;

                int idx = (listBoxFrames.SelectedIndex >= 0 ? listBoxFrames.SelectedIndex : currentIndex);
                if (idx < 0) idx = 0;

                if (isPlayingForward)
                {
                    if (idx < imagePaths.Count - 1)
                        idx++;
                    else
                    {
                        // Stop automatically at the last frame.
                        btnStop_Click(null, EventArgs.Empty);
                        return;
                    }
                }
                else if (isPlayingBackward)
                {
                    if (idx > 0)
                        idx--;
                    else
                    {
                        btnStop_Click(null, EventArgs.Empty);
                        return;
                    }
                }

                // Keep the list and trackbar in sync while the timer advances frames.
                if (idx >= 0 && idx < imagePaths.Count)
                {
                    // Setting SelectedIndex triggers ShowImage via event
                    if (listBoxFrames.SelectedIndex != idx)
                        listBoxFrames.SelectedIndex = idx;
                    trackBarFrame.Value = idx;
                }
            }
            catch (Exception ex)
            {
                AddLog($"재생 중 오류: {ex.Message}");
                btnStop_Click(null, EventArgs.Empty);
            }
        }

    }
}

