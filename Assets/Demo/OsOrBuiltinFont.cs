using UnityEngine;

namespace ConditionalLog.Demo
{
    /// <summary>
    /// Demo/playground UGUI Text: OS CJK first, then Unity Legacy/Arial.
    /// Limits: Win/Mac KO names only — Linux/missing OS fonts fall back to Legacy (no Hangul).
    /// OS fonts are not bundled; not for shipping. For reliable KO, embed a TTF (e.g. Demo/Fonts).
    /// </summary>
    public static class OsOrBuiltinFont
    {
        public static readonly string[] DefaultCjkOsNames =
        {
            "Malgun Gothic",
            "맑은 고딕",
            "Apple SD Gothic Neo",
            "Noto Sans CJK KR",
            "Arial Unicode MS",
        };

        public static Font Resolve()
        {
            return Resolve(DefaultCjkOsNames, 36);
        }

        public static Font Resolve(string[] preferOsNames, int size = 36)
        {
            if (preferOsNames != null && preferOsNames.Length > 0)
            {
                Font os = Font.CreateDynamicFontFromOSFont(preferOsNames, size);
                if (os != null)
                {
                    return os;
                }
            }

            Font builtin = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (builtin != null)
            {
                return builtin;
            }

            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
    }
}
