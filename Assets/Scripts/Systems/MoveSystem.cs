using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class MoveSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<MovementComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;
        readonly EcsPoolInject<BulletComponent> _bulletPool = default;
        readonly EcsPoolInject<PlayerTag> _playerPool = default;
        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var moveComponent = ref _filter.Pools.Inc1.Get(entity);

                ref var viewComponent = ref _viewPool.Value.Get(entity);

                if(_bulletPool.Value.Has(entity)) viewComponent.GameObject.transform.Translate(moveComponent.Direction * moveComponent.MoveSpeed * Time.deltaTime * 0.13f);
                if(_enemyPool.Value.Has(entity)) viewComponent.GameObject.transform.position = Vector3.MoveTowards(viewComponent.GameObject.transform.position, _viewPool.Value.Get(_state.Value.EntityPlayer).GameObject.transform.position, moveComponent.MoveSpeed * Time.deltaTime);
                if (_playerPool.Value.Has(entity)) 
                {
                    viewComponent.GameObject.transform.position = Vector3.MoveTowards(viewComponent.GameObject.transform.position, _state.Value.Encounter.Point.position, moveComponent.MoveSpeed * Time.deltaTime);
                    viewComponent.GameObject.transform.LookAt(_state.Value.Encounter.Point);
                    if (viewComponent.GameObject.transform.position == _state.Value.Encounter.Point.position)
                    {
                        _state.Value.CurrentEncounter++;
                        if (_state.Value.CurrentEncounter % 2 == 0)
                        {
                            ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                            interfaceComp.StorePanel.GetHolderStore().gameObject.SetActive(true);
                            _animatorPool.Value.Get(entity).Animator.SetBool("isRun", false);
                            _filter.Pools.Inc1.Del(entity);
                        }
                        else
                        {
                            _animatorPool.Value.Get(entity).Animator.SetBool("isRun", false);
                            _spawnPool.Value.Add(_world.Value.NewEntity());
                            _filter.Pools.Inc1.Del(entity);
                        }
                        continue;
                    }    
                }
                if(_animatorPool.Value.Has(entity))
                {
                    _animatorPool.Value.Get(entity).Animator.SetBool("isAttack", false);
                    _animatorPool.Value.Get(entity).Animator.SetBool("isRun", true);
                }
            }
        }
    }
}