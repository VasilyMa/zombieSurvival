using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    public sealed class EcsStartup : MonoBehaviour
    {
        EcsSystems _initSystems, _runSystems, _menuSystems, _winSystems, _loseSystems, _fixedSystems;
        public EcsWorld World;
        private GameState _gameState;
        [Header("Systems")]
        public Vector3 CameraOffset;
        public int Counters;
        public int EnemiesOnCounter;
        public PoolBullet BulletPool;
        public DamagepopupPool DamageopopupPool;
        public PoolBlood BloodPool;
        public LayerMask LayerEnemies;
        public Counter CurrentEncounter;

        [Space(10)]
        [Header("Configs")]
        public PlayerConfig PlayerConfig;
        public EnemiesConfig EnemiesConfig;
        public EncounterConfigs EncounterConfigs;
        public InterfaceConfig InterfaceConfig;

        void Start ()
        {
            World = new EcsWorld();
            GameState.Clear();
            _gameState = GameState.Initialize(this);
            _gameState.GameMode = GameMode.runSystems;
            _gameState.Load();
            _initSystems = new EcsSystems(World, _gameState);
            _runSystems = new EcsSystems(World, _gameState);
            _menuSystems = new EcsSystems(World, _gameState);
            _winSystems = new EcsSystems(World, _gameState);
            _loseSystems = new EcsSystems(World, _gameState);
            _fixedSystems = new EcsSystems(World, _gameState);

            _initSystems
                .Add(new InitPlayer())
                .Add(new InitInput())
                .Add(new InitCamera())
                .Add(new InitSpawnSystem())
                .Add(new InitEncounters())
                .Add(new InitInterface())
            ;

            //_menuSystems
            //;

            _runSystems
                .Add(new MoveSystem())
                .Add(new SpawnSystem())
                .Add(new PlayerShootSystem())
                .Add(new AttackSystem())
                .Add(new SpawnSystem())
                .Add(new DamageSystem())
                .Add(new EncounterSystem())
            ;

            _fixedSystems
                .Add(new CameraSystem())
                .Add(new ShootSystem())

            ;
            //_winSystems
            //;

            //_loseSystems
            //;

#if UNITY_EDITOR 
            _initSystems.Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ());
#endif

            InjectAllSystems(_initSystems, _runSystems, _menuSystems, _winSystems, _loseSystems, _fixedSystems);
            InitAllSystems(_initSystems, _runSystems, _menuSystems, _winSystems, _loseSystems, _fixedSystems);
        }
        private void InjectAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Inject();
            }
        }

        private void InitAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Init();
            }
        }
        void Update ()
        {
            _initSystems?.Run();
            if (GameState.Get().GameMode.HasFlag(GameMode.runSystems)) _runSystems?.Run();
            if (GameState.Get().GameMode.HasFlag(GameMode.menuSystem)) _menuSystems?.Run();
            if (GameState.Get().GameMode.HasFlag(GameMode.winSystem)) _winSystems?.Run();
            if (GameState.Get().GameMode.HasFlag(GameMode.loseSystem)) _loseSystems?.Run();
        }
        private void FixedUpdate()
        {
            if (GameState.Get().GameMode.HasFlag(GameMode.runSystems)) _fixedSystems?.Run();
        }

        void OnDestroy () {
            if (_initSystems != null)
            {
                _initSystems.Destroy();
                _initSystems.GetWorld().Destroy();
                _initSystems = null;
            }

            if (_runSystems != null)
            {
                _runSystems.Destroy();
                _runSystems = null;
            }

            if (_menuSystems != null)
            {
                _menuSystems.Destroy();
                _menuSystems = null;
            }

            if (_winSystems != null)
            {
                _winSystems.Destroy();
                _winSystems = null;
            }

            if (_loseSystems != null)
            {
                _loseSystems.Destroy();
                _loseSystems = null;
            }

            // cleanup custom worlds here.

            // cleanup default world.

            if (World != null) {
                World.Destroy ();
                World = null;
            }
        }
        private void OnApplicationQuit()
        {
            _gameState.Save();
        }
    }
}