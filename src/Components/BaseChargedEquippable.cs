using UnityEngine;

namespace InventoryEquippables
{
    public class BaseChargedEquippable : InventoryEquippable
    {
        public float CurrentCharge { get; private set; }
        public virtual float MaximumCharge => 100f;
        public virtual float TickTime => 10f;
        public virtual float EnergyTakenPerTick => 10f;

        private float CurrentTimer = 0;


        public override void Awake()
        {
            base.Awake();
            RestoreCharge();
        }

        public override void Update()
        {
            base.Update();

            if (IsEquipped)
            {
                CurrentTimer += Time.deltaTime;
                if (CurrentTimer >= TickTime)
                {
                    OnTick();
                    CurrentTimer = 0;
                }
            }
        }

        public virtual void OnTick()
        {
            if (!IsEquipped) return;
        }

        public void AddCharge(float amount)
        {
            CurrentCharge += amount;

            if (CurrentCharge >= MaximumCharge)
            {
                CurrentCharge = MaximumCharge;
                //full
                OnEnergyFull();
            }
        }

        public void RemoveCharge(float amount)
        {
            CurrentCharge -= amount;

            if (CurrentCharge <= 0)
            {
                CurrentCharge = 0;
                //empty
                OnEnergyEmpty();
            }
        }

        ///generally used if you plan to save the equippable data, in this case the energy level would need restoring to whatever it was at save time
        public void SetChargeLevel(float amount)
        {
            CurrentCharge = 0;
            AddCharge(amount);
        }


        public virtual void OnEnergyFull()
        {

        }

        public virtual void OnEnergyEmpty()
        {

        }

        public bool HasEnoughEnergy(float EnergyRequired)
        {
            return CurrentCharge >= EnergyRequired;
        }

        public void RestoreCharge()
        {
            AddCharge(MaximumCharge);
        }
    }
}
