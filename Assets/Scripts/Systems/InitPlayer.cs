using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitPlayer : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<PlayerTag> _playerTag = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;

        private int entity;
        public void Init (IEcsSystems systems) 
        {
            entity = _world.Value.NewEntity();

            _state.Value.EntityPlayer = entity;

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            ref var playerComponent = ref _playerTag.Value.Add(entity);
            ref var animatorComponent = ref _animatorPool.Value.Add(entity);
            ref var healthComponent = ref _healthPool.Value.Add(entity);
            ref var damageComponent = ref _damagePool.Value.Add(entity);

            var playerObject = GameObject.Instantiate(_state.Value.PlayerConfig.PlayerObject, _state.Value.Encounter.Point.position, Quaternion.identity);

            healthComponent.Health = _state.Value.PlayerConfig.BaseHealth;
            healthComponent.MaxHealth = healthComponent.Health;

            if (playerObject.TryGetComponent<ECSInfo>(out var Ecsinfo))
                viewComponent.ECSInfo = Ecsinfo;

            animatorComponent.Animator = viewComponent.ECSInfo.Animator;
            viewComponent.ECSInfo.Entity = entity;
            viewComponent.ECSInfo.Healthbar.ChangeHealth(1);
            viewComponent.GameObject = playerObject;
            viewComponent.Rigidbody = viewComponent.GameObject.GetComponent<Rigidbody>();
            viewComponent.FirePoint = viewComponent.GameObject.GetComponentInChildren<FirePoint>().transform;
            damageComponent.DamageAmount = 10;
        }
    }
}