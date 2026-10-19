# Changelog

이 파일은 [Semantic Versioning](https://semver.org/) 형식을 따른다.

## [Unreleased]

## [0.2.1] - 2026-10-20

### Added

- `LogTagFilter.SetAllEnabled` / `SetAllDisabled` · Settings 태그 섹션 **전부 켜기/끄기** 버튼.

### Changed

- Settings 메뉴: 최상위 **Conditional Log** → **Tools → Conditional Log → Settings**.

### Notes

- Git URL pin: `https://github.com/monet5379/unity-conditional-log.git?path=/Packages/com.monet5379.conditional-log#v0.2.1`

## [0.2.0] - 2026-10-20

### Added

- `GameLog` format 오버로드 (`string format, params object[]`). Progress / Info / Warning / Error / Except, 태그 유무. `string.Format`은 `[Conditional("UNITY_EDITOR")]` 메서드 안에서만 실행한다.

### Notes

- Git URL pin: `https://github.com/monet5379/unity-conditional-log.git?path=/Packages/com.monet5379.conditional-log#v0.2.0`

## [0.1.0] - 2026-10-19

### Added

- **Runtime:** `LogTag` · `LogTagCatalogAttribute` · `LogTagFilter.KnownTags`
- **Editor:** `[LogTagCatalog]` static class에서 태그 수집 (`TypeCache`) → Settings 목록
- **Tests:** `ConditionalLog.Editor.Tests` / `LogTagAndFilterTests` (Edit Mode)

### Changed

- **Breaking:** 패키지 기본 `GameLogTags` enum 제거. 소비 프로젝트에 `[LogTagCatalog]` static class로 태그를 선언한다 (필드 이름 `GameLogTags` 등은 호출부 마이그레이션용으로 자유).
- **Breaking:** UI 언어 선택(English/한국어) 제거. `ConditionalLogLang` · `ConditionalLogLocale.SetLanguage` · `LanguageChanged` · `Conditional Log → Language` 메뉴 삭제.
- **Breaking:** Play **F1** IMGUI 오버레이(`LogOverlay` / `LogOverlayHost`) 제거. 필터는 **Conditional Log → Settings**만 사용한다.
- Settings·콘솔 레벨 라벨(`level.*`)은 영어(Progress, Info, Warning, Error, Except). 그 외 패키지 UI 문자열은 한글 `ConditionalLogStrings`를 사용한다.
- Edit Mode 카탈로그 프로브 태그 이름을 **`Test`** 로 단순화 (`ConditionalLog.Editor.Tests.Probe` 제거).

### Notes

- Git URL pin: `https://github.com/monet5379/unity-conditional-log.git?path=/Packages/com.monet5379.conditional-log#v0.1.0`
