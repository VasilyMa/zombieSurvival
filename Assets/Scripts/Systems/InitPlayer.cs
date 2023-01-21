using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;


namespace Client {
    sealed class InitPlayer : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<PlayerTag> _playerTag = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;

        private int entity;
        public void Init (IEcsSystems systems) 
        {
            entity = _world.Value.NewEntity();

            _state.Value.EntityPlayer = entity;

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            ref var playerComponent = ref _playerTag.Value.Add(entity);
            ref var animatorComponent = ref _animatorPool.Value.Add(entity);
            ref var moveComponent = ref _movePool.Value.Add(entity);
            ref var healthComponent = ref _healthPool.Value.Add(entity);
            ref var damageComponent = ref _damagePool.Value.Add(entity);

            var playerObject = _state.Value.PlayerConfig.PlayerObject;
        }
    }
}