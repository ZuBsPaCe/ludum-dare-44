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

        #endregion Public Vars

        #region Public Methods

        public void KillPlayer(Player player)
        {
            player.Kill();

            Player newPlayer = Instantiate(_gameInfo.PlayerPrefab, _gameInfo.LevelInfos[_currentLevel].SpawnPoint, Quaternion.identity);

            Camera.main.GetComponent<CameraFollow>().FollowTarget = newPlayer.transform;
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
            Debug.Log("Game Awake");

            _gameInfo = Resources.Load<GameInfo>("GameInfo");
            Debug.Assert(_gameInfo);

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        #endregion MonoBehaviour

        #region Private Methods

        #endregion Private Methods
    }
}
