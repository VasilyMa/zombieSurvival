using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class ShootSystem : IEcsRunSystem {

        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<ShootEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<BulletComponent> _bulletPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;

        readonly EcsPoolInject<DamageEvent> _damageEvent = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var shootComp = ref _filter.Pools.Inc1.Get(entity);
                ref var viewComp = ref _viewPool.Value.Get(entity);
                ref var damageComp = ref _damagePool.Value.Get(entity);
                ref var bulletComp = ref _bulletPool.Value.Get(entity);

                int maxColliders = 10;
                Collider[] hitColliders = new Collider[maxColliders];
                int numColliders = Physics.OverlapSphereNonAlloc(viewComp.GameObject.transform.position, 0.25f, hitColliders);

                for (int i = 0; i < numColliders; i++)
                {
                    if (hitColliders[i].gameObject.CompareTag("Enemy"))
                    {
                        var damageEventEntity = _world.Value.NewEntity();
                        ref var damageEventComp = ref _damageEvent.Value.Add(damageEventEntity);
                        damageEventComp.DamageAmount = damageComp.DamageAmount;
                        if (hitColliders[i].TryGetComponent<ECSInfo>(out var enemy))
                            damageEventComp.DamagableEntity = enemy.Entity;

                        viewComp.GameObject.SetActive(false);

                        _world.Value.DelEntity(entity);

                        break;
                    }
                }

            }
        }
    }
}