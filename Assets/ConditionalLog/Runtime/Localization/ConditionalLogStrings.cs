using System.Collections.Generic;

namespace ConditionalLog
{
    /// <summary>
    /// Package UI strings for Settings / overlay / level labels (English).
    /// Optional Korean: <c>ConditionalLogStrings.Ko.cs</c>. Does not localize <c>GameLog.*</c> message bodies.
    /// </summary>
    public static class ConditionalLogStrings
    {
        static readonly Dictionary<string, string> En = new Dictionary<string, string>
        {
            { "editor.window_title", "Log Settings" },
            { "editor.heading", "Conditional Log" },
            { "editor.help",
                "Level and tag filters change editor visibility only. Player builds strip GameLog.* calls via [Conditional]." },
            { "editor.levels", "Levels" },
            { "editor.tags", "Tags" },
            { "editor.all_on", "All On" },
            { "editor.all_off", "All Off" },
            { "editor.search", "Search" },

            { "overlay.title", "Conditional Log (F1)" },
            { "overlay.help",
                "Editor visibility only. Player builds strip GameLog.* via [Conditional]." },

            { "level.progress", "Progress" },
            { "level.info", "Info" },
            { "level.warning", "Warning" },
            { "level.error", "Error" },
            { "level.except", "Except" },
        };

        public static string Get(string key, ConditionalLogLang lang)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            // Remove this branch if you delete ConditionalLogStrings.Ko.cs
            if (lang == ConditionalLogLang.Korean
                && ConditionalLogStringsKo.TryGet(key, out var ko))
                return ko;

            if (En.TryGetValue(key, out var en))
                return en;

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
    }
}
