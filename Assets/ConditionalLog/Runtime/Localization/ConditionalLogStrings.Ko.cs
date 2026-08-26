using System.Collections.Generic;

namespace ConditionalLog
{
    /// <summary>
    /// Optional Korean overlays for package UI (Settings / overlay / level labels).
    /// Delete this file (and the Korean branch in <see cref="ConditionalLogStrings.Get"/>) if unused.
    /// </summary>
    public static class ConditionalLogStringsKo
    {
        static readonly Dictionary<string, string> Table = new Dictionary<string, string>
        {
            { "editor.window_title", "로그 설정" },
            { "editor.heading", "Conditional Log" },
            { "editor.help",
                "레벨·태그 필터는 에디터 표시만 바꿉니다. 플레이어 빌드에서는 [Conditional]로 Log.* 호출이 제거됩니다." },
            { "editor.levels", "레벨" },
            { "editor.tags", "태그" },
            { "editor.all_on", "전부 켜기" },
            { "editor.all_off", "전부 끄기" },
            { "editor.search", "검색" },
            { "editor.tags_empty",
                "등록된 태그가 없습니다. 태그를 넣은 Log.*를 한 번 호출하면 여기에 나타납니다." },

            { "overlay.title", "Conditional Log (F1)" },
            { "overlay.help",
                "에디터 표시만. 플레이어 빌드에서는 [Conditional]로 Log.*가 제거됩니다." },
            { "overlay.tags_empty",
                "태그가 있는 Log.* 호출이 여기에 나타납니다." },

            { "level.progress", "진행" },
            { "level.info", "정보" },
            { "level.warning", "경고" },
            { "level.error", "오류" },
            { "level.except", "예외" },
        };

        public static bool TryGet(string key, out string value) => Table.TryGetValue(key, out value);
    }
}
