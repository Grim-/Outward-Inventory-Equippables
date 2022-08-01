using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryEquippables
{
    //You extend from BaseInventoryEquippable and override the methods to do *stuff* on equip, what this is, is upto you!
    public class YourEquippableType : InventoryEquippable
    {
        public override void OnEquipped(Character CharacterToEquip)
        {
            base.OnEquipped(CharacterToEquip);
        }

        public override void OnUnEquipped(Character CharacterToEquip)
        {
            base.OnUnEquipped(CharacterToEquip);
        }
    }
}
