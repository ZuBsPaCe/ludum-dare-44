using UnityEngine;
using zs.Helpers;

namespace zs.Levels
{
    public class Level7 : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Spikes _spikes1 = null;

        [SerializeField]
        private Spikes _spikes2 = null;

        [SerializeField]
        private Spikes _spikes3 = null;

        [SerializeField]
        private Spikes _left1 = null;

        [SerializeField]
        private Spikes _left2 = null;

        [SerializeField]
        private Spikes _right1 = null;

        [SerializeField]
        private Spikes _right2 = null;

        #endregion Serializable Fields

        #region Private Vars

        private float _lastToggleTime = 0;

        private int _mode = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void Toggle()
        {
            _spikes1.Toggle();
            _spikes2.Toggle();
            _spikes3.Toggle();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_spikes1);
            Debug.Assert(_spikes2);
            Debug.Assert(_spikes3);
            Debug.Assert(_left1);
            Debug.Assert(_left2);
            Debug.Assert(_right1);
            Debug.Assert(_right2);
        }

        void Start()
        {
            _left1.Toggle();
            _left2.Toggle();
        }

        void Update()
        {
            float waitTime = _mode == 1 || _mode == 3 ? 0.5f : 3;
            if (Time.time - _lastToggleTime > waitTime)
            {
                if (_mode == 0)
                {
                    // Right side running
                    _right1.Stop();
                    _right2.Stop();
                    _mode++;
                }
                else if (_mode == 1)
                {
                    // All Stopped
                    _left1.TurnCW();
                    _left2.TurnCW();
                    _mode++;
                }
                else if (_mode == 2)
                {
                    // Left side running
                    _left1.Stop();
                    _left2.Stop();
                    _mode++;
                }
                else if (_mode == 3)
                {
                    // All Stopped
                    _right1.TurnCCW();
                    _right2.TurnCCW();
                    _mode = 0;
                }

                _lastToggleTime = Time.time;
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
