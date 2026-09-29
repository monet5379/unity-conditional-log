using System.Collections.Generic;

namespace ConditionalLog
{
    // ponytail: 미설정=on. 비활성 태그만 보관. CSV/EditorPrefs는 Editor.
    public static class LogTagFilter
    {
        private static readonly HashSet<GameLogTags> Disabled = new HashSet<GameLogTags>();

        public static bool IsEnabled(GameLogTags tag)
        {
            if (tag == GameLogTags.None)
            {
                return true;
            }

            return !Disabled.Contains(tag);
        }

        public static void SetEnabled(GameLogTags tag, bool enabled)
        {
            if (tag == GameLogTags.None)
            {
                return;
            }

            if (enabled)
            {
                Disabled.Remove(tag);
            }
            else
            {
                Disabled.Add(tag);
            }
        }

        public static void ApplyDisabled(IEnumerable<GameLogTags> tags)
        {
            Disabled.Clear();

            if (tags == null)
            {
                return;
            }

            foreach (GameLogTags tag in tags)
            {
                if (tag == GameLogTags.None)
                {
                    continue;
                }

                Disabled.Add(tag);
            }
        }

        public static List<GameLogTags> GetDisabledTags()
        {
            var list = new List<GameLogTags>(Disabled);
            list.Sort((a, b) => string.CompareOrdinal(a.ToString(), b.ToString()));
            return list;
        }
    }
}
