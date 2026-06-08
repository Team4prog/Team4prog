using System;
using System.Drawing;
using System.Windows.Forms;

namespace Team4prog.UI
{
    public class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            InitializeUI();
            EnableDrag(this);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // HelpForm
            // 
            BackColor = Color.FromArgb(24, 24, 28);
            ClientSize = new Size(651, 959);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(460, 420);
            Name = "HelpForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }

        private void InitializeUI()
        {
            var borderPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(24, 24, 28),
                Padding = new Padding(2)
            };

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(34, 34, 40),
                Padding = new Padding(22)
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(34, 34, 40)
            };

            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = "DonkeyCar UI Help",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(192, 255, 192),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var closeButton = new Button
            {
                BackColor = Color.FromArgb(255, 128, 128),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Margin = new Padding(0),
                Size = new Size(36, 30),
                Text = "X",
                UseVisualStyleBackColor = false
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => Close();

            var guideText = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(34, 34, 40),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Padding = new Padding(0, 12, 0, 0),
                TextAlign = ContentAlignment.TopLeft,
                Text =
                    "Tub Manager\r\n" +
                    "- 폴더 열기: DonkeyCar의 tub/image 폴더를 불러옵니다.\r\n" +
                    "- 주행 데이터 탐색기: 프레임 목록에서 이미지를 선택해 확인합니다.\r\n" +
                    "- 이미지 미리보기: 선택한 프레임을 중앙 화면에서 확인합니다.\r\n" +
                    "- 프레임 삭제: 선택한 이미지와 연결된 JSON 및 catalog 항목을 삭제합니다.\r\n" +
                    "- 삭제 로그 관리: 삭제된 프레임 기록을 확인합니다.\r\n" +
                    "- 재생 컨트롤: 이전/다음 이동, 재생, 역재생, 정지 기능을 제공합니다.\r\n" +
                    "- Tub Cleaner: 시작점/끝점을 지정하여 여러 프레임을 한 번에 삭제합니다.\r\n" +
                    "- Filter: angle / throttle 조건으로 프레임을 필터링합니다.\r\n\r\n" +
                    "Trainer\r\n" +
                    "- 차량 폴더 선택: DonkeyCar의 mycar 폴더를 선택합니다.\r\n" +
                    "- 모델 선택: 기존 모델을 선택해 이어서 학습할 수 있습니다.\r\n" +
                    "- 학습: 버튼을 눌러 모델 학습을 시작합니다.\r\n" +
                    "- Training Loss Chart: 학습 진행 상황과 loss 값을 그래프로 확인합니다.\r\n" +
                    "- 모델 관리 기능: 생성된 모델 목록을 확인하거나 삭제할 수 있습니다.\r\n\r\n" +
                    "Help Window\r\n" +
                    "- 이 창은 항상 위에 표시됩니다.\r\n" +
                    "- 창 어디든 마우스로 드래그하여 이동할 수 있습니다.\r\n" +
                    "- 닫기 버튼을 누르면 Help 창만 종료됩니다."
             };

            headerPanel.Controls.Add(title);
            headerPanel.Controls.Add(closeButton);
            contentPanel.Controls.Add(guideText);
            contentPanel.Controls.Add(headerPanel);
            borderPanel.Controls.Add(contentPanel);
            Controls.Add(borderPanel);

            Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(192, 255, 192), 2);
                e.Graphics.DrawRectangle(pen, 1, 1, Width - 3, Height - 3);
            };
        }

        private void EnableDrag(Control control)
        {
            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Capture = false;

                    Message m = Message.Create(Handle, 0xA1, (IntPtr)2, IntPtr.Zero);
                    WndProc(ref m);
                }
            };

            foreach (Control child in control.Controls)
            {
                EnableDrag(child);
            }
        }
    }
}
