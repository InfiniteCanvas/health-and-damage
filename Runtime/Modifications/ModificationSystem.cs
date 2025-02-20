using System;
using InfiniteCanvas.Utilities.Extensions;
using JetBrains.Annotations;
using MessagePipe;
using Unity.Collections;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem.Modifications
{
    [UsedImplicitly]
    public class ModificationSystem : IDisposable, IStartable
    {
        private readonly IDisposable                             _disposable;
        // key: health id
        private          NativeHashMap<int, ModificationProfile> _profiles;
        private readonly ModificationDB                          _modificationDB;

        public ModificationProfile this[int hash] => _profiles.GetValueOrDefault(hash);

        public ModificationSystem(ISubscriber<AddModificationToTarget>      addModificationSubscriber,
                                  ISubscriber<RemoveModificationFromTarget> removeModificationSubscriber,
                                  ModificationDB                            modificationDB)
        {
            var bag = DisposableBag.CreateBuilder(2);
            _modificationDB = modificationDB;
            addModificationSubscriber.Subscribe(AddModificationToTargetHandler).AddTo(bag);
            removeModificationSubscriber.Subscribe(RemoveModificationFromTargetHandler).AddTo(bag);
            _profiles = new NativeHashMap<int, ModificationProfile>(64, Allocator.Persistent);
            _disposable = bag.Build();
        }

        private void RemoveModificationFromTargetHandler(RemoveModificationFromTarget message)
        {
            if (!_profiles.ContainsKey(message.TargetId)) return;
            var mod = _modificationDB[message.ModificationId];
            _profiles[message.TargetId].Remove(mod);
        }

        private void AddModificationToTargetHandler(AddModificationToTarget message)
        {
            var mod = _modificationDB[message.ModificationId];
            if (_profiles.TryGetValue(message.TargetId, out var profile))
                profile.Add(mod);
            else
                _profiles.Add(message.TargetId, new ModificationProfile(mod));
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            if (_profiles.IsCreated)
                _profiles.Dispose();
        }

        public void Start() { }
    }
}