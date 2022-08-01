using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InventoryEquippables
{
    //Character component that holds reference to active Equippable
    public class CharacterInventoryEquippable : MonoBehaviour
    {
        public Item EquippedItem { get; private set; }
        public InventoryEquippable EquippedInventoryEquippable => EquippedItem.GetComponent<InventoryEquippable>();
        public Character ParentCharacter => gameObject.GetComponent<Character>();
        public bool HasEquipped => EquippedItem != null ? true : false;

        public void Equip(Item ItemEngine)
        {
            EquippedItem = ItemEngine;

            if (EquippedItem != null && EquippedInventoryEquippable != null)
            {
                EquippedInventoryEquippable.OnEquipped(ParentCharacter);
                //ParentCharacter.CharacterUI.ShowInfoNotification("Equipped");
            }


            if (!Helpers.HasButtonHighlight(EquippedItem.UID))
            {
                Helpers.CreateButtonHighlight(EquippedItem.UID, EquippedItem.m_refItemDisplay, EquippedInventoryEquippable.EquippableBorderColor, new Vector2(5, 5), EquippedItem.m_refItemDisplay.transform);
            }
        }
        
        public void UnEquip()
        {
            if (EquippedItem != null && EquippedInventoryEquippable != null)
            {
                EquippedInventoryEquippable.OnUnEquipped(ParentCharacter);
                //ParentCharacter.CharacterUI.ShowInfoNotification("UnEquipped");
            }

            if (Helpers.HasButtonHighlight(EquippedItem.UID))
            {
                Helpers.DestroyButtonHighlight(EquippedItem.UID);
            }

            EquippedItem = null;
        }
    }
}
