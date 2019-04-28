using System;
using UnityEngine;
using UnityEngine.UI;
using zs.Helpers;
using zs.Logic;

namespace zs.Main
{
    public class StartScreen : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Image _life = null;

        [SerializeField]
        private Button _startNewButton = null;

        [SerializeField]
        private Button _startOrContinueButton = null;

        [SerializeField]
        private MainMenu _mainMenu = null;

        [SerializeField]
        private FadeScreen _fadeScreen = null;

        #endregion Serializable Fields

        #region Private Vars

        private LifeCycle _cycle = LifeCycle.RedUp;
        private float _green = 255;
        private float _red = 0;
        private float _blue = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void StartOrContinuePressed()
        {
            if (_fadeScreen.Fading)
            {
                return;
            }

            if (PlayerPrefs.HasKey("CanContinue"))
            {
                _mainMenu.ShowLevelScreen();
            }
            else
            {
                _fadeScreen.PerformFade("Intro", "A walk in the woods", StartIntroLevel);
            }
        }

        public void StartNewPressed()
        {
            if (_fadeScreen.Fading)
            {
                return;
            }

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            _mainMenu.ShowLevelScreen();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_fadeScreen);
            Debug.Assert(_life);
            Debug.Assert(_startNewButton);
            Debug.Assert(_startOrContinueButton);
            Debug.Assert(_mainMenu);
        }

        void Start()
        {
            if (PlayerPrefs.HasKey("CanContinue"))
            {
                _startNewButton.gameObject.SetActive(true);

                _startOrContinueButton.GetComponentInChildren<Text>().text = "Continue";
            }
            else
            {
                _startNewButton.gameObject.SetActive(false);
                _startOrContinueButton.GetComponentInChildren<Text>().text = "Start";
            }
        }
	
        void Update()
        {
            Game.Instance.PerformLifeCycle(ref _cycle, ref _red, ref _green, ref _blue, ref _life);
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void StartIntroLevel()
        {
            Game.Instance.LoadLevel(0);
        }

        #endregion Private Methods
    }
}
