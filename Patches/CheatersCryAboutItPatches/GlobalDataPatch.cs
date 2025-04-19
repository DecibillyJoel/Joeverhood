using UnityEngine;
using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Joeverhood.libs.ILStepper;
using Joeverhood.libs.HarmonyXExtensions;

using LogLevel = BepInEx.Logging.LogLevel;
using Object = UnityEngine.Object;
using OpCodes = System.Reflection.Emit.OpCodes;
using Random = System.Random;
using BepInEx;

namespace Joeverhood.Patches.CheatersCryAboutItPatches;

[HarmonyPatch(typeof(GlobalData))]
public static class GlobalDataPatch
{
    [HarmonyPatch(nameof(GlobalData.UpdateReplayBattleHighScore))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static void UpdateReplayBattleHighScore_Prefix(ref int score){
        if (!Joeverhood.AreCheatersCryingAboutIt) return;

        score = 0;
    }

    [HarmonyPatch(nameof(GlobalData.UpdateReplayBattleStarsCount))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static void UpdateReplayBattleStarsCount_Prefix(ref int stars){
        if (!Joeverhood.AreCheatersCryingAboutIt) return;
        
        stars = 0;
    }
}