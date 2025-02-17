using System;
using InfiniteCanvas.HealthDamageSystem.Messages;
using MessagePipe;
using Unity.Collections;
using UnityEngine;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Resistances
{
    public sealed class ResistanceSystem : IDisposable, IFixedTickable
    {
        private readonly IDisposable                           _disposable;
        private          NativeHashMap<int, ResistanceProfile> _profiles;

        public ResistanceSystem(ISubscriber<AddResistanceToTarget> addResistanceToTarget)
        {
            var builder = DisposableBag.CreateBuilder(2);

            _profiles = new NativeHashMap<int, ResistanceProfile>(100, Allocator.Persistent);
            builder.Add(_profiles);
            addResistanceToTarget.Subscribe(AddResistanceToTargetHandler).AddTo(builder);

            _disposable = builder.Build();
        }

        public void Dispose()
        {
            Debug.Log("Disposing ResistanceSystem");
            foreach (var kvp in _profiles) kvp.Value.Dispose();

            _disposable?.Dispose();
        }

        public void FixedTick() { }

        public ResistanceProfile? GetProfile(int id) => _profiles.TryGetValue(id, out var profile) ? profile : null;

        private void AddResistanceToTargetHandler(AddResistanceToTarget message)
        {
            if (_profiles.TryGetValue(message.TargetID, out var profile)) profile.Add(message.Resistance);
            else _profiles.Add(message.TargetID, new ResistanceProfile(message.Resistance));
        }
    }
}