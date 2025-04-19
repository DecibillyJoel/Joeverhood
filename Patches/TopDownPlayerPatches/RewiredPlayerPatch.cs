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

namespace Joeverhood.Patches.TopDownPlayerPatches;

[HarmonyPatch(typeof(Rewired.Player))]
public static class RewiredPlayerPatch
{
    public static bool IsHoldingSprint => Joeverhood.sprintButtonDisabled.Value == false && 
                                          Joeverhood.sprintButton.Value != KeyCode.None && 
                                          UnityInput.Current.GetKey(Joeverhood.sprintButton.Value);

    [HarmonyPatch(nameof(Rewired.Player.GetButtonDoublePressHold), typeof(string))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static bool GetButtonDoublePressHold_Prefix(Rewired.Player __instance, ref bool __result, string actionName){
        if (!IsHoldingSprint) return true;

        __result = __instance.GetButton(actionName);
        return false;
    }

    [HarmonyPatch(nameof(Rewired.Player.GetNegativeButtonDoublePressHold), typeof(string))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static bool GetNegativeButtonDoublePressHold_Prefix(Rewired.Player __instance, ref bool __result, string actionName){
        if (!IsHoldingSprint) return true;

        __result = __instance.GetNegativeButton(actionName);
        return false;
    }
}