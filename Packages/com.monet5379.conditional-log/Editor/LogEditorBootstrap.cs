using System.Collections.Generic;
using System.Reflection;
using ConditionalLog;
using UnityEditor;

namespace ConditionalLog.Editor
{
    [InitializeOnLoad]
    internal static class LogEditorBootstrap
    {
        static LogEditorBootstrap()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Reload();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode
                || state == PlayModeStateChange.EnteredPlayMode)
            {
                Reload();
            }
        }

        internal static void Reload()
        {
            GameLog.SetLevel(GameLog.LogLevel.Progress, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Progress), true));
            GameLog.SetLevel(GameLog.LogLevel.Info, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Info), true));
            GameLog.SetLevel(GameLog.LogLevel.Warning, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Warning), true));
            GameLog.SetLevel(GameLog.LogLevel.Error, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Error), true));
            GameLog.SetLevel(GameLog.LogLevel.Except, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Except), true));
            LogTagFilter.SetKnownTags(CollectCatalogTags());
            LogTagFilter.ApplyDisabled(LogEditorPrefs.LoadDisabledTags());
        }

        private static IEnumerable<LogTag> CollectCatalogTags()
        {
            var tags = new List<LogTag>();
            foreach (var type in TypeCache.GetTypesWithAttribute<LogTagCatalogAttribute>())
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = fields[i];
                    if (field.FieldType != typeof(LogTag))
                    {
                        continue;
                    }

                    if (field.GetValue(null) is not LogTag tag || tag.IsNone)
                    {
                        continue;
                    }

                    tags.Add(tag);
                }
            }

            return tags;
        }
    }
}
