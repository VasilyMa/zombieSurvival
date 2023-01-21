using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class GameState
    {
        private static GameState _gameState = null;


        public EcsSystems EcsSystems;
        public EcsWorld EcsWorld; 
        
        public GameMode GameMode;

        public int EntityPlayer;

        public PlayerConfig PlayerConfig;

        private GameState(in EcsStartup ecsStartup)
        {
            EcsWorld = ecsStartup.World;
        }
        public static void Clear()
        {
            _gameState = null;
        }

        public static GameState Initialize(in EcsStartup ecsStartup)
        {
            if (_gameState is null)
            {
                _gameState = new GameState(in ecsStartup);
            }

            return _gameState;
        }

        public static GameState Get()
        {
            return _gameState;
        }
    }
    public enum GameMode { runSystems, menuSystem, loseSystem, winSystem}
}