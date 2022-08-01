using HarmonyLib;
using NodeCanvas.DialogueTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryEquippables
{
    [HarmonyPatch(typeof(ItemDisplayOptionPanel))]
    public static class ItemDisplayOptionPanelPatches
    {
        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions)), HarmonyPostfix]
        private static void EquipmentMenu_GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {        
            foreach (var current in ExampleEquippableMod.CustomItemOptions)
            {
                if (!__result.Contains(current.Key) && current.Value.ShouldAddActionDelegate(__instance.LocalCharacter, __instance.m_pendingItem, __instance, current.Key))
                {
                    //ExampleEquippableMod.Log.LogMessage($"Adding Action {current.Key}");
                    __result.Add(current.Key);
                }
            }
        }



        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed)), HarmonyPrefix]
        private static void EquipmentMenu_ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            Character owner = __instance.m_characterUI.TargetCharacter;
            Item CurrentItem = __instance.m_pendingItem;
            foreach (var CustomAction in ExampleEquippableMod.CustomItemOptions)
            {
                if (_actionID == CustomAction.Key)
                {
                    //ExampleEquippableMod.Log.LogMessage($"Calling Action {CustomAction.Key}");
                    CustomAction.Value?.OnCustomActionPressed(owner, CurrentItem, __instance, _actionID);
                }
            }
        }


        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText)), HarmonyPrefix]
        private static bool EquipmentMenu_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            //ExampleEquippableMod.Log.LogMessage($"Getting Action Text {_actionID}");
            foreach (var CustomAction in ExampleEquippableMod.CustomItemOptions)
            {
                //ExampleEquippableMod.Log.LogMessage($"Getting Custom Action Text {CustomAction.Key}");
                if (_actionID == CustomAction.Key)
                {
                    Character owner = __instance.m_characterUI.TargetCharacter;
                    CharacterInventoryEquippable equippableComponent = owner.GetComponent<CharacterInventoryEquippable>();
                    Item CurrentItem = __instance.m_pendingItem;

                    if (equippableComponent != null && !equippableComponent.HasEquipped)
                    {
                        __result = CustomAction.Value.Equip_String;
                    }
                    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem == equippableComponent.EquippedItem)
                    {
                        __result = CustomAction.Value.UnEquip_String;
                    }
                    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem != equippableComponent.EquippedItem)
                    {
                        __result = $"Switch";
                    }

                    //ExampleEquippableMod.Log.LogMessage($"Setting Action String{CustomAction.Key}");
                    return false;
                }          
            }

            return true;
        }
    }


    //This patch unequips it no matter where its removed from, so even between bags, safest way to keep it stable for now.
    [HarmonyPatch(typeof(ItemContainer))]
    public class ItemContainerRemoveItem
    {
        [HarmonyPatch(nameof(ItemContainer.RemoveItem)), HarmonyPrefix]
        static void ItemContainerRemoveItem_Prefix(ItemContainer __instance, Item _itemToRemove)
        {
            CharacterInventoryEquippable equippableComponent = __instance.OwnerCharacter.gameObject.GetComponentInParent<CharacterInventoryEquippable>();

            if (equippableComponent != null && equippableComponent.HasEquipped && equippableComponent.EquippedItem == _itemToRemove)
            {
                equippableComponent.UnEquip();
            }
        }
    }
}
