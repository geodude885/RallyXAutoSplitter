using MelonLoader;
using RallyXAutoSplitter.Adapters;
using RallyXAutoSplitter.Services;

[assembly: MelonInfo(typeof(RallyXAutoSplitter.Mod), "RallyXAutoSplitter", "1.1.0", "ACutiePi")]
[assembly: MelonGame("Buckethead Entertainment", "RUMBLE")]

namespace RallyXAutoSplitter;

public class Mod : MelonMod
{
    public static MelonLogger.Instance Logger { get; private set; } = null!;
    public static AutoSplitterService Service { get; private set; } = null!;
    public static LiveSplitService LiveSplitService { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Logger = LoggerInstance;
        var loggerAdapter = new MelonLoggerAdapter(LoggerInstance);
        var tcpFactory = new TcpClientFactory();
        LiveSplitService = new(loggerAdapter, tcpFactory);
        Service = new(LiveSplitService, loggerAdapter);

        Logger.Msg($"Connected to LiveSplit Server: {LiveSplitService.EnsureConnected()}");

        HarmonyInstance.PatchAll();
    }
}
