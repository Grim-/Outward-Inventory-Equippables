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
        //public static int CustomEquipActionID = 90909;

        //public static string Equip_String = "Activate ";
        //public static string UnEquip_String = "DeActivate";
        //public static Color EquippableBorderColor = Color.cyan;

        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions)), HarmonyPostfix]
        private static void EquipmentMenu_GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {
            //InventoryEquippableComp equippableComponent = __instance.LocalCharacter.GetComponent<InventoryEquippableComp>();
            //Item CurrentItem = __instance.m_pendingItem;
            //BaseInventoryEquippable baseInventoryEquippable = CurrentItem.GetComponent<BaseInventoryEquippable>();
            //if (equippableComponent != null && baseInventoryEquippable != null)
            //{
            //    __result.Add(CustomEquipActionID);
            //}
            foreach (var current in ExampleEquippableMod.Instance.CustomItemOptions.Keys)
            {

                if (!__result.Contains(current))
                {
                    __result.Add(current);
                }
            }         
        }



        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed)), HarmonyPrefix]
        private static void EquipmentMenu_ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            Character owner = __instance.m_characterUI.TargetCharacter;
            Item CurrentItem = __instance.m_pendingItem;
            foreach (var CustomAction in ExampleEquippableMod.Instance.CustomItemOptions)
            {
                if (_actionID == CustomAction.Key)
                {
                    CustomAction.Value?.OnCustomActionPressed(owner, CurrentItem, __instance, _actionID);
                }
            }

            //if (_actionID == CustomEquipActionID)
            //{
            //    Character owner = __instance.m_characterUI.TargetCharacter;
            //    InventoryEquippableComp equippableComponent = owner.GetComponent<InventoryEquippableComp>();
            //    Item CurrentItem = __instance.m_pendingItem;

            //    //If there's no artifact currently equipped and the artifact component exists
            //    if (equippableComponent != null && !equippableComponent.HasEquipped)
            //    {
            //        //equip the current item in the artifact slot
            //        equippableComponent.Equip(CurrentItem);
            //    }
            //    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem == equippableComponent.EquippedItem)
            //    {
            //        equippableComponent.UnEquip();
            //    }
            //    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem != equippableComponent.EquippedItem)
            //    {
            //        //remove
            //        equippableComponent.UnEquip();

            //        //add
            //        equippableComponent.Equip(CurrentItem);
            //    }
            //}
        }


        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText)), HarmonyPrefix]
        private static bool EquipmentMenu_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            foreach (var CustomAction in ExampleEquippableMod.Instance.CustomItemOptions)
            {
                if (_actionID == CustomAction.Key)
                {
                    Character owner = __instance.m_characterUI.TargetCharacter;
                    InventoryEquippableComp equippableComponent = owner.GetComponent<InventoryEquippableComp>();
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

                    return false;
                }
                else continue;           
            }

            //if (_actionID == CustomEquipActionID)
            //{
            //    Character owner = __instance.m_characterUI.TargetCharacter;
            //    InventoryEquippableComp equippableComponent = owner.GetComponent<InventoryEquippableComp>();
            //    Item CurrentItem = __instance.m_pendingItem;

            //    if (equippableComponent != null && !equippableComponent.HasEquipped)
            //    {
            //        __result = Equip_String;

            //    }
            //    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem == equippableComponent.EquippedItem)
            //    {
            //        __result = UnEquip_String;
            //    }
            //    else if (equippableComponent != null && equippableComponent.HasEquipped && CurrentItem != equippableComponent.EquippedItem)
            //    {
            //        __result = $"Switch";
            //    }

            //    return false;
            //}

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
            InventoryEquippableComp equippableComponent = __instance.OwnerCharacter.gameObject.GetComponentInParent<InventoryEquippableComp>();

            if (equippableComponent != null && equippableComponent.HasEquipped && equippableComponent.EquippedItem == _itemToRemove)
            {
                equippableComponent.UnEquip();
            }
        }
    }

    //[HarmonyPatch(typeof(ItemContainer), nameof(ItemContainer.AddItem), new Type[] { typeof(Item)})]
    //public class ItemContainerAddItem
    //{
    //    public static Color EquippableBorderColor = Color.cyan;
    //    static void Prefix(ItemContainer __instance, Item _item)
    //    {
    //        InventoryEquippableComp equippableComponent = __instance.OwnerCharacter.gameObject.GetComponentInParent<InventoryEquippableComp>();

    //        if (equippableComponent != null && equippableComponent.HasEquipped && equippableComponent.EquippedItem == _item)
    //        {
    //            if (!Helpers.HasButtonHighLight(equippableComponent.EquippedItem.UID))
    //            {
    //                Helpers.CreateButtonHighlight(_item.UID, _item.m_refItemDisplay, equippableComponent.EquippedInventoryEquippable.EquippableBorderColor, new Vector2(5, 5), _item.m_refItemDisplay.transform);
    //            }
    //        }
    //    }
    //}



}
