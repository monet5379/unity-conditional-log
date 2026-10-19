# Conditional Log — Agent Guide

```text
profile: personal
```

공개 패키지·실험용이다. Cursor는 **이 repo + unity-studio-kit** 멀티 루트로 연다.

| Kit | 경로 |
|-----|------|
| 공통 | `unity-studio-kit/docs/common/` · `.cursor/rules/common/` |
| 글쓰기 | `unity-studio-kit/docs/common/WritingGuide.md` |
| 프로필 | `unity-studio-kit/docs/personal/` · `.cursor/rules/personal/` |

## 프로젝트 개요

- **목적:** 에디터 전용 조건부 개발 로그 (`GameLog`) — 플레이어 빌드에서 Conditional 제거
- **Git 루트:** 이 저장소 최상단
- **Unity:** `./` (에셋은 repo 루트 `Assets/`)

## 레이아웃

```text
unity-conditional-log/
├── AGENTS.md
├── README.md
├── docs/architecture/Architecture_ConditionalLog.md
├── Packages/com.monet5379.conditional-log/  ← UPM 패키지
└── Assets/Demo/                             ← 놀이터. 비설치
```

## 에이전트가 지킬 것

- Kit **common** + **personal** (README 계약 + `Architecture_*`; Plan/Optimization/ADR는 요청 시만).
- `.meta` 생성·수정·삭제 금지.
- 커밋은 사용자 **명시 요청** 시에만. Kit CommitMessages — `type(scope): 한글 제목` + **본문(why) 필수**
- Demo를 출시 스키마·필수 설치 경로로 취급하지 않음.
- 구조·경계·public API·필터/Settings 흐름 변경 시 `docs/architecture/Architecture_ConditionalLog.md` 갱신. 계약 변경 시 README 먼저.

## 이 repo만의 메모

- 네임스페이스: `ConditionalLog` · Demo `ConditionalLog.Demo`
- 의존: Unity 기본만 (Input System 없음)

### 커밋 (이 repo)

| 항목 | 값 |
|------|-----|
| 언어 | 한글 |
| scope (정본) | `package`, `demo`, `docs` |
