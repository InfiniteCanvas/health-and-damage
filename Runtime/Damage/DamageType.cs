using System;
using UnityEngine;

namespace InfiniteCanvas.HealthDamageSystem.Damage
{
    [CreateAssetMenu(fileName = "DamageType", menuName = "Infinite Canvas/Damage Type", order = 0)]
    public class DamageType : ScriptableObject
    {
    #region Serialized Fields

        // save it as meta data, but don't allow editing
        [SerializeField, HideInInspector] private int _typeID;

    #endregion

        public int TypeID => _typeID;

    #region Event Functions

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_typeID == 0)
                _typeID = Guid.NewGuid().GetHashCode();
        }
#endif

    #endregion

        public override string ToString() => $"{this.name}, {nameof(TypeID)}: {TypeID}";
    }
}