using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Joeverhood
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Joeverhood : BaseUnityPlugin
    {
        public const string pluginGuid = "com.github.decibillyjoel.joeverhood";
        public const string pluginName = "Joeverhood (by DBJ)";
        public const string pluginVersion = "0.0.1";

        public static Harmony harmony = new Harmony(pluginGuid);

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        public void OnDestroy()
        {
            Logger.LogInfo("Unpatching all methods...");
            harmony.UnpatchSelf();
        }
    }
}
