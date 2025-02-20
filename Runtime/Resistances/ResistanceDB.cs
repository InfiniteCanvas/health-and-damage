using System.Collections.Generic;
using InfiniteCanvas.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Resistances
{
    [UsedImplicitly]
    public class ResistanceDB : IStartable
    {
        public Resistance this[int hash] => _map.GetValueOrDefault(hash);
        private readonly Dictionary<int, Resistance> _map = new();

        private void Initialize()
        {
            Debug.Log("Loading resistance containers..");
            var containers =
                AddressablesLoader.GetAllAssets<ResistanceContainer>(Addressables.MergeMode.Union, "Resistance");
            foreach (var container in containers)
            {
                _map.Add(container.Resistance.GetHashCode(), container.Resistance);
            }
        }

        public void Start() => Initialize();
    }
}