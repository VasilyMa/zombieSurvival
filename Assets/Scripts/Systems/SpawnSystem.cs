using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class SpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsFilterInject<Inc<SpawnComponent>> _filter = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                for (int i = 0; i < Random.Range(_state.Value.MinimumEnemies, _state.Value.MaximumEnemies); i++)
                {
                    var newEntity = _world.Value.NewEntity();

                    var spawnPoint = new Vector3(Random.Range(-10, 11),-0.5f,_state.Value.Encounter.SpawnPoint.position.z);

                    var newEnemy = GameObject.Instantiate(_state.Value.EnemiesConfig.Enemies[0], spawnPoint, Quaternion.identity);

                    ref var viewComp = ref _viewPool.Value.Add(newEntity);
                    ref var animComp = ref _animatorPool.Value.Add(newEntity);
                    ref var healthComp = ref _healthPool.Value.Add(newEntity);
                    ref var moveComp = ref _movePool.Value.Add(newEntity);
                    ref var damageComp = ref _damagePool.Value.Add(newEntity);

                    ref var enemy = ref _enemyPool.Value.Add(newEntity);

                    viewComp.GameObject = newEnemy;
                    viewComp.Rigidbody = viewComp.GameObject.GetComponent<Rigidbody>();

                    moveComp.Direction = viewComp.GameObject.transform.position - _viewPool.Value.Get(_state.Value.EntityPlayer).GameObject.transform.position;
                    viewComp.GameObject.transform.LookAt(_viewPool.Value.Get(_state.Value.EntityPlayer).GameObject.transform.position);
                    
                    moveComp.MoveSpeed = 1;
                    viewComp.ECSInfo = viewComp.GameObject.GetComponent<ECSInfo>();
                    animComp.Animator = viewComp.ECSInfo.Animator;
                    viewComp.ECSInfo.Entity = newEntity;
                    viewComp.ECSInfo.Healthbar.ChangeHealth(1);
                    viewComp.ECSInfo.Init(_world.Value, _state.Value);
                    healthComp.Health = 30;
                    healthComp.MaxHealth = healthComp.Health;
                    damageComp.DamageAmount = 10;
                }

                _state.Value.MaximumEnemies++;
                _state.Value.MinimumEnemies++;
                _world.Value.DelEntity(entity);
            }
        }
    }
}