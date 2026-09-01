# Conditional Log

에디터 전용 Unity 로그예요. 레벨·태그 필터는 에디터 콘솔 가시성만 바꾸고, `[Conditional("UNITY_EDITOR")]`는 플레이어 빌드에서 `Log.*` 호출과 인자 평가를 제거해요.

![개요](docs/images/overview.ko.png)

정본: [`docs/diagrams/overview.ko.mmd`](docs/diagrams/overview.ko.mmd)

## 설치

`unity-conditional-log/Assets/ConditionalLog`를 프로젝트 `Assets/`로 통째 복사해요 (Runtime/Editor asmdef 유지).

이 저장소 Unity 프로젝트에는 이미 `Assets/ConditionalLog`와 필터 놀이터 `Assets/Demo`가 있어요. **Demo는 설치 대상이 아니에요.**

## 불변조건

- 필터를 꺼도 컴파일 제거가 아니에요. 플레이어 호출을 지우는 것은 `[Conditional]`뿐이에요.
- 태그는 호출부 문자열이에요. 복사 단위에는 도메인 enum이 없어요. 필요하면 게임 코드에 enum 래퍼를 두세요.
- 릴리스에 남길 메시지는 `Log` 밖의 `Debug.Log*`를 쓰세요.
- 핫 패스에 `$""` + `Log.*`를 두지 마세요.

## 한계

- `[Conditional]`은 `UNITY_EDITOR`에만 걸려 있어요. 에디터(Play 포함)에서는 **레벨·태그를 꺼도** 호출문은 남고, 호출부 인자(`$""`·계산 등)는 `Write` early return **전에** 평가돼요. 필터는 콘솔 출력만 막아요.
- 에디터 쪽 그 비용을 없애려고 두 번째 컴파일 심볼이나 `Func<string>` 지연 메시지 API를 두지는 않아요. 필터를 꺼도 핫 패스 `Log.*`에 무거운 인자를 두지 않는 것이 이 패키지의 **현재 한계**예요.

## 이 패키지가 아닌 것

- 플레이어 빌드용 로그 파이프라인 (릴리스 메시지는 `Debug.Log*` 직접)
- 도메인 태그 enum·게임별 `GameLog` 래퍼 (소비 쪽 코드에 둠)
- Demo의 태그/enum을 출시 템플릿으로 쓰는 것
- `Log.*` **메시지 본문**을 어떻게 저장·다국어화할지 (호출부 선택; 보통 주언어 하나로 충분해요)

## 사용

API는 `ConditionalLog.Log`예요. 태그 문자열과 래퍼는 **게임 코드**에 두세요.

```csharp
using System.Diagnostics;
using ConditionalLog;

public static class GameLog
{
    public const string Combat = "Combat";

    [Conditional("UNITY_EDITOR")]
    public static void Info(string tag, string message)
    {
        Log.Info(tag, message);
    }
}

GameLog.Info(GameLog.Combat, $"dmg={damage}");
```

래퍼에 `[Conditional("UNITY_EDITOR")]`가 없으면 호출부의 보간 문자열이 플레이어 빌드에서 그대로 평가돼요.

메뉴: **Conditional Log → Settings**. Play에서는 **F1** 오버레이.

![Log Settings](docs/images/settings.png)

![F1 오버레이 (Play)](docs/images/f1-overlay.png)

글: [Conditional 로그와 빌드 비용](https://monet5379.github.io/notes/conditional-log-build-cost/)

## 현재 설계

- **컴파일 제거:** `Log.Progress` / `Info` / `Warning` / `Error` / `Except`만. 필터는 호출문을 지우지 않아요.
- **레벨:** 메모리. `EditorPrefs` (`ConditionalLog.Level.*`). 기본 on. 에디터 로드·Play 진입 시 다시 읽어요.
- **태그:** 미설정 = on. 태그 있는 `Log.*`가 이 도메인에서 한 번 나가야 Settings/F1에 나타나요. **비활성** 태그만 CSV로 persist (`ConditionalLog.Tag.Disabled`). Known 목록은 persist하지 않아요. 도메인 리로드 후, 꺼 두지 않은 태그는 목록에서 사라져요.
- **F1 오버레이:** 에디터 Play만. IMGUI (`Event`), Input System 없음. Game 뷰 포커스. 이 저장소 데모 HUD 상단에 `F1 — log filters`를 띄워요.
- **패키지 UI 언어:** Settings·오버레이·레벨 라벨은 영어 기본 (`ConditionalLogStrings`). 한글은 선택 파일 `ConditionalLogStrings.Ko.cs` — 필요 없으면 그 파일과 `Get`의 Korean 분기를 지우면 돼요. `Log.*` **인자 메시지**는 로컬라이즈하지 않아요.
- **출력:** 통과한 호출은 모두 `Debug.Log`예요. `LogWarning` / `LogError`로 나누지 않아 설계 메모가 Unity 경고·에러 채널과 섞이지 않아요. 콘솔 줄은 `<color>[Level]</color> [tag] message` (레벨 토큰만 색).
- **데모:** `Assets/Demo` 놀이터 (설치 대상 아님). `DemoStrings` / `DemoStrings.Ko`는 이 놀이터에서 UI·로그 문구를 붙인 **예시**일 뿐, 로그 i18n 템플릿이 아니에요. 스프라이트는 [Brackeys’ Platformer Bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle) (CC0; analogStudios_, RottingPixels). `Assets/brackeys_platformer_assets/LICENSE & CREDITS.txt`.

## 라이선스

[MIT](LICENSE)
