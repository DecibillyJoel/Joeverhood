using UnityEngine;
using System.Runtime.CompilerServices;
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

[HarmonyPatch(typeof(SteamHelper))]
public static class SteamHelperPatch
{
    [HarmonyPatch(nameof(SteamHelper.UnlockAchievement))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool UnlockAchievement_Prefix(bool __runOriginal){
        return __runOriginal && !Joeverhood.AreCheatersCryingAboutIt; // Skip the original method
    }

    [HarmonyPatch(nameof(SteamHelper.IncrementStats))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IncrementStats_Prefix(bool __runOriginal){
        return __runOriginal && !Joeverhood.AreCheatersCryingAboutIt; // Skip the original method
    }

    // Erm what the bobo?
    [HarmonyPatch(nameof(SteamHelper.IncrementJumpRolls))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IncrementJumpRolls_Prefix(bool __runOriginal){
        return __runOriginal && !Joeverhood.AreCheatersCryingAboutIt; // Skip the original method
    }

    [HarmonyPatch(nameof(SteamHelper.UploadScore))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool UploadScore_Prefix(bool __runOriginal){
        return __runOriginal && !Joeverhood.AreCheatersCryingAboutIt; // Skip the original method
    }

    [HarmonyPatch(nameof(SteamHelper.UploadScoreForceUpdate))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool UploadScoreForceUpdate_Prefix(bool __runOriginal){
        return __runOriginal && !Joeverhood.AreCheatersCryingAboutIt; // Skip the original method
    }
}