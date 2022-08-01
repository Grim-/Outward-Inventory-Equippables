using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InventoryEquippables.Interfaces
{
    //turn them into an interface
    public interface IEquipmentReactions
    {
        public string GetParentItemUID();
        public void OnEquipped(Character Character);
        public void OnUnEquipped(Character Character);
        public void OnRecieveHit(DamageTakenEvent playerTakeDamage);
        public void OnDealtHit(DamageDoneEvent playerDealDamage);
        public void OnStatusEffectAdded(StatusEffect Status);
        public void OnStatusEffectRemoved(StatusEffect Status);
        public void OnDeathMessage(Character Character);
        public void OnDodgeInput(Vector3 InputDetect);
        public void OnUse(Item skill);
        public void OnKnock(bool _down);
        public bool IsInCharacterPouch(Character character);
        public bool IsInCharacterBag(Character character);
        public void OnCharacterRecievedBlock(MonoBehaviour hitBehaviour, float _damage, Vector3 _hitDir, float _angle, float _angleDir, Character _dealerChar, float _knockBack);
    }
}
