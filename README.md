# 🌟 STARXR - AI Chat Service Project

**STARXR**는 WebGL 플랫폼을 지원하는 프로젝트로, 여러 서비스 중 저는 AI 채팅 서비스를 담당하였습니다.

---

## 📝 프로젝트 개요
STARXR 내 AI 채팅 서비스는 다음의 세 가지 입력 방식을 지원합니다
- **텍스트 입력**을 통한 AI 질의
- **음성(STT) 입력**을 통한 AI 질의
- **이미지를 통한** AI 질의
- **음성합성(TTS)** 통한 사용자 지정 음성으로 응답 제공

🎯 사용자 경험을 극대화하기 위해, 유명인 목소리 또는 개인의 목소리를 등록하여 AI 응답을 해당 음성으로 출력하는 기능을 포함하였습니다! 

---

## 🚀 담당 역할 (My Contributions)

🔹 AI 채팅 시스템 개발<br>
> ✔️ 입력 방식 통합 (텍스트, 음성, 이미지)<br>
✔️ 음성 인식(STT) 및 음성 합성(TTS) 적용<br>
✔️ 사용자 음성 커스터마이징 기능 구현<br>

🔹 시스템 설계 및 최적화<br>
> ✔️ WebGL 환경 최적화 (메모리 및 성능 고려)<br>
✔️ 코드 구조 설계 (SOLID 원칙 적용, 인터페이스 기반 구조화)<br>
✔️ AI API 및 STT 라이브러리 연동<br>

---

## ✨ 주요 기능 (Features)
- **다양한 입력 방식 지원**: 텍스트, 음성(STT), 이미지 기반 질문 가능
- **커스터마이징된 음성 출력**: 유명인 또는 개인의 목소리를 등록해 답변을 해당 목소리로 청취 가능
- **WebGL 플랫폼 최적화**: 웹 브라우저 환경에서 원활하게 동작
- **AI API 연동**: 자연스러운 대화 제공

---
🛠️ 코드 구조 (Code Structure)
SOLID 원칙을 적용하여, 유지보수성과 확장성을 고려한 설계

📦 STARXR <br>
├── 📁 Scripts # 주요 기능을 담당하는 C# 스크립트 <br>
  &nbsp;&nbsp;&nbsp;├── 📁 AI <br>
  &nbsp;&nbsp;&nbsp;├── 📁 Components/ → 각 기능별 하위 모듈 (Managers, Items) <br> 
  &nbsp;&nbsp;&nbsp;├── 📁 Core/ → 공통 설정 및 핵심 로직 <br> 
  &nbsp;&nbsp;&nbsp;├── 📁 Data/ → 데이터 전송 객체(DTO), 모델 관리 <br> 
  &nbsp;&nbsp;&nbsp;├── 📁 Interfaces/ → 인터페이스 관리 (의존성 주입을 위해 분리) <br> 
  &nbsp;&nbsp;&nbsp;├── 📁 Plugins/ → AI API 및 외부 라이브러리 통합 <br>

  
📌 주요 클래스 설명

AIChatServiceManager → 중앙 컨트롤러, 여러 기능을 조율하고 <br>
➡ **📂 전체 코드 보기**: [AIChatServiceManager.cs](https://github.com/kalstjd96/StarXR/blob/main/Components/Chat/AIChatServiceManager.cs)
<br>

Manager (예: AudioPlayManager, MicManager) → 각 기능별 상태 및 로직 관리 <br>
➡ **📂 전체 코드 보기**: [MicManager.cs (L70-L97)](https://github.com/kalstjd96/StarXR/blob/main/Components/Microphone/MicManager.cs#L70-L97)
<br>

Service (예: GptCommunicationService) → 비즈니스 로직 및 외부 API 통신 담당 <br>
➡ **📂 전체 코드 보기**: [GptCommunicationService.cs](https://github.com/kalstjd96/StarXR/blob/main/Services/GptCommunicationService.cs)
<br>

ServiceImpl → 실제 SDK/API 통신을 담당, 사용자는 직접 접근하지 않음 <br>
➡ **📂 전체 코드 보기**: [GptCommunicationServiceImpl.cs](https://github.com/kalstjd96/StarXR/blob/main/Services/GptCommunicationServiceImpl.cs)
<br>

WebGLBridge  → WebGL 환경에서는 Unity의 Microphone 클래스를 사용할 수 없기 때문에, 브라우저의 네이티브 API와 상호작용하기 위해 JavaScript 라이브러리(.jslib)를 활용하여 마이크 입력을 처리  <br>
➡ **📂 전체 코드 보기**: [WebGLBridge.cs](https://github.com/kalstjd96/StarXR/blob/main/Core/WebGLBridge.cs)
<br>


## 🛠️ 코드 구성 (Code Structure)
프로젝트는 다음과 같은 구조로 구성되어 있습니다:
- **Scripts/**
  - **AI/**  
    - **Components**: 각 기능별로 하위 폴더를 만들어 Managers와 Items로 세분화, Managers에는 각 기능의 핵심 로직을 담당하는 매니저 스크립트를, Items에는 해당 기능과 관련된 UI 요소나 데이터 아이템 스크립트
    - **Core**: 공통적으로 사용되는 설정과 핵심 서비스, Utilities에는 도구 및 상태 관리 관련 스크립트
    - **Data**: 데이터 전송 객체(DTO)와 모델
    - **Interfaces**: 모든 인터페이스를 한곳에 모아둬서 의존성 주입 및 SOLID 원칙에 부합하도록 설계
  - **Plugins/**: AI API 통합 플러그인 및 외부 라이브러리

---

### 🎤 WebGL 환경에서 마이크 입력 처리  
- Unity WebGL에서는 `Microphone` API를 사용할 수 없음  
- 이를 해결하기 위해 **JavaScript (`.jslib`)를 활용하여 브라우저의 마이크 입력을 직접 처리**  
- `WebGLBridge.cs`를 통해 Unity와 JavaScript 간의 **데이터 교환을 수행**  

📂 **관련 파일**
- **📄 WebGLBridge.cs** → Unity ↔ JavaScript 데이터 브리지 역할
- **📄 WebMicInput.jslib** → 브라우저에서 마이크 입력을 받아 Unity로 전달
