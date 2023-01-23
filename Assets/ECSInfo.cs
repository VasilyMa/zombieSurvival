using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {

    public class ECSInfo : MonoBehaviour
    {
        public HealthBar Healthbar;
        public int Entity;
        public Animator Animator;
        public int Damage;
        private EcsWorld _world;
        private GameState _state;
        private EcsPool<DamageEvent> _damageEvent = default;
        private EcsPool<DamageComponent> _damagePool = default;

        public void Init(EcsWorld world, GameState state)
        {
            world = state.EcsWorld;
            _world = world;
            _state = state;
            _damageEvent = _world.GetPool<DamageEvent>();
            _damagePool = _world.GetPool<DamageComponent>();
        }

        public void DamageEvent()
        {
            var newEntity = _world.NewEntity();
            ref var damageEvent = ref _damageEvent.Add(newEntity);
            damageEvent.DamagableEntity = _state.EntityPlayer;
            damageEvent.DamageAmount = _damagePool.Get(Entity).DamageAmount;
        }
    }
}
