using System;
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
            ApplyTitle();
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

            DrawLevelToggle(GameLog.LogLevel.Progress);
            DrawLevelToggle(GameLog.LogLevel.Info);
            DrawLevelToggle(GameLog.LogLevel.Warning);
            DrawLevelToggle(GameLog.LogLevel.Error);
            DrawLevelToggle(GameLog.LogLevel.Except);

            EditorGUI.indentLevel--;
        }

        private void DrawLevelToggle(GameLog.LogLevel level)
        {
            bool enabled = GameLog.IsLevelEnabled(level);
            bool next = EditorGUILayout.Toggle(
                ConditionalLogLocale.T(ConditionalLogStrings.LevelKey(level)),
                enabled);
            if (next != enabled)
            {
                GameLog.SetLevel(level, next);
                LogEditorPrefs.SaveLevels();
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(
                ConditionalLogLocale.T(ConditionalLogStrings.LevelDescriptionKey(level)),
                EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
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
            IReadOnlyList<LogTag> knownTags = LogTagFilter.KnownTags;
            for (int i = 0; i < knownTags.Count; i++)
            {
                LogTag tag = knownTags[i];
                string label = tag.Name;
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
