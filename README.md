# Conditional Log

Editor-only Unity log. Level and tag filters change the console in the editor; `[Conditional("UNITY_EDITOR")]` strips `Log.*` calls and argument evaluation in player builds.

## Install

Copy `unity-conditional-log/Assets/ConditionalLog` into your project’s `Assets/` (keep Runtime/Editor asmdefs).

This repo’s Unity project already has it under `Assets/ConditionalLog` and a filter playground under `Assets/Demo`.

## Invariants

- Filter off is not compile-out. Only `[Conditional]` removes player call sites.
- Tags are caller strings. The copyable unit does not ship a domain enum — put an enum wrapper in game code if you want one.
- Release messages use `Debug.Log*` outside `Log`.
- Do not leave `$""` + `Log.*` on a hot path.

## Usage

API is `ConditionalLog.Log`. Tags and any wrapper live in **your** game code.

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

A wrapper without `[Conditional("UNITY_EDITOR")]` still evaluates the caller’s `$""` in player builds.

`Assets/Demo` is a filter playground only (string tag literals). It is not a tag/enum template.

Menu: **Conditional Log → Settings**. Play mode: **F1** overlay.

Write-up: [Conditional log and build cost](https://monet5379.github.io/notes/conditional-log-build-cost/)

## Current design

- **Compile-out:** only `Log.Progress` / `Info` / `Warning` / `Error` / `Except`. Filters do not strip call sites.
- **Levels:** in-memory; `EditorPrefs` (`ConditionalLog.Level.*`). Default on. Reloaded on editor load and Play enter.
- **Tags:** unset = on. A tag appears in Settings/F1 after a tagged `Log.*` in this domain. **Disabled** tags persist as CSV (`ConditionalLog.Tag.Disabled`). The known list is **not** persisted — domain reload clears it unless the tag was disabled.
- **F1 overlay:** editor Play only. IMGUI (`Event`), no Input System. Focus the Game view. This repo’s demo HUD shows `F1 — log filters` at the top.
- **Output:** every passing call uses `Debug.Log` — not `LogWarning` / `LogError`, so design notes stay off Unity’s warning/error channels. The console line is `<color>[Level]</color> [tag] message` (level token only).
- **Demo:** `Assets/Demo` in this Unity project. Sprites from [Brackeys’ Platformer Bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle) (CC0; analogStudios_, RottingPixels). See `Assets/brackeys_platformer_assets/LICENSE & CREDITS.txt`.

## License

[MIT](LICENSE)

---

# Conditional Log

에디터 전용 Unity 로그입니다. 레벨·태그 필터는 에디터 콘솔 가시성만 바꾸고, `[Conditional("UNITY_EDITOR")]`는 플레이어 빌드에서 `Log.*` 호출과 인자 평가를 제거합니다.

## 설치

`unity-conditional-log/Assets/ConditionalLog`를 프로젝트 `Assets/`로 통째 복사합니다 (Runtime/Editor asmdef 유지).

이 저장소 Unity 프로젝트에는 이미 `Assets/ConditionalLog`와 필터 놀이터 `Assets/Demo`가 있습니다.

## 불변조건

- 필터를 꺼도 컴파일 제거가 아닙니다. 플레이어 호출을 지우는 것은 `[Conditional]`뿐입니다.
- 태그는 호출부 문자열입니다. 복사 단위에는 도메인 enum이 없습니다. 필요하면 게임 코드에 enum 래퍼를 둡니다.
- 릴리스에 남길 메시지는 `Log` 밖의 `Debug.Log*`를 씁니다.
- 핫 패스에 `$""` + `Log.*`를 두지 않습니다.

## 사용

API는 `ConditionalLog.Log`입니다. 태그 문자열과 래퍼는 **게임 코드**에 둡니다.

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

래퍼에 `[Conditional("UNITY_EDITOR")]`가 없으면 호출부의 보간 문자열이 플레이어 빌드에서 그대로 평가됩니다.

`Assets/Demo`는 필터 놀이터일 뿐입니다 (string 태그 리터럴). 태그/enum 템플릿이 아닙니다.

메뉴: **Conditional Log → Settings**. Play에서는 **F1** 오버레이.

글: [Conditional 로그와 빌드 비용](https://monet5379.github.io/notes/conditional-log-build-cost/)

## 현재 설계

- **컴파일 제거:** `Log.Progress` / `Info` / `Warning` / `Error` / `Except`만. 필터는 호출문을 지우지 않습니다.
- **레벨:** 메모리. `EditorPrefs` (`ConditionalLog.Level.*`). 기본 on. 에디터 로드·Play 진입 시 다시 읽습니다.
- **태그:** 미설정 = on. 태그 있는 `Log.*`가 이 도메인에서 한 번 나가야 Settings/F1에 나타납니다. **비활성** 태그만 CSV로 persist (`ConditionalLog.Tag.Disabled`). Known 목록은 persist하지 않습니다. 도메인 리로드 후, 꺼 두지 않은 태그는 목록에서 사라집니다.
- **F1 오버레이:** 에디터 Play만. IMGUI (`Event`), Input System 없음. Game 뷰 포커스. 이 저장소 데모 HUD 상단에 `F1 — log filters`를 띄웁니다.
- **출력:** 통과한 호출은 모두 `Debug.Log`입니다. `LogWarning` / `LogError`로 나누지 않아 설계 메모가 Unity 경고·에러 채널과 섞이지 않습니다. 콘솔 줄은 `<color>[Level]</color> [tag] message` (레벨 토큰만 색).
- **데모:** 이 Unity 프로젝트의 `Assets/Demo`. 스프라이트는 [Brackeys’ Platformer Bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle) (CC0; analogStudios_, RottingPixels). `Assets/brackeys_platformer_assets/LICENSE & CREDITS.txt`.

## 라이선스

[MIT](LICENSE)
