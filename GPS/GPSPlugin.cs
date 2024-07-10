using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EquinoxsModUtils;
using HarmonyLib;
using UnityEngine;

namespace GPS
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class GPSPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.GPS";
        private const string PluginName = "GPS";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        public static ConfigEntry<float> yOffset;
        public static ConfigEntry<float> xOffset;

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            xOffset = Config.Bind("General", "X Offset", 70f, new ConfigDescription("Controls the horizontal position of the GUI. Increase until it's centered with your compass."));
            yOffset = Config.Bind("General", "Y Offset", 250f, new ConfigDescription("Controls the vertical position of the GUI. Increase until it sits below your compass."));

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void OnGUI() {
            if (!ModUtils.hasGameLoaded) return;

            if (Player.instance == null) return;
            if (Player.instance.fpcontroller == null) return;

            Vector3 playerPosition = Player.instance.fpcontroller.transform.position;
            string x = playerPosition.x.ToString("#").PadLeft(5);
            string y = playerPosition.y.ToString("#").PadLeft(5);
            string z = playerPosition.z.ToString("#").PadLeft(5);
            string gps = $"{x}, {y}, {z}";
            GUIContent content = new GUIContent(gps);

            GUIStyle labelStyle = new GUIStyle() {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white, background = null }
            };
            Vector2 size = labelStyle.CalcSize(content);

            GUI.Label(new Rect(Screen.width - size.x - xOffset.Value, yOffset.Value, size.x, size.y), content, labelStyle);
        }
    }
}
