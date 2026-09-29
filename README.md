# Conditional Log

에디터 전용 Unity 로그예요. 레벨·태그 필터는 에디터 콘솔 가시성만 바꾸고, `[Conditional("UNITY_EDITOR")]`는 플레이어 빌드에서 `GameLog.*` 호출과 인자 평가를 제거해요.

![개요](docs/images/overview.ko.png)

정본: [`docs/diagrams/overview.ko.mmd`](docs/diagrams/overview.ko.mmd)

## 설치

`unity-conditional-log/Assets/ConditionalLog`를 프로젝트 `Assets/`로 통째 복사해요 (Runtime/Editor asmdef 유지).

이 저장소 Unity 프로젝트에는 이미 `Assets/ConditionalLog`와 `SampleScene`의 필터 놀이터 `Assets/Demo`가 있어요. **Demo는 설치 대상이 아니에요.**

## 불변조건

- 필터를 꺼도 컴파일 제거가 아니에요. 플레이어 호출을 지우는 것은 `[Conditional]`뿐이에요.
- 태그는 `GameLogTags` enum이에요. `None`이 아닌 멤버만 Settings/F1에 나타나고, 태그 없는 오버로드는 콘솔에 태그 칸을 넣지 않아요.
- 릴리스에 남길 메시지는 `GameLog` 밖의 `Debug.Log*`를 쓰세요.
- 핫 패스에 `$""` + `GameLog.*`를 두지 마세요.

## 한계

- `[Conditional]`은 `UNITY_EDITOR`에만 걸려 있어요. 에디터(Play 포함)에서는 **레벨·태그를 꺼도** 호출문은 남고, 호출부 인자(`$""`·계산 등)는 `Write` early return **전에** 평가돼요. 필터는 콘솔 출력만 막아요.
- 에디터 쪽 그 비용을 없애려고 두 번째 컴파일 심볼이나 `Func<string>` 지연 메시지 API를 두지는 않아요. 필터를 꺼도 핫 패스 `GameLog.*`에 무거운 인자를 두지 않는 것이 이 패키지의 **현재 한계**예요.

## 이 패키지가 아닌 것

- 플레이어 빌드용 로그 파이프라인 (릴리스 메시지는 `Debug.Log*` 직접)
- 패키지 기본 `GameLogTags` 멤버를 출시 템플릿으로 쓰는 것 (프로젝트에 맞게 enum을 고치세요)
- `GameLog.*` **메시지 본문**을 어떻게 저장·다국어화할지 (호출부 선택; 보통 주언어 하나로 충분해요)

## 사용

API는 `ConditionalLog.GameLog`예요. 태그는 `GameLogTags` enum만 받아요.

```csharp
using ConditionalLog;

GameLog.Info(GameLogTags.Hud, "Press Start");
GameLog.Info($"boot complete"); // 태그 없음 — 콘솔에 [tag] 칸 없음
```

`[Conditional("UNITY_EDITOR")]`가 `GameLog.*`에 걸려 있어서 플레이어 빌드에서는 호출과 인자 평가가 제거돼요.

메뉴: **Conditional Log → Settings**. Play에서는 **F1** 오버레이.

![Log Settings](docs/images/settings.png)

글: [Conditional 로그와 빌드 비용](https://monet5379.github.io/notes/conditional-log-build-cost/)

## 현재 설계

- **컴파일 제거:** `GameLog.Progress` / `Info` / `Warning` / `Error` / `Except`만. 필터는 호출문을 지우지 않아요.
- **레벨:** 메모리. `EditorPrefs` (`ConditionalLog.Level.*`). 기본 on. 에디터 로드·Play 진입 시 다시 읽어요.
- **태그:** `GameLogTags` 멤버(`None` 제외)가 Settings/F1에 항상 나타나요. 미설정 = on. **비활성** 태그만 CSV로 persist (`ConditionalLog.Tag.Disabled`).
- **F1 오버레이:** 에디터 Play만. IMGUI (`Event`), Input System 없음. Game 뷰 포커스. `LogPlayground` `OnGUI` 상단에 `F1 — log filters` 힌트를 띄워요.
- **패키지 UI 언어:** Settings·오버레이·레벨 라벨은 영어 기본 (`ConditionalLogStrings`). 한글은 선택 파일 `ConditionalLogStrings.Ko.cs` — 필요 없으면 그 파일과 `Get`의 Korean 분기를 지우면 돼요. `GameLog.*` **인자 메시지**는 로컬라이즈하지 않아요.
- **출력:** 통과한 호출은 모두 `Debug.Log`예요. `LogWarning` / `LogError`로 나누지 않아 설계 메모가 Unity 경고·에러 채널과 섞이지 않아요. 콘솔 줄은 `<color>[Level] [tag] message</color>` (전체 줄에 레벨 색; 태그 없으면 `[tag]` 칸 없음).
- **데모:** `Assets/Demo` 놀이터 (설치 대상 아님). `SampleScene`의 `LogPlayground` (`DemoGame.cs`)가 Play 진입 시 레벨·태그 샘플 로그를 한 번 보내요. **Space**로 같은 묶음을 다시 보내고, **F1**로 레벨·태그 필터를 켜고 끄며 콘솔 가시성을 확인하세요.

## 라이선스

[MIT](LICENSE)
