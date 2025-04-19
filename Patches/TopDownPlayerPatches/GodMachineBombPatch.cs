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

[HarmonyPatch(typeof(RocaVS.PlayParticleSystem))]
public static class GodMachineBombPatch
{
    [HarmonyPatch(nameof(RocaVS.PlayParticleSystem.Run))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyPrefix]
    public static void Run_Prefix(RocaVS.PlayParticleSystem __instance){
        float time = Joeverhood.godMachineBombScalarDisabled.Value ? 1f : Joeverhood.godMachineBombScalar.Value;
        float scalar = 1f / Mathf.Max(time, float.Epsilon);

        GameObject GO = __instance.thisparticleSystem.gameObject;
        if (!GO.name.StartsWith("FistPound_DevGnomes")) return;

        ParticleSystem? PS_Component = GO.transform.GetChild(0).Find("FistFX")?.GetComponent<ParticleSystem>();
        if (PS_Component == null) return;

        var PS = PS_Component.main;
        
        PS.simulationSpeed = scalar;
        PS.startDelay = (1f - time) * scalar;
    }
}