using HarmonyLib;

namespace BarberClockTower.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        internal static int surgeonSourceCount;
        private static StartOfRoundPatch Instance;

        [HarmonyPatch("openingDoorsSequence")]
        [HarmonyPostfix]
        private static void PatchOpeningDoorsSequence()
        {
            ClaySurgeonAIPatch.surgeonSources.Clear();
            surgeonSourceCount = 0;
        }
    }
}