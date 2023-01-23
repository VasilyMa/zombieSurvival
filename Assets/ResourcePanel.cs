using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    public class ResourcePanel : MonoBehaviour
    {
        EcsWorld _world;
        GameState _state;
        [SerializeField] Text _moneyAmount; 
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
        public void SetMoney()
        {
            _moneyAmount.text = $"Money: {_state.Money}";
        }
    }
}
