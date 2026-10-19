using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace ConditionalLog.Editor
{
    internal static class LogEditorPrefs
    {
        public const string Prefix = "ConditionalLog.";
        public const string TagDisabled = Prefix + "Tag.Disabled";

        public static string LevelKey(GameLog.LogLevel level)
        {
            return Prefix + "Level." + level;
        }

        public static void SaveLevels()
        {
            EditorPrefs.SetBool(LevelKey(GameLog.LogLevel.Progress), GameLog.LevelProgress);
            EditorPrefs.SetBool(LevelKey(GameLog.LogLevel.Info), GameLog.LevelInfo);
            EditorPrefs.SetBool(LevelKey(GameLog.LogLevel.Warning), GameLog.LevelWarning);
            EditorPrefs.SetBool(LevelKey(GameLog.LogLevel.Error), GameLog.LevelError);
            EditorPrefs.SetBool(LevelKey(GameLog.LogLevel.Except), GameLog.LevelExcept);
        }

        public static void SaveTags()
        {
            List<string> disabled = LogTagFilter.GetDisabledTags();
            var builder = new StringBuilder();
            for (int i = 0; i < disabled.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                builder.Append(disabled[i]);
            }

            EditorPrefs.SetString(TagDisabled, builder.ToString());
        }

        public static IEnumerable<string> LoadDisabledTags()
        {
            string raw = EditorPrefs.GetString(TagDisabled, string.Empty);
            if (string.IsNullOrEmpty(raw))
            {
                yield break;
            }

            string[] parts = raw.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                string name = parts[i].Trim();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                yield return name;
            }
        }
    }
}
