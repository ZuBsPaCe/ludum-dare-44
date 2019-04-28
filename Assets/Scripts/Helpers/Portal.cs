﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using zs.Logic;

namespace zs.Helpers
{
    [Serializable]
    public class PrePortalEvent : UnityEvent
    {
    }

    public class Portal : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private SpriteRenderer _portalSprite = null;

        [SerializeField]
        private SpriteRenderer _heartSprite = null;

        [SerializeField]
        private bool _showHeart = false;

        [SerializeField]
        private FadeScreen _fadeScreen = null;

        [SerializeField]
        private string _mainText = null;

        [SerializeField]
        private string _subText = null;

        [SerializeField]
        private bool _returnToLevelScreen = false;

        public PrePortalEvent PrePortalEvent = null;

        #endregion Serializable Fields

        #region Private Vars

        private bool _portalEntered = false;

        #endregion Private Vars

        #region Public Vars

        #endregion Public Vars

        #region Public Methods

        public void EnterPortal()
        {
            if (_portalEntered)
            {
                return;
            }

            Player player = Game.Instance.CurrentPlayer;

            if (player != null)
            {
                player.LevelDone();
            }

            _portalEntered = true;

            if (_showHeart)
            {
                _heartSprite.enabled = false;

                if (PlayerPrefs.HasKey("EarnedHearts"))
                {
                    int earnedHearts = PlayerPrefs.GetInt("EarnedHearts");
                    PlayerPrefs.SetInt("EarnedHearts", earnedHearts + 10);
                }
                else
                {
                    PlayerPrefs.SetInt("EarnedHearts", 10);
                }

                PlayerPrefs.Save();
            }

            if (PrePortalEvent.GetPersistentEventCount() == 0)
            {
                PerformPortal();
            }
            else
            {
                PrePortalEvent.Invoke();
            }
        }

        public void PerformPortal()
        {
            if (_fadeScreen != null)
            {
                _fadeScreen.PerformFade(_mainText, _subText, ReallyPerformPortal);
            }
            else
            {
                ReallyPerformPortal();
            }
        }

        private void ReallyPerformPortal()
        {
            string level = SceneManager.GetActiveScene().name;

            if (PlayerPrefs.HasKey(level))
            {
                int previousLifesLost = PlayerPrefs.GetInt(level);

                if (previousLifesLost > Game.Instance.LifesLost)
                {
                    PlayerPrefs.SetInt(level, Game.Instance.LifesLost);
                }
            }
            else
            {
                PlayerPrefs.SetInt(level, Game.Instance.LifesLost);
            }

            PlayerPrefs.Save();

            if (_returnToLevelScreen)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                Game.Instance.LoadNextLevel();
            }
        }

        #endregion Public Methods

        #region MonoBehaviour

        void Awake()
        {
            Debug.Assert(_portalSprite);
            Debug.Assert(_heartSprite);

            if (_showHeart)
            {
                _portalSprite.gameObject.SetActive(false);
                _heartSprite.gameObject.SetActive(true);
            }
        }

        void Start()
        {
        }

        void Update()
        {
            if (_portalEntered)
            {
                return;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main");
            }
        }

        #endregion MonoBehaviour

        #region Private Methods

        #endregion Private Methods
    }
}
