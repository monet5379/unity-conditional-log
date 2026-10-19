using System;
using System.Collections.Generic;

namespace ConditionalLog
{
    public static class LogTagFilter
    {
        private static readonly HashSet<string> Disabled =
            new HashSet<string>(StringComparer.Ordinal);

        private static readonly List<LogTag> KnownTagsInternal = new List<LogTag>();

        public static IReadOnlyList<LogTag> KnownTags => KnownTagsInternal;

        public static bool IsEnabled(LogTag tag)
        {
            if (tag.IsNone)
            {
                return true;
            }

            return !Disabled.Contains(tag.Name);
        }

        public static void SetEnabled(LogTag tag, bool enabled)
        {
            if (tag.IsNone)
            {
                return;
            }

            if (enabled)
            {
                Disabled.Remove(tag.Name);
            }
            else
            {
                Disabled.Add(tag.Name);
            }
        }

        public static void SetAllEnabled()
        {
            Disabled.Clear();
        }

        public static void SetAllDisabled()
        {
            for (int i = 0; i < KnownTagsInternal.Count; i++)
            {
                LogTag tag = KnownTagsInternal[i];
                if (tag.IsNone)
                {
                    continue;
                }

                Disabled.Add(tag.Name);
            }
        }

        public static void ApplyDisabled(IEnumerable<string> tagNames)
        {
            Disabled.Clear();

            if (tagNames == null)
            {
                return;
            }

            foreach (string name in tagNames)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                Disabled.Add(name);
            }
        }

        public static List<string> GetDisabledTags()
        {
            var list = new List<string>(Disabled);
            list.Sort(StringComparer.Ordinal);
            return list;
        }

        public static void SetKnownTags(IEnumerable<LogTag> tags)
        {
            KnownTagsInternal.Clear();

            if (tags == null)
            {
                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (LogTag tag in tags)
            {
                if (tag.IsNone || !seen.Add(tag.Name))
                {
                    continue;
                }

                KnownTagsInternal.Add(tag);
            }
        }
    }
}
