using InfiniteCanvas.HealthDamageSystem.Resistances;

namespace InfiniteCanvas.HealthDamageSystem.Messages
{
    public readonly struct AddResistanceToTarget
    {
        public readonly int        TargetID;
        public readonly Resistance Resistance;
    }
}