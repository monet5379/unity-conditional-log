# architecture

Conditional Log **현재 구조** Architecture 허브이다. 설치·불변조건·범위 밖은 [루트 README](../../README.md) 계약이고, 타입·필터·persist·출력 경로는 [Architecture_ConditionalLog](Architecture_ConditionalLog.md)가 정본이다. 코드와 어긋나면 **코드가 이긴다**.

글쓰기·골격: [unity-studio-kit — Documentation (Architecture)](https://github.com/monet5379/unity-studio-kit/blob/main/docs/game/Documentation.md#architecture-작성-규약).

## 문서

| Architecture | 내용 |
| --- | --- |
| [Architecture_ConditionalLog](Architecture_ConditionalLog.md) | 패키지 경계 · 타입 · 필터 · Settings · persist · 출력 |

## 코드 위치

| | 경로 |
| --- | --- |
| UPM 패키지 | `Packages/com.monet5379.conditional-log/` (`Runtime/` · `Editor/` · `Tests/Editor/` · `Samples~/GameLogTagsCatalog/`) |
| Demo (비설치) | `Assets/Demo/` · `LogPlayground` + `GameLogTags` — **이 repo**는 Demo 카탈로그만 (샘플 Import 금지, 태그는 샘플과 동기화) |
| UPM 샘플 (Import) | 소비 프로젝트용 · `GameLog Tags Catalog` → `ConditionalLog.Sample.GameLogTags` |
| Edit Mode 테스트 | `ConditionalLog.Editor.Tests` · `LogTagAndFilterTests` — `manifest.json` `testables`에 패키지 id 필요 시 [패키지 README — Tests](../../Packages/com.monet5379.conditional-log/README.md#tests) |
| 다이어그램 | `docs/diagrams/overview.ko.mmd` · `overview.mmd` → `docs/images/overview*.png` |

설치·요구 사항·사용: [루트 README](../../README.md).
