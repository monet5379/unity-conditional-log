#if UNITY_EDITOR
using System;
using UnityEngine;

namespace ConditionalLog
{
    public static class LogOverlay
    {
        public static Action PersistLevels;
        public static Action PersistTags;
    }

    internal sealed class LogOverlayHost : MonoBehaviour
    {
        private static bool _spawned;

        private bool _open;
        private bool _f1Held;
        private Rect _windowRect = new Rect(12f, 12f, 320f, 420f);
        private Vector2 _scroll;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (_spawned)
            {
                return;
            }

            _spawned = true;
            GameObject go = new GameObject("ConditionalLog.Overlay");
            DontDestroyOnLoad(go);
            go.AddComponent<LogOverlayHost>();
        }

        private void OnDestroy()
        {
            _spawned = false;
        }

        private void OnGUI()
        {
            Event current = Event.current;
            if (current != null && current.keyCode == KeyCode.F1)
            {
                if (current.type == EventType.KeyDown && !_f1Held)
                {
                    _f1Held = true;
                    _open = !_open;
                    current.Use();
                }
                else if (current.type == EventType.KeyUp)
                {
                    _f1Held = false;
                    current.Use();
                }
            }

            if (!_open)
            {
                return;
            }

            _windowRect = GUI.Window(
                GetInstanceID(),
                _windowRect,
                DrawWindow,
                ConditionalLogLocale.T("overlay.title"));
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label(ConditionalLogLocale.T("overlay.help"));

            _scroll = GUILayout.BeginScrollView(_scroll);

            GUILayout.Label(ConditionalLogLocale.T("editor.levels"));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ConditionalLogLocale.T("editor.all_on")))
            {
                GameLog.SetLogLevelAll();
                LogOverlay.PersistLevels?.Invoke();
            }

            if (GUILayout.Button(ConditionalLogLocale.T("editor.all_off")))
            {
                GameLog.SetLogLevelOff();
                LogOverlay.PersistLevels?.Invoke();
            }

            GUILayout.EndHorizontal();

            DrawLevel(GameLog.LogLevel.Progress);
            DrawLevel(GameLog.LogLevel.Info);
            DrawLevel(GameLog.LogLevel.Warning);
            DrawLevel(GameLog.LogLevel.Error);
            DrawLevel(GameLog.LogLevel.Except);

            GUILayout.Space(8f);
            GUILayout.Label(ConditionalLogLocale.T("editor.tags"));

            foreach (GameLogTags tag in Enum.GetValues(typeof(GameLogTags)))
            {
                if (tag == GameLogTags.None)
                {
                    continue;
                }

                DrawTag(tag);
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }

        private static void DrawLevel(GameLog.LogLevel level)
        {
            bool enabled = GameLog.IsLevelEnabled(level);
            bool next = GUILayout.Toggle(enabled, ConditionalLogLocale.T(ConditionalLogStrings.LevelKey(level)));
            if (next == enabled)
            {
                return;
            }

            GameLog.SetLevel(level, next);
            LogOverlay.PersistLevels?.Invoke();
        }

        private static void DrawTag(GameLogTags tag)
        {
            bool enabled = LogTagFilter.IsEnabled(tag);
            bool next = GUILayout.Toggle(enabled, tag.ToString());
            if (next == enabled)
            {
                return;
            }

            LogTagFilter.SetEnabled(tag, next);
            LogOverlay.PersistTags?.Invoke();
        }
    }
}
#endif
