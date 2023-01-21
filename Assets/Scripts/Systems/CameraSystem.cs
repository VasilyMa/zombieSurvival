using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CameraSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CameraComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        private Vector3 offset = new Vector3(0f, 0f, 0f);
        private float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;
        private Transform target;
        private Transform camera;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var cameraComponent = ref _filter.Pools.Inc1.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                target = viewComponent.GameObject.transform;
                camera = cameraComponent.Camera.transform;
                offset = _state.Value.CameraOffset;
                
                Vector3 targetPosition = target.position + offset;
                camera.position = Vector3.SmoothDamp(camera.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}