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
            GameLog.SetLevel(GameLog.LogLevel.Progress, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Progress), true));
            GameLog.SetLevel(GameLog.LogLevel.Info, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Info), true));
            GameLog.SetLevel(GameLog.LogLevel.Warning, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Warning), true));
            GameLog.SetLevel(GameLog.LogLevel.Error, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Error), true));
            GameLog.SetLevel(GameLog.LogLevel.Except, EditorPrefs.GetBool(LogEditorPrefs.LevelKey(GameLog.LogLevel.Except), true));
            LogTagFilter.ApplyDisabled(LogEditorPrefs.LoadDisabledTags());
        }
    }
}
