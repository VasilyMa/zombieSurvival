using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class AttackSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<EnemyTag, MovementComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(entity);
                ref var animatorComponent = ref _animatorPool.Value.Get(entity);

                int maxColliders = 10;
                Collider[] hitColliders = new Collider[maxColliders];
                int numColliders = Physics.OverlapSphereNonAlloc(viewComponent.GameObject.transform.position, 1f, hitColliders);

                for (int i = 0; i < numColliders; i++)
                {
                    if (hitColliders[i].gameObject.CompareTag("Player"))
                    {
                        animatorComponent.Animator.SetBool("isRun", false);
                        animatorComponent.Animator.SetBool("isAttack", true);
                        _filter.Pools.Inc2.Del(entity);
                        break;
                    }
                }

            }
        }
    }
}