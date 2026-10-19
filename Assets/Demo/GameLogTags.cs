using ConditionalLog;

namespace ConditionalLog.Demo
{
    [LogTagCatalog]
    public static class GameLogTags
    {
        public static readonly LogTag Boot = new LogTag("Boot");
        public static readonly LogTag Hud = new LogTag("Hud");
        public static readonly LogTag Obstacle = new LogTag("Obstacle");
        public static readonly LogTag Runner = new LogTag("Runner");
    }
}
