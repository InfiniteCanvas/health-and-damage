using System;
using System.Collections.Generic;
using System.Linq;
using InfiniteCanvas.HealthDamageSystem.Damage;
using InfiniteCanvas.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Pool;

namespace InfiniteCanvas.HealthDamageSystem.Modifications
{
    [CreateAssetMenu(fileName = "ModificationSet", menuName = "Infinite Canvas/Modification Set", order = 0)]
    public class ModificationSet : ScriptableObject
    {
        public List<ModificationParameters> SerializedModifications = new();
        public Modification[]               Modifications           = Array.Empty<Modification>();

        [Serializable]
        public class ModificationParameters
        {
            [HideInInspector] public int        Hash;
            public                   DamageType DamageType;

            [Tooltip("Flat modification of damage type. Additive stacking - calculated as [new value] = [old value] + [this value]\n"
                   + "Damage calculation: result = (damage - flat) * (1-percent)\n"
                   + "Examples:\n"
                   + "+1 -> Reduces 1 flat damage\n"
                   + "-1 -> Adds 1 flat damage\n"
                   + "CAUTION: Damage will need to surpass flat reduction to do any damage! Damage cannot be reduced under 0.")]
            public float Flat;

            [Tooltip("Percent modification of damage type. Additive stacking - calculated as [new value] = [old value] + [this value])\n"
                   + "Damage calculation: result = (damage - flat) * (1-percent)\n"
                   + "Examples:\n"
                   + "+1.5 (150% Resistance) -> Will heal for 50% of damage\n"
                   + "+1 (100% Resistance)-> Will reduce damage to 0\n"
                   + "-1 (-100% Resistance)-> Will amplify damage by 100%")]
            public float Percent;
        }

        private void OnValidate()
        {
            Modifications =
                new Modification[SerializedModifications.Count(parameters => parameters.DamageType != null)];
            var i = 0;
            var hashes = HashSetPool<int>.Get();
            foreach (var modification in SerializedModifications.Where(parameters => parameters.DamageType != null))
            {
                if (modification.Hash == 0 || hashes.Contains(modification.Hash))
                    modification.Hash = Guid.NewGuid().GetHashCode();
                Modifications[i] = new Modification(modification.DamageType.TypeID,
                                                    modification.Flat,
                                                    modification.Percent,
                                                    modification.Hash);
                hashes.Add(modification.Hash);
                i++;
            }

            hashes.Release();
        }
    }
}