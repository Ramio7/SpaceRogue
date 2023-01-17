namespace Scriptables.Health
{
    public interface IHealthInfo
    {
        float MaximumHealth { get; }
        float StartingHealth { get; }
        float HealthRegen { get; }
        float DamageImmunityFrameDuration { get; }
    }
}