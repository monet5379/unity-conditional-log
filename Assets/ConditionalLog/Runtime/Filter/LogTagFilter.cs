using System.Collections.Generic;

namespace ConditionalLog
{
    // ponytail: 미설정=on. 비활성 태그만 보관. CSV/EditorPrefs는 Editor.
    public static class LogTagFilter
    {
        private static readonly HashSet<string> Disabled = new HashSet<string>();
        private static readonly HashSet<string> Known = new HashSet<string>();

        public static void Register(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            Known.Add(tag);
        }

        public static bool IsEnabled(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return true;
            }

            return !Disabled.Contains(tag);
        }

        public static void SetEnabled(string tag, bool enabled)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            Register(tag);

            if (enabled)
            {
                Disabled.Remove(tag);
            }
            else
            {
                Disabled.Add(tag);
            }
        }

        public static void ApplyDisabled(IEnumerable<string> tags)
        {
            Disabled.Clear();

            if (tags == null)
            {
                return;
            }

            foreach (string tag in tags)
            {
                if (string.IsNullOrEmpty(tag))
                {
                    continue;
                }

                Disabled.Add(tag);
                Known.Add(tag);
            }
        }

        public static List<string> GetKnownTags()
        {
            var list = new List<string>(Known);
            list.Sort(System.StringComparer.Ordinal);
            return list;
        }

        public static List<string> GetDisabledTags()
        {
            var list = new List<string>(Disabled);
            list.Sort(System.StringComparer.Ordinal);
            return list;
        }
    }
}
