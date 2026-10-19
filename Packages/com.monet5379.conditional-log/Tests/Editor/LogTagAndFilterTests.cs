using System.Collections.Generic;
using System.Linq;
using ConditionalLog.Editor;
using NUnit.Framework;
using UnityEngine;

namespace ConditionalLog.Tests
{
    [LogTagCatalog]
    internal static class LogTagCatalogTestProbe
    {
        public static readonly LogTag Probe = new LogTag("Test");
    }

    [TestFixture]
    public class LogTagAndFilterTests
    {
        List<string> _savedDisabled;

        [SetUp]
        public void SetUp()
        {
            _savedDisabled = LogTagFilter.GetDisabledTags();
        }

        [TearDown]
        public void TearDown()
        {
            LogTagFilter.ApplyDisabled(_savedDisabled);
        }

        [Test]
        public void LogTag_DefaultIsNone()
        {
            LogTag none = default;
            Assert.That(none.IsNone, Is.True);
            Assert.That(none.Equals(default(LogTag)), Is.True);
        }

        [Test]
        public void LogTag_EqualsUsesOrdinalComparison()
        {
            var lower = new LogTag("alpha");
            var upper = new LogTag("Alpha");
            var same = new LogTag("alpha");

            Assert.That(lower, Is.EqualTo(same));
            Assert.That(lower, Is.Not.EqualTo(upper));
            Assert.That(lower.GetHashCode(), Is.Not.EqualTo(upper.GetHashCode()));
        }

        [Test]
        public void LogTagFilter_SetEnabledAndApplyDisabled_ByTagName()
        {
            var tag = new LogTag("FilterTestTag");

            LogTagFilter.SetEnabled(tag, false);
            Assert.That(LogTagFilter.IsEnabled(tag), Is.False);
            Assert.That(LogTagFilter.GetDisabledTags(), Does.Contain("FilterTestTag"));

            LogTagFilter.SetEnabled(tag, true);
            Assert.That(LogTagFilter.IsEnabled(tag), Is.True);
            Assert.That(LogTagFilter.GetDisabledTags(), Does.Not.Contain("FilterTestTag"));

            LogTagFilter.ApplyDisabled(new[] { "FilterTestTag", "Other" });
            Assert.That(LogTagFilter.IsEnabled(tag), Is.False);
            Assert.That(LogTagFilter.IsEnabled(new LogTag("Other")), Is.False);
            Assert.That(LogTagFilter.IsEnabled(new LogTag("Missing")), Is.True);

            LogTagFilter.ApplyDisabled(null);
            Assert.That(LogTagFilter.IsEnabled(tag), Is.True);
        }

        [Test]
        public void Reload_IncludesLogTagCatalogProbeInKnownTags()
        {
            LogEditorBootstrap.Reload();

            IReadOnlyList<LogTag> known = LogTagFilter.KnownTags;
            Assert.That(
                known.Any(t => t.Equals(LogTagCatalogTestProbe.Probe)),
                Is.True,
                "Expected TypeCache catalog collection to include the test probe tag.");
        }

        [Test]
        public void Info_FormatOverload_WritesFormattedBody()
        {
            bool infoWasEnabled = GameLog.LevelInfo;
            GameLog.SetLevel(GameLog.LogLevel.Info, true);
            var tag = new LogTag("FormatProbe");
            LogTagFilter.SetEnabled(tag, true);

            string tagged = null;
            string untagged = null;
            Application.LogCallback handler = (message, stackTrace, type) =>
            {
                if (message.Contains("[FormatProbe]"))
                {
                    tagged = message;
                }
                else if (message.Contains("format-plain"))
                {
                    untagged = message;
                }
            };

            Application.logMessageReceived += handler;
            try
            {
                GameLog.Info(tag, "format-probe {0}/{1}", 7, 2);
                GameLog.Info("format-plain {0}", 3);
            }
            finally
            {
                Application.logMessageReceived -= handler;
                GameLog.SetLevel(GameLog.LogLevel.Info, infoWasEnabled);
                LogTagFilter.SetEnabled(tag, true);
            }

            Assert.That(tagged, Does.Contain("[FormatProbe] format-probe 7/2"));
            Assert.That(untagged, Does.Contain("format-plain 3"));
        }
    }
}
