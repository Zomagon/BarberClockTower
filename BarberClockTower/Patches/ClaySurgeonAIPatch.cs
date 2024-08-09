using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BarberClockTower.Patches
{
    [HarmonyPatch(typeof(ClaySurgeonAI))]
    internal class ClaySurgeonAIPatch
    {

        private static AudioClip clockTowerMus = null;

        internal static Dictionary<ClaySurgeonAI, AudioSource> surgeonSources = new Dictionary<ClaySurgeonAI, AudioSource>();

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void claySurgeonMusicStart(ClaySurgeonAI __instance)
        {
            if (clockTowerMus == null)
            {
                if (BarberClockTower.ModAssets == null)
                {
                    BarberClockTower.mls.LogInfo("Failed to load clockTowerMus asset bundle");
                    return;
                }
                clockTowerMus = BarberClockTower.ModAssets.LoadAsset<AudioClip>("Clock Tower - Scissorman.mp3");
            }

            if (StartOfRoundPatch.surgeonSourceCount < 1)
            {
                AudioSource newSource = __instance.gameObject.AddComponent<AudioSource>();
                newSource.clip = clockTowerMus;
                newSource.volume = 40f;
                newSource.loop = true;
                newSource.dopplerLevel = 0.0f;
                newSource.spatialBlend = 1f;
                newSource.maxDistance = 300f;
                newSource.spatialize = false;
                surgeonSources[__instance] = newSource;
            }

            StartOfRoundPatch.surgeonSourceCount += 1;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void claySurgeonMusicUpdate(ClaySurgeonAI __instance)
        {
            AudioSource source = surgeonSources[__instance];

            if (__instance.isEnemyDead)
            {
                source.Stop();
            }
            else if (!source.isPlaying)
            {
                if (source.time > 0.0f && source.time <= clockTowerMus.length - 0.1)
                    source.UnPause();
                else
                    source.Play();
            }
        }

    }
}
