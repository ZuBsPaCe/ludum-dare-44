using System;
using UnityEngine;
using UnityEngine.Events;

namespace zs.Helpers
{
    [Serializable]
    public class ToggleButtonEvent : UnityEvent
    {
    }

    [Serializable]
    public class TurnOnButtonEvent : UnityEvent
    {
    }

    [Serializable]
    public class TurnOffButtonEvent : UnityEvent
    {
    }

    public class ToggleButton : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private bool _on = true;

        [SerializeField]
        private SpriteRenderer _buttonSprite = null;

        [SerializeField]
        private AudioSource _audioSource = null;

        public ToggleButtonEvent Toggled;
        public TurnOnButtonEvent TurnedOn;
        public TurnOffButtonEvent TurnedOff;

        #endregion Serializable Fields

        #region Private Vars

        private int _triggerCount = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        void Toggle()
        {
            if (_on)
            {
                _on = false;
                TurnedOff.Invoke();
            }
            else
            {
                _on = true;
                TurnedOn.Invoke();
            }

            _audioSource.Play();
            
            Toggled.Invoke();

            _buttonSprite.transform.localPosition = Vector3.down * 0.1f;

            UpdateColor();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_audioSource);
            Debug.Assert(_buttonSprite);
            UpdateColor();
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (_triggerCount == 0 && _buttonSprite.transform.localPosition != Vector3.zero)
            {
                _buttonSprite.transform.localPosition = Vector3.MoveTowards(_buttonSprite.transform.localPosition, Vector3.zero, 0.05f * Time.deltaTime);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (_triggerCount == 0)
            {
                Toggle();
            }

            _triggerCount += 1;
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            _triggerCount -= 1;
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void UpdateColor()
        {
            if (_on)
            {
                _buttonSprite.color = Color.green;
            }
            else
            {
                _buttonSprite.color = Color.red;
            }
        }

        #endregion Private Methods
    }
}
