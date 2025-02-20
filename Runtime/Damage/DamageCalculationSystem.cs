using System;
using InfiniteCanvas.HealthDamageSystem.Modifications;
using JetBrains.Annotations;
using MessagePipe;
using Unity.Collections;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Damage
{
    [UsedImplicitly]
    public sealed class DamageCalculationSystem : IFixedTickable, IDisposable, IPostFixedTickable
    {
        private          NativeRingQueue<DamageRequest> _damageRequestQueue;
        private          NativeHashMap<int, float>      _damageToApply;
        private readonly IDisposable                    _disposable;
        private readonly HealthDB                       _healthDB;
        private readonly ModificationSystem             _modificationSystem;

        public DamageCalculationSystem(ISubscriber<DamageRequest> damageRequestSubscriber,
                                       ModificationSystem         modificationSystem,
                                       HealthDB                   healthDB)
        {
            var bag = DisposableBag.CreateBuilder(1);

            _healthDB = healthDB;
            _modificationSystem = modificationSystem;
            damageRequestSubscriber.Subscribe(DamageRequestHandler).AddTo(bag);
            _damageRequestQueue = new NativeRingQueue<DamageRequest>(256, Allocator.Persistent);
            bag.Add(_damageRequestQueue);

            _disposable = bag.Build();
        }

        public void Dispose() => _disposable?.Dispose();

        public void FixedTick()
        {
            while (_damageRequestQueue.TryDequeue(out var damageRequest))
            {
                var health = _healthDB[damageRequest.TargetID];
                var mods = _modificationSystem[damageRequest.DamageTypeID];
                var (flat, percent) = mods.GetModification(damageRequest.DamageTypeID);
                var damage = (damageRequest.Damage - flat) * percent;
                if (_damageToApply.ContainsKey(damageRequest.TargetID))
                    _damageToApply[damageRequest.TargetID] += damage;
                else
                    _damageToApply.Add(damageRequest.TargetID, damage);
            }
        }

        public void PostFixedTick()
        {
            foreach (var pair in _damageToApply)
            {
                var health = _healthDB[pair.Key];
                health.CurrentHP -= pair.Value;
            }

            _damageToApply.Clear();
        }

        private void DamageRequestHandler(DamageRequest message) => _damageRequestQueue.Enqueue(message);
    }
}