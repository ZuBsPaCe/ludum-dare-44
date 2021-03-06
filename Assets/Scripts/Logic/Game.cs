﻿using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

        private int _lifesLost = 0;

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

        public int LifesLost
        {
            get { return _lifesLost; }
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

        public int EarnedHearts
        {
            get
            {
                if (!PlayerPrefs.HasKey("EarnedHearts"))
                {
                    return 0;
                }

                return PlayerPrefs.GetInt("EarnedHearts");
            }
        }

        public int TotalHearts
        {
            get
            {
                int total = EarnedHearts;

                for (int i = 0; i <= 16; i++)
                {
                    if (!PlayerPrefs.HasKey("Level " + i))
                    {
                        continue;
                    }

                    int lifesLost = PlayerPrefs.GetInt("Level " + i);
                    total -= lifesLost;
                }

                return total;
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

            _lifesLost += 1;

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

        public void PerformLifeCycle(ref LifeCycle cycle, ref float red, ref float green, ref float blue, ref Image life)
        {
            const float speed = 30;

            switch (cycle)
            {
                case LifeCycle.RedUp:
                    red += speed * Time.deltaTime;
                    if (red >= 255)
                    {
                        red = 255;
                        cycle = LifeCycle.GreenDown;
                    }
                    break;

                case LifeCycle.GreenDown:
                    green -= speed * Time.deltaTime;
                    if (green <= 0)
                    {
                        green = 0;
                        cycle = LifeCycle.BlueUp;
                    }
                    break;

                case LifeCycle.BlueUp:
                    blue += speed * Time.deltaTime;
                    if (blue >= 255)
                    {
                        blue = 255;
                        cycle = LifeCycle.RedDown;
                    }
                    break;

                case LifeCycle.RedDown:
                    red -= speed * Time.deltaTime;
                    if (red <= 0)
                    {
                        red = 0;
                        cycle = LifeCycle.GreenUp;
                    }
                    break;

                case LifeCycle.GreenUp:
                    green += speed * Time.deltaTime;
                    if (green >= 255)
                    {
                        green= 255;
                        cycle = LifeCycle.BlueDown;
                    }
                    break;

                case LifeCycle.BlueDown:
                    blue -= speed * Time.deltaTime;
                    if (blue <= 0)
                    {
                        blue = 0;
                        cycle = LifeCycle.RedUp;
                    }
                    break;
            }

            life.color = new Color(red / 255f, green / 255f, blue / 255f, 100 / 255f);
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
                _lifesLost = 0;

                _currentSpawnPoint = FindObjectOfType<SpawnPoint>();
                Debug.Assert(_currentSpawnPoint);

                _currentPlayer = Instantiate(_gameInfo.PlayerPrefab, _currentSpawnPoint.transform.position, Quaternion.identity);
                Camera.main.GetComponent<CameraFollow>().FollowTarget = _currentPlayer.transform;
            }
        }

        #endregion Private Methods
    }
}
