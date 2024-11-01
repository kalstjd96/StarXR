# 🌟 STARXR - AI Chat Service Project

**STARXR**는 WebGL 플랫폼을 지원하는 프로젝트로, AI 채팅 서비스를 포함하고 있습니다. 이 서비스는 다음과 같은 기능을 제공합니다:

---

## 📝 프로젝트 개요
STARXR의 AI 채팅 서비스는 다음의 세 가지 입력 방식을 지원합니다:
- **텍스트 입력**을 통한 질문
- **마이크를 통한 질문** (STT 기능을 사용)
- **이미지를 통한 질문**

답변은 텍스트 및 음성으로 제공되며, 등록된 목소리(유명인의 목소리 또는 개인의 목소리)를 사용해 음성으로 출력할 수 있습니다. 이 기능은 사용자 경험을 극대화하고 상호작용을 자연스럽게 만듭니다.

---

## ✨ 주요 기능 (Features)
- **다양한 입력 방식 지원**: 텍스트, 음성(STT), 이미지 기반 질문 가능
- **커스터마이징된 음성 출력**: 유명인 또는 개인의 목소리를 등록해 답변을 해당 목소리로 청취 가능
- **WebGL 플랫폼 최적화**: 웹 브라우저 환경에서 원활하게 동작

---

## 🛠️ 코드 구성 (Code Structure)
프로젝트는 다음과 같은 구조로 구성되어 있습니다:
- **Assets/**
  - **Scripts/**  
    - **ChatManager.cs**: 채팅 입력 및 출력 관리
    - **STTService.cs**: 음성 인식 및 STT 기능 구현
    - **VoiceOutput.cs**: 커스터마이징된 음성 출력 기능 관리
  - **Plugins/**: AI API 통합 플러그인 및 외부 라이브러리

구조
AIChatManager: 여러 기능을 조율하고 호출하는 중앙 컨트롤러.
Manager: 특정 기능의 로직과 상태 관리 담당 (예: AudioPlayManager, MicManager).
Service: 외부 통신과 비즈니스 로직 처리 담당 (예: GptCommunicationService). 
서비스는 SDK나 API와의 직접 통신을 담당하고, 사용자에게는 노출되지 않습니다.

Service: 외부 API와의 통신과 비즈니스 로직 담당.
Manager: UI와 로직을 연결하고, 필요한 데이터를 AIChatManager로 전달.
> **주의**: 실제 코드에는 회사의 기밀 정보나 민감한 로직이 포함되어 있어, 공개적으로는 세부 코드를 제공하지 않습니다. 필요 시 담당자에게는 세부 내용을 검토할 수 있도록 별도로 제공합니다.

---

## 🧑‍💻 사용된 기술 및 도구 (Technologies Used)
- **Unity 버전**: 2022.3.37f1
- **WebGL**: 브라우저 기반 환경 지원
- **AI API**: 음성 합성 및 텍스트 분석
- **STT 라이브러리**: Speech-to-Text 기능 구현

---

## 📢 라이선스 및 사용 제한 (License)
이 프로젝트는 비공개 프로젝트로, 회사 정책에 따라 공유 및 배포에 제한이 있습니다.

---

## 🔐 보안 및 기밀 유지 (Security and Privacy)
- 이 프로젝트의 핵심 로직 및 민감한 데이터는 공개적으로 공유되지 않습니다.
- 필요 시 포트폴리오 담당자에게 비공개 환경에서 프로젝트를 검토할 수 있도록 요청하세요.
