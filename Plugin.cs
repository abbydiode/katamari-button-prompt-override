using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using MyGame;
using MyGame.InputStatus;

namespace ButtonPromptOverride;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    public static ManualLogSource logger;
    public static ConfigEntry<IconType> iconType;
    public static ConfigEntry<bool> overrideEnabled;
    private void Awake() {
        logger = Logger;
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} loaded!");
        
        overrideEnabled = Config.Bind("", "Enabled", true, "Whether to override the button prompts");
        iconType = Config.Bind("", "IconType", IconType.PS4, "Which button prompts to force the game to use");

        Logger.LogInfo(overrideEnabled.Value ? $"Button prompts overriden to {iconType.Value}" : "Button prompt override disabled");
        
        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
    }
}

[HarmonyPatch(typeof(InputPadRewired), "IconType", MethodType.Getter)]
public class InputPadRewired_IconType {
    public static IconType Postfix(IconType __result) => Plugin.overrideEnabled.Value ? Plugin.iconType.Value : __result;
}
