namespace InventoryEquippables
{

    //this example grants the player the cool boon on every tick and reduces the energy by EnergyTakenPerTick
    public class ChargedEquippable : BaseChargedEquippable
    {
        public float ManaCostToRecharge = 20f;
        public override float TickTime => 10f;

        public string BoonIdentifier = "Rage";
        public override void OnEquipped(Character Character)
        {
            base.OnEquipped(Character);
            ApplyBoon(Character);
        }

        public override void OnUnEquipped(Character Character)
        {
            RemoveBoon(Character);
            base.OnUnEquipped(Character);
        }

        //do soemthing every tick
        public override void OnTick()
        {
            base.OnTick();
            EquippedCharacter.CharacterUI.ShowInfoNotification($"Ticking {transform.name}. Charge Level {CurrentCharge}");

            ApplyBoon(EquippedCharacter);
        }

        //do something when out of charge
        public override void OnEnergyEmpty()
        {
            base.OnEnergyEmpty();
            ReChargeWithMana();
        }


        private void ReChargeWithMana()
        {
            //now out of energy, drain Equipped Character mana to regen
            if (EquippedCharacter.Stats.m_mana >= ManaCostToRecharge)
            {
                EquippedCharacter.Stats.UseMana(null, ManaCostToRecharge);
                AddCharge(MaximumCharge);
                EquippedCharacter.CharacterUI.ShowInfoNotification($"Drained {ManaCostToRecharge} Mana. Charge Level {CurrentCharge}");
            }
        }

        private void ApplyBoon(Character Character)
        {
            if (HasEnoughEnergy(EnergyTakenPerTick))
            {
                if (!Character.StatusEffectMngr.HasStatusEffect(BoonIdentifier))
                {
                    RemoveCharge(EnergyTakenPerTick);
                    Character.StatusEffectMngr.AddStatusEffect(BoonIdentifier);
                    EquippedCharacter.CharacterUI.ShowInfoNotification($"Removed {EnergyTakenPerTick} Energy. Charge Level {CurrentCharge}");
                }
            }
        }

        private void RemoveBoon(Character Character)
        {
            if (Character.StatusEffectMngr.HasStatusEffect(BoonIdentifier))
            {
                Character.StatusEffectMngr.RemoveStatusWithIdentifierName(BoonIdentifier);
                EquippedCharacter.CharacterUI.ShowInfoNotification($"Removing boon.");
            }
        }

        public override void OnStatusEffectAdded(StatusEffect Status)
        {
            base.OnStatusEffectAdded(Status);


            EquippedCharacter.CharacterUI.ShowInfoNotification($"Status Added : {Status.IdentifierName}");
        }

        public override void OnStatusEffectRemoved(StatusEffect Status)
        {
            base.OnStatusEffectRemoved(Status);

            EquippedCharacter.CharacterUI.ShowInfoNotification($"Status Removed : {Status.IdentifierName}");
        }
    }
}
