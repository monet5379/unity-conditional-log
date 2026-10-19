using System.Diagnostics;

namespace ConditionalLog
{
    // ponytail: 에디터 전용 개발 로그. 레벨은 메모리만. persist는 Editor.
    public static class GameLog
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
            Write(LogLevel.Progress, default, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Progress(LogTag tag, string message)
        {
            Write(LogLevel.Progress, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Progress(string format, params object[] args)
        {
            Write(LogLevel.Progress, default, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Progress(LogTag tag, string format, params object[] args)
        {
            Write(LogLevel.Progress, tag, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(string message)
        {
            Write(LogLevel.Info, default, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(LogTag tag, string message)
        {
            Write(LogLevel.Info, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(string format, params object[] args)
        {
            Write(LogLevel.Info, default, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Info(LogTag tag, string format, params object[] args)
        {
            Write(LogLevel.Info, tag, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(string message)
        {
            Write(LogLevel.Warning, default, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(LogTag tag, string message)
        {
            Write(LogLevel.Warning, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(string format, params object[] args)
        {
            Write(LogLevel.Warning, default, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warning(LogTag tag, string format, params object[] args)
        {
            Write(LogLevel.Warning, tag, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(string message)
        {
            Write(LogLevel.Error, default, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(LogTag tag, string message)
        {
            Write(LogLevel.Error, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(string format, params object[] args)
        {
            Write(LogLevel.Error, default, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(LogTag tag, string format, params object[] args)
        {
            Write(LogLevel.Error, tag, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(string message)
        {
            Write(LogLevel.Except, default, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(LogTag tag, string message)
        {
            Write(LogLevel.Except, tag, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(string format, params object[] args)
        {
            Write(LogLevel.Except, default, string.Format(format, args));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Except(LogTag tag, string format, params object[] args)
        {
            Write(LogLevel.Except, tag, string.Format(format, args));
        }

        public enum LogLevel
        {
            Progress,
            Info,
            Warning,
            Error,
            Except,
        }

        private static void Write(LogLevel level, LogTag tag, string message)
        {
            if (!IsLevelEnabled(level))
            {
                return;
            }

            if (!tag.IsNone && !LogTagFilter.IsEnabled(tag))
            {
                return;
            }

            string body = !tag.IsNone
                ? $"[{tag}] {message}"
                : $"[{ConditionalLogLocale.T(ConditionalLogStrings.LevelKey(level))}] {message}";
            string formatted = level == LogLevel.Info
                ? body
                : $"<color={LevelColor(level)}>{body}</color>";

            UnityEngine.Debug.Log(formatted);
        }

        private static string LevelColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Progress => "#6B8E23",
                LogLevel.Warning => "#FFD54F",
                LogLevel.Error => "#E57373",
                LogLevel.Except => "#4169E1",
                _ => "#FFFFFF",
            };
        }
    }
}
