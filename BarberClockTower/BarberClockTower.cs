using BarberClockTower.Patches;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BarberClockTower
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class BarberClockTower : BaseUnityPlugin
    {
        private const string modGUID = "Zomagon.BarberClockTower";
        private const string modName = "Barber Clock Tower Mod";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static BarberClockTower Instance;

        static internal ManualLogSource mls;

        public static AssetBundle ModAssets;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("The Scissorman has awoken.");

            harmony.PatchAll(typeof(BarberClockTower));
            harmony.PatchAll(typeof(ClaySurgeonAIPatch));
            harmony.PatchAll(typeof(StartOfRoundPatch));

            var bundleName = "barberclocktower";
            ModAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), bundleName));
            if (ModAssets == null)
            {
                Logger.LogError($"Failed to load custom assets.");
                return;
            }
        }
    }
}
