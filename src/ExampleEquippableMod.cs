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
        public const string GUID = "myname.mymod";
        public const string NAME = "MyMod";
        public const string VERSION = "1.0.0";
        internal static ManualLogSource Log;

        public Dictionary<int, CustomItemDisplayMenuOption> CustomItemOptions { get; private set; }

        public static ExampleEquippableMod Instance { get; private set; }

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            CustomItemOptions = new Dictionary<int, CustomItemDisplayMenuOption>();
            Log = this.Logger;
            Instance = this;
            SL.OnPacksLoaded += SL_OnPacksLoaded;

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.


            RegisterCustomMenuOption(9090, "Equip TEST", "UnEquip TEST", OnCustomActionPress);
            new Harmony(GUID).PatchAll();
        }

        private void SL_OnPacksLoaded()
        {
            //Retrieve the prefab for your custom item 
            SL_Item TestItem = new SL_Item()
            {
                Target_ItemID = 5601001,
                New_ItemID = -19500,
                Name = "Test Equippable",
            };


            TestItem.ApplyTemplate();

           Item TestItemPrefab = ResourcesPrefabManager.Instance.GetItemPrefab(-19500);
           TestItemPrefab.gameObject.AddComponent<YourChargeBasedEquippable>();
        }


        public void RegisterCustomMenuOption(int newCustomID, string EquipString, string UnEquipString, Action<Character, Item, ItemDisplayOptionPanel, int> OnCustomActionPressed)
        {
            if (!CustomItemOptions.ContainsKey(newCustomID))
            {
                CustomItemOptions.Add(newCustomID, new CustomItemDisplayMenuOption(newCustomID, EquipString, UnEquipString, OnCustomActionPressed));
            }
            else
            {
                Log.LogMessage($"Custom ID Already Exists for  {newCustomID} ");
            }
        }

        public void OnCustomActionPress(Character Character, Item CurrentItem, ItemDisplayOptionPanel ItemPanelInstance, int ActionID)
        {
            InventoryEquippableComp equippableComponent = Character.GetComponent<InventoryEquippableComp>();

            //If there's no artifact currently equipped and the artifact component exists
            if (equippableComponent != null && !equippableComponent.HasEquipped)
            {
                //equip the current item in the artifact slot
                equippableComponent.Equip(CurrentItem);
            }
            else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem == equippableComponent.EquippedItem)
            {
                equippableComponent.UnEquip();
            }
            else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem != equippableComponent.EquippedItem)
            {
                //remove
                equippableComponent.UnEquip();

                //add
                equippableComponent.Equip(CurrentItem);
            }
        }

    }
}
