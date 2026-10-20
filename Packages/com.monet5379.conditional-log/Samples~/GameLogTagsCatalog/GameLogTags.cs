using ConditionalLog;

[LogTagCatalog]
public static class GameLogTags
{
    public static readonly LogTag Addressable = new LogTag("Addressable", "Assets");
    public static readonly LogTag Resource = new LogTag("Resource", "Assets");

    public static readonly LogTag JsonData = new LogTag("JsonData", "Data");
    public static readonly LogTag GamePref = new LogTag("GamePref", "Data");
    public static readonly LogTag GameData = new LogTag("GameData", "Data");
    public static readonly LogTag ScriptableData = new LogTag("ScriptableData", "Data");

    public static readonly LogTag Setting = new LogTag("Setting", "Setting");
    public static readonly LogTag Audio = new LogTag("Audio", "Setting");
    public static readonly LogTag Video = new LogTag("Video", "Setting");
    public static readonly LogTag Input = new LogTag("Input", "Setting");

    public static readonly LogTag HUD = new LogTag("HUD", "UI");
    public static readonly LogTag Popup = new LogTag("Popup", "UI");
    public static readonly LogTag Notice = new LogTag("Notice", "UI");
    public static readonly LogTag Floaty = new LogTag("Floaty", "UI");
}
