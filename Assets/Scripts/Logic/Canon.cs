using UnityEngine;

namespace zs.Logic
{
    public class Canon : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private SpriteRenderer _barrelSprite = null;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_barrelSprite);
        }

        void Start()
        {
        }
	
        void Update()
        {
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
