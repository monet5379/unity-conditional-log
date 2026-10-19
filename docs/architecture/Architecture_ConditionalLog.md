# Architecture: ConditionalLog

## 개요 (Overview)

*에디터에서 레벨·태그로 **콘솔 가시성**만 조절하고, 플레이어 빌드에서는 `[Conditional("UNITY_EDITOR")]`로 `GameLog.*` 호출과 인자 평가를 제거한다.*

Conditional Log는 `Packages/com.monet5379.conditional-log/` UPM 패키지의 에디터 전용 개발 로그이다. 게임 코드는 `GameLog.Progress` / `Info` / `Warning` / `Error` / `Except`만 호출하고, 필터는 호출문을 지우지 않는다.

**계약(불변조건) 정본**은 [루트 README](../../README.md)이다. 이 문서는 구조·호출 관계만 다룬다.

![개요](../images/overview.ko.png)

다이어그램 소스: [`docs/diagrams/overview.ko.mmd`](../diagrams/overview.ko.mmd)

## 책임과 경계 (Responsibilities & Boundaries)

*Runtime API·필터 상태와 Editor 설정 UI·persist를 구분하고, 소비자가 책임질 영역은 문서 밖으로 넘긴다.*

### 포함 범위 (In Scope)

- `GameLog` public API · 레벨·태그 필터 · `Debug.Log` 출력 포맷
- Editor: **Conditional Log → Settings** (유일한 필터 UI), `EditorPrefs` persist
- 패키지 UI 문자열: `ConditionalLogStrings` (한글, `level.*`·`level.*.desc`만 영어) · `ConditionalLogLocale.T` → `Get`
- 이 repo `Assets/Demo` — 필터 놀이터 (**설치·복사 대상 아님**)

### 제외 범위 (Out of Scope)

- 플레이어 빌드용 로그 파이프라인 · 릴리스 메시지 (`Debug.Log*` 직접)
- `GameLog.*` **인자 메시지** 본문의 저장·다국어화
- 두 번째 compile symbol · `Func<string>` 지연 메시지 API (에디터 인자 평가 비용 제거)
- Input System 연동 · `LogWarning` / `LogError` 채널 분리
- Play Mode IMGUI 필터 오버레이 (과거 F1 등) · Settings UI **언어 전환** 메뉴
- 상세: [루트 README — 이 패키지가 아닌 것](../../README.md#이-패키지가-아닌-것)

## 주요 타입과 관계 (Key Types & Relationships)

*asmdef 경계와 설정·출력 경로별 역할을 표로 정리한다.*

| 트리 | asmdef | 역할 |
| --- | --- | --- |
| `Packages/.../Runtime/` | `ConditionalLog.Runtime` | `GameLog`, `LogTag`, `LogTagFilter`, UI 로케일 |
| `Packages/.../Editor/` | `ConditionalLog.Editor` | Settings 창, `EditorPrefs`, 부트스트랩 |
| `Packages/.../Tests/Editor/` | `ConditionalLog.Editor.Tests` | Edit Mode — `LogTag` · 필터 · 카탈로그 수집 · 프로브 태그 `Test` (`LogTagCatalogTestProbe`) |
| `Assets/Demo/` | `ConditionalLog.Demo` | `LogPlayground` · `[LogTagCatalog] GameLogTags` — **비설치** |

| 타입 | 레이어 | 책임 |
| --- | --- | --- |
| `GameLog` | Runtime | `[Conditional("UNITY_EDITOR")]` public API · `string` 및 `format` + `params object[]` (태그 유무) · `string.Format`은 메서드 안 · 메모리 레벨 플래그 · `SetLevel` / `SetLogLevelAll` / `SetLogLevelOff` · `Write` → `Debug.Log` |
| `LogTag` | Runtime | 태그 값 (`Name`, `IsNone`) · Ordinal 동등 |
| `LogTagCatalogAttribute` | Runtime | 소비자 static class 마커 — Editor가 `TypeCache`로 수집 |
| `LogTagFilter` | Runtime | 태그 on/off (미설정 = on, **비활성 목록**만 메모리 보관) · `KnownTags` (카탈로그 union) |
| `LogEditorBootstrap` | Editor | `InitializeOnLoad` · Play/Edit 진입 시 prefs → `GameLog` / `LogTagFilter` |
| `LogEditorPrefs` | Editor | `ConditionalLog.Level.*` · `ConditionalLog.Tag.Disabled` (CSV) |
| `LogSettingsWindow` | Editor | 메뉴 Settings · 레벨 토글 아래 `level.*.desc` · 태그 검색 · 변경 시 즉시 `SaveLevels` / `SaveTags` |
| `ConditionalLogLocale` | Runtime | 패키지 UI 문자열 lookup · `T(key)` |
| `ConditionalLogStrings` | Runtime | 패키지 UI 문자열 (한글) · `level.*` 키만 영어 (Progress … Except) |

태그는 소비 프로젝트의 `[LogTagCatalog]` **static class**에 선언한 `public static readonly LogTag` 필드이다. `IsNone`(빈 이름)은 수집·UI에서 제외하며, 태그 없는 오버로드는 콘솔 줄에 `[tag]` 칸을 넣지 않는다.

```mermaid
flowchart TB
  subgraph call [호출 — UNITY_EDITOR]
    GL[GameLog.*]
    W[Write]
    L{레벨 on?}
    T{태그 on?}
    DL[Debug.Log]
    GL --> W --> L
    L -->|no| H[출력 없음]
    L -->|yes| T
    T -->|no| H
    T -->|yes| DL
  end

  subgraph player [플레이어 빌드]
    GL2[GameLog.*] -.->|Conditional 제거| X[호출·인자 없음]
  end
```

## 흐름 (Flow)

*에디터에서 prefs 로드 → 호출 필터 → Settings에서 persist. Play 진입 시 `Reload()`로 같은 prefs를 다시 적용한다.*

### 런타임 흐름 — 로그 출력 (Runtime Flow)

1. 게임 코드가 `GameLog.*(message)`, `GameLog.*(format, args)`, `GameLog.*(LogTag, message)`, `GameLog.*(LogTag, format, args)` 중 하나를 호출한다. format 오버로드는 메서드 안에서 `string.Format` 한다.
2. **플레이어 빌드:** `[Conditional("UNITY_EDITOR")]`로 호출과 인자 평가가 컴파일에서 제거.
3. **에디터( Edit / Play ):**
   - 호출과 인자(`$""` 등)는 **항상** 평가.
   - `Write`에서 레벨 off 또는 태그 off면 return (콘솔만 숨김).
4. 통과 시 본문: 태그 있으면 `[{Name}] message`, 없으면 `[{level.* 영어 라벨}] message` (`ConditionalLogStrings.LevelKey`). Info는 무색 문자열, 그 외 레벨은 `<color={LevelColor}>`로 전체 본문을 감싼 뒤 **모두** `UnityEngine.Debug.Log`로 출력 (`LogWarning` / `LogError` 미사용).

### 에디터 흐름 — 설정 persist (Editor Flow)

1. `Reload()`: `EditorPrefs` bool (`ConditionalLog.Level.<LogLevel>`) → `GameLog.SetLevel`; `TypeCache` + `[LogTagCatalog]` static class의 `LogTag` 필드 수집 → `LogTagFilter.SetKnownTags`; disabled 태그 이름 CSV (`ConditionalLog.Tag.Disabled`) → `LogTagFilter.ApplyDisabled`.
2. Edit Mode 진입 · Play Mode 진입 시 `Reload()` 재실행.
3. **Settings** (`LogSettingsWindow`): 레벨·태그 토글 변경마다 `SaveLevels` 또는 `SaveTags`. 레벨 **전부 켜기/끄기**는 `GameLog.SetLogLevelAll` / `SetLogLevelOff` 후 `SaveLevels`. 태그는 `KnownTags` 목록을 한 줄씩 토글한다.

### persist 키 (`LogEditorPrefs`)

| 키 | 내용 |
| --- | --- |
| `ConditionalLog.Level.<LogLevel>` | 레벨 bool (기본 on) |
| `ConditionalLog.Tag.Disabled` | 비활성 태그 **이름** CSV (활성 목록 저장 아님) |

## 불변조건과 주의점 (Invariants & Gotchas)

*README 계약을 구현 관점에서 풀어 쓴다. 문구 충돌 시 [루트 README](../../README.md)가 이긴다.*

- **필터 ≠ 컴파일 제거:** 레벨·태그 off는 `Write` early return만. 플레이어에서 호출을 지우는 것은 `[Conditional]`뿐.
- **에디터 인자 평가:** 필터 off여도 호출문과 인자는 남는다. 핫 패스에 `$""` + `GameLog.*`를 두지 않는다 ([README — 한계](../../README.md)).
- **태그 persist:** 활성 태그 목록이 아니라 **비활성 태그 이름만** CSV로 저장한다. 카탈로그에 필드를 추가하고 domain reload 후 Settings에 자동 노출(기본 on).
- **테스트 카탈로그 태그:** manifest `testables`에 `com.monet5379.conditional-log`가 포함되면 `ConditionalLog.Editor.Tests`의 `[LogTagCatalog] LogTagCatalogTestProbe.Probe` (`LogTag("Test")`)가 Settings **`Test`** 행으로 `KnownTags`에 들어간다 (`Reload_IncludesLogTagCatalogProbeInKnownTags`). 게임 태그가 아니면 끄거나 무시한다.
- **출력 채널:** 설계 메모가 Unity Console의 Warning/Error 스택과 섞이지 않도록 전부 `Debug.Log`이다.
- **Demo:** `Assets/Demo/LogPlayground` — Play 시작·Space로 샘플 로그. 필터는 Settings. 설치 복사본에 포함하지 않는다.

## 변경 가이드 (Change Guidelines)

*구조·계약을 바꿀 때 README와 이 문서를 함께 갱신한다.*

| 변경 | 확인할 경로 |
| --- | --- |
| 로그 레벨 추가 | `GameLog.LogLevel`, `Write`, `LevelColor`, `LogEditorPrefs.LevelKey`, Settings draw 목록 |
| 태그 카탈로그 | 소비자 `[LogTagCatalog]` class · `LogEditorBootstrap.CollectCatalogTags` · `LogTagFilter.SetKnownTags` · Settings의 `KnownTags` 루프 |
| 새 public API | `[Conditional("UNITY_EDITOR")]` 유지 · `Write` 경유 여부 |
| Edit Mode 테스트 | `Tests/Editor/LogTagAndFilterTests` · `ConditionalLog.Editor.Tests.asmdef` |
| UI 문자열 | `ConditionalLogStrings` 키 · `ConditionalLogLocale.T` |
| persist 키 | `LogEditorPrefs.Prefix` 하위만 사용 (`ConditionalLog.*`) |

구조·경계·public API·부트/필터 흐름을 바꾸면 이 파일을 갱신한다. 불변조건 문구가 바뀌면 **README를 먼저** 고친다.

## 관련 문서 (See also)

- [architecture README](README.md) — 허브
- [Conditional 로그와 빌드 비용](https://monet5379.github.io/notes/conditional-log-build-cost/) — 배경 메모
- [unity-studio-kit — Documentation (Architecture)](https://github.com/monet5379/unity-studio-kit/blob/main/docs/game/Documentation.md#architecture-작성-규약)
