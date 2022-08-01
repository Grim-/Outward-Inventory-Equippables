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
        public bool ActionsAdded = false;
        public static Dictionary<int, CustomItemDisplayMenuOption> CustomItemOptions { get; private set; }

        public static ExampleEquippableMod Instance { get; private set; }

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            CustomItemOptions = new Dictionary<int, CustomItemDisplayMenuOption>();
            Log = this.Logger;
            Instance = this;
            SL.OnPacksLoaded += SL_OnPacksLoaded;

            RegisterCustomMenuOption(90190, "Equip TEST", "UnEquip TEST", ToggleEquipAction);
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
           TestItemPrefab.gameObject.AddComponent<ChargedEquippable>();
        }


        public void RegisterCustomMenuOption(int newCustomID, string EquipString, string UnEquipString, Action<Character, Item, ItemDisplayOptionPanel, int> OnCustomActionPressed)
        {
            if (!CustomItemOptions.ContainsKey(newCustomID))
            {
                CustomItemOptions.Add(newCustomID, new CustomItemDisplayMenuOption(newCustomID, EquipString, UnEquipString, OnCustomActionPressed, ShouldAddCustomAction));
            }
            else
            {
                Log.LogMessage($"Custom ID Already Exists for  {newCustomID} ");
            }
        }

        private bool ShouldAddCustomAction(Character Character, Item CurrentItem, ItemDisplayOptionPanel ItemPanelInstance, int ActionID)
        {
            CharacterInventoryEquippable equippableComponent = Character.GetComponent<CharacterInventoryEquippable>();

            if (equippableComponent)
            {
                return true;
            }

            return false;
        }

        public void ToggleEquipAction(Character Character, Item CurrentItem, ItemDisplayOptionPanel ItemPanelInstance, int ActionID)
        {
            CharacterInventoryEquippable equippableComponent = Character.GetComponent<CharacterInventoryEquippable>();

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
