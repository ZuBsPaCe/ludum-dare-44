using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using zs.Helpers;
using zs.Main;

namespace zs.Logic
{
    public class Game : MonoBehaviour
    {
        #region Serializable Fields
        #endregion Serializable Fields

        #region Private Vars

        private static Game _instance = null;

        private GameInfo _gameInfo = null;
        private int _currentLevel = 0;

        private SpawnPoint _currentSpawnPoint = null;
        private Player _currentPlayer = null;

        private Transform _bulletContainer;

        #endregion Private Vars

        #region Public Vars

        public static Game Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject game = new GameObject("Game");
                    game.AddComponent<Game>();
                }

                return _instance;
            }
            set { _instance = value; }
        }

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        public Transform BulletContainer
        {
            get
            {
                if (_bulletContainer == null)
                {
                    GameObject go = new GameObject("Bullets");
                    _bulletContainer = go.transform;
                }

                return _bulletContainer;
            } 
        }

        #endregion Public Vars

        #region Public Methods

        public void KillPlayer(bool stuck = false)
        {
            #if UNITY_EDITOR
            if (_currentPlayer.Invincible)
            {
                return;
            }
            #endif

            _currentPlayer.Kill(stuck);

            _currentPlayer = Instantiate(_gameInfo.PlayerPrefab, _currentSpawnPoint.transform.position, Quaternion.identity);
            Camera.main.GetComponent<CameraFollow>().FollowTarget = _currentPlayer.transform;
        }

        public void LoadLevel(int level)
        {
            _currentLevel = level; 
            SceneManager.LoadScene("Level " + level);
        }

        public void LoadNextLevel()
        {
            LoadLevel(_currentLevel + 1);
        }

        #endregion Public Methods

        #region MonoBehaviour

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Debug.Log("Game Awake");

            _gameInfo = Resources.Load<GameInfo>("GameInfo");
            Debug.Assert(_gameInfo);

            Instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.StartsWith("Level"))
            {
                _currentSpawnPoint = FindObjectOfType<SpawnPoint>();
                Debug.Assert(_currentSpawnPoint);

                _currentPlayer = Instantiate(_gameInfo.PlayerPrefab, _currentSpawnPoint.transform.position, Quaternion.identity);
                Camera.main.GetComponent<CameraFollow>().FollowTarget = _currentPlayer.transform;
            }
        }

        #endregion Private Methods
    }
}
