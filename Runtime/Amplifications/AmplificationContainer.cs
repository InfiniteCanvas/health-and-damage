using System;
using InfiniteCanvas.HealthDamageSystem.Damage;
using UnityEngine;

namespace InfiniteCanvas.HealthDamageSystem.Amplifications
{
    [CreateAssetMenu(fileName = "Amplification", menuName = "Infinite Canvas/Amplification", order = 0)]
    public class AmplificationContainer : ScriptableObject
    {
    #region Serialized Fields

        public DamageType DamageType;

        [Tooltip("Flat amplification of damage type. Additive stacking.")]
        public float Flat;

        [Tooltip("Percent amplification of damage type. Multiplicative stacking.")]
        public float Percent;

    #endregion

        public Amplification Amplification;

    #region Event Functions

        private void OnValidate()
        {
            if (DamageType != null)
                Amplification = new Amplification(DamageType.TypeID, Flat, Percent, Guid.NewGuid().GetHashCode());
        }

    #endregion
    }
}