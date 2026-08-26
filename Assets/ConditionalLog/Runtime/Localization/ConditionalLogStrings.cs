using System.Collections.Generic;

namespace ConditionalLog
{
    /// <summary>
    /// Package UI strings for Settings / overlay / level labels (English).
    /// Optional Korean: <c>ConditionalLogStrings.Ko.cs</c>. Does not localize <c>Log.*</c> message bodies.
    /// </summary>
    public static class ConditionalLogStrings
    {
        static readonly Dictionary<string, string> En = new Dictionary<string, string>
        {
            { "editor.window_title", "Log Settings" },
            { "editor.heading", "Conditional Log" },
            { "editor.help",
                "Level and tag filters change editor visibility only. Player builds strip Log.* calls via [Conditional]." },
            { "editor.levels", "Levels" },
            { "editor.tags", "Tags" },
            { "editor.all_on", "All On" },
            { "editor.all_off", "All Off" },
            { "editor.search", "Search" },
            { "editor.tags_empty",
                "No tags registered yet. Log with a tag to list it here." },

            { "overlay.title", "Conditional Log (F1)" },
            { "overlay.help",
                "Editor visibility only. Player builds strip Log.* via [Conditional]." },
            { "overlay.tags_empty",
                "Tagged Log.* calls appear here." },

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

        public static string LevelKey(Log.LogLevel level)
        {
            return level switch
            {
                Log.LogLevel.Progress => "level.progress",
                Log.LogLevel.Info => "level.info",
                Log.LogLevel.Warning => "level.warning",
                Log.LogLevel.Error => "level.error",
                Log.LogLevel.Except => "level.except",
                _ => "level.info",
            };
        }
    }
}
