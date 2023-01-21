using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class InitInput : IEcsInitSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;
        public void Init (IEcsSystems systems) {
            var entity = _world.Value.NewEntity();
            _inputPool.Value.Add(entity);
        }
    }
}