
using UnityEngine;

using UnityEngine.EventSystems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client
{
    public class Item : MonoBehaviour
    {
        EcsWorld _world;
        GameState _state;

        public int ItemID;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }
    }
}
