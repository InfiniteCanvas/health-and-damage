using InfiniteCanvas.HealthDamageSystem.Amplifications;

namespace InfiniteCanvas.HealthDamageSystem.Messages
{
    public readonly struct AddAmplificationToTarget
    {
        public readonly int           TargetID;
        public readonly Amplification Amplification;
    }
}