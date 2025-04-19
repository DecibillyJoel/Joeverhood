using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Joeverhood
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Joeverhood : BaseUnityPlugin
    {
        public const string pluginGuid = "com.github.decibillyjoel.joeverhood";
        public const string pluginName = "Joeverhood";
        public const string pluginVersion = "0.0.1";

        public static ConfigEntry<bool> swapWeaponButtonDisabled = null!;
        public static ConfigEntry<KeyCode> swapWeaponButton = null!;

        public static ConfigEntry<bool> sprintButtonDisabled = null!;
        public static ConfigEntry<KeyCode> sprintButton = null!;

        public static ConfigEntry<bool> godMachineBombScalarDisabled = null!;
        public static ConfigEntry<float> godMachineBombScalar = null!;

        public static bool AreCheatersCryingAboutIt => 
            (swapWeaponButtonDisabled.Value == false && swapWeaponButton.Value != KeyCode.None) ||
            (sprintButtonDisabled.Value == false && sprintButton.Value != KeyCode.None);

        public static ManualLogSource PluginLogger = null!;
        public static readonly Harmony harmony = new(pluginGuid);

        public static void Log(LogLevel logLevel, string logMessage)
        {
            PluginLogger.Log(logLevel, $"{logMessage}");
        }

        public static void Log(string logMessage)
        {
            Log(LogLevel.Info, logMessage);
        }

        private void ValidateMinMaxOrder(ConfigEntry<int> minEntry, ConfigEntry<int> maxEntry) 
        {
            if (minEntry.Value > maxEntry.Value) {
                Log(LogLevel.Warning, $"|{minEntry.Definition.Key}| is greater than |{maxEntry.Definition.Key}! Swapping values...");
                (minEntry.Value, maxEntry.Value) = (maxEntry.Value, minEntry.Value);
            }
        }

        private void ValidateConfigAndApplyPatches()
        {
            Log(LogLevel.Debug, "Validating config...");

            Config.SettingChanged -= ScheduleValidateConfigAndApplyPatches;

            // Validation goes here
            
            Config.SettingChanged += ScheduleValidateConfigAndApplyPatches;

            Log(LogLevel.Debug, "Unpatching...");
            harmony.UnpatchSelf();

            Log(LogLevel.Debug, "Patching...");
            try
            {
                harmony.PatchAll(typeof(Joeverhood).Assembly);
            } catch (Exception e) {
                Log(LogLevel.Error, $"Error encountered while attempting to patch:\n{e}");

                harmony.UnpatchSelf();
            }

            Log(LogLevel.Debug, "Finished config validation and patching!");
        }

        private void ScheduleValidateConfigAndApplyPatches(object? eventSender = null, SettingChangedEventArgs? eventArgs = null)
        {
            Log(LogLevel.Debug, "Scheduling config validation...");

            string validationMethodName = nameof(ValidateConfigAndApplyPatches);
            CancelInvoke(validationMethodName);
            Invoke(validationMethodName, 0.33f);
        }
        private void Awake()
        {
            PluginLogger = Logger;
            Log("Joeverhood loading...");

            swapWeaponButtonDisabled = Config.Bind("Swap Weapon", "Disable Swap Weapon Modification", false, new ConfigDescription("Disables the swap weapon code modification."));
            swapWeaponButton = Config.Bind("Swap Weapon", "Swap Weapon Button", KeyCode.LeftShift, new ConfigDescription("The key to swap to your previously held weapon. Default: Left Shift"));

            sprintButtonDisabled = Config.Bind("Overworld Shmovement", "Disable Overworld Shmovement Modification", false, new ConfigDescription("Disables the overworld shmovement code modification."));
            sprintButton = Config.Bind("Overworld Shmovement", "Sprint Button", KeyCode.LeftShift, new ConfigDescription("The key to sprint. Default: Left Shift"));

            godMachineBombScalarDisabled = Config.Bind("God Machine", "Disable God Machine Modification", false, new ConfigDescription("Disables the God Machine code modification."));
            godMachineBombScalar = Config.Bind("God Machine", "God Machine Bomb Screen Time Scalar", 0.25f, new ConfigDescription("By how much to scale the amount of time for which God Machine's bombs linger on screen. e.g. 0.25s means the bomb itself (not the indicator, which is unchanged) will only cover the screen for 0.25 seconds. Default: 0.25", new AcceptableValueRange<float>(0f, 1f)));

            ValidateConfigAndApplyPatches();

            Log("Joeverhood loaded successfully!");
        }
    }
}
