using InventoryEquippables;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventoryEquippables.Interfaces;

namespace InventoryEquippables
{
    //Nothing is supposed to be made out of the class, but rather create a new class that extends from this one.
    public abstract class InventoryEquippable : MonoBehaviour, IEquipmentReactions
    {
        public Item ParentItem => GetComponent<Item>();
        public Character EquippedCharacter { get; private set; }
        public bool IsEquipped => EquippedCharacter != null ? true : false;

        public bool HasEquipVisual = false;
        public EquipVisualInformation EquipVisualInformation = new EquipVisualInformation("Inventory-Equippable", "firewing", "FirewingVFX", EquipmentSlot.EquipmentSlotIDs.Back);
        private GameObject EquipVisualInstance;
        public Color EquippableBorderColor = Color.cyan;

        public virtual void Awake()
        {

        }

        #region Equippable Methods

        public virtual void OnEquipped(Character Character)
        {
            EquippedCharacter = Character;


            if (EquipVisualInformation != null)
            {
                CreateEquipVisual();
            }


            if (!Helpers.HasButtonHighlight(ParentItem.UID))
            {
                Helpers.CreateButtonHighlight(ParentItem.UID, ParentItem.m_refItemDisplay, EquippableBorderColor, new UnityEngine.Vector2(5, 5));
            }
        }

        public virtual void OnUnEquipped(Character Character)
        {
            if (Helpers.HasButtonHighlight(ParentItem.UID))
            {
                Helpers.DestroyButtonHighlight(ParentItem.UID);
            }

            if (EquipVisualInformation != null)
            {
                DestroyEquipVisual();
            }

            EquippedCharacter = null;
        }

        public virtual void Update()
        {
            if (!IsEquipped) return;
        }


        #endregion



        #region Character Events and Methods

        public virtual void OnStatusEffectAdded(StatusEffect Status)
        {
            if (!IsEquipped) return;
        }

        public virtual void OnStatusEffectRemoved(StatusEffect Status)
        {
            if (!IsEquipped) return;
        }

        #endregion


        private GameObject CreateEquipVisual()
        {
            if (EquipVisualInstance)
            {
                DestroyEquipVisual();
            }

            if (EquipVisualInformation != null)
            {
                GameObject VisualPrefab = Helpers.GetFromAssetBundle<GameObject>(EquipVisualInformation.SLPackName, EquipVisualInformation.AssetBundleName, EquipVisualInformation.PrefabName);

                if (VisualPrefab != null)
                {
                    EquipVisualInstance = GameObject.Instantiate(VisualPrefab, GetEquipmentSlotTransform(EquipVisualInformation.EquipmentSlotID));
                    EquipVisualInstance.transform.localPosition = Vector3.zero;

                    return EquipVisualInstance;
                }           
            }

            return null;
        }


        private void DestroyEquipVisual()
        {
            if (EquipVisualInstance != null)
            {
                Destroy(EquipVisualInstance);
                EquipVisualInstance = null;
            }
        }

        private Transform GetEquipmentSlotTransform(EquipmentSlot.EquipmentSlotIDs EquipmentSlotID)
        {
            if (EquippedCharacter != null)
            {
                return EquippedCharacter.Inventory.Equipment.GetEquipmentVisualSlotTransform(EquipmentSlotID);
            }

            return null;
        }


        public void OnRecieveHit(DamageTakenEvent playerTakeDamage)
        {
            if (!IsEquipped) return;
        }

        public void OnDealtHit(DamageDoneEvent playerDealDamage)
        {
            if (!IsEquipped) return;
        }

        public void OnDeathMessage(Character Character)
        {
            if (!IsEquipped) return;
        }

        public void OnDodgeInput(Vector3 InputDetect)
        {
            if (!IsEquipped) return;
        }

        public void OnUse(Item skill)
        {
            if (!IsEquipped) return;
        }

        public void OnKnock(bool _down)
        {
            if (!IsEquipped) return;
        }

        public bool IsInCharacterPouch(Character character)
        {
           return character.Inventory.Pouch.Contains(ParentItem.UID);
        }

        public bool IsInCharacterBag(Character character)
        {
            List<Item> FoundItems = character.Inventory.GetOwnedItems(_itemID: ParentItem.ItemID);

            foreach (var item in FoundItems)
            {
                if (item.UID == ParentItem.UID)
                {
                    return true;
                }
            }

            return false;
        }

        public void OnCharacterRecievedBlock(MonoBehaviour hitBehaviour, float _damage, Vector3 _hitDir, float _angle, float _angleDir, Character _dealerChar, float _knockBack)
        {
            if (!IsEquipped) return;
        }

        public string GetParentItemUID()
        {
            return ParentItem != null ? ParentItem.UID : String.Empty;
        }
    }
}
