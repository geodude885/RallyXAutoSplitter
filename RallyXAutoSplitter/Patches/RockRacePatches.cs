using HarmonyLib;
using Il2CppRUMBLE.Environment.Minigames;

namespace RallyXAutoSplitter.Patches;

[HarmonyPatch(typeof(ParkMinigame), "StartMinigame")]
public static class RockRaceStartPatch
{
    internal static void Postfix(RockRace __instance)
        => Mod.Service.RaceStarted(__instance);
}

[HarmonyPatch(typeof(ParkMinigame), "OnMiniGameEnded")]
public static class OnMiniGameEndedPatch
{
    public static void Postfix( RockRace __instance, short winningPlayer)
        => Mod.Service.RaceEnded(__instance, winningPlayer);
}