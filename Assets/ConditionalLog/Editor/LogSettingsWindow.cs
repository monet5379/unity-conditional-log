using System;
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
                GameLog.SetLogLevelAll();
                LogEditorPrefs.SaveLevels();
            }

            if (GUILayout.Button(ConditionalLogLocale.T("editor.all_off")))
            {
                GameLog.SetLogLevelOff();
                LogEditorPrefs.SaveLevels();
            }

            EditorGUILayout.EndHorizontal();

            DrawLevelToggle(GameLog.LogLevel.Progress, ConditionalLogStrings.LevelKey(GameLog.LogLevel.Progress));
            DrawLevelToggle(GameLog.LogLevel.Info, ConditionalLogStrings.LevelKey(GameLog.LogLevel.Info));
            DrawLevelToggle(GameLog.LogLevel.Warning, ConditionalLogStrings.LevelKey(GameLog.LogLevel.Warning));
            DrawLevelToggle(GameLog.LogLevel.Error, ConditionalLogStrings.LevelKey(GameLog.LogLevel.Error));
            DrawLevelToggle(GameLog.LogLevel.Except, ConditionalLogStrings.LevelKey(GameLog.LogLevel.Except));

            EditorGUI.indentLevel--;
        }

        private void DrawLevelToggle(GameLog.LogLevel level, string labelKey)
        {
            bool enabled = GameLog.IsLevelEnabled(level);
            bool next = EditorGUILayout.Toggle(ConditionalLogLocale.T(labelKey), enabled);
            if (next == enabled)
            {
                return;
            }

            GameLog.SetLevel(level, next);
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

            string search = _tagSearch?.Trim() ?? string.Empty;
            foreach (GameLogTags tag in Enum.GetValues(typeof(GameLogTags)))
            {
                if (tag == GameLogTags.None)
                {
                    continue;
                }

                string label = tag.ToString();
                if (search.Length > 0 && label.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                bool enabled = LogTagFilter.IsEnabled(tag);
                bool next = EditorGUILayout.Toggle(label, enabled);
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
