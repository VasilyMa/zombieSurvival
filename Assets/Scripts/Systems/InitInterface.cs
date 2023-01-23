using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class InitInterface : MonoBehaviour, IEcsInitSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Init (IEcsSystems systems) 
        {
            var entity = _world.Value.NewEntity();
            _state.Value.EntityInterface = entity;

            ref var interfaceComp = ref _interfacePool.Value.Add(entity);
            interfaceComp.ResourcePanel = FindObjectOfType<ResourcePanel>();
            interfaceComp.ResourcePanel.Init(_world.Value, _state.Value);
            interfaceComp.ResourcePanel.SetMoney();
            interfaceComp.StorePanel = FindObjectOfType<StorePanel>();
            interfaceComp.StorePanel.Init(_world.Value, _state.Value);
        }
    }
}