using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitSpawnSystem : IEcsInitSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;

        public void Init (IEcsSystems systems) {
            var entity = _world.Value.NewEntity();
            _state.Value.EntitySpawner = entity;

            _spawnPool.Value.Add(entity);
        }
    }
}