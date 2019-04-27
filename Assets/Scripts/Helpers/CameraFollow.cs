using UnityEngine;

namespace zs.Helpers
{
    public class CameraFollow : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Transform _followTarget = null;

        #endregion Serializable Fields

        #region Private Vars

        #endregion Private Vars

        #region Public Vars

        public Transform FollowTarget
        {
            get { return _followTarget; }
            set { _followTarget = value; }
        }

        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (_followTarget != null)
            {
                transform.position = Vector3.Lerp(transform.position, _followTarget.transform.position.with_z(transform.position.z), 0.1f);
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
