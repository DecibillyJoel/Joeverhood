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
using Mono.Cecil.Cil;

namespace Joeverhood.Patches.CheatersCryAboutItPatches;

[HarmonyPatch(typeof(GameplayPointSystem))]
public static class GameplayPointSystemPatch
{
    [HarmonyPatch(nameof(GameplayPointSystem.CalculateScoreAndUpload))]
    [HarmonyPriority(priority: int.MinValue)]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> CalculateScoreAndUpload_Transpiler(IEnumerable<CodeInstruction> methodIL, ILGenerator methodGenerator, MethodBase methodBase){
        if (!Joeverhood.AreCheatersCryingAboutIt) return methodIL;
        
        ILStepper stepper = new(methodIL, methodGenerator, methodBase);

        if (stepper.TryGotoIL(code => code.LoadsProperty(type: typeof(SteamManager), name: "Initialized")) != null)
            stepper.TryRemoveIL(endIndex: stepper.CurrentIndex + 2);
        stepper.GotoIndex(0);

        if (stepper.TryGotoIL(code => code.Calls(type: typeof(Steamworks.SteamUser), name: "BLoggedOn")) != null)
            stepper.TryRemoveIL(endIndex: stepper.CurrentIndex + 2);
        stepper.GotoIndex(0);

        return stepper.Instructions;
    }
}