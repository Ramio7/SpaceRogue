using System;

namespace Scriptables.Health
{
    public sealed class ShieldInfo : IShieldInfo
    {
        public float MaximumShield { get; private set; }
        public float StartingShield { get; private set; }
        public float Cooldown { get; private set; }

        public ShieldInfo(IShieldInfo shieldInfo)
        {
            MaximumShield = shieldInfo.MaximumShield;
            StartingShield = shieldInfo.StartingShield;
            Cooldown = shieldInfo.Cooldown;
        }

        public ShieldInfo(float shieldAmount, float startingShield, float cooldown)
        {
            MaximumShield = shieldAmount;
            StartingShield = startingShield;
            Cooldown = cooldown;
        }

        public void SetShieldAmount(float value)
        {
            StartingShield = Math.Clamp(value, 1, MaximumShield);
        }
    }
}