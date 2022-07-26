using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InventoryEquippables.Patches
{

    [HarmonyPatch(typeof(CharacterInventory), nameof(CharacterInventory.DropItem), new Type[] { typeof(Item), typeof(Transform), typeof(bool) })]
    public class CharacterInventoryDropItem
    {
        static void Postfix(CharacterInventory __instance, Item _item, Transform _newParent = null, bool _playAnim = true)
        {
            InventoryEquippableComp equippableComponent = __instance.m_character.gameObject.GetComponentInParent<InventoryEquippableComp>();

            if (equippableComponent != null && equippableComponent.HasEquipped && equippableComponent.EquippedItem == _item)
            {
                if (_newParent == null)
                {
                    equippableComponent.UnEquip();
                }

            }
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    public class CharacterAwakePatch
    {
        static void Postfix(Character __instance)
        {
            if (__instance.IsLocalPlayer) __instance.gameObject.AddComponent<InventoryEquippableComp>();
        }
    }

    [HarmonyPatch(nameof(StatusEffectManager.AddStatusEffect))]
    public class StatusEffectManagerOnStatusAdded
    {
        static void StatusEffectManager_OnStatusAddeddPrefix(StatusEffectManager __instance, string _statusPrefabName, string[] _splitData)
        {
            InventoryEquippableComp equippableComponent = __instance.m_character.GetComponent<InventoryEquippableComp>();

            if (equippableComponent != null && equippableComponent.HasEquipped)
            {
                StatusEffect statusEffect = ResourcesPrefabManager.Instance.GetStatusEffectPrefab(_statusPrefabName);
                if (statusEffect)
                {
                    equippableComponent.EquippedInventoryEquippable.OnStatusEffectAdded(statusEffect);
                }
            }
        }
    }

    [HarmonyPatch(nameof(StatusEffectManager.RemoveStatus))]
    public class StatusEffectManagerOnStatusRemoved
    {
        static void StatusEffectManager_OnStatusRemovedPrefix(StatusEffectManager __instance, string _uid)
        {
            InventoryEquippableComp equippableComponent = __instance.m_character.GetComponent<InventoryEquippableComp>();

            if (equippableComponent != null && equippableComponent.HasEquipped)
            {
                StatusEffect statusEffect = null;
                if (__instance.m_statuses.TryGetValue(_uid, out statusEffect))
                {
                    if (statusEffect)
                    {
                        equippableComponent.EquippedInventoryEquippable.OnStatusEffectRemoved(statusEffect);
                    }
                }
            }
        }
    }
}
