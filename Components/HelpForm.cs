using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Team4prog.Components
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            InitializeUI();

        }

        private void InitializeUI()
        {
            // ===== 폼 기본 설정 =====
            this.Text = "사용 가이드";
            this.Size = new Size(520, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;

            // ⭐ 연두색 테두리
            this.Padding = new Padding(2);
            this.BackColor = Color.FromArgb(180, 255, 180);

            // ===== 내부 컨텐츠 패널 =====
            Panel contentPanel = new Panel();
            contentPanel.BackColor = Color.FromArgb(30, 30, 30);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(20);

            this.Controls.Add(contentPanel);

            // ===== 제목 =====
            Label lblTitle = new Label();
            lblTitle.Padding = new Padding(0, 0, 0, 10);
            lblTitle.Text = "사용 가이드";
            lblTitle.Font = new Font("한컴 고딕", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Top;

            // ===== 구분선 =====
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(70, 70, 70);
            line.Height = 1;
            line.Dock = DockStyle.Top;
            line.Margin = new Padding(0, 10, 0, 10);
            line.Dock = DockStyle.Top;

            // ===== 내용 =====
            TextBox txtGuide = new TextBox();
            txtGuide.TabStop = false;
            txtGuide.Multiline = true;
            txtGuide.ReadOnly = true;
            txtGuide.Dock = DockStyle.Fill;
            txtGuide.BorderStyle = BorderStyle.None;
            txtGuide.BackColor = Color.FromArgb(30, 30, 30);
            txtGuide.ForeColor = Color.Gainsboro;
            txtGuide.Font = new Font("한컴 고딕", 10);
            txtGuide.ScrollBars = ScrollBars.Vertical;  // ⭐ 스크롤 활성화
            txtGuide.Dock = DockStyle.Fill;             // ⭐ 영역 꽉 채우기


            txtGuide.Text =
        @"[Tub Navigator 사용 방법]

📁 폴더 열기
- 이미지 및 JSON 데이터가 있는 폴더를 불러옵니다.

🖼 Frame Navigator
- 프레임 목록을 확인하고 원하는 이미지를 선택합니다.

🗑 삭제
- 현재 선택된 프레임 데이터를 삭제합니다.

⬅ Set Left
- 현재 프레임을 작업 시작 지점으로 설정합니다.

➡ Set Right
- 현재 프레임을 작업 종료 지점으로 설정합니다.

🎚 Set Filter
- Angle, Throttle 조건을 설정하여 특정 데이터만 선택합니다.

❌ Clear
- 설정한 필터를 초기화합니다.

♻ Restore
- 삭제한 데이터를 다시 복원합니다.

🔄 Reload
- 폴더 데이터를 다시 불러옵니다.

🎮 Play Controls
- ▶ Play Forward : 다음 프레임으로 이동합니다.
- ◀ Play Backward : 이전 프레임으로 이동합니다.
- ⏹ Stop : 재생을 중지합니다.
- ⏮ Prev : 이전 프레임 방향으로 자동 재생합니다.
- ⏭ Next : 다음 프레임 방향으로 자동 재생합니다.
- Speed : 재생 속도를 조절합니다.

[Trainer 사용 방법]

📂 Select Car Folder
- 학습에 사용할 데이터 폴더를 선택합니다.

🧠 Model Type
- 사용할 AI 모델 유형을 선택합니다. (예: linear, categorical)

📦 Choose Model
- 기존에 저장된 모델을 불러옵니다.

✏ Comment
- 학습 모델에 대한 설명이나 메모를 입력합니다.

🚀 Train
- 선택한 데이터와 설정을 기반으로 AI 학습을 시작합니다.

📊 Loss Chart
- 학습 과정에서의 손실 값 변화를 그래프로 확인합니다.

🗂 Pilot Manager
- 학습된 모델을 관리하는 영역입니다.
  - Model List : 저장된 모델 목록을 확인합니다.
  - Delete Model : 선택한 모델을 삭제합니다.
  - Update Comment : 모델 설명을 수정합니다.

⚙ Config Editor
- 학습에 필요한 설정값을 추가/수정합니다.
  - + : 새로운 설정 항목 추가
  - Save myconfig : 현재 설정을 저장";

            // ===== 닫기 버튼 =====
            Button btnClose = new Button();
            btnClose.Text = "닫기";
            btnClose.Size = new Size(90, 32);
            btnClose.Dock = DockStyle.Bottom;   // ⭐ 핵심
            btnClose.Height = 40;
            btnClose.BackColor = Color.FromArgb(70, 130, 180);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;

            btnClose.Click += (s, e) => this.Close();

            // ===== 패널에 추가 =====
            contentPanel.Controls.Add(txtGuide);   // Fill (마지막에 먹힘)
            contentPanel.Controls.Add(btnClose);   // Bottom
            contentPanel.Controls.Add(line);
            contentPanel.Controls.Add(lblTitle);   // Top
        }
    }
}
