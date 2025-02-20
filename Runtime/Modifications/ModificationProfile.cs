using System;
using Unity.Collections;

namespace InfiniteCanvas.HealthDamageSystem.Modifications
{
    public struct ModificationProfile : IDisposable
    {
        private NativeHashSet<Modification> _modifications;
        private NativeHashMap<int, float>   _flat;
        private NativeHashMap<int, float>   _percent;

        public ModificationProfile(Modification modification)
        {
            _modifications = new NativeHashSet<Modification>(1, Allocator.Persistent);
            _flat = new NativeHashMap<int, float>(1,    Allocator.Persistent);
            _percent = new NativeHashMap<int, float>(1, Allocator.Persistent);
        }

        public (float flat, float percent) GetModification(int damageTypeId)
        {
            if (_flat.TryGetValue(damageTypeId, out var flat) && _percent.TryGetValue(damageTypeId, out var percent))
                return (flat, percent);
            return (0, 0);
        }

        public void Add(Modification modification)
        {
            _modifications.Add(modification);
            UpdateCache();
        }

        public void Remove(Modification modification)
        {
            _modifications.Remove(modification);
            UpdateCache();
        }

        private void UpdateCache()
        {
            _flat.Clear();
            _percent.Clear();
            foreach (var modification in _modifications)
            {
                if (_flat.TryGetValue(modification.DamageTypeID, out var flat))
                    _flat[modification.DamageTypeID] = flat + modification.Flat;
                else
                    _flat.Add(modification.DamageTypeID, modification.Flat);

                if (_percent.TryGetValue(modification.DamageTypeID, out var percent))
                    _percent[modification.DamageTypeID] = percent + modification.Percent;
                else
                    _percent.Add(modification.DamageTypeID, modification.Percent);
            }
        }

        public void Dispose()
        {
            _modifications.Dispose();
            _flat.Dispose();
            _percent.Dispose();
        }
    }
}