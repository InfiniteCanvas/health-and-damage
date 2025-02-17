using System;
using Unity.Collections;

namespace InfiniteCanvas.HealthDamageSystem.Resistances
{
    public struct ResistanceProfile : IDisposable
    {
        private NativeHashSet<Resistance> _resistances;

        // key: damage type id, cache calculated resistances
        private NativeHashMap<int, float> _flat;
        private NativeHashMap<int, float> _percentage;

        public (float flat, float percentage) GetResistance(int damageTypeId)
        {
            if (_flat.TryGetValue(damageTypeId, out var flat)
             && _percentage.TryGetValue(damageTypeId, out var percentage)) return (flat, percentage);

            return (0, 1);
        }

        public ResistanceProfile(Resistance res)
        {
            _resistances = new NativeHashSet<Resistance>(1, Allocator.Persistent);
            _flat = new NativeHashMap<int, float>(1,       Allocator.Persistent);
            _percentage = new NativeHashMap<int, float>(1, Allocator.Persistent);
            Add(res);
        }

        public void Add(Resistance res)
        {
            _resistances.Add(res);
            UpdateCache();
        }

        public void Remove(Resistance res)
        {
            _resistances.Remove(res);
            UpdateCache();
        }

        private void UpdateCache()
        {
            _flat.Clear();
            _percentage.Clear();
            foreach (var resistance in _resistances)
            {
                if (_flat.TryGetValue(resistance.DamageTypeID, out var flat))
                    _flat[resistance.DamageTypeID] = flat + resistance.Flat;
                else
                    _flat.Add(resistance.DamageTypeID, resistance.Flat);

                if (_percentage.TryGetValue(resistance.DamageTypeID, out var percentage))
                    _percentage[resistance.DamageTypeID] = (1 - percentage) * resistance.Percentage;
                else
                    _percentage.Add(resistance.DamageTypeID, 1 - resistance.Percentage);
            }
        }

        public void Dispose()
        {
            if (_resistances.IsCreated) _resistances.Dispose();
            if (_flat.IsCreated) _flat.Dispose();
            if (_percentage.IsCreated) _percentage.Dispose();
        }
    }
}