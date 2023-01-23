using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class GameState
    {
        private static GameState _gameState = null;

        public Counter Encounter;
        public int CurrentEncounter;
        public int Money;
        public int MinimumEnemies = 1;
        public int MaximumEnemies = 3;
        public EcsSystems EcsSystems;
        public EcsWorld EcsWorld; 
        
        public GameMode GameMode;

        public int EntitySpawner;
        public int EntityPlayer;
        public int EntityCamera;
        public int EntityInterface;

        public Vector3 CameraOffset;

        public EncounterConfigs EncounterConfigs;
        public PlayerConfig PlayerConfig;
        public EnemiesConfig EnemiesConfig;
        public InterfaceConfig InterfaceConfig;
        public PoolBullet PoolBullet;
        public PoolBlood PoolBlood;
        public DamagepopupPool PoolDamagepopup;
        public LayerMask LayerEnemies;

        private GameState(in EcsStartup ecsStartup)
        {
            EcsWorld = ecsStartup.World;
            PlayerConfig = ecsStartup.PlayerConfig;
            CameraOffset = ecsStartup.CameraOffset;
            PoolBullet = ecsStartup.BulletPool;
            LayerEnemies = ecsStartup.LayerEnemies;
            EnemiesConfig = ecsStartup.EnemiesConfig;
            EncounterConfigs = ecsStartup.EncounterConfigs;
            Encounter = ecsStartup.CurrentEncounter;
            PoolDamagepopup = ecsStartup.DamageopopupPool;
            PoolBlood = ecsStartup.BloodPool;
            InterfaceConfig = ecsStartup.InterfaceConfig;
        }
        public static void Clear()
        {
            _gameState = null;
        }
        public void Save()
        {
            PlayerPrefs.SetInt("Money", Money);
        }
        public void Load()
        {
            Money = PlayerPrefs.GetInt("Money");
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
    public enum GameMode { runSystems, menuSystem, loseSystem, winSystem, moveSystem}
}