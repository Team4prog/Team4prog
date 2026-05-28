자율주행 데이터 정제 및 UI 학습기 (SimpleTrainer)

## 1. 개요
* **C# 프로그래밍 및 Windows Forms 기반 실무 프로젝트**
* **1줄 소개**: 자율주행 인공지능 차량(DonkeyCar)의 대용량 주행 데이터를 파싱 및 정제하고, Windows 환경에서 파이썬 가상환경 연동을 통해 AI 모델 학습을 원클릭으로 수행하는 완성형 데스크톱 제어 솔루션
* **사용한 플랫폼**: 
  * C#, .NET 10.0 Windows Forms, Visual Studio 2022, GitHub
* **사용한 컨트롤**: 
  * Panel, GroupBox, Button, TextBox, ComboBox, ListBox, TrackBar, PictureBox, FlowLayoutPanel
* **핵심 기능**:
  * **대용량 JSONL 데이터 파싱**: DonkeyCar 고유의 구조화된 데이터 파일을 고속으로 파싱하여 프레임 단위로 데이터 연동
  * **주행 데이터 시각화**: 프레임별 실제 주행 이미지와 조향각(Angle), 스로틀(Throttle) 수치를 매핑하여 차트와 화면에 동시 출력
  * **고속 데이터 정제 (Tub Cleaner)**: 주행 실수나 벽 충돌 구간의 시작과 끝을 지정하여 범위로 일괄 삭제 및 복구하는 정밀 트리밍 기능
  * **스마트 필터링**: 사용자가 조건식과 연산자(`>`, `<`, `==`)를 조합하여 특정 위험 주행 데이터만 동적으로 필터링 및 조정이 가능하도록 방어 코드 작성
  * **원클릭 AI 모델 학습 (Trainer)**: CLI 터미널 조작 없이 로컬 환경 및 WSL 환경의 Python 가상환경을 호출하여 비동기식 AI 모델 학습 및 코멘트 기록 수행
* **화면 구성**:
  * **Tub Manager 뷰**: 좌측 프레임 리스트 박스, 중앙 메인 블랙박스 이미지 창, 하단 텔레메트리 차트 패널 및 재생 제어 그리드 배치
  * **Trainer 뷰**: 로컬/WSL 환경에 독립적으로 작동하는 경로 브라우저, 학습 파라미터(Model Type) 제어 박스, 실시간 학습 로그 출력 및 모델 관리(Pilot Manager) 그리드 배치

---

## 2. 주요 구현 내용 및 기능 설명 (역할 분담)

### 👤 양채현 (팀장) - 시스템 통합 및 Python 연동 (AI 처리 담당)
* **구현 내용**: C# UI 프로그램과 백엔드 파이썬 AI 프로세스 간의 가교 역할 수행
* **상세 기능 설명**:
  * `Environment.GetEnvironmentVariable` 및 `TryConvertWslUncPath`를 구현하여 Windows 로컬 경로와 WSL(Linux) 파일 시스템 간의 UNC 경로 자동 변환 시스템 구축
  * `ProcessStartInfo`를 활용해 가상환경 내부의 `python.exe`를 안전하게 비동기 서브 프로세스로 실행
  * `OutputDataReceived` 및 `ErrorDataReceived` 이벤트를 리디렉션하여 파이썬 런타임 로그를 `listBoxLog`에 실시간 스트리밍 및 동기화 구현
  * 학습 중 중복 실행을 막는 `isTrainingRunning` 상태 플래그 및 예외 처리 가동

### 👤 강민규 (팀원) - 메인 UI 및 컨트롤 구현 (UI 담당)
* **구현 내용**: 사용자 경험(UX) 극대화를 위한 화면 구성 및 디바이스 독립적 레이아웃 설계
* **상세 기능 설명**:
  * 상단 네비게이션 커스텀 컨트롤(`AppNavigationBar`)을 통해 데이터 관리 패널(`panelTubManager`)과 학습 패널(`panelTrainer`) 간의 직관적인 화면 전환 로직 구현
  * 서로 다른 노트북 모니터 환경에서 해상도 및 배율 차이로 인해 컴포넌트가 깨지거나 겹치는 현상을 해결하기 위해 `AutoScaleMode`를 `Font` 방식으로 최적화 세팅
  * 각 컨트롤 그룹에 유연한 `Anchor` 및 `Dock` 속성을 지정하여 창 크기 변경 시 UI 요소가 비율에 맞춰 자동 스케일링되도록 방어적 레이아웃 구성

### 👤 최율도 (팀원) - 데이터 파싱 및 파일 처리 (데이터 담당)
* **구현 내용**: DonkeyCar 고유의 대용량 주행 로그 파일 고속 처리 인프라 구축
* **상세 기능 설명**:
  * `Newtonsoft.Json` 라이브러리를 고도화하여 줄바꿈 단위로 JSON이 결합된 대용량 주행 로그 데이터를 끊김 없이 파싱하는 `CatalogJsonlReader` 설계
  * 로드된 주행 수치 데이터와 `images/` 폴더 내 JPG 이미지 파일 간의 1:1 인덱스 매핑 테이블 최적화
  * 대용량 파일 IO 처리 중 UI가 멈추는 프리징 현상을 방지하기 위해 데이터 버퍼 처리 기법 활용

### 👤 박경일 (팀원) - 기능 보조 및 데이터 관리 (기능 담당)
* **구현 내용**: 주행 품질 향상을 위한 주행 데이터 필터링 및 고속 편집 로직 구현
* **상세 기능 설명**:
  * 콤보박스로 선택된 연산자(`>`, `<`, `==`)와 텍스트박스로 입력받은 기준값을 조합하여 조건에 맞는 프레임만 추출하는 동적 필터 시스템 보조
  * `Tub Cleaner` 기능 내에서 특정 주행 에러 발생 구간을 마우스 드래그 및 마킹으로 지정하여 범위(Range) 단위로 일괄 비활성화 및 복구하는 트랜잭션형 편집 기능 보조
  * `NumericUpDown` 컨트롤을 활용해 타이머 기반 재생 로직의 딜레이를 계산하여 0.1x ~ 5.0x 배속 재생 시 시각화 차트가 유연하게 싱크되도록 보조 제어

---

## 3. 시작하기 (Getting Started)

### 사전 준비사항 (Prerequisites)
1. **.NET 10.0 SDK**가 타겟 PC에 설치되어 있어야 합니다.
2. AI 학습 프로세스 연동을 위해 사용 중인 파이썬 가상환경 경로를 윈도우 환경 변수에 등록해야 합니다.
   * **Windows 로컬 환경**: 환경 변수에 `DONKEYCAR_PYTHON`을 생성하고, 패키지가 설치된 가상환경 내부의 `python.exe` 절대 경로를 지정합니다.
   * **WSL 환경**: 환경 변수에 `DONKEYCAR_WSL_PYTHON`을 생성하고, WSL 내부의 파이썬 가상환경 실행 경로(예: `~/miniconda3/envs/e2e_env/bin/python`)를 입력합니다.

<img width="1919" height="1079" alt="tub manage화면" src="https://github.com/user-attachments/assets/8f19fba8-fa02-4d9a-8a63-e8f8673cae39" />
<img width="1919" height="1079" alt="trainer 화면" src="https://github.com/user-attachments/assets/4bcf63a7-7c68-4da0-8dd7-350deca189a3" />
<img width="1347" height="1079" alt="Folder 선택시" src="https://github.com/user-attachments/assets/00fd0dfa-9d47-465f-b849-fe5c3e552af2" />
<img width="359" height="418" alt="학습 중" src="https://github.com/user-attachments/assets/827173d7-318e-42df-b209-8ea32e7fbd44" />
<img width="1377" height="962" alt="학습 완료_1" src="https://github.com/user-attachments/assets/6aac4d63-7601-471a-b802-a71480f64996" />
<img width="1247" height="820" alt="학습 완료_2" src="https://github.com/user-attachments/assets/5c445e75-cb57-490c-8a4c-a054755fd025" />
