using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace ConditionalLog.Editor
{
    internal static class LogEditorPrefs
    {
        public const string Prefix = "ConditionalLog.";
        public const string TagDisabled = Prefix + "Tag.Disabled";

        public static string LevelKey(Log.LogLevel level)
        {
            return Prefix + "Level." + level;
        }

        public static void SaveLevels()
        {
            EditorPrefs.SetBool(LevelKey(Log.LogLevel.Progress), Log.LevelProgress);
            EditorPrefs.SetBool(LevelKey(Log.LogLevel.Info), Log.LevelInfo);
            EditorPrefs.SetBool(LevelKey(Log.LogLevel.Warning), Log.LevelWarning);
            EditorPrefs.SetBool(LevelKey(Log.LogLevel.Error), Log.LevelError);
            EditorPrefs.SetBool(LevelKey(Log.LogLevel.Except), Log.LevelExcept);
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
                string tag = parts[i].Trim();
                if (!string.IsNullOrEmpty(tag))
                {
                    yield return tag;
                }
            }
        }
    }
}
