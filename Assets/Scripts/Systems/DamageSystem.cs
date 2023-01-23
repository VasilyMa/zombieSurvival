using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client {
    sealed class DamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsFilterInject<Inc<DamageEvent>> _damageFilter = default;

        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<PlayerTag> _playerPool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;
        readonly EcsPoolInject<EncounterComponent> _encountPool = default;

        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _damageFilter.Value)
            {
                ref var damageComp = ref _damageFilter.Pools.Inc1.Get(entity);

                var damageEntity = damageComp.DamagableEntity;

                ref var healthComponent = ref _healthPool.Value.Get(damageEntity);
                ref var viewComponent = ref _viewPool.Value.Get(damageEntity);

                

                healthComponent.Health -= damageComp.DamageAmount;

                viewComponent.ECSInfo.Healthbar.ChangeHealth((float)healthComponent.Health / (float)healthComponent.MaxHealth);

                var popup = _state.Value.PoolDamagepopup.GetPool().GetAvailableElement();
                popup.gameObject.transform.position = new Vector3(viewComponent.GameObject.transform.position.x, viewComponent.GameObject.transform.position.y + 1.5f, viewComponent.GameObject.transform.position.z);
                popup.SetDamageAmount(damageComp.DamageAmount.ToString());

                var blood = _state.Value.PoolBlood.GetPool().GetAvailableElement();
                blood.transform.position = new Vector3(viewComponent.GameObject.transform.position.x, viewComponent.GameObject.transform.position.y + 1.5f, viewComponent.GameObject.transform.position.z);
                blood.Invoke();


                if (healthComponent.Health <= 0)
                    DieEvent(damageEntity);

                DeleteEntity(entity);
            }
        }

        private void DieEvent(int entity)
        {
            ref var viewComp = ref _viewPool.Value.Get(entity);
            if (_playerPool.Value.Has(entity))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                // to do player died
                return;
            }
            if (_enemyPool.Value.Has(entity))
            {
                _state.Value.Money += Random.Range(1, 10);
                _state.Value.Save();
                viewComp.GameObject.gameObject.SetActive(false);
                _encountPool.Value.Add(_world.Value.NewEntity());
                _interfacePool.Value.Get(_state.Value.EntityInterface).ResourcePanel.SetMoney();
                DeleteEntity(entity);
            }
        }
        void DeleteEntity(int entity)
        {
            _world.Value.DelEntity(entity);
        }
    }
}