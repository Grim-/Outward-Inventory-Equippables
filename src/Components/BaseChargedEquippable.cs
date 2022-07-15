using UnityEngine;

namespace InventoryEquippables
{
    public class BaseChargedEquippable : BaseInventoryEquippable
    {
        public float CurrentCharge { get; private set; }
        public virtual float MaximumCharge => 100f;
        public virtual float TickTime => 10f;
        public virtual float EnergyTakenPerTick => 10f;

        private float CurrentTimer = 0;
        public override void Awake()
        {
            base.Awake();

            CurrentCharge = MaximumCharge;
        }



        public override void Update()
        {
            base.Update();
            CurrentTimer += Time.deltaTime;
            if (IsEquipped && CurrentTimer >= TickTime)
            {
                OnTick();
                CurrentTimer = 0;
            }
        }

        public virtual void OnTick()
        {
           
        }

        public void AddEnergy(float amount)
        {
            CurrentCharge += amount;

            if (CurrentCharge >= MaximumCharge)
            {
                CurrentCharge = MaximumCharge;
                //full
                OnEnergyFull();
            }
        }

        public void RemoveEnergy(float amount)
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
        public void SetEnergyLevel(float amount)
        {
            CurrentCharge = 0;
            AddEnergy(amount);
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
    }
}
