using System.Diagnostics;

namespace ConditionalLog
{
    // ponytail: 에디터 전용 개발 로그. 레벨은 메모리만. persist는 Editor.
    public static class Log
    {
        public static bool LevelProgress { get; private set; } = true;
        public static bool LevelInfo { get; private set; } = true;
        public static bool LevelWarning { get; private set; } = true;
        public static bool LevelError { get; private set; } = true;
        public static bool LevelExcept { get; private set; } = true;

        public static void SetLogLevelAll()
        {
            LevelProgress = true;
            LevelInfo = true;
            LevelWarning = true;
            LevelError = true;
            LevelExcept = true;
        }

        public static void SetLogLevelOff()
        {
            LevelProgress = false;
            LevelInfo = false;
            LevelWarning = false;
            LevelError = false;
            LevelExcept = false;
        }

        public static void SetLevel(LogLevel level, bool enabled)
        {
            switch (level)
            {
                case LogLevel.Progress:
                    LevelProgress = enabled;
                    break;
                case LogLevel.Info:
                    LevelInfo = enabled;
                    break;
                case LogLevel.Warning:
                    LevelWarning = enabled;
                    break;
                case LogLevel.Error:
                    LevelError = enabled;
                    break;
                case LogLevel.Except:
                    LevelExcept = enabled;
                    break;
            }
        }

        public static bool IsLevelEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.Progress => LevelProgress,
                LogLevel.Info => LevelInfo,
                LogLevel.Warning => LevelWarning,
                LogLevel.Error => LevelError,
                LogLevel.Except => LevelExcept,
                _ => true,
            };
        }

        [Conditional("UNITY_EDITOR")]
        public static void Progress(string message)
        {
            Write(LogLevel.Progress, null, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Progress(string tag, string message)
        {
            Write(LogLevel.Progress, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(string message)
        {
            Write(LogLevel.Info, null, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(string tag, string message)
        {
            Write(LogLevel.Info, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(string message)
        {
            Write(LogLevel.Warning, null, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(string tag, string message)
        {
            Write(LogLevel.Warning, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(string message)
        {
            Write(LogLevel.Error, null, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(string tag, string message)
        {
            Write(LogLevel.Error, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(string message)
        {
            Write(LogLevel.Except, null, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(string tag, string message)
        {
            Write(LogLevel.Except, tag, message);
        }

        public enum LogLevel
        {
            Progress,
            Info,
            Warning,
            Error,
            Except,
        }

        private static void Write(LogLevel level, string tag, string message)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                LogTagFilter.Register(tag);
            }

            if (!IsLevelEnabled(level))
            {
                return;
            }

            if (!LogTagFilter.IsEnabled(tag))
            {
                return;
            }

            // ponytail: Unity 경고·에러 채널과 섞지 않음. 레벨은 접두사+색만.
            string hex = LevelColor(level);
            string formatted = !string.IsNullOrEmpty(tag)
                ? $"<color={hex}>[{level}]</color> [{tag}] {message}"
                : $"<color={hex}>[{level}]</color> {message}";

            UnityEngine.Debug.Log(formatted);
        }

        private static string LevelColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Progress => "#6B8E23",
                LogLevel.Info => "#7FDBFF",
                LogLevel.Warning => "#FFD54F",
                LogLevel.Error => "#E57373",
                LogLevel.Except => "#4169E1",
                _ => "#7FDBFF",
            };
        }
    }
}
