namespace InventoryEquippables
{

    //this example grants the player the cool boon on every tick and reduces the energy by EnergyTakenPerTick
    public class YourChargeBasedEquippable : BaseChargedEquippable
    {
        public float ManaCostToRecharge = 20f;

        public override float TickTime => 10f;

        public override void OnEquip(Character CharacterToEquip)
        {
            base.OnEquip(CharacterToEquip);

            //reapply buff on equip if doesnt have, subtract a ticks worth of energy (or not)
            if (HasEnoughEnergy(EnergyTakenPerTick))
            {
                if (!CharacterToEquip.StatusEffectMngr.HasStatusEffect("Cool"))
                {
                    CharacterToEquip.StatusEffectMngr.AddStatusEffect("Cool");
                    RemoveEnergy(EnergyTakenPerTick);
                }
            }
        }

        //do soemthing every tick
        public override void OnTick()
        {
            base.OnTick();

            ApplyBoon(EquippedCharacter);
            EquippedCharacter.CharacterUI.ShowInfoNotification($"Ticking {transform.name}. Charge Level {CurrentCharge}");
        }

        //do something when out of charge
        public override void OnEnergyEmpty()
        {
            base.OnEnergyEmpty();


            //now out of energy, drain Equipped Character mana to regen


            if (EquippedCharacter.Stats.m_mana >= ManaCostToRecharge)
            {
                EquippedCharacter.Stats.UseMana(null, ManaCostToRecharge);
                AddEnergy(MaximumCharge);
                EquippedCharacter.CharacterUI.ShowInfoNotification($"Drained {ManaCostToRecharge} Mana. Charge Level {CurrentCharge}");
            }
        }

        //do something when fully charged
        public override void OnEnergyFull()
        {
            base.OnEnergyFull();
        }


        private void ApplyBoon(Character Character)
        {
            if (HasEnoughEnergy(EnergyTakenPerTick))
            {
                Character.StatusEffectMngr.AddStatusEffect("Cool");
                RemoveEnergy(EnergyTakenPerTick);
                EquippedCharacter.CharacterUI.ShowInfoNotification($"Removed {EnergyTakenPerTick} Energy. Charge Level {CurrentCharge}");
            }
        }
    }
}
