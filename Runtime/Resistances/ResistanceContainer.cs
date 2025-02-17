using System;
using InfiniteCanvas.HealthDamageSystem.Damage;
using UnityEngine;

namespace InfiniteCanvas.HealthDamageSystem.Resistances
{
    [CreateAssetMenu(fileName = "Resistance", menuName = "Infinite Canvas/Resistance", order = 0)]
    public class ResistanceContainer : ScriptableObject
    {
    #region Serialized Fields

        public DamageType DamageType;

        [Tooltip("Flat reduction of damage type. Additive stacking.")]
        public float Flat;

        [Tooltip("Percent reduction of damage type. Multiplicative stacking.")]
        public float Percent;

    #endregion

        public Resistance Resistance;

    #region Event Functions

        private void OnValidate()
        {
            if (DamageType != null)
                Resistance = new Resistance(DamageType.TypeID, Flat, Percent, Guid.NewGuid().GetHashCode());
        }

    #endregion
    }
}