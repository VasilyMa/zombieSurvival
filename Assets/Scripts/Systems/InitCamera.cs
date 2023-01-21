using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class InitCamera : MonoBehaviour, IEcsInitSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;

        int entity;
        public void Init (IEcsSystems systems) {
            entity = _world.Value.NewEntity();
            _state.Value.EntityCamera = entity;

            ref var cameraComponent = ref _cameraPool.Value.Add(entity);
            cameraComponent.Camera = FindObjectOfType<Camera>();
        }
    }
}