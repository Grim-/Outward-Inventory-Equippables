using HarmonyLib;
using InventoryEquippables.Interfaces;
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
            CharacterInventoryEquippable equippableComponent = __instance.m_character.gameObject.GetComponentInParent<CharacterInventoryEquippable>();

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
            __instance.gameObject.AddComponent<CharacterInventoryEquippable>();
        }
    }


    #region Status

    [HarmonyPatch(nameof(StatusEffectManager.AddStatusEffect))]
    public class StatusEffectManagerOnStatusAdded
    {
        static void StatusEffectManager_OnStatusAddeddPrefix(StatusEffectManager __instance, string _statusPrefabName, string[] _splitData)
        {
            IEquipmentReactions[] equipmentReactions = __instance.m_character.GetComponentsInChildren<IEquipmentReactions>();

            StatusEffect statusEffect = ResourcesPrefabManager.Instance.GetStatusEffectPrefab(_statusPrefabName);

            if (statusEffect)
            {
                foreach (var equipmentReaction in equipmentReactions)
                {
                    equipmentReaction.OnStatusEffectAdded(statusEffect);
                }
            }
        }
    }

    [HarmonyPatch(nameof(StatusEffectManager.RemoveStatus))]
    public class StatusEffectManagerOnStatusRemoved
    {
        static void StatusEffectManager_OnStatusRemovedPrefix(StatusEffectManager __instance, string _uid)
        {
            IEquipmentReactions[] equipmentReactions = __instance.m_character.GetComponentsInChildren<IEquipmentReactions>();

            StatusEffect statusEffect = null;
            if (__instance.m_statuses.TryGetValue(_uid, out statusEffect))
            {
                if (statusEffect)
                {
                    foreach (var equipmentReaction in equipmentReactions)
                    {
                        equipmentReaction.OnStatusEffectRemoved(statusEffect);
                    }
                }
            }
        }
    }

    #endregion
}
