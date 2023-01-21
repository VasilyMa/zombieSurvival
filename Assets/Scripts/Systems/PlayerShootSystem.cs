using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerShootSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<InputComponent>> _filter = default;

        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;

        readonly EcsPoolInject<ShootEvent> _shootEvent = default;

        float timeToShoot = .25f;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(_state.Value.EntityPlayer);

                if (timeToShoot > 0)
                {
                    timeToShoot -= Time.deltaTime;
                    continue;
                }
                else if (timeToShoot <= 0)
                {
                    timeToShoot = 0;
                }

                if (Input.GetMouseButton(0))
                {
                    var point = GetTargetPosition();

                    int maxColliders = 10;
                    Collider[] hitColliders = new Collider[maxColliders];
                    int numColliders = Physics.OverlapSphereNonAlloc(point, 2, hitColliders);
                    for (int i = 0; i < numColliders; i++)
                    {
                        if (hitColliders[i].gameObject.CompareTag("Enemy"))
                        {
                            viewComponent.GameObject.transform.LookAt(hitColliders[i].transform.position);

                            //to do instatiate with pools
                            var bullet = GameObject.Instantiate(_state.Value.Bullet, viewComponent.GameObject.transform.position, viewComponent.GameObject.transform.rotation);
                            var shootEntity = _world.Value.NewEntity();
                            ref var viewComp = ref _viewPool.Value.Add(shootEntity);
                            ref var shootComp = ref _shootEvent.Value.Add(shootEntity);
                            ref var damageComp = ref _damagePool.Value.Add(shootEntity);
                            ref var moveComp = ref _movePool.Value.Add(shootEntity);
                            viewComp.GameObject = bullet;
                            viewComp.Rigidbody = bullet.GetComponent<Rigidbody>();
                            moveComp.Direction = point - viewComponent.GameObject.transform.position;
                            moveComp.Rotation = moveComp.Direction;
                            moveComp.MoveSpeed = 5;

                            timeToShoot = .25f;
                            break;
                        }
                    }
                }
            }
        }
        Vector3 GetTargetPosition()
        {
            Vector3 position = Vector3.zero;
            var camera = _cameraPool.Value.Get(_state.Value.EntityCamera).Camera;
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                position = hit.point;
                Debug.Log($"Collider is {hit.collider}, Position hit is {position}");
            }
            return position;
        }
    }
}