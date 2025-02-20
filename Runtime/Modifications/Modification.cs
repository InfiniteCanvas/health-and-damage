using System;

namespace InfiniteCanvas.HealthDamageSystem.Modifications
{
    public readonly struct Modification : IEquatable<Modification>
    {
        private readonly int _hashCode;

        public bool Equals(Modification other) => _hashCode == other._hashCode;

        public override bool Equals(object obj) => obj is Modification other && Equals(other);

        public override int GetHashCode() => _hashCode;

        public static bool operator ==(Modification left, Modification right) => left.Equals(right);

        public static bool operator !=(Modification left, Modification right) => !left.Equals(right);

        public readonly int   DamageTypeID;
        public readonly float Flat;
        public readonly float Percent;

        public Modification(int damageTypeID, float flat, float percent) : this()
        {
            DamageTypeID = damageTypeID;
            Percent = percent;
            Flat = flat;
            _hashCode = Guid.NewGuid().GetHashCode();
        }

        public Modification(int damageTypeID, float flat, float percent, int hashCode)
        {
            _hashCode = hashCode;
            DamageTypeID = damageTypeID;
            Percent = percent;
            Flat = flat;
        }
    }
}