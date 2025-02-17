using System;
using InfiniteCanvas.HealthDamageSystem.Messages;
using MessagePipe;
using Unity.Collections;
using UnityEngine;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Amplifications
{
    public sealed class AmplificationSystem : IFixedTickable, IDisposable
    {
        private readonly IDisposable                              _disposable;
        private          NativeHashMap<int, AmplificationProfile> _profiles;

        public AmplificationSystem(ISubscriber<AddAmplificationToTarget> addAmplificationToTarget)
        {
            var bag = DisposableBag.CreateBuilder(2);

            _profiles = new NativeHashMap<int, AmplificationProfile>(100, Allocator.Persistent);
            addAmplificationToTarget.Subscribe(AddAmplificationHandler).AddTo(bag);
            _profiles.AddTo(bag);

            _disposable = bag.Build();
        }

        public void Dispose()
        {
            Debug.Log("Disposing AmplificationSystem");
            foreach (var kvp in _profiles) kvp.Value.Dispose();

            _disposable?.Dispose();
        }

        public void FixedTick() { }

        public AmplificationProfile? GetProfile(int id) => _profiles.TryGetValue(id, out var profile) ? profile : null;

        private void AddAmplificationHandler(AddAmplificationToTarget message)
        {
            if (_profiles.TryGetValue(message.TargetID, out var profile)) profile.Add(message.Amplification);
            else _profiles.Add(message.TargetID, new AmplificationProfile(message.Amplification));
        }
    }
}