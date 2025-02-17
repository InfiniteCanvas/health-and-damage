using System;

namespace InfiniteCanvas.HealthDamageSystem.Amplifications
{
    public readonly struct Amplification : IEquatable<Amplification>
    {
        private readonly int   _hashCode;
        public readonly  int   DamageTypeID;
        public readonly  float Percentage;
        public readonly  float Flat;

        public bool Equals(Amplification other) => _hashCode == other._hashCode;

        public override bool Equals(object obj) => obj is Amplification other && Equals(other);

        public override int GetHashCode() => _hashCode;

        public static bool operator ==(Amplification left, Amplification right) => left.Equals(right);

        public static bool operator !=(Amplification left, Amplification right) => !left.Equals(right);

        public Amplification(int damageTypeID, float percentage, float flat) : this()
        {
            DamageTypeID = damageTypeID;
            Percentage = percentage;
            Flat = flat;
            _hashCode = Guid.NewGuid().GetHashCode();
        }

        public Amplification(int damageTypeID, float percentage, float flat, int hashCode)
        {
            _hashCode = hashCode;
            DamageTypeID = damageTypeID;
            Percentage = percentage;
            Flat = flat;
        }
    }
}