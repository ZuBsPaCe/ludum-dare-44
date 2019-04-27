using UnityEngine;

namespace zs.Logic
{
    public class Bullet : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private float _speed = 5f;

        #endregion Serializable Fields

        #region Private Vars

        private Vector3 _direction;

        #endregion Private Vars

        #region Public Vars

        public Vector3 Direction
        {
            set { _direction = value; }
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
	
        void FixedUpdate()
        {
            transform.position = transform.position + _direction * _speed * Time.deltaTime;
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
