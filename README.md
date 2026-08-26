# Conditional Log

**English** | [한국어](README.ko.md)

Editor-only Unity logging. Level and tag filters change console visibility in the editor only. `[Conditional("UNITY_EDITOR")]` strips `Log.*` calls and their argument evaluation from player builds.

## Install

Copy `unity-conditional-log/Assets/ConditionalLog` into your project `Assets/` (keep the Runtime/Editor asmdefs).

This repo’s Unity project already includes `Assets/ConditionalLog` and a filter playground at `Assets/Demo`. **Demo is not part of the install.**

## Invariants

- Turning a filter off is not compile-time removal. Only `[Conditional]` removes player-build calls.
- Tags are call-site strings. The copy unit has no domain tag enum — add an enum wrapper in game code if you need one.
- Messages that must ship in release use `Debug.Log*` outside `Log`.
- Do not put `$""` + `Log.*` on a hot path.

## Out of scope

- A player-build logging pipeline (use `Debug.Log*` directly for release messages)
- Domain tag enums or a game-specific `GameLog` wrapper (those live in consumer code)
- Treating Demo tags/enums as a shipping template

## Usage

The API is `ConditionalLog.Log`. Keep tag strings and wrappers in **game code**.

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

Without `[Conditional("UNITY_EDITOR")]` on the wrapper, interpolated strings at the call site are still evaluated in player builds.

Menu: **Conditional Log → Settings**. In Play mode, **F1** opens the overlay.

![Log Settings](docs/images/settings.png)

![F1 overlay (Play)](docs/images/f1-overlay.png)

Note (Korean): [Conditional logs and build cost](https://monet5379.github.io/notes/conditional-log-build-cost/)

## Design notes

- **Compile stripping:** `Log.Progress` / `Info` / `Warning` / `Error` / `Except` only. Filters do not remove call sites.
- **Levels:** In memory. `EditorPrefs` (`ConditionalLog.Level.*`). Default on. Re-read on editor load and when entering Play.
- **Tags:** Unconfigured = on. A tagged `Log.*` must fire once in this domain before it appears in Settings/F1. Only **disabled** tags persist as CSV (`ConditionalLog.Tag.Disabled`). The known list is not persisted. After a domain reload, tags you did not disable drop off the list.
- **F1 overlay:** Editor Play only. IMGUI (`Event`), no Input System. Needs Game view focus. This repo’s demo HUD shows `F1 — log filters` at the top.
- **Output:** Passed calls all use `Debug.Log`. They are not split into `LogWarning` / `LogError`, so design notes stay out of Unity’s warning/error channels. Console lines look like `<color>[Level]</color> [tag] message` (color on the level token only).
- **Demo:** `Assets/Demo` playground. Sprites from [Brackeys’ Platformer Bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle) (CC0; analogStudios_, RottingPixels). See `Assets/brackeys_platformer_assets/LICENSE & CREDITS.txt`.

## License

[MIT](LICENSE)

English prose may be AI-assisted. If wording conflicts, prefer the [Korean README](README.ko.md) or the code.
