using System;
using UnityEngine;
using UnityEngine.UI;

namespace zs.Helpers
{
    public class FadeScreen : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Image _bottomPanel = null;

        [SerializeField]
        private Image _topPanel = null;

        [SerializeField]
        private Text _mainText = null;

        [SerializeField]
        private Text _subText = null;

        #endregion Serializable Fields

        #region Private Vars

        private enum FadeState
        {
            FadeIn,
            FadeInText,
            TextShown,
            FadeOut,
        }

        private FadeState _fadeState = FadeState.FadeIn;
        private Action _action = null;

        private float _textShownStartTime = 0;

        #endregion Private Vars

        #region Public Vars

        public bool Fading = false;

        #endregion Public Vars

        #region Public Methods

        public void PerformFade(string main, string sub, Action action)
        {
            _fadeState = FadeState.FadeIn;
            Fading = true;

            _bottomPanel.color = new Color(0, 0, 0, 1);
            _bottomPanel.gameObject.SetActive(false);

            _topPanel.color = new Color(0, 0, 0, 0);
            _topPanel.gameObject.SetActive(true);

            _mainText.text = main;
            _subText.text = sub;

            _mainText.gameObject.SetActive(false);
            _subText.gameObject.SetActive(false);

            _action = action;

            this.gameObject.SetActive(true);
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_bottomPanel);
            Debug.Assert(_topPanel);
            Debug.Assert(_mainText);
            Debug.Assert(_subText);
        }

        void Update()
        {
            const float speedFadeIn = 1f;
            const float speedFadeInText = 0.33f;
            const float textShownTime = 3f;
            const float speedFadeOut = 0.5f;

            switch (_fadeState)
            {
                case FadeState.FadeIn:
                    if (_topPanel.color.a < 1)
                    {
                        float newAlpha = Mathf.Min(_topPanel.color.a + speedFadeIn * Time.deltaTime, 1);
                        _topPanel.color = new Color(0, 0, 0, newAlpha);
                    }
                    else
                    {
                        _fadeState = FadeState.FadeInText;

                        _mainText.gameObject.SetActive(true);
                        _subText.gameObject.SetActive(true);
                        _bottomPanel.gameObject.SetActive(true);
                    }
                    break;

                case FadeState.FadeInText:
                    if (_topPanel.color.a > 0)
                    {
                        float newAlpha = Mathf.Max(_topPanel.color.a - speedFadeInText * Time.deltaTime, 0);
                        _topPanel.color = new Color(0, 0, 0, newAlpha);
                    }
                    else
                    {
                        _fadeState = FadeState.TextShown;
                        _textShownStartTime = Time.time;
                    }
                    break;

                case FadeState.TextShown:
                    if (Time.time - _textShownStartTime >= textShownTime)
                    {
                        _fadeState = FadeState.FadeOut;
                    }
                    break;

                case FadeState.FadeOut:
                    if (_topPanel.color.a < 1)
                    {
                        float newAlpha = Mathf.Min(_topPanel.color.a + speedFadeOut * Time.deltaTime, 1);
                        _topPanel.color = new Color(0, 0, 0, newAlpha);
                    }
                    else
                    {
                        Fading = false;
                        _action.Invoke();
                    }
                    break;
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
