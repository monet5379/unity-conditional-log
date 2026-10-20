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
            Event current = Event.current;
            if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Space)
            {
                EmitSampleLogs();
                current.Use();
            }
        }

        private static void EmitSampleLogs()
        {
            GameLog.Info("sample info (no tag)");
            GameLog.Progress(GameLogTags.Addressable, "addressable progress");
            GameLog.Info(GameLogTags.HUD, "hud info");
            GameLog.Warning(GameLogTags.JsonData, "json data warning");
            GameLog.Error(GameLogTags.Audio, "audio error");
        }
    }
}
