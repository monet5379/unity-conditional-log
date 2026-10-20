# Conditional Log

에디터 전용 **개발 로그** UPM 패키지이다. `GameLog`로 레벨·태그 필터링된 콘솔 출력을 쓰고, 플레이어 빌드에서는 `[Conditional("UNITY_EDITOR")]`로 호출과 인자 평가를 제거한다.

## 요구 사항

- Unity **6000.3** 이상
- 추가 패키지 의존성 없음

## 설치

### Git URL (권장)

1. **Window → Package Manager → + → Install package from git URL**
2. 아래 URL 입력 (태그는 릴리스 버전에 맞게 변경):

```text
https://github.com/monet5379/unity-conditional-log.git?path=/Packages/com.monet5379.conditional-log#v0.2.2
```

### 로컬 임베디드

이 레포의 `Packages/com.monet5379.conditional-log` 폴더를 Unity 프로젝트 `Packages/` 아래에 두면 Package Manager **In Project**에 표시된다.

## Samples

Package Manager에서 **Conditional Log** 패키지 → **Samples** → **GameLog Tags Catalog** → **Import**.

`Assets/Samples/com.monet5379.conditional-log/<version>/GameLogTagsCatalog/`에 복사된다. asmdef `ConditionalLog.Sample.GameLogTags`가 `ConditionalLog.Runtime`을 참조한다. 예시 태그는 Assets · Data · Setting · UI category foldout 확인용이며, 이름·category는 프로젝트에 맞게 고친다.

개발용 [GitHub 레포](https://github.com/monet5379/unity-conditional-log)는 `Assets/Demo` 카탈로그만 쓰고 이 샘플 **Import는 하지 않는다** (이중 `[LogTagCatalog]`).

## 태그 카탈로그

태그는 `LogTag` 값이다. **소비 프로젝트**에 `[LogTagCatalog]`가 붙은 `public static` 클래스를 두고, `public static readonly LogTag` 필드로 선언한다. Settings에는 카탈로그에 수집된 태그만 나타난다 (기본 on, 끈 태그만 `EditorPrefs`에 저장). Settings foldout 그룹은 `new LogTag("이름", "카테고리")`의 **두 번째 인자**로만 정한다 (생략 시 foldout 없이 나열).

카탈로그·`GameLog` 호출 스크립트가 **자체 asmdef** 안에 있으면 그 asmdef가 **`ConditionalLog.Runtime`을 참조**해야 한다 (Inspector **Assembly Definition References**에 추가). asmdef 없이 `Assets/` 기본 어셈블리만 쓰면 `autoReferenced`로 자동 참조된다.

```csharp
using ConditionalLog;

[LogTagCatalog]
public static class GameLogTags
{
    public static readonly LogTag Hud = new LogTag("Hud", "Gameplay");
    public static readonly LogTag Boot = new LogTag("Boot", "Data");
}
```

## 사용

```csharp
GameLog.Info(GameLogTags.Hud, "Press Start");
GameLog.Info(GameLogTags.Hud, "hp {0}", hp);
GameLog.Info("boot complete"); // 태그 없음 — 콘솔에 [tag] 칸 없음
```

포맷·`<color>` 등 **변환**이 필요하면 `GameLog.Info(text.ToColorString())`처럼 변환식을 `GameLog.*(` **인자 안**에만 둔다. `var content = …` 후 `GameLog.Info(content)`나 변환 없이 `GameLog.Info(text)`는 쓰지 않는다. ([레포 README — 불변조건](https://github.com/monet5379/unity-conditional-log#%EB%B6%88%EB%B3%80%EC%A1%B0%EA%B1%B4))

메뉴 **Tools → Conditional Log → Settings** — **레벨** / **태그** 탭. 태그 탭에서 category foldout·검색·전부 펼치기/접기·전부 켜기/끄기.

## Tests

| Test Runner 탭 | asmdef | 테스트 |
| --- | --- | --- |
| **Edit Mode** | `ConditionalLog.Editor.Tests` | `LogTagAndFilterTests` |

Test Runner에 테스트가 보이지 않으면 프로젝트 `Packages/manifest.json`에 `"testables": ["com.monet5379.conditional-log"]`를 추가한다.

> [!NOTE]
> `testables`로 Edit Mode 테스트를 켠 프로젝트에서는 카탈로그 수집 검증용 **`Test`** 태그가 Settings에 함께 나타날 수 있다 (`LogTagAndFilterTests`의 `LogTagCatalogTestProbe`). 게임 태그가 아니면 끄거나 무시하면 된다.

## 레포 문서

불변조건·Architecture는 [GitHub 레포 README](https://github.com/monet5379/unity-conditional-log) 및 `docs/` 를 참고한다.

## License

[MIT](https://github.com/monet5379/unity-conditional-log/blob/main/LICENSE)
