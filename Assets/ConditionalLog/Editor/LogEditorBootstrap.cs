using UnityEditor;

namespace ConditionalLog.Editor
{
    [InitializeOnLoad]
    internal static class LogEditorBootstrap
    {
        static LogEditorBootstrap()
        {
            LogOverlay.PersistLevels = LogEditorPrefs.SaveLevels;
            LogOverlay.PersistTags = LogEditorPrefs.SaveTags;
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
            Log.SetLevel(Log.LogLevel.Progress, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(Log.LogLevel.Progress), true));
            Log.SetLevel(Log.LogLevel.Info, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(Log.LogLevel.Info), true));
            Log.SetLevel(Log.LogLevel.Warning, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(Log.LogLevel.Warning), true));
            Log.SetLevel(Log.LogLevel.Error, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(Log.LogLevel.Error), true));
            Log.SetLevel(Log.LogLevel.Except, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(Log.LogLevel.Except), true));
            LogTagFilter.ApplyDisabled(LogEditorPrefs.LoadDisabledTags());
        }
    }
}
