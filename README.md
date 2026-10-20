# Conditional Log

에디터 전용 Unity **개발 로그** 패키지이다. `GameLog`로 레벨·태그 필터링된 콘솔 출력을 쓰고, 플레이어 빌드에서는 `[Conditional("UNITY_EDITOR")]`로 호출과 인자 평가를 제거한다.

**구조·필터·Settings·persist** 상세는 [Architecture](docs/architecture/README.md)가 정본이다. 아래 **불변조건**만 README 계약이다.

## 요구 사항

- Unity **6000.3** 이상 (`com.monet5379.conditional-log` `package.json`)
- 추가 패키지 의존성 없음

![개요](docs/images/overview.ko.png)

다이어그램 소스: [`overview.ko.mmd`](docs/diagrams/overview.ko.mmd) · [`overview.mmd`](docs/diagrams/overview.mmd). 렌더 PNG: [`overview.ko.png`](docs/images/overview.ko.png) · [`overview.png`](docs/images/overview.png)

## 설치

### Git URL (권장)

Package Manager → **Install package from git URL**:

```text
https://github.com/monet5379/unity-conditional-log.git?path=/Packages/com.monet5379.conditional-log#v0.2.2
```

### 로컬 임베디드

`Packages/com.monet5379.conditional-log` 를 소비 프로젝트 `Packages/` 아래에 둔다.

패키지 README·CHANGELOG: [`Packages/com.monet5379.conditional-log/README.md`](Packages/com.monet5379.conditional-log/README.md).

> [!NOTE]
> **이 repo에서 확인 (선택)**
>
> - **개발용 repo:** `Assets/Demo`의 `[LogTagCatalog] GameLogTags`만 쓴다. Package Manager에서 **GameLog Tags Catalog** 샘플 **Import는 하지 않는다** (Demo와 Import한 샘플이 같이 있으면 `[LogTagCatalog]`가 이중 수집된다). 배포용 예시 정본은 `Samples~/GameLogTagsCatalog`이며, Demo 카탈로그는 샘플과 맞춰 둔다. 소비 프로젝트는 [패키지 README — Samples](Packages/com.monet5379.conditional-log/README.md#samples)를 따른다.
> - Unity로 repo 루트를 연다 → `Assets/Scenes/SampleScene` Play → `Assets/Demo/LogPlayground`가 샘플 로그를 보낸다. **Space**로 반복. 필터는 **Tools → Conditional Log → Settings**.
> - `Assets/Demo`는 **패키지·설치 대상이 아니다.** 카탈로그·호출 코드가 asmdef 안이면 **`ConditionalLog.Runtime`** (`Packages/.../Runtime/ConditionalLog.Runtime.asmdef`)을 참조해야 한다 — Inspector 참조가 비어 있으면 Console에 `LogTag` CS0246이 난다.
> - Settings의 **`Test`** 태그는 Edit Mode 테스트용 카탈로그 프로브이며 Demo 카탈로그 태그와 별개이다.

## 불변조건

- 필터를 꺼도 컴파일 제거가 아니다. 플레이어 빌드에서 호출을 지우는 것은 `[Conditional]`뿐이다.
- 태그는 `LogTag`이며, 소비 프로젝트의 `[LogTagCatalog]` **static class**에 `public static readonly LogTag` 필드로 선언한다. Settings foldout은 `new LogTag(name, category)`의 category(선택)로만 묶는다. 카탈로그에 수집된 태그만 Settings에 나타나고, 태그 없는 오버로드는 콘솔에 태그 칸을 넣지 않는다.
- 릴리스에 남길 메시지는 `GameLog` 밖의 `Debug.Log*`를 쓴다.
- 핫 패스에 `$""` + `GameLog.*`를 두지 않는다.

persist 키·출력 포맷·UI 문자열: [Architecture_ConditionalLog](docs/architecture/Architecture_ConditionalLog.md).

## 한계

- `[Conditional]`은 `UNITY_EDITOR`에만 걸려 있다. 에디터(Play 포함)에서는 **레벨·태그를 꺼도** 호출문은 남고, 호출부 인자(`$""`·계산 등)는 `Write` early return **전에** 평가된다. 필터는 콘솔 출력만 막는다.
- 에디터 인자 비용을 없애기 위한 두 번째 컴파일 심볼·`Func<string>` 지연 API는 제공하지 않는다. 필터 off여도 핫 패스 `GameLog.*`에 무거운 인자를 두지 않는 것이 **현재 한계**이다.

## 이 패키지가 아닌 것

- 플레이어 빌드용 로그 파이프라인 (릴리스 메시지는 `Debug.Log*` 직접)
- 패키지가 대신 정의해 주는 태그 목록 (프로젝트 카탈로그 클래스에 맞게 선언한다)
- Play Mode IMGUI 필터 오버레이 (필터는 **Tools → Conditional Log → Settings**만)
- Settings·패키지 UI **언어 전환** (`level.*` 콘솔·토글 라벨은 영어, 그 외 Settings UI 문자열은 한글 고정)
- `GameLog.*` **메시지 본문** 저장·다국어화 (호출부 선택; 보통 주언어 하나로 충분하다)

## 사용

```csharp
using ConditionalLog;

[LogTagCatalog]
public static class GameLogTags
{
    public static readonly LogTag Hud = new LogTag("Hud", "UI");
}

GameLog.Info(GameLogTags.Hud, "Press Start");
GameLog.Info(GameLogTags.Hud, "hp {0}", hp);
GameLog.Info("boot complete"); // 태그 없음 — 콘솔에 [tag] 칸 없음
```

카탈로그·`GameLog` 호출 코드가 **자체 asmdef** 안이면 그 asmdef가 **`ConditionalLog.Runtime`을 참조**해야 한다. asmdef 없이 기본 `Assembly-CSharp`만 쓰면 `autoReferenced`로 잡힌다. 자세한 절차는 [패키지 README — 태그 카탈로그](Packages/com.monet5379.conditional-log/README.md#태그-카탈로그).

메뉴 **Tools → Conditional Log → Settings** — **레벨** / **태그** 탭. 태그 탭은 category foldout·검색·전부 켜기/끄기를 제공한다.

![Log Settings](docs/images/settings.png)

## 문서

| 문서 | 내용 |
| --- | --- |
| [Architecture](docs/architecture/README.md) | 허브 · [Architecture_ConditionalLog](docs/architecture/Architecture_ConditionalLog.md) |
| [overview.ko.mmd](docs/diagrams/overview.ko.mmd) · [overview.mmd](docs/diagrams/overview.mmd) | 개요 (Mermaid · PNG `docs/images/overview*.png`) |
| [Conditional 로그와 빌드 비용](https://monet5379.github.io/notes/conditional-log-build-cost/) | 배경 메모 |

## 라이선스

[MIT](LICENSE)

---

README 본문은 AI-assisted로 작성·갱신했다. README와 코드·Architecture가 충돌하면 **코드**가 이긴다.
