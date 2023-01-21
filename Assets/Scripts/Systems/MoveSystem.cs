using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class MoveSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<MovementComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var moveComponent = ref _filter.Pools.Inc1.Get(entity);

                ref var viewComponent = ref _viewPool.Value.Get(entity);

                viewComponent.Rigidbody.velocity = moveComponent.Direction * moveComponent.MoveSpeed;
                viewComponent.GameObject.transform.rotation = Quaternion.LookRotation(moveComponent.Rotation);
            }
        }
    }
}