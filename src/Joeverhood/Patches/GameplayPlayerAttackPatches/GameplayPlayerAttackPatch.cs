using UnityEngine;
using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ILUtils;
using ILUtils.HarmonyXtensions;

using LogLevel = BepInEx.Logging.LogLevel;
using Object = UnityEngine.Object;
using OpCodes = System.Reflection.Emit.OpCodes;
using Random = System.Random;
using BepInEx;

namespace Joeverhood.Patches.GameplayPlayerAttackPatches;

[HarmonyPatch(typeof(GameplayPlayerAttack))]
public static class GameplayPlayerAttackPatch
{
    public static GameplayPlayerWeapon? lastWeapon;
    public static GameplayPlayerWeapon? currentWeapon;
    public static bool swapButtonReleased = true;

    [HarmonyPatch(nameof(GameplayPlayerAttack.Awake))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static void Awake_Prefix(GameplayPlayerAttack __instance){
        // Reset everything
        lastWeapon = null;
        currentWeapon = null;
        swapButtonReleased = true;
    }

    [HarmonyPatch(nameof(GameplayPlayerAttack.SwitchWeapons))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static void SwitchWeapons_Prefix(GameplayPlayerAttack __instance){
        // Track weapon change
        if (currentWeapon != __instance.CurrentWeaponHolding)
        {
            lastWeapon = currentWeapon ?? __instance.CurrentWeaponHolding;
            currentWeapon = __instance.CurrentWeaponHolding;
        }

        // Set release flag to true if button is disabled / non-existent / released
        if (Joeverhood.swapWeaponButtonDisabled.Value == true ||
            Joeverhood.swapWeaponButton.Value == KeyCode.None || 
            !UnityInput.Current.GetKeyDown(Joeverhood.swapWeaponButton.Value)
        ) 
        {
            swapButtonReleased = true;
            return;
        }

        // Early return if button was not released
        if (!swapButtonReleased) return;

        // Swap to previous weapon
        swapButtonReleased = false;
        __instance.UpdateNewWeapon(lastWeapon);
    }
}