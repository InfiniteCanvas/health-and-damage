using System;

namespace InfiniteCanvas.HealthDamageSystem.Resistances
{
    public readonly struct Resistance : IEquatable<Resistance>
    {
        public bool Equals(Resistance other) => _hashCode == other._hashCode;

        public override bool Equals(object obj) => obj is Resistance other && Equals(other);

        public override int GetHashCode() => _hashCode;

        public static bool operator ==(Resistance left, Resistance right) => left.Equals(right);

        public static bool operator !=(Resistance left, Resistance right) => !left.Equals(right);

        private readonly int   _hashCode;
        public readonly  int   DamageTypeID;
        public readonly  float Percentage;
        public readonly  float Flat;

        public Resistance(int damageTypeID, float percentage, float flat)
        {
            DamageTypeID = damageTypeID;
            Percentage = percentage;
            Flat = flat;
            _hashCode = Guid.NewGuid().GetHashCode();
        }

        public Resistance(int damageTypeID, float percentage, float flat, int hashCode)
        {
            _hashCode = hashCode;
            DamageTypeID = damageTypeID;
            Percentage = percentage;
            Flat = flat;
        }
    }
}