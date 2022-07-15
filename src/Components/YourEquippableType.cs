using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryEquippables
{
    //You extend from BaseInventoryEquippable and override the methods to do *stuff* on equip, what this is, is upto you!
    public class YourEquippableType : BaseInventoryEquippable
    {
        public override void OnEquip(Character CharacterToEquip)
        {
            base.OnEquip(CharacterToEquip);
        }

        public override void OnUnEquip(Character CharacterToEquip)
        {
            base.OnUnEquip(CharacterToEquip);
        }
    }
}
