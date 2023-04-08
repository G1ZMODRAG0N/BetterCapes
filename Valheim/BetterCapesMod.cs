using BepInEx;
using HarmonyLib;
using UnityEngine;
using ItemManager;

//inside the plugin
namespace BetterCapesMod
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInProcess("valheim.exe")]

    //make a public class YourMod : that implements BaseUnityPlugin
    public class BetterCapes : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("g1zmodrag0n.BetterCapes");
        private const string ModName = "BetterCapes";
        private const string ModVersion = "1.0.0";
        private const string ModGUID = "org.bepinex.plugins.bettercapes";

        //initialize mod plugin thru bepinex
        void Awake()
        {
            Item NeckCape = new Item("neckcape", "NeckCape", "Cape");
            NeckCape.Name.English("Neck Cape"); // You can use this to fix the display name in code
            NeckCape.Description.English("Neck cape.");
            NeckCape.Crafting.Add(CraftingTable.Workbench, 3);
            NeckCape.MaximumRequiredStationLevel = 5; // Limits the crafting station level required to upgrade or repair the item to 5
            NeckCape.RequiredItems.Add("Wood", 4);
            NeckCape.RequiredUpgradeItems.Add("Wood", 2);
            NeckCape.CraftAmount = 1;
            NeckCape.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
                                   // The icon for the item will have the same rotation as the item in unity

            ItemManager.PrefabManager.RegisterPrefab("ironfang", "axeVisual"); // If our axe has a special visual effect, like a glow, we can skip adding it to the ObjectDB this way
            ItemManager.PrefabManager.RegisterPrefab("ironfang", "axeSound"); // Same for special sound effects

            //heroShield.DropsFrom.Add("Greydwarf", 0.3f, 1, 2); // A Greydwarf has a 30% chance, to drop 1-2 hero shields.

            harmony.PatchAll(); //also can use harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginId);
        }

        //destroy and unpatch on close
        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        //harmony patch class with HarmonyPatch attribute
        //attribute takes the type of the class we want to patch, and the name of the method we want to patch.
        //Best practice is to use the nameof operator to get the name, rather than a string
        [HarmonyPatch(typeof(Character), nameof(Character.Jump))]
        class Jump_Patch
        {
            static void Postfix(ref float ___m_jumpForce)
            {
                Debug.Log($"Jump force: {___m_jumpForce}");
                ___m_jumpForce = 15;
                Debug.Log($"Modified jump force: {___m_jumpForce}");

            }
        }
    }
}

//Character.m_swimSpeed is a float with default value 2
//
