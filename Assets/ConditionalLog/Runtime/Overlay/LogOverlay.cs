#if UNITY_EDITOR
using System;
using System.Collections.Generic;
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

            _windowRect = GUI.Window(GetInstanceID(), _windowRect, DrawWindow, "Conditional Log (F1)");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label("Editor visibility only. Player builds strip Log.* via [Conditional].");

            _scroll = GUILayout.BeginScrollView(_scroll);

            GUILayout.Label("Levels");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("All On"))
            {
                Log.SetLogLevelAll();
                LogOverlay.PersistLevels?.Invoke();
            }

            if (GUILayout.Button("All Off"))
            {
                Log.SetLogLevelOff();
                LogOverlay.PersistLevels?.Invoke();
            }

            GUILayout.EndHorizontal();

            DrawLevel(Log.LogLevel.Progress, "Progress");
            DrawLevel(Log.LogLevel.Info, "Info");
            DrawLevel(Log.LogLevel.Warning, "Warning");
            DrawLevel(Log.LogLevel.Error, "Error");
            DrawLevel(Log.LogLevel.Except, "Except");

            GUILayout.Space(8f);
            GUILayout.Label("Tags");

            List<string> known = LogTagFilter.GetKnownTags();
            if (known.Count == 0)
            {
                GUILayout.Label("Tagged Log.* calls appear here.");
            }
            else
            {
                for (int i = 0; i < known.Count; i++)
                {
                    DrawTag(known[i]);
                }
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }

        private static void DrawLevel(Log.LogLevel level, string label)
        {
            bool enabled = Log.IsLevelEnabled(level);
            bool next = GUILayout.Toggle(enabled, label);
            if (next == enabled)
            {
                return;
            }

            Log.SetLevel(level, next);
            LogOverlay.PersistLevels?.Invoke();
        }

        private static void DrawTag(string tag)
        {
            bool enabled = LogTagFilter.IsEnabled(tag);
            bool next = GUILayout.Toggle(enabled, tag);
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
