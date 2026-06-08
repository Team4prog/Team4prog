# Team 4 프로그래밍 언어 및 실습 기말 과제

## 1. 프로젝트 개요

### 프로젝트 주제

**C# Windows Forms 기반 DonkeyCar 주행 데이터 관리 및 AI 학습 제어 프로그램**

### 1줄 소개

자율주행 인공지능 차량 DonkeyCar의 주행 이미지와 조향/스로틀 데이터를 불러와 시각화하고, 잘못된 주행 구간을 정제한 뒤, Windows 또는 WSL 환경에서 AI 모델 학습까지 실행할 수 있는 데스크톱 제어 프로그램입니다.

### 개발 기간

| 구분 | 내용 |
|---|---|
| 개발 시작 | 2026.05.16 |
| 개발 종료 | 2026.06.08 |
| 총 개발 기간 | 약 3주 |
| 기준 | GitHub main 브랜치 커밋 기록 기준 |

본 프로젝트는 초기 Windows Forms 프로젝트 구성부터 DonkeyCar 데이터 로드, Tub Manager 기능, Trainer 연동, UI 개선, 예외 처리, README 정리까지 단계적으로 개발되었습니다.

### 개발 환경

| 구분 | 내용 |
|---|---|
| 개발 언어 | C# |
| 프레임워크 | .NET 10.0 Windows Forms |
| IDE | Visual Studio 2022 |
| 패키지 | Newtonsoft.Json 13.0.3 |
| 실행 환경 | Windows |
| 연동 환경 | Python, DonkeyCar, WSL |
| 형상 관리 | Git, GitHub |

### 사용한 주요 컨트롤

- `Panel`
- `GroupBox`
- `Button`
- `TextBox`
- `ComboBox`
- `ListBox`
- `TrackBar`
- `PictureBox`
- `NumericUpDown`
- `FlowLayoutPanel`
- `FolderBrowserDialog`
- `OpenFileDialog`

## 2. 과제 단계별 완료 현황

| 단계 | 구현 항목 | 완료 상태 | 상세 설명 |
|---|---|---:|---|
| 1 | Windows Forms 프로젝트 생성 | 완료 | `.NET 10.0 Windows Forms` 기반 프로젝트로 구성되어 있으며 `Team4prog.csproj`에서 `UseWindowsForms`가 활성화되어 있습니다. |
| 2 | 메인 화면 UI 구성 | 완료 | `Form1.Designer.cs`와 `Form1.cs`를 통해 Tub Manager 화면과 Trainer 화면을 하나의 메인 폼 안에 구성했습니다. |
| 3 | 화면 전환 기능 | 완료 | `AppNavigationBar` 커스텀 컨트롤을 사용해 `[Tub Manage]`, `[Trainer]`, `?` 버튼을 제공하고, 각 버튼 클릭 시 패널을 전환합니다. |
| 4 | DonkeyCar 이미지 폴더 불러오기 | 완료 | `FolderBrowserDialog`로 이미지 또는 tub 폴더를 선택하고, jpg/png 이미지를 목록에 로드합니다. |
| 5 | DonkeyCar catalog 파일 파싱 | 완료 | `catalog_0.catalog` JSONL 파일을 `CatalogJsonlReader`로 줄 단위 파싱하여 이미지, angle, throttle 데이터를 연결합니다. |
| 6 | 일반 JSON 메타데이터 파싱 | 완료 | catalog가 없는 경우 개별 `.json` 파일에서 `user/angle`, `user/throttle`, `angle`, `throttle` 값을 읽어옵니다. |
| 7 | 프레임 목록 표시 | 완료 | `ListBox`에 프레임 이미지 파일명을 표시하고 선택한 프레임을 중앙 화면에 보여줍니다. |
| 8 | 이미지 미리보기 | 완료 | `PictureBox`에 현재 프레임 이미지를 표시하며, `Zoom` 모드로 이미지 비율을 유지합니다. |
| 9 | 주행 데이터 표시 | 완료 | 현재 프레임의 `Frame`, `Angle`, `Throttle` 값을 하단 데이터 영역에 출력합니다. |
| 10 | TrackBar 프레임 이동 | 완료 | `TrackBar`를 움직이면 해당 인덱스의 이미지와 주행 데이터가 동기화되어 표시됩니다. |
| 11 | 이전/다음 프레임 이동 | 완료 | `<`, `>` 버튼으로 프레임을 한 칸씩 이동할 수 있습니다. |
| 12 | 정방향/역방향 자동 재생 | 완료 | `>>`, `<<`, `Stop` 버튼과 `Timer`를 이용해 정방향 및 역방향 재생을 지원합니다. |
| 13 | 재생 속도 조절 | 완료 | `NumericUpDown` 값으로 재생 속도를 변경하고 타이머 간격을 계산합니다. |
| 14 | Angle/Throttle 차트 시각화 | 완료 | `chartPanel`에 angle과 throttle 값을 선 그래프로 그려 주행 패턴을 시각화합니다. |
| 15 | 현재 프레임 차트 하이라이트 | 완료 | 현재 선택된 프레임 위치를 차트 위에 붉은 영역으로 표시합니다. |
| 16 | 차트 클릭 이동 | 완료 | 차트의 x축 위치를 클릭하면 해당 프레임 인덱스로 이동합니다. |
| 17 | 단일 프레임 삭제 | 완료 | 선택한 이미지 파일을 실제 파일 시스템에서 삭제하고, 연결 JSON 및 catalog 항목도 제거합니다. |
| 18 | Tub Cleaner 범위 지정 | 완료 | `Set Left`, `Set Right` 버튼으로 삭제할 프레임 구간의 시작과 끝을 지정합니다. |
| 19 | 범위 삭제 | 완료 | 지정한 범위의 프레임을 메모리 목록에서 일괄 제거하고 목록/차트/트랙바를 갱신합니다. |
| 20 | 범위 복구 | 완료 | 삭제된 프레임을 저장해 두었다가 `Restore` 버튼으로 기존 위치 근처에 복원합니다. |
| 21 | 필터링 기능 | 완료 | angle, throttle 값과 비교 연산자 조건을 이용해 조건에 맞는 프레임만 필터링합니다. |
| 22 | 필터 입력 검증 | 완료 | -1.0 ~ 1.0 범위를 벗어나거나 숫자가 아닌 입력은 빨간색으로 표시합니다. |
| 23 | 필터 해제 | 완료 | `Clear` 버튼으로 원본 프레임 목록과 데이터를 복구합니다. |
| 24 | 폴더 새로고침 | 완료 | `Reload` 버튼으로 마지막으로 불러온 폴더를 다시 로드합니다. |
| 25 | Trainer 화면 구성 | 완료 | 차량 폴더 선택, 모델 타입 선택, 모델 선택, 학습 실행, 모델 관리 영역을 구성했습니다. |
| 26 | Python 학습 프로세스 실행 | 완료 | `ProcessStartInfo`를 이용해 DonkeyCar 학습 명령을 외부 프로세스로 실행합니다. |
| 27 | Windows 로컬 Python 연동 | 완료 | `DONKEYCAR_PYTHON` 환경 변수를 읽어 로컬 Python 실행 경로로 학습을 실행합니다. |
| 28 | WSL 경로 및 WSL 실행 연동 | 완료 | WSL UNC 경로, Linux 절대 경로, Windows 경로를 WSL 실행용 경로로 변환합니다. |
| 29 | 학습 로그 실시간 출력 | 완료 | `OutputDataReceived`, `ErrorDataReceived` 이벤트를 사용해 학습 로그를 `listBoxLog`에 실시간 출력합니다. |
| 30 | 학습 중복 실행 방지 | 완료 | `isTrainingRunning` 플래그로 동시에 여러 학습이 실행되지 않도록 제어합니다. |
| 31 | 학습 중지 | 완료 | 학습 중 `Train` 버튼이 `Stop`으로 바뀌며, 실행 중인 프로세스 트리를 종료할 수 있습니다. |
| 32 | 학습 loss 차트 | 완료 | 로그에서 `loss`, `val_loss` 값을 정규식으로 추출하여 `Training Loss Chart`에 표시합니다. |
| 33 | 모델 목록 관리 | 완료 | `models` 폴더의 `.h5`, `.keras`, `.tflite`, `.onnx`, `.pt`, `.pkl` 파일을 찾아 목록에 표시합니다. |
| 34 | 모델 선택 및 삭제 | 완료 | 기존 모델 파일을 선택해 이어서 학습하거나, 선택한 모델 파일을 삭제할 수 있습니다. |
| 35 | 도움말 창 | 완료 | 상단 `?` 버튼을 통해 별도 도움말 창을 열 수 있습니다. |
| 36 | 반응형 레이아웃 | 완료 | 창 크기 변경 시 주요 컨트롤 위치와 크기를 다시 계산하여 UI 겹침을 줄였습니다. |
| 37 | 예외 처리 및 로그 | 완료 | 파일 로드, 이미지 표시, 삭제, 필터, 학습 실행 등 주요 기능에 예외 처리와 로그 출력을 적용했습니다. |
| 38 | Trainer 하단 로그 출력  | 완료 | 학습 실행 시 Python 프로세스의 출력 로그가 Trainer 창 하단의 ListBox 영역에 실시간으로 표시되도록 구현했습니다. |
| 39 | 이미지 삭제 휴지통 기능     | 완료 | Tub Manager에서 삭제한 이미지와 연결된 angle/throttle 데이터를 바로 제거하지 않고 휴지통 목록에 보관하도록 구현했습니다. |
| 40 | 휴지통 Restore 복구    | 완료 | 폴더 열기 버튼 아래에 휴지통 영역을 구성하고, `Restore` 버튼을 통해 삭제한 프레임을 기존 위치 근처로 복구할 수 있도록 구현했습니다. |
| 41 | 휴지통 데이터 학습 방지     | 완료 | 휴지통에 삭제 예정 데이터가 남아 있을 경우 Trainer에서 학습을 실행하지 않고 경고 메시지를 표시하도록 구현했습니다. |
| 42 | 모델 파일 자동 생성 방식 개선 | 완료 | 기존 모델 파일을 덮어쓰지 않고, 학습 실행 시 날짜와 모델 타입을 포함한 새 모델 파일명으로 저장되도록 구현했습니다. |

## 3. 핵심 기능 설명

### 3.1 Tub Manager

Tub Manager는 DonkeyCar 주행 데이터를 불러오고, 프레임 단위로 확인하며, 잘못된 구간을 정리하는 데이터 관리 화면입니다.

#### 폴더 열기

- `폴더 열기` 버튼으로 DonkeyCar tub 폴더 또는 이미지 폴더를 선택합니다.
- 선택한 폴더에 `catalog_0.catalog`가 있으면 catalog 기반으로 데이터를 로드합니다.
- catalog가 없는 경우 `images/` 폴더 또는 현재 폴더에서 jpg/png 이미지를 검색합니다.
- 이미지 파일명 앞 숫자를 기준으로 정렬하여 실제 주행 순서에 가깝게 프레임을 배치합니다.

#### catalog JSONL 파싱

- DonkeyCar의 `catalog_0.catalog` 파일은 한 줄에 하나의 JSON 객체가 들어 있는 JSONL 형식입니다.
- `CatalogJsonlReader.ReadFrames()`는 `File.ReadLines()`를 사용해 파일을 줄 단위로 읽습니다.
- 각 줄은 `FrameData` 모델로 변환됩니다.
- 사용하는 주요 필드는 다음과 같습니다.

| catalog key | 프로그램 필드 | 의미 |
|---|---|---|
| `_index` | `Index` | 프레임 순번 |
| `cam/image_array` | `ImagePath` | 이미지 경로 |
| `user/angle` | `Angle` | 조향값 |
| `user/throttle` | `Throttle` | 스로틀값 |

#### 프레임 표시

- 좌측 `ListBox`에는 로드된 이미지 파일명이 표시됩니다.
- 중앙 `PictureBox`에는 선택된 프레임 이미지가 출력됩니다.
- 이미지는 파일 잠김을 방지하기 위해 스트림으로 읽은 뒤 `Bitmap`으로 복사하여 표시합니다.
- 이전 이미지는 `Dispose()` 처리하여 GDI 리소스 누수를 줄였습니다.

#### 주행 데이터 표시

- 선택한 프레임의 인덱스, angle, throttle 값을 화면에 표시합니다.
- 값이 없거나 파싱할 수 없는 경우 `N/A`로 표시합니다.
- angle/throttle 값은 소수점 둘째 자리까지 출력합니다.

#### 프레임 이동

- `ListBox` 선택
- `TrackBar` 이동
- `<`, `>` 버튼
- 차트 클릭
- 정방향/역방향 자동 재생

위 입력 방식이 모두 같은 프레임 선택 로직으로 연결되어 이미지, 텍스트 데이터, 트랙바, 차트 하이라이트가 함께 갱신됩니다.

### 3.2 주행 데이터 차트

하단 차트는 angle과 throttle의 변화를 한눈에 확인하기 위한 시각화 기능입니다.

- angle은 파란색 선으로 표시합니다.
- throttle은 노란색 선으로 표시합니다.
- 중앙 기준선은 0 값을 의미합니다.
- 현재 프레임은 붉은색 반투명 막대로 강조됩니다.
- 차트의 x축을 클릭하면 해당 위치의 프레임으로 이동합니다.
- 차트 이미지는 캐시 비트맵으로 관리하여 프레임 이동 시 전체 차트를 매번 다시 그리지 않도록 최적화했습니다.

### 3.3 Tub Cleaner

Tub Cleaner는 주행 실수, 충돌, 불필요한 구간을 빠르게 정리하기 위한 기능입니다.

#### 범위 지정

- 정리할 시작 프레임을 선택한 뒤 `Set Left`를 누릅니다.
- 정리할 끝 프레임을 선택한 뒤 `Set Right`를 누릅니다.
- 선택된 범위는 `[left, right]` 형태로 표시됩니다.
- left/right 순서가 반대로 지정되어도 내부적으로 작은 인덱스부터 큰 인덱스까지 정리합니다.

#### 범위 삭제

- `Delete` 버튼을 누르면 지정된 범위의 프레임이 현재 목록에서 제거됩니다.
- 삭제 전 이미지 경로, angle, throttle, 원래 인덱스를 따로 저장합니다.
- 삭제 후 `ListBox`, `TrackBar`, 차트가 다시 갱신됩니다.

#### 복구

- `Restore` 버튼을 누르면 삭제된 프레임을 기존 위치 근처에 다시 삽입합니다.
- 복원 후 삭제 버퍼는 초기화됩니다.
- 현재 구현은 메모리 목록 기준 복구 기능이며, 단일 파일 삭제처럼 실제 삭제된 파일을 되살리는 기능은 아닙니다.

### 3.4 단일 프레임 삭제

선택한 프레임 하나를 실제 파일 시스템에서 삭제하는 기능입니다.

- 선택한 이미지 파일 삭제
- 같은 이름의 `.json` 파일이 있으면 함께 삭제
- 관련 catalog 파일에서 해당 이미지 항목 제거
- 삭제 전 2단계 확인 메시지 표시
- 삭제 실패 시 권한 오류, I/O 오류, 일반 오류를 구분해 로그 출력

이 기능은 실제 파일을 삭제하므로, Tub Cleaner의 범위 삭제와 다르게 복구가 어렵습니다.

### 3.5 스마트 필터링

필터 기능은 angle과 throttle 조건에 맞는 프레임만 빠르게 찾기 위한 기능입니다.

지원 연산자는 다음과 같습니다.

- `>`
- `<`
- `>=`
- `<=`
- `==`

지원하는 필터 대상은 다음과 같습니다.

- Angle
- Throttle

예시:

| 조건 | 의미 |
|---|---|
| Angle `>` `0.5` | 오른쪽으로 크게 조향한 프레임만 표시 |
| Angle `<` `-0.5` | 왼쪽으로 크게 조향한 프레임만 표시 |
| Throttle `>` `0.8` | 높은 속도 구간만 표시 |
| Angle `==` `0` | 직진에 가까운 구간 표시 |

입력값 검증도 구현되어 있습니다.

- 숫자가 아닌 값은 잘못된 입력으로 표시됩니다.
- DonkeyCar 정규화 값 기준인 `-1.0 ~ 1.0` 범위를 벗어나면 빨간색 배경으로 표시됩니다.
- `Clear` 버튼을 누르면 필터 적용 전 원본 목록으로 복원됩니다.

### 3.6 Trainer

Trainer는 C# UI에서 Python 기반 DonkeyCar 학습 명령을 실행하는 화면입니다.

#### 차량 폴더 선택

- `Select Car Folder` 버튼으로 DonkeyCar `mycar` 폴더를 선택합니다.
- 선택한 폴더에 `manage.py` 또는 `train.py`가 있는지 검사합니다.
- 폴더가 존재하지 않거나 학습 파일이 없으면 오류 메시지를 표시합니다.

#### 모델 타입 선택

`cmbModelType`에서 다음 모델 타입을 선택할 수 있습니다.

- `linear`
- `categorical`

모델 타입이 비어 있으면 기본값으로 `linear`를 사용합니다.

#### 학습 실행

`Train` 버튼을 누르면 Python 학습 프로세스가 시작됩니다.

로컬 Windows 환경에서는 다음 흐름으로 실행됩니다.

- 환경 변수 `DONKEYCAR_PYTHON` 확인
- 값이 없으면 `python` 명령 사용
- 선택한 mycar 폴더를 WorkingDirectory로 지정
- `train.py` 또는 `manage.py train` 실행

WSL 환경에서는 다음 흐름으로 실행됩니다.

- WSL UNC 경로 또는 Linux 절대 경로 감지
- Windows 경로를 `/mnt/c/...` 형식으로 변환
- `wsl.exe`를 통해 bash 명령 실행
- conda 환경 `e2e_env` 활성화
- `models` 폴더 생성 후 학습 명령 실행

#### 학습 중지

- 학습이 실행 중일 때 `Train` 버튼은 `Stop`으로 바뀝니다.
- `Stop`을 누르면 현재 학습 프로세스와 하위 프로세스를 종료합니다.
- 중복 학습 실행을 막기 위해 `isTrainingRunning` 상태 플래그를 사용합니다.

#### 실시간 로그 출력

- Python 프로세스의 표준 출력과 표준 오류를 리디렉션합니다.
- 출력된 로그는 `listBoxLog`에 실시간으로 추가됩니다.
- 학습 종료 후 전체 로그를 파일로 저장합니다.
- Windows 경로에서는 선택한 차량 폴더의 `logs` 폴더에 저장합니다.
- Linux 절대 경로에서는 Windows 문서 폴더의 `Team4progTrainingLogs`에 저장합니다.

#### 학습 loss 차트

- 학습 로그에서 `loss:`와 `val_loss:` 값을 정규식으로 추출합니다.
- 추출된 값은 `Training Loss Chart`에 실시간으로 표시됩니다.
- `loss`는 초록색, `val_loss`는 주황색 선으로 표시됩니다.

#### Trainer 하단 실시간 로그 출력

Trainer 화면에서는 학습 실행 중 발생하는 로그를 화면 하단의 ListBox 영역에 실시간으로 출력하도록 구현했습니다.

* `Train` 버튼을 누르면 Python 또는 WSL 학습 프로세스가 실행됩니다.
* 학습 프로세스의 표준 출력과 표준 오류를 받아와 Trainer 창 하단 로그 ListBox에 표시합니다.
* 로그가 추가될 때마다 가장 최근 로그가 보이도록 스크롤 위치를 자동으로 갱신합니다.
* 학습 시작, 학습 명령, 오류 메시지, 학습 완료, 학습 중지 상태를 모두 로그로 확인할 수 있습니다.
* 사용자는 별도의 터미널을 보지 않아도 WinForms 프로그램 안에서 학습 진행 상태를 확인할 수 있습니다.

이 기능을 통해 학습이 정상적으로 진행 중인지, 오류가 발생했는지, 모델이 생성되었는지를 프로그램 내부에서 바로 확인할 수 있습니다.

#### 이미지 삭제 휴지통 및 Restore 복구 기능

Tub Manager에서는 이미지 삭제 시 데이터를 바로 완전히 삭제하지 않고, 휴지통에 임시 보관한 뒤 필요하면 복구할 수 있도록 기능을 개선했습니다.

* 폴더 열기 버튼 아래에 휴지통 영역을 추가했습니다.
* 사용자가 이미지 또는 프레임을 삭제하면 해당 데이터가 휴지통 목록에 저장됩니다.
* 삭제 시 이미지 경로, angle 값, throttle 값, 기존 프레임 위치를 함께 저장합니다.
* 삭제 후에는 현재 프레임 목록, TrackBar, 차트가 갱신됩니다.
* `Restore` 버튼을 누르면 휴지통에 있던 데이터가 기존 인덱스 근처로 다시 복구됩니다.
* 복구 후에는 프레임 목록, TrackBar, 차트가 다시 갱신됩니다.
* 실수로 삭제한 프레임을 다시 살릴 수 있어 데이터 정제 과정의 안정성이 높아졌습니다.

이 기능은 주행 데이터 정리 중 실수로 필요한 프레임을 삭제했을 때 다시 복구할 수 있도록 만든 안전장치입니다.

#### 휴지통 데이터 존재 시 학습 방지

Trainer의 `Train` 기능에는 학습 전 데이터 정리 상태를 확인하는 검증 로직을 추가했습니다.

* 학습 실행 전에 휴지통에 남아 있는 데이터가 있는지 확인합니다.
* 휴지통에 삭제 예정 데이터가 남아 있으면 학습을 바로 실행하지 않습니다.
* 경고 메시지 창을 표시해 사용자가 먼저 휴지통 데이터를 복원하거나 완전히 정리하도록 안내합니다.
* 이 상태에서는 Python 학습 프로세스가 실제로 실행되지 않도록 차단했습니다.
* 삭제 예정 데이터가 학습에 포함되거나, catalog와 이미지 목록이 어긋난 상태로 학습되는 문제를 예방할 수 있습니다.

이 기능은 Tub Manager의 데이터 정제 과정과 Trainer의 모델 학습 과정이 자연스럽게 연결되도록 해 주는 역할을 합니다.

#### 모델 파일 자동 생성 방식 개선

기존 학습 방식에서는 같은 모델 파일명을 사용할 경우 기존 모델이 덮어써질 가능성이 있었습니다. 이를 방지하기 위해 학습 실행 시마다 새로운 모델 파일명이 자동 생성되도록 개선했습니다.

* 학습을 시작할 때 현재 날짜와 시간을 기준으로 새 모델 파일명을 생성합니다.
* 모델 파일명에는 학습 시각과 모델 타입이 함께 포함됩니다.
* 예시 파일명은 다음과 같습니다.

```text
20260608_143210_linear.h5
20260608_143210_categorical.h5
```

* 생성된 모델은 `models` 폴더 안에 저장됩니다.
* 기존 모델 파일을 직접 덮어쓰지 않으므로, 이전 학습 결과를 보존할 수 있습니다.
* 학습을 여러 번 실행해도 각 결과물이 별도 파일로 남아 모델 비교가 쉬워집니다.
* 학습 후 Pilot Manager에서 새로 생성된 모델 목록을 확인할 수 있습니다.

이 기능을 통해 사용자는 학습 결과를 누적 관리할 수 있고, 가장 성능이 좋은 모델을 나중에 선택해서 사용할 수 있습니다.

#### Pilot Manager

학습 모델 파일을 관리하는 기능입니다.

- `models` 폴더에서 모델 파일 자동 탐색
- 지원 확장자: `.h5`, `.keras`, `.tflite`, `.onnx`, `.pt`, `.pkl`
- `.h5`, `.keras` 모델은 이어서 학습 가능한 모델로 우선 표시
- `Choose Model`로 외부 모델 파일 선택 가능
- `Delete Model`로 선택한 모델 파일 삭제 가능
- WSL Linux 경로의 모델도 `find`, `stat`, `rm` 명령으로 조회 및 삭제 처리 가능

## 4. 역할 분담

### 양채현 - 시스템 통합 및 Python 연동

담당 영역은 C# Windows Forms UI와 Python DonkeyCar 학습 프로세스의 연결입니다.

구현 내용:

- `ProcessStartInfo`를 사용한 외부 Python 학습 프로세스 실행
- `DONKEYCAR_PYTHON` 환경 변수 기반 로컬 Python 실행
- WSL UNC 경로 및 Linux 절대 경로 처리
- Windows 경로를 WSL `/mnt/...` 경로로 변환
- `wsl.exe`를 통한 bash 학습 명령 실행
- `OutputDataReceived`, `ErrorDataReceived` 기반 실시간 로그 출력
- `isTrainingRunning`을 이용한 중복 학습 실행 방지
- 학습 중 `Stop` 버튼으로 프로세스 종료
- 학습 로그 파일 저장
- 학습 loss/val_loss 실시간 차트 연동

관련 파일:

- `Features/Trainer/Form1.Trainer.cs`
- `Features/Trainer/Form1.LossChart.cs`

### 강민규 - 메인 UI 및 컨트롤 구현

담당 영역은 전체 화면 구성, 패널 전환, 컨트롤 배치, 반응형 레이아웃입니다.

구현 내용:

- Tub Manager 화면 구성
- Trainer 화면 구성
- `AppNavigationBar` 커스텀 네비게이션 바 구현
- `[Tub Manage]`, `[Trainer]`, `?` 버튼 구성
- `panelTubManager`, `panelTrainer` 전환 로직 구현
- `ApplyResponsiveLayout()`을 통한 창 크기별 컨트롤 재배치
- `Anchor`, `Dock`, `AutoScroll`, `MinimumSize` 설정
- `PictureBoxSizeMode.Zoom` 적용으로 이미지 비율 유지
- 도움말 팝업 창 구현

관련 파일:

- `Form1.cs`
- `Form1.Designer.cs`
- `Components/AppNavigationBar.cs`
- `HelpForm.cs`

### 최율도 - 데이터 파싱 및 파일 처리

담당 영역은 DonkeyCar 주행 데이터 로딩, catalog 파싱, 이미지와 메타데이터 매핑입니다.

구현 내용:

- `Newtonsoft.Json` 기반 JSON 파싱
- `catalog_0.catalog` JSONL 파일 줄 단위 파싱
- `FrameData` 모델 구현
- catalog의 이미지 경로를 실제 파일 경로로 변환
- `images/` 폴더 자동 탐색
- 이미지 파일명 숫자 기준 정렬
- 개별 JSON 파일의 `user/angle`, `user/throttle` 파싱
- 이미지와 angle/throttle 데이터의 인덱스 정렬
- 이미지 누락, JSON 파싱 오류, 파일 개수 불일치 로그 처리

관련 파일:

- `Features/Catalog/CatalogJsonlReader.cs`
- `Features/Catalog/FrameData.cs`
- `Features/TubManager/Form1.FrameFiles.cs`

### 박경일 - 기능 보조 및 데이터 관리

담당 영역은 데이터 정제, 필터링, 재생 제어, 차트 상호작용입니다.

구현 내용:

- Tub Cleaner 범위 지정
- 범위 삭제 및 복구
- 단일 프레임 삭제
- catalog 항목 제거
- angle/throttle 조건 필터링
- 입력값 유효성 검사
- 필터 해제 및 원본 목록 복원
- 정방향/역방향 재생
- 재생 속도 조절
- angle/throttle 차트 표시
- 현재 프레임 차트 하이라이트
- 차트 클릭 이동

관련 파일:

- `Features/TubManager/Form1.TubCleaner.cs`
- `Features/TubManager/Form1.Filter.cs`
- `Features/TubManager/Form1.Playback.cs`
- `Features/TubManager/Form1.Chart.cs`

## 5. 프로젝트 구조

```text
Team4prog
├─ Components
│  └─ AppNavigationBar.cs
├─ Features
│  ├─ Catalog
│  │  ├─ CatalogJsonlReader.cs
│  │  └─ FrameData.cs
│  ├─ Trainer
│  │  ├─ Form1.Trainer.cs
│  │  └─ Form1.LossChart.cs
│  └─ TubManager
│     ├─ Form1.Chart.cs
│     ├─ Form1.Filter.cs
│     ├─ Form1.FrameFiles.cs
│     ├─ Form1.Playback.cs
│     └─ Form1.TubCleaner.cs
├─ Properties
├─ Form1.cs
├─ Form1.Designer.cs
├─ HelpForm.cs
├─ Program.cs
├─ Team4prog.csproj
└─ Team4prog.slnx
```

## 6. 실행 방법

### 6.1 사전 준비

1. Windows PC에 Visual Studio 2022를 설치합니다.
2. `.NET 10.0 SDK`를 설치합니다.
3. DonkeyCar 학습을 실행하려면 Python 또는 WSL 환경을 준비합니다.
4. Visual Studio에서 `Team4prog.slnx`를 엽니다.
5. NuGet 패키지 `Newtonsoft.Json`이 복원되었는지 확인합니다.

### 6.2 로컬 Windows Python 환경 변수

로컬 Python으로 학습하려면 다음 환경 변수를 설정합니다.

```text
DONKEYCAR_PYTHON=C:\path\to\python.exe
```

예시:

```text
DONKEYCAR_PYTHON=C:\Users\user\miniconda3\envs\e2e_env\python.exe
```

### 6.3 WSL 환경 변수

WSL 배포판을 명시하고 싶으면 다음 환경 변수를 설정합니다.

```text
DONKEYCAR_WSL_DISTRO=Ubuntu-22.04
```

설정하지 않으면 설치된 WSL 배포판 중 첫 번째 항목을 사용하고, 찾지 못하면 기본값으로 `Ubuntu-22.04`를 사용합니다.

## 7. 사용 방법

### Tub Manager 사용 순서

1. `[Tub Manage]` 화면으로 이동합니다.
2. `폴더 열기` 버튼을 클릭합니다.
3. DonkeyCar tub 폴더 또는 이미지 폴더를 선택합니다.
4. 좌측 목록에서 프레임을 선택합니다.
5. 중앙 이미지와 하단 주행 데이터를 확인합니다.
6. TrackBar, 이전/다음 버튼, 재생 버튼으로 프레임을 탐색합니다.
7. 잘못된 이미지나 프레임을 삭제하면 해당 데이터는 휴지통으로 이동합니다.
8. 실수로 삭제한 경우 폴더 열기 버튼 아래의 휴지통 영역에서 `Restore`를 눌러 복구합니다.
9. 삭제한 데이터가 남아 있으면 학습이 실행되지 않으므로, 학습 전에 휴지통 데이터를 정리합니다.

### Trainer 사용 순서

1. `[Trainer]` 화면으로 이동합니다.
2. `Select Car Folder`로 DonkeyCar `mycar` 폴더를 선택합니다.
3. `Model Type`에서 `linear` 또는 `categorical`을 선택합니다.
4. `Train` 버튼을 눌러 학습을 시작합니다.
5. 학습 중 출력되는 로그는 Trainer 창 하단 ListBox에서 실시간으로 확인합니다.
6. 휴지통에 데이터가 남아 있으면 학습 불가 메시지가 표시되고 학습은 실행되지 않습니다.
7. 학습이 완료되면 날짜와 모델 타입이 포함된 새 모델 파일이 `models` 폴더에 생성됩니다.
8. Pilot Manager에서 생성된 모델을 확인하거나 삭제할 수 있습니다.

## 8. 예외 처리 및 안정성

프로그램은 다음 상황에 대한 방어 코드를 포함합니다.

- 폴더가 존재하지 않는 경우
- 이미지 파일이 없는 경우
- 이미지 파일 접근 권한이 없는 경우
- 이미지 파일이 다른 프로세스에서 사용 중인 경우
- JSON 파싱에 실패한 경우
- catalog 파일의 일부 줄이 잘못된 경우
- catalog에는 있으나 실제 이미지가 없는 경우
- 이미지 개수와 JSON 개수가 다른 경우
- 필터 입력값이 숫자가 아닌 경우
- angle/throttle 값이 범위를 벗어난 경우
- 학습 폴더에 `manage.py` 또는 `train.py`가 없는 경우
- Python 또는 WSL 실행 파일을 찾을 수 없는 경우
- 학습 프로세스가 비정상 종료된 경우
- 모델 파일이 존재하지 않는 경우
- 모델 삭제 권한이 없는 경우

## 9. 구현상 특징

- 기능별 partial class 분리로 코드 가독성을 높였습니다.
- `Catalog`, `TubManager`, `Trainer`, `Components` 폴더로 역할을 구분했습니다.
- 이미지 표시 시 파일 잠김을 방지하기 위해 `FileStream`으로 읽은 뒤 복사본을 사용합니다.
- 차트는 캐시 비트맵을 사용해 프레임 이동 시 렌더링 부담을 줄였습니다.
- 학습 프로세스는 UI 스레드를 막지 않도록 비동기 방식으로 실행합니다.
- 로그 출력은 항상 최신 항목이 보이도록 `TopIndex`를 갱신합니다.
- WSL 경로와 Windows 경로를 모두 고려해 학습 실행 범위를 넓혔습니다.

## 10. 간단 코드 리뷰

### 10.1 잘된 점

- 기능별로 `Catalog`, `TubManager`, `Trainer`, `Components` 폴더를 나누고 partial class를 활용해 한 파일에 모든 기능이 몰리지 않도록 구성한 점이 좋습니다.
- DonkeyCar의 `catalog_0.catalog` JSONL 파일을 줄 단위로 읽어 처리하므로, 데이터가 많아져도 전체 파일을 한 번에 메모리에 올리는 방식보다 안정적입니다.
- 이미지 표시 시 기존 이미지를 `Dispose()`하고, 스트림으로 읽은 뒤 `Bitmap` 복사본을 사용해 이미지 파일 잠김과 GDI 리소스 누수를 줄이려는 처리가 들어가 있습니다.
- 프레임 선택, TrackBar 이동, 차트 클릭, 자동 재생이 같은 프레임 표시 흐름으로 연결되어 있어 사용자 입장에서 조작 방식이 자연스럽습니다.
- Python 학습 프로세스를 WinForms UI에서 직접 실행하고, 표준 출력과 오류 로그를 실시간으로 보여 주도록 구성한 점은 프로젝트 완성도를 높여 줍니다.
- 파일 로드, JSON 파싱, 이미지 표시, 삭제, 학습 실행 부분에 예외 처리가 들어가 있어 갑작스러운 프로그램 종료 가능성을 줄였습니다.

### 10.2 개선하면 좋은 점

- `LoadImages`, `ShowImage`, `RunTraining`처럼 기능이 많은 메서드는 더 작은 메서드로 분리하면 유지보수가 쉬워집니다.
- 일부 UI 문자열이 한글과 영어가 섞여 있으므로, 최종 제출 전 버튼명과 메시지 문구를 한글 중심으로 통일하면 완성도가 더 높아집니다.
- 단일 프레임 삭제는 실제 파일을 바로 삭제하므로, 백업 폴더로 이동하거나 휴지통으로 보내는 방식으로 바꾸면 실수 복구가 쉬워집니다.
- 현재는 `catalog_0.catalog` 중심으로 처리하므로, 추후에는 `catalog_*.catalog` 파일을 자동으로 찾아 여러 catalog 파일을 병합하는 기능을 추가할 수 있습니다.
- 학습 환경명이 고정되어 있는 부분은 UI 입력값이나 설정 파일로 분리하면 다른 PC에서도 실행하기 쉬워집니다.
- 반복해서 사용하는 색상, 확장자 목록, 로그 문구 등은 상수나 별도 설정으로 분리하면 코드 중복을 줄일 수 있습니다.

### 10.3 종합 평가

전체적으로 이 프로젝트는 단순한 화면 구성 과제 수준을 넘어, 실제 DonkeyCar 주행 데이터 확인, 정제, 학습 실행까지 연결한 점이 장점입니다. 특히 데이터 파싱, 이미지 미리보기, 차트 시각화, WSL/Python 연동을 하나의 WinForms 프로그램 안에 통합했다는 점에서 실습 과제의 요구사항을 충실히 반영했습니다. 다만 장기적으로 유지보수하기 위해서는 메서드 분리, 설정값 외부화, 삭제 복구 방식 개선을 추가하면 더 안정적인 프로그램이 될 수 있습니다.

## 11. 실행 화면

### Tub Manager 화면

<img width="1919" height="1079" alt="tub manage 화면" src="https://github.com/user-attachments/assets/8f19fba8-fa02-4d9a-8a63-e8f8673cae39" />

### Trainer 화면

<img width="1919" height="1079" alt="trainer 화면" src="https://github.com/user-attachments/assets/4bcf63a7-7c68-4da0-8dd7-350deca189a3" />

### 폴더 선택 화면

<img width="1347" height="1079" alt="Folder 선택시" src="https://github.com/user-attachments/assets/00fd0dfa-9d47-465f-b849-fe5c3e552af2" />

### 학습 중 화면

<img width="359" height="418" alt="학습 중" src="https://github.com/user-attachments/assets/827173d7-318e-42df-b209-8ea32e7fbd44" />

### 학습 완료 화면

<img width="1377" height="962" alt="학습 완료 1" src="https://github.com/user-attachments/assets/6aac4d63-7601-471a-b802-a71480f64996" />

<img width="1247" height="820" alt="학습 완료 2" src="https://github.com/user-attachments/assets/5c445e75-cb57-490c-8a4c-a054755fd025" />

## 12. 최종 정리

본 프로젝트는 단순 이미지 뷰어가 아니라 DonkeyCar 주행 데이터를 실제 학습 전처리 흐름에 맞춰 관리할 수 있도록 구현된 Windows Forms 기반 데스크톱 프로그램입니다.

완료된 핵심 기능은 다음과 같습니다.

- DonkeyCar 주행 이미지 로드
- catalog JSONL 파싱
- angle/throttle 데이터 매핑
- 프레임 단위 시각화
- 차트 기반 주행 데이터 확인
- 정방향/역방향 재생
- 속도 조절
- 단일 프레임 삭제
- 범위 삭제 및 복구
- 조건 기반 필터링
- Windows/WSL Python 학습 실행
- 실시간 학습 로그 출력
- loss/val_loss 차트 표시
- 모델 파일 목록 관리 및 삭제
- 도움말 및 반응형 UI

따라서 과제 요구사항 중 Windows Forms UI 구성, 파일 처리, 데이터 파싱, 예외 처리, 사용자 입력 처리, 외부 프로세스 연동, 시각화, 기능별 역할 분담 구현이 모두 반영되었습니다.
