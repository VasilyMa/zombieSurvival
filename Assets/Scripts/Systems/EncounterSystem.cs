using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class EncounterSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<EncounterComponent>> _filter = default;
        readonly EcsFilterInject<Inc<EnemyTag>> _enemiesCount = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var encounter in _filter.Value)
            {
                var numsOfEnemies = _enemiesCount.Value.GetEntitiesCount();

                if (numsOfEnemies > 0)
                {
                    _world.Value.DelEntity(encounter);
                    continue;
                }

                var newEncounter = GameObject.Instantiate(_state.Value.EncounterConfigs.Encounter, Vector3.zero, Quaternion.identity);

                //float newPlacement = _state.Value.CurrentEncounter.transform.localScale.z / 2 + newEncounter.transform.localScale.z / 2;

                newEncounter.transform.position = new Vector3(_state.Value.Encounter.transform.position.x, _state.Value.Encounter.transform.position.y, _state.Value.Encounter.transform.position.z + 20);

                _state.Value.Encounter = newEncounter;

                ref var moveComponent = ref _movePool.Value.Add(_state.Value.EntityPlayer);
                moveComponent.MoveSpeed = 2f;

                _world.Value.DelEntity(encounter);
            }
        }
    }
}