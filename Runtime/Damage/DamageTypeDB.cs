using System.Collections.Generic;
using UnityEngine;

namespace InfiniteCanvas.HealthDamageSystem.Damage
{
    public static class DamageTypeDB
    {
        private static readonly Dictionary<int, DamageType> _typeMap = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            // Auto-register all damage types in Resources
            var types = Resources.LoadAll<DamageType>("");
            foreach (var t in types) _typeMap[t.TypeID] = t;
        }

        public static DamageType Resolve(int typeID) => _typeMap.GetValueOrDefault(typeID);
    }
}