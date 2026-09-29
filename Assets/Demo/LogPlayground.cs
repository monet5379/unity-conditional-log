using ConditionalLog;
using UnityEngine;

namespace ConditionalLog.Demo
{
    public sealed class LogPlayground : MonoBehaviour
    {
        private void Start()
        {
            EmitSampleLogs();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10f, 10f, 400f, 24f), "F1 — log filters");

            Event current = Event.current;
            if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Space)
            {
                EmitSampleLogs();
                current.Use();
            }
        }

        private static void EmitSampleLogs()
        {
            GameLog.Info("sample info");
            GameLog.Progress(GameLogTags.Boot, "boot progress");
            GameLog.Info(GameLogTags.Hud, "hud info");
            GameLog.Warning(GameLogTags.Obstacle, "obstacle warning");
            GameLog.Error(GameLogTags.Runner, "runner error");
        }
    }
}
