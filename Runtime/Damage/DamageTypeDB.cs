using System.Collections.Generic;
using InfiniteCanvas.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Damage
{
    [UsedImplicitly]
    public class DamageTypeDB : IStartable
    {
        /// <summary>
        /// Gets the DamageType by Id.
        /// </summary>
        /// <param name="typeId">DamageType's <see cref="DamageType.TypeID"/></param>
        /// <remarks>
        /// Result can be null if Id is not found
        /// </remarks>
        public DamageType this[int typeId] => _typeMap.GetValueOrDefault(typeId);

        private readonly Dictionary<int, DamageType> _typeMap = new();

        public void Start()
        {
            Debug.Log("loading damage types..");
            var damageTypes = AddressablesLoader.GetAllAssets<DamageType>(Addressables.MergeMode.Union, "DamageType");
            foreach (var damageType in damageTypes)
            {
                _typeMap.Add(damageType.TypeID, damageType);
            }
        }
    }
}