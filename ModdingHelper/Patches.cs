using HarmonyLib;
using ModdingHelper.UnityEditor;
using UnityEngine;

namespace ModdingHelper
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(GameCanvas), "Awake")]
        [HarmonyPostfix]
        private static void Postfix(GameCanvas __instance)
        {
            Plugin.mls.LogInfo("Setting up Unity Editor on Canvas parent...");

            // Show Player
            CustomCameraController controller = Camera.main.GetComponent<CustomCameraController>();
            AccessTools.Method(typeof(CustomCameraController), "ShowCharacter", new[] { typeof(bool) })?.Invoke(controller, new object[] { true });

            __instance.gameObject.AddComponent<CustomEditorManager>();
        }

        //private static bool enabledHelper = false;

        //[HarmonyPatch(typeof(PlayerNetwork), "Update")]
        //[HarmonyPostfix]
        //private static void Postfix()
        //{
        //    if (!enabledHelper && Input.GetKeyDown(KeyCode.F8)) ApplyModHelper();
        //}

        //private static void ApplyModHelper()
        //{
        //    Plugin.mls.LogInfo("Applying Mod Helper...");

        //    enabledHelper = true;

        //    // Show Player
        //    CustomCameraController controller = Camera.main.GetComponent<CustomCameraController>();
        //    AccessTools.Method(typeof(CustomCameraController), "ShowCharacter", new[] { typeof(bool) })?.Invoke(controller, new object[] { true });

        //    // Disable MonoBehaviours
        //    foreach (MonoBehaviour script in Camera.main.GetComponents<MonoBehaviour>())
        //    {
        //        if (script != null && !(script is Camera)) script.enabled = false;
        //    }

        //    // Apply Mod Helper Behaviour
        //    Camera.main.gameObject.AddComponent<ModManagerBehaviour>();
        //}
    }
}
