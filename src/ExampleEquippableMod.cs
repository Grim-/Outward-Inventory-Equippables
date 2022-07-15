using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryEquippables
{

    [BepInPlugin(GUID, NAME, VERSION)]
    public class ExampleEquippableMod : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "myname.mymod";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "MyMod";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.0.0";
        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource Log;

        public static int DefaultEngine = -9000;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;

            SL.OnPacksLoaded += SL_OnPacksLoaded;

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();
        }

        private void SL_OnPacksLoaded()
        {
            //Retrieve the prefab for your custom item
            Item Engine = ResourcesPrefabManager.Instance.GetItemPrefab(DefaultEngine);
            //add your specific equippable component to it
            Engine.gameObject.AddComponent<YourChargeBasedEquippable>();
        }

        public enum PrototypeItemEngineIDS
        {
            BaseEngine = -9000
        }

    }
}
