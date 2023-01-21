using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class InitJoystick : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<JoystickComponent> _joystickPool = default;

        int entity;

        public void Init (IEcsSystems systems) 
        {
            entity = _world.Value.NewEntity();

            _state.Value.EntityJoystick = entity;

            ref var joystickComponent = ref _joystickPool.Value.Add(entity);
            joystickComponent.Joystick = _state.Value.Joystick;
        }
    }
}