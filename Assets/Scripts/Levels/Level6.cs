using UnityEngine;
using zs.Helpers;

namespace zs.Levels
{
    public class Level6 : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Spikes _spikes1 = null;

        [SerializeField]
        private Spikes _spikes2 = null;

        [SerializeField]
        private Spikes _spikes3 = null;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void Toggle1()
        {
            _spikes1.Toggle();
            _spikes1.ToggleTravel();
        }

        public void Toggle2()
        {
            _spikes2.Toggle();
            _spikes2.ToggleTravel();
        }

        public void Toggle3()
        {
            _spikes3.Toggle();
            _spikes3.ToggleTravel();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_spikes1);
            Debug.Assert(_spikes2);
            Debug.Assert(_spikes3);
        }

        void Start()
        {
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
