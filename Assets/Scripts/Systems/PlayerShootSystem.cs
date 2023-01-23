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
        readonly EcsPoolInject<BulletComponent> _bulletPool = default;

        float timeToShoot = .25f;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                ref var damageCompoennt = ref _damagePool.Value.Get(_state.Value.EntityPlayer);

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
                    var target = GetTargetPosition();

                    if(target.gameObject.CompareTag("Enemy"))
                    {
                        viewComponent.GameObject.transform.LookAt(target);
                        //to do instatiate with pools
                        var bullet = _state.Value.PoolBullet.GetPool().GetAvailableElement();
                        bullet.transform.position = viewComponent.FirePoint.position;
                        var shootEntity = _world.Value.NewEntity();

                        ref var viewComp = ref _viewPool.Value.Add(shootEntity);
                        ref var shootComp = ref _shootEvent.Value.Add(shootEntity);
                        ref var damageComp = ref _damagePool.Value.Add(shootEntity);
                        ref var moveComp = ref _movePool.Value.Add(shootEntity);
                        ref var projectileComp = ref _bulletPool.Value.Add(shootEntity);

                        viewComp.GameObject = bullet.gameObject;
                        viewComp.Rigidbody = bullet.GetComponent<Rigidbody>();
                        projectileComp.Projectile = bullet.GetComponent<Projectile>();
                        moveComp.Direction = target.position - viewComponent.GameObject.transform.position;
                        moveComp.Rotation = moveComp.Direction;
                        moveComp.MoveSpeed = 40;
                        damageComp.DamageAmount = damageCompoennt.DamageAmount;

                        timeToShoot = .25f;
                        break;
                    }
                }
            }
        }
        Transform GetTargetPosition()
        {
            Transform target = null;
            var camera = _cameraPool.Value.Get(_state.Value.EntityCamera).Camera;
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Mathf.Infinity, _state.Value.LayerEnemies
            {
                target = hit.transform;
            }
            return target;
        }
    }
}