using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ConditionalLog.Editor
{
    internal sealed class LogSettingsWindow : EditorWindow
    {
        private const string MenuPath = "Conditional Log/Settings";

        private Vector2 _scroll;
        private string _tagSearch = string.Empty;
        private bool _levelsFoldout = true;
        private bool _tagsFoldout = true;

        [MenuItem(MenuPath)]
        private static void Open()
        {
            var window = GetWindow<LogSettingsWindow>();
            window.ApplyTitle();
            window.Show();
        }

        private void OnEnable()
        {
            LogEditorBootstrap.Reload();
            ConditionalLogLocale.LanguageChanged += OnLanguageChanged;
            ApplyTitle();
        }

        private void OnDisable()
        {
            ConditionalLogLocale.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            ApplyTitle();
            Repaint();
        }

        private void ApplyTitle()
        {
            titleContent = new GUIContent(ConditionalLogLocale.T("editor.window_title"));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(ConditionalLogLocale.T("editor.heading"), EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(ConditionalLogLocale.T("editor.help"), MessageType.Info);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            DrawLevelsSection();
            EditorGUILayout.Space(8);
            DrawTagsSection();

            EditorGUILayout.EndScrollView();
        }

        private void DrawLevelsSection()
        {
            _levelsFoldout = EditorGUILayout.Foldout(
                _levelsFoldout,
                ConditionalLogLocale.T("editor.levels"),
                true);
            if (!_levelsFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(ConditionalLogLocale.T("editor.all_on")))
            {
                Log.SetLogLevelAll();
                LogEditorPrefs.SaveLevels();
            }

            if (GUILayout.Button(ConditionalLogLocale.T("editor.all_off")))
            {
                Log.SetLogLevelOff();
                LogEditorPrefs.SaveLevels();
            }

            EditorGUILayout.EndHorizontal();

            DrawLevelToggle(Log.LogLevel.Progress, ConditionalLogStrings.LevelKey(Log.LogLevel.Progress));
            DrawLevelToggle(Log.LogLevel.Info, ConditionalLogStrings.LevelKey(Log.LogLevel.Info));
            DrawLevelToggle(Log.LogLevel.Warning, ConditionalLogStrings.LevelKey(Log.LogLevel.Warning));
            DrawLevelToggle(Log.LogLevel.Error, ConditionalLogStrings.LevelKey(Log.LogLevel.Error));
            DrawLevelToggle(Log.LogLevel.Except, ConditionalLogStrings.LevelKey(Log.LogLevel.Except));

            EditorGUI.indentLevel--;
        }

        private void DrawLevelToggle(Log.LogLevel level, string labelKey)
        {
            bool enabled = Log.IsLevelEnabled(level);
            bool next = EditorGUILayout.Toggle(ConditionalLogLocale.T(labelKey), enabled);
            if (next == enabled)
            {
                return;
            }

            Log.SetLevel(level, next);
            LogEditorPrefs.SaveLevels();
        }

        private void DrawTagsSection()
        {
            _tagsFoldout = EditorGUILayout.Foldout(
                _tagsFoldout,
                ConditionalLogLocale.T("editor.tags"),
                true);
            if (!_tagsFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            _tagSearch = EditorGUILayout.TextField(
                ConditionalLogLocale.T("editor.search"),
                _tagSearch);

            List<string> known = LogTagFilter.GetKnownTags();
            if (known.Count == 0)
            {
                EditorGUILayout.HelpBox(ConditionalLogLocale.T("editor.tags_empty"), MessageType.None);
                EditorGUI.indentLevel--;
                return;
            }

            string search = _tagSearch?.Trim() ?? string.Empty;
            for (int i = 0; i < known.Count; i++)
            {
                string tag = known[i];
                if (search.Length > 0 && tag.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                bool enabled = LogTagFilter.IsEnabled(tag);
                bool next = EditorGUILayout.Toggle(tag, enabled);
                if (next == enabled)
                {
                    continue;
                }

                LogTagFilter.SetEnabled(tag, next);
                LogEditorPrefs.SaveTags();
            }

            EditorGUI.indentLevel--;
        }
    }
}
