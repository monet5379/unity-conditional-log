using System;
using System.Collections.Generic;
using ConditionalLog;

namespace ConditionalLog.Demo
{
    /// <summary>
    /// Playground-only display strings (English). Optional Korean: <c>DemoStrings.Ko.cs</c>.
    /// Not a logging i18n template — <c>Log.*</c> message bodies stay a call-site choice.
    /// </summary>
    public static class DemoStrings
    {
        static readonly Dictionary<string, string> En = new Dictionary<string, string>
        {
            { "demo.hint_f1", "F1 — log filters" },
            { "demo.hud.start", "Press Space to start" },
            { "demo.hud.game_over", "Game over\nPress Space to restart" },
            { "demo.log.press_start", "press space to start" },
            { "demo.log.collision", "collision" },
            { "demo.log.game_over", "game over" },
            { "demo.log.press_restart", "press space to restart" },
            { "demo.log.run_start", "run start" },
            { "demo.log.no_camera", "no main camera" },
            { "demo.log.no_font", "no builtin font" },
            { "demo.log.spawn_skipped", "spawn skipped: template or sprite missing" },
            { "demo.log.spawn_height", "spawn height={0}" },
            { "demo.log.jump_no_rb", "jump skipped: no rigidbody" },
            { "demo.log.jump", "jump" },
            { "demo.log.missing_sprite", "missing sprite {0}" },
        };

        public static string T(string key) => Get(key, ConditionalLogLocale.Current);

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

        static string Get(string key, ConditionalLogLang lang)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            // Remove this branch if you delete DemoStrings.Ko.cs
            if (lang == ConditionalLogLang.Korean
                && DemoStringsKo.TryGet(key, out var ko))
                return ko;

            if (En.TryGetValue(key, out var en))
                return en;

            return key;
        }
    }
}
