using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class JoystickController : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<InputComponent>> _filter = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<JoystickComponent> _joystickPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var joystickComponent = ref _joystickPool.Value.Get(_state.Value.EntityJoystick);
                ref var moveComponent = ref _movePool.Value.Get(_state.Value.EntityPlayer);
                ref var viewComponent = ref _viewPool.Value.Get(_state.Value.EntityPlayer);

                if (joystickComponent.Joystick.Horizontal != 0 || joystickComponent.Joystick.Vertical != 0)
                {
                    moveComponent.Direction = new Vector3(joystickComponent.Joystick.Horizontal, viewComponent.Rigidbody.velocity.y, joystickComponent.Joystick.Vertical);
                    moveComponent.Rotation = new Vector3(joystickComponent.Joystick.Horizontal, viewComponent.Rigidbody.velocity.y, joystickComponent.Joystick.Vertical);
                }
                else
                    moveComponent.Direction = Vector3.zero;
                /*
                if (player.Joystick.Horizontal != 0 || player.Joystick.Vertical != 0)
                {
                    if (player.DistanceToTarget <= 10 && player.Target != null)  //целимся во врага, если он близко
                    {
                        Vector3 direction = (player.Target.transform.position - player.transform.position).normalized;
                        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 10 * Time.deltaTime);
                    }
                    else
                        player.transform.rotation = Quaternion.LookRotation(player.Rigidbody.velocity); //поворачиваем героя, если цели нет или она далеко
                }*/
            }
        }
    }
}