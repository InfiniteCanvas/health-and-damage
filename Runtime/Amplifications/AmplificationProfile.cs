using System;
using Unity.Collections;

namespace InfiniteCanvas.HealthDamageSystem.Amplifications
{
    public struct AmplificationProfile : IDisposable
    {
        private NativeHashSet<Amplification> _amplifications;

        // key: damage type id, cache calculated resistances
        private NativeHashMap<int, float> _flat;
        private NativeHashMap<int, float> _percentage;

        public (float flat, float percentage) GetResistance(int damageTypeId)
        {
            if (_flat.TryGetValue(damageTypeId, out var flat)
             && _percentage.TryGetValue(damageTypeId, out var percentage)) return (flat, percentage);

            return (0, 1);
        }

        public AmplificationProfile(Amplification amp)
        {
            _amplifications = new NativeHashSet<Amplification>(1, Allocator.Persistent);
            _flat = new NativeHashMap<int, float>(1,       Allocator.Persistent);
            _percentage = new NativeHashMap<int, float>(1, Allocator.Persistent);
            Add(amp);
        }

        public void Add(Amplification res)
        {
            _amplifications.Add(res);
            UpdateCache();
        }

        public void Remove(Amplification res)
        {
            _amplifications.Remove(res);
            UpdateCache();
        }

        private void UpdateCache()
        {
            _flat.Clear();
            _percentage.Clear();
            foreach (var resistance in _amplifications)
            {
                if (_flat.TryGetValue(resistance.DamageTypeID, out var flat))
                    _flat[resistance.DamageTypeID] = flat + resistance.Flat;
                else
                    _flat.Add(resistance.DamageTypeID, resistance.Flat);

                if (_percentage.TryGetValue(resistance.DamageTypeID, out var percentage))
                    _percentage[resistance.DamageTypeID] = (1 + percentage) * resistance.Percentage;
                else
                    _percentage.Add(resistance.DamageTypeID, 1 + resistance.Percentage);
            }
        }

        public void Dispose()
        {
            if (_amplifications.IsCreated) _amplifications.Dispose();
            if (_flat.IsCreated) _flat.Dispose();
            if (_percentage.IsCreated) _percentage.Dispose();
        }
    }
}