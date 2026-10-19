using System;

namespace ConditionalLog
{
    public readonly struct LogTag : IEquatable<LogTag>
    {
        public string Name { get; }

        public LogTag(string name)
        {
            Name = name;
        }

        public bool IsNone => string.IsNullOrEmpty(Name);

        public bool Equals(LogTag other) =>
            string.Equals(Name, other.Name, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is LogTag other && Equals(other);

        public override int GetHashCode() =>
            Name == null ? 0 : StringComparer.Ordinal.GetHashCode(Name);

        public static bool operator ==(LogTag left, LogTag right) => left.Equals(right);

        public static bool operator !=(LogTag left, LogTag right) => !left.Equals(right);

        public override string ToString() => Name ?? string.Empty;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LogTagCatalogAttribute : Attribute
    {
    }
}
