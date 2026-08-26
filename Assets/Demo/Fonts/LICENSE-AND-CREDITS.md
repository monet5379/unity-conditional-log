# Fonts — License & Credits

TTF sources under `Fonts/` for personal Unity packages that use Locale (EN + KO UI).  
Copy into the **consumer** project as `Assets/Demo/Fonts/` (demo/playground only — not the install unit `Assets/<Package>/`).

| File | Script coverage | Typical use |
|------|-----------------|-------------|
| `NotoSans-Medium.ttf` | Latin (English) | EN UI labels |
| `NotoSansKR-Medium.ttf` | Hangul (+ Latin in family) | KO UI labels; may cover both if you use one font |

This folder is **not** a TMP Static atlas set. For multi-language Static SDF / warmup pipelines, use sibling `unity-tmp-font` demo fonts instead.

OFL full text is not vendored here (`OFL.txt` omitted). Use the link below, or add `OFL.txt` from the Google Fonts / notofonts package when you redistribute fonts publicly.

---

## Noto Sans (Google Fonts)

**Applies to:** `NotoSans-Medium.ttf`, `NotoSansKR-Medium.ttf` in this folder.

| Item | Detail |
|------|--------|
| License | [SIL Open Font License 1.1](https://scripts.sil.org/OFL) |
| Copyright | Copyright (c) Google LLC and contributors |
| Source | [Google Fonts — Noto Sans](https://fonts.google.com/noto/specimen/Noto+Sans) · [Noto Sans KR](https://fonts.google.com/noto/specimen/Noto+Sans+KR) |

You may use, study, modify, and redistribute under the OFL. Do not sell the font files by themselves. Keep license notices when redistributing modified versions.

Full license text: [OFL.txt on GitHub (notofonts)](https://github.com/notofonts/noto-fonts/blob/main/LICENSE), or `OFL.txt` in the Google Fonts download.

---

## Templates vs fonts

| Component | License |
|-----------|---------|
| `Locale/` (C# templates) | Same as this private templates repo (or consumer package license after copy) |
| `Fonts/` (Noto Sans TTF) | SIL OFL 1.1 (above) |

When copying fonts into a public package repo, keep this file (or an equivalent) next to the TTFs under `Assets/Demo/Fonts/`.

---

# Fonts — 라이선스 및 출처

Locale(EN·KO UI)을 쓰는 개인 Unity 패키지용 TTF입니다.  
소비 프로젝트에는 **`Assets/Demo/Fonts/`** 로 복사하세요 (놀이터·데모만 — 설치 단위 `Assets/<Package>/` 아님).

| 파일 | 문자 | 용도 |
|------|------|------|
| `NotoSans-Medium.ttf` | 라틴(영문) | EN UI |
| `NotoSansKR-Medium.ttf` | 한글(+ 패밀리 내 라틴) | KO UI; 한 폰트로 둘 다 쓸 수도 있음 |

TMP Static SDF·웜업 파이프라인용이 **아닙니다**. 다국어 Static atlas는 sibling `unity-tmp-font` 데모 폰트를 보세요.

이 폴더에는 `OFL.txt`를 넣지 않았습니다. 아래 링크를 쓰거나, 공개 재배포 시 Google Fonts / notofonts 패키지의 `OFL.txt`를 함께 두세요.

---

## Noto Sans (Google Fonts)

**대상:** 이 폴더의 `NotoSans-Medium.ttf`, `NotoSansKR-Medium.ttf`.

| 항목 | 내용 |
|------|------|
| 라이선스 | [SIL Open Font License 1.1](https://scripts.sil.org/OFL) |
| 저작권 | Copyright (c) Google LLC and contributors |
| 출처 | [Google Fonts — Noto Sans](https://fonts.google.com/noto/specimen/Noto+Sans) · [Noto Sans KR](https://fonts.google.com/noto/specimen/Noto+Sans+KR) |

OFL 조건 하에 사용·연구·수정·재배포 가능. **폰트 파일 단독 유료 판매 금지.** 수정본 재배포 시 라이선스 문구 유지.

전문: [notofonts GitHub OFL.txt](https://github.com/notofonts/noto-fonts/blob/main/LICENSE), 또는 Google Fonts 패키지 내 `OFL.txt`.

---

## 템플릿 vs 폰트

| 구분 | 라이선스 |
|------|----------|
| `Locale/` (C# 템플릿) | 이 private 템플릿 repo / 복사 후 소비 패키지 라이선스 |
| `Fonts/` (Noto Sans TTF) | SIL OFL 1.1 (위 참고) |

공개 패키지 repo로 폰트를 복사할 때는 이 파일(또는 동등 고지)을 TTF와 함께 `Assets/Demo/Fonts/`에 두세요.
