using System;
using UnityEngine;

namespace ConditionalLog
{
    /// <summary>
    /// Current UI language, persistence, lookup, and change event.
    /// </summary>
    public static class ConditionalLogLocale
    {
        public const string PrefsKey = "ConditionalLog.UI.Language";

        static ConditionalLogLang _current;
        static bool _initialized;

        public static event Action LanguageChanged;

        public static ConditionalLogLang Current
        {
            get
            {
                EnsureInitialized();
                return _current;
            }
        }

        public static void SetLanguage(ConditionalLogLang lang)
        {
            EnsureInitialized();
            if (_current == lang)
                return;

            _current = lang;
            PlayerPrefs.SetInt(PrefsKey, (int)lang);
            PlayerPrefs.Save();
            LanguageChanged?.Invoke();
        }

        public static string T(string key) => ConditionalLogStrings.Get(key, Current);

        public static string T(string key, params object[] args)
        {
            var format = T(key);
            if (args == null || args.Length == 0)
                return format;
            try
            {
                return string.Format(format, args);
            }
            catch (FormatException)
            {
                return format;
            }
        }

        static void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;
            var stored = PlayerPrefs.GetInt(PrefsKey, (int)ConditionalLogLang.English);
            _current = stored == (int)ConditionalLogLang.Korean
                ? ConditionalLogLang.Korean
                : ConditionalLogLang.English;
        }
    }
}
