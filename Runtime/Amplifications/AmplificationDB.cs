using System.Collections.Generic;
using InfiniteCanvas.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Amplifications
{
    [UsedImplicitly]
    public class AmplificationDB : IStartable
    {
        public Amplification this[int hash] => _map.GetValueOrDefault(hash);
        private readonly Dictionary<int, Amplification> _map = new();

        public void Start()
        {
            Debug.Log("Loading amplifications..");
            var amps =
                AddressablesLoader.GetAllAssets<AmplificationContainer>(Addressables.MergeMode.Union, "Amplification");
            foreach (var amp in amps)
            {
                _map.Add(amp.GetHashCode(), amp.Amplification);
            }
        }
    }
}