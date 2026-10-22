using System;
using System.Collections.Generic;
using ConditionalLog;
using UnityEditor;
using UnityEngine;

namespace ConditionalLog.Editor
{
    internal sealed class LogSettingsWindow : EditorWindow
    {
        private const string MenuPath = "Tools/Conditional Log/Settings";
        private const float BulkActionButtonWidth = 84f;
        private const float BulkActionButtonHeight = 20f;

        private enum SettingsTab
        {
            Levels = 0,
            Tags = 1,
        }

        private Vector2 _scroll;
        private SettingsTab _selectedTab = SettingsTab.Levels;
        private string _tagSearch = string.Empty;
        private readonly Dictionary<string, bool> _tagCategoryFoldouts =
            new Dictionary<string, bool>(StringComparer.Ordinal);

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

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            DrawSettingsTabBar();
            DrawSelectedTabHelp();
            EditorGUILayout.Space(4);

            switch (_selectedTab)
            {
                case SettingsTab.Levels:
                    DrawLevelsSection();
                    break;
                case SettingsTab.Tags:
                    DrawTagsSection();
                    break;
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawSettingsTabBar()
        {
            EditorGUILayout.BeginHorizontal();
            if (SettingsTabButton(SettingsTab.Levels, ConditionalLogLocale.T("editor.levels")))
            {
                _selectedTab = SettingsTab.Levels;
            }

            if (SettingsTabButton(SettingsTab.Tags, ConditionalLogLocale.T("editor.tags")))
            {
                _selectedTab = SettingsTab.Tags;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectedTabHelp()
        {
            string key = _selectedTab == SettingsTab.Levels
                ? "editor.help.levels"
                : "editor.help.tags";
            EditorGUILayout.HelpBox(ConditionalLogLocale.T(key), MessageType.Info);
        }

        private bool SettingsTabButton(SettingsTab tab, string label)
        {
            bool selected = _selectedTab == tab;
            Color previousBackground = GUI.backgroundColor;
            if (selected)
            {
                GUI.backgroundColor = ActiveTabBackground;
            }

            try
            {
                return GUILayout.Toggle(
                    selected,
                    label,
                    EditorStyles.toolbarButton,
                    GUILayout.Width(BulkActionButtonWidth),
                    GUILayout.Height(BulkActionButtonHeight));
            }
            finally
            {
                GUI.backgroundColor = previousBackground;
            }
        }

        private static Color ActiveTabBackground =>
            EditorGUIUtility.isProSkin
                ? new Color(0.28f, 0.52f, 0.32f, 1f)
                : new Color(0.62f, 0.86f, 0.62f, 1f);

        private void DrawLevelsSection()
        {
            EditorGUILayout.BeginHorizontal();
            if (BulkActionButton(ConditionalLogLocale.T("editor.all_on")))
            {
                GameLog.SetLogLevelAll();
                LogEditorPrefs.SaveLevels();
            }

            if (BulkActionButton(ConditionalLogLocale.T("editor.all_off")))
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
        }

        private void DrawLevelToggle(GameLog.LogLevel level)
        {
            bool enabled = GameLog.IsLevelEnabled(level);
            Color previousContentColor = GUI.contentColor;
            if (enabled)
            {
                GUI.contentColor = EnabledToggleContentColor;
            }

            bool next = EditorGUILayout.Toggle(
                ConditionalLogLocale.T(ConditionalLogStrings.LevelKey(level)),
                enabled);
            GUI.contentColor = previousContentColor;
            if (next != enabled)
            {
                GameLog.SetLevel(level, next);
                LogEditorPrefs.SaveLevels();
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(
                ConditionalLogLocale.T(ConditionalLogStrings.LevelDescriptionKey(level)),
                EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        private void DrawTagsSection()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (BulkActionButton(ConditionalLogLocale.T("editor.all_on")))
            {
                LogTagFilter.SetAllEnabled();
                LogEditorPrefs.SaveTags();
            }

            if (BulkActionButton(ConditionalLogLocale.T("editor.all_off")))
            {
                LogTagFilter.SetAllDisabled();
                LogEditorPrefs.SaveTags();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            if (BulkActionButton(ConditionalLogLocale.T("editor.categories_expand_all")))
            {
                SetAllCategoryFoldouts(true);
            }

            if (BulkActionButton(ConditionalLogLocale.T("editor.categories_collapse_all")))
            {
                SetAllCategoryFoldouts(false);
            }

            EditorGUILayout.EndHorizontal();

            _tagSearch = EditorGUILayout.TextField(
                ConditionalLogLocale.T("editor.search"),
                _tagSearch);

            string search = _tagSearch?.Trim() ?? string.Empty;
            List<TagCategorySection> sections = BuildTagCategorySections(LogTagFilter.KnownTags);
            for (int s = 0; s < sections.Count; s++)
            {
                TagCategorySection section = sections[s];
                if (search.Length > 0 && !SectionMatchesSearch(section.Tags, search))
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("box");

                if (!section.HasCategory)
                {
                    DrawTagsInCategory(section.Tags, search);
                    DrawCategoryBulkActions(section.Tags);
                    EditorGUILayout.EndVertical();
                    continue;
                }

                if (!_tagCategoryFoldouts.TryGetValue(section.FoldoutKey, out bool categoryOpen))
                {
                    categoryOpen = true;
                    _tagCategoryFoldouts[section.FoldoutKey] = categoryOpen;
                }

                bool categoryHasEnabledTag = SectionHasAnyEnabledTag(section.Tags);
                Color previousCategoryContentColor = GUI.contentColor;
                if (categoryHasEnabledTag)
                {
                    GUI.contentColor = EnabledToggleContentColor;
                }

                categoryOpen = EditorGUILayout.Foldout(categoryOpen, section.Label, true);
                GUI.contentColor = previousCategoryContentColor;
                _tagCategoryFoldouts[section.FoldoutKey] = categoryOpen;
                if (categoryOpen)
                {
                    DrawTagsInCategory(section.Tags, search);
                    DrawCategoryBulkActions(section.Tags);
                }

                EditorGUILayout.EndVertical();
            }
        }

        private readonly struct TagCategorySection
        {
            public TagCategorySection(string foldoutKey, string label, bool hasCategory, List<LogTag> tags)
            {
                FoldoutKey = foldoutKey;
                Label = label;
                HasCategory = hasCategory;
                Tags = tags;
            }

            public string FoldoutKey { get; }

            public string Label { get; }

            public bool HasCategory { get; }

            public List<LogTag> Tags { get; }
        }

        private static List<TagCategorySection> BuildTagCategorySections(IReadOnlyList<LogTag> knownTags)
        {
            var sections = new List<TagCategorySection>();
            if (knownTags == null || knownTags.Count == 0)
            {
                return sections;
            }

            var tagsByKey = new Dictionary<string, List<LogTag>>(StringComparer.Ordinal);
            var keyOrder = new List<string>();
            for (int i = 0; i < knownTags.Count; i++)
            {
                LogTag tag = knownTags[i];
                string key = string.IsNullOrEmpty(tag.Category) ? string.Empty : tag.Category;
                if (!tagsByKey.TryGetValue(key, out List<LogTag> bucket))
                {
                    bucket = new List<LogTag>();
                    tagsByKey.Add(key, bucket);
                    keyOrder.Add(key);
                }

                bucket.Add(tag);
            }

            for (int i = 0; i < keyOrder.Count; i++)
            {
                string key = keyOrder[i];
                List<LogTag> tags = tagsByKey[key];
                if (tags.Count == 0)
                {
                    continue;
                }

                if (key.Length == 0)
                {
                    sections.Add(new TagCategorySection(string.Empty, string.Empty, false, tags));
                    continue;
                }

                sections.Add(new TagCategorySection(key, key, true, tags));
            }

            return sections;
        }

        private static bool SectionMatchesSearch(IReadOnlyList<LogTag> tags, string search)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool SectionHasAnyEnabledTag(IReadOnlyList<LogTag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (LogTagFilter.IsEnabled(tags[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static Color EnabledToggleContentColor =>
            EditorGUIUtility.isProSkin
                ? new Color(0.55f, 0.88f, 0.55f)
                : new Color(0.12f, 0.5f, 0.18f);

        private static bool BulkActionButton(string label)
        {
            return GUILayout.Button(
                label,
                GUILayout.Width(BulkActionButtonWidth),
                GUILayout.Height(BulkActionButtonHeight));
        }

        private static void DrawCategoryBulkActions(IReadOnlyList<LogTag> tags)
        {
            EditorGUILayout.BeginHorizontal();
            if (BulkActionButton(ConditionalLogLocale.T("editor.all_on")))
            {
                SetSectionTagsEnabled(tags, true);
            }

            if (BulkActionButton(ConditionalLogLocale.T("editor.all_off")))
            {
                SetSectionTagsEnabled(tags, false);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void SetSectionTagsEnabled(IReadOnlyList<LogTag> tags, bool enabled)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                LogTagFilter.SetEnabled(tags[i], enabled);
            }

            LogEditorPrefs.SaveTags();
        }

        private void SetAllCategoryFoldouts(bool expanded)
        {
            List<TagCategorySection> sections = BuildTagCategorySections(LogTagFilter.KnownTags);
            for (int i = 0; i < sections.Count; i++)
            {
                TagCategorySection section = sections[i];
                if (!section.HasCategory)
                {
                    continue;
                }

                _tagCategoryFoldouts[section.FoldoutKey] = expanded;
            }
        }

        private static void DrawTagsInCategory(IReadOnlyList<LogTag> tags, string search)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                LogTag tag = tags[i];
                string label = tag.Name;
                if (search.Length > 0 && label.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                bool enabled = LogTagFilter.IsEnabled(tag);
                Color previousContentColor = GUI.contentColor;
                if (enabled)
                {
                    GUI.contentColor = EnabledToggleContentColor;
                }

                bool next = EditorGUILayout.Toggle(label, enabled);
                GUI.contentColor = previousContentColor;
                if (next == enabled)
                {
                    continue;
                }

                LogTagFilter.SetEnabled(tag, next);
                LogEditorPrefs.SaveTags();
            }
        }
    }
}
