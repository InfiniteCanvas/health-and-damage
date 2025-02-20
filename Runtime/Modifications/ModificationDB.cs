using System.Collections.Generic;
using InfiniteCanvas.Utilities;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Modifications
{
    [UsedImplicitly]
    public class ModificationDB : IStartable
    {
        private readonly Dictionary<int, Modification> _modifications = new();

        public Modification this[int hash] => _modifications.GetValueOrDefault(hash);

        public void Start()
        {
            var modContainer =
                AddressablesLoader.GetAllAssets<ModificationSet>(Addressables.MergeMode.Union, "Modification");
            foreach (var container in modContainer)
            {
                foreach (var modification in container.Modifications)
                {
                    _modifications.Add(modification.GetHashCode(), modification);
                }
            }
        }
    }
}