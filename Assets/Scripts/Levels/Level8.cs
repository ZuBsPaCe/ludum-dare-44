using UnityEngine;
using zs.Helpers;

namespace zs.Levels
{
    public class Level8 : MonoBehaviour
    {
        #region Serializable Fields 
        
        [SerializeField]
        private Spikes _spikes1 = null;

        [SerializeField]
        private Spikes _spikes2 = null;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void Toggle()
        {
            _spikes1.Toggle();
            _spikes2.Toggle();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_spikes1);
            Debug.Assert(_spikes2);
        }

        void Start()
        {
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
