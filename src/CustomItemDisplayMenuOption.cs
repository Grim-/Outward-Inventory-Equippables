using System;

namespace InventoryEquippables
{
    public class CustomItemDisplayMenuOption
    {
        public int CustomEquipActionID = 10000;
        public string Equip_String = "Activate ";
        public string UnEquip_String = "De-Activate";

        public Action<Character, Item, ItemDisplayOptionPanel, int> OnCustomActionPressed;

        public CustomItemDisplayMenuOption(int customEquipActionID, string equip_String, string unEquip_String, Action<Character, Item, ItemDisplayOptionPanel, int> onCustomActionPressed)
        {
            CustomEquipActionID = customEquipActionID;
            Equip_String = equip_String;
            UnEquip_String = unEquip_String;
            OnCustomActionPressed = onCustomActionPressed;
        }
    }
}
