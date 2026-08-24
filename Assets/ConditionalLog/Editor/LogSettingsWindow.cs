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
            GetWindow<LogSettingsWindow>("Log Settings");
        }

        private void OnEnable()
        {
            LogEditorBootstrap.Reload();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Conditional Log", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Level and tag filters change editor visibility only. Player builds strip Log.* calls via [Conditional].",
                MessageType.Info);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            DrawLevelsSection();
            EditorGUILayout.Space(8);
            DrawTagsSection();

            EditorGUILayout.EndScrollView();
        }

        private void DrawLevelsSection()
        {
            _levelsFoldout = EditorGUILayout.Foldout(_levelsFoldout, "Levels", true);
            if (!_levelsFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("All On"))
            {
                Log.SetLogLevelAll();
                LogEditorPrefs.SaveLevels();
            }

            if (GUILayout.Button("All Off"))
            {
                Log.SetLogLevelOff();
                LogEditorPrefs.SaveLevels();
            }

            EditorGUILayout.EndHorizontal();

            DrawLevelToggle(Log.LogLevel.Progress, "Progress");
            DrawLevelToggle(Log.LogLevel.Info, "Info");
            DrawLevelToggle(Log.LogLevel.Warning, "Warning");
            DrawLevelToggle(Log.LogLevel.Error, "Error");
            DrawLevelToggle(Log.LogLevel.Except, "Except");

            EditorGUI.indentLevel--;
        }

        private void DrawLevelToggle(Log.LogLevel level, string label)
        {
            bool enabled = Log.IsLevelEnabled(level);
            bool next = EditorGUILayout.Toggle(label, enabled);
            if (next == enabled)
            {
                return;
            }

            Log.SetLevel(level, next);
            LogEditorPrefs.SaveLevels();
        }

        private void DrawTagsSection()
        {
            _tagsFoldout = EditorGUILayout.Foldout(_tagsFoldout, "Tags", true);
            if (!_tagsFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            _tagSearch = EditorGUILayout.TextField("Search", _tagSearch);

            List<string> known = LogTagFilter.GetKnownTags();
            if (known.Count == 0)
            {
                EditorGUILayout.HelpBox("No tags registered yet. Log with a tag to list it here.", MessageType.None);
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
