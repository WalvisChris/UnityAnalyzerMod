using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
namespace ModdingHelper
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.kroes.moddinghelper";
        private const string NAME = "Modding Helper";
        private const string VERSION = "1.0.0";
        internal static ManualLogSource mls;
        internal static Plugin instance;
        internal static Harmony harmony = new Harmony(GUID);

        private void Awake()
        {
            if (instance == null) instance = this;
            harmony.PatchAll(typeof(Patches));
            mls = Logger;
            mls.LogInfo("Loaded succesfully!");
        }
    }
}
