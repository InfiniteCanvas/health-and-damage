using System.Collections.Generic;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem
{
    public sealed class HealthDB : IStartable
    {
        private Dictionary<int, Health> _healthMap;
        public Health this[int hash] => _healthMap.GetValueOrDefault(hash);

        public void Start() => _healthMap = new Dictionary<int, Health>();

        public void Add(Health health) => _healthMap.Add(health.EntityId, health);
    }
}