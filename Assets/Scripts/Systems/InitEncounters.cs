using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitEncounters : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<EncounterComponent> _encounterPool = default;
        public void Init (IEcsSystems systems) 
        {
            var entity = _world.Value.NewEntity();
            _encounterPool.Value.Add(entity);
        }
    }
}