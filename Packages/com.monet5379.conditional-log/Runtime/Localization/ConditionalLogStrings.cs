using System.Collections.Generic;

namespace ConditionalLog
{
    /// <summary>
    /// Package UI strings for Settings (Korean). Level toggle·console prefix labels are English.
    /// Does not localize <c>GameLog.*</c> message bodies.
    /// </summary>
    public static class ConditionalLogStrings
    {
        static readonly Dictionary<string, string> Table = new Dictionary<string, string>
        {
            { "editor.window_title", "로그 설정" },
            { "editor.heading", "조건부 로그" },
            { "editor.help.levels",
                "레벨 필터는 에디터 콘솔에 GameLog.* 출력이 보일지 정합니다. 꺼져 있어도 호출문은 남으며, 플레이어 빌드에서는 [Conditional]로 GameLog.* 호출이 제거됩니다." },
            { "editor.help.tags",
                "태그 필터는 에디터에서 태그별 출력만 끕니다. [LogTagCatalog]에 선언한 태그만 목록에 나타나며, 기본은 켜짐입니다. 플레이어 빌드에서는 [Conditional]로 GameLog.* 호출이 제거됩니다." },
            { "editor.levels", "레벨" },
            { "editor.tags", "태그" },
            { "editor.all_on", "전부 켜기" },
            { "editor.all_off", "전부 끄기" },
            { "editor.categories_expand_all", "전부 펼치기" },
            { "editor.categories_collapse_all", "전부 접기" },
            { "editor.search", "검색" },

            { "level.progress", "Progress" },
            { "level.progress.desc", "GameLog.Progress — 단계·진행 (로딩, 초기화, 상태 전환 등)" },
            { "level.info", "Info" },
            { "level.info.desc", "GameLog.Info — 일반 개발 정보" },
            { "level.warning", "Warning" },
            { "level.warning.desc", "GameLog.Warning — 주의·이상 가능 (콘솔은 Debug.Log, Warning 색)" },
            { "level.error", "Error" },
            { "level.error.desc", "GameLog.Error — 실패·오류 (콘솔은 Debug.Log, Error 색)" },
            { "level.except", "Except" },
            { "level.except.desc", "GameLog.Except — catch·예외 처리 경로 (Except 색)" },
        };

        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            if (Table.TryGetValue(key, out var value))
                return value;

            return key;
        }

        public static string LevelKey(GameLog.LogLevel level)
        {
            return level switch
            {
                GameLog.LogLevel.Progress => "level.progress",
                GameLog.LogLevel.Info => "level.info",
                GameLog.LogLevel.Warning => "level.warning",
                GameLog.LogLevel.Error => "level.error",
                GameLog.LogLevel.Except => "level.except",
                _ => "level.info",
            };
        }

        public static string LevelDescriptionKey(GameLog.LogLevel level)
        {
            return level switch
            {
                GameLog.LogLevel.Progress => "level.progress.desc",
                GameLog.LogLevel.Info => "level.info.desc",
                GameLog.LogLevel.Warning => "level.warning.desc",
                GameLog.LogLevel.Error => "level.error.desc",
                GameLog.LogLevel.Except => "level.except.desc",
                _ => "level.info.desc",
            };
        }
    }
}
