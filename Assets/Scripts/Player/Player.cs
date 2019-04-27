using UnityEngine;

namespace zs.Player
{
    public class Player : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private float _horSpeed = 5f;

        [SerializeField]
        private float _horAcceleration = 0.5f;

        [SerializeField]
        private float _gravity = 5f;

        [SerializeField]
        private Transform _spriteTransform = null;

        [SerializeField]
        private float _spriteLerp = 20f;

        [SerializeField]
        private GameObject _bottomLeft = null;

        [SerializeField]
        private GameObject _bottomCenter = null;

        [SerializeField]
        private GameObject _bottomRight = null;

        #endregion Serializable Fields

        #region Private Vars

        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _horTargetVelocity = Vector3.zero;


        private Vector3 _spritePos = Vector3.zero;


        private RaycastHit2D[] _raycastHits = new RaycastHit2D[5];

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_spriteTransform);

            Debug.Assert(_bottomLeft && _bottomCenter && _bottomRight);
        }

        void Start()
        {
            _spritePos = transform.position;
        }

        void Update()
        {
            float hor = Input.GetAxisRaw("Horizontal");

            if (hor < 0.1f && hor > -0.1f)
            {
                _horTargetVelocity = Vector3.zero;
            }
            else if (hor > 0)
            {
                _horTargetVelocity.x = _horSpeed;
            }
            else if (hor < 0)
            {
                _horTargetVelocity.x = -_horSpeed;
            }

            _spritePos = Vector3.Lerp(_spritePos, transform.position, _spriteLerp * Time.deltaTime);
            _spriteTransform.position = _spritePos;
        }
	
        void FixedUpdate()
        {
            float minY = float.MinValue;


            int layerMask = ~LayerMask.GetMask("Player");;
            int hitCount = Physics2D.RaycastNonAlloc(_bottomCenter.transform.position, Vector2.down, _raycastHits, 10f, layerMask);

            if (hitCount > 0)
            {
                float distance = float.PositiveInfinity;
                Vector2 hitPos = Vector2.zero;

                for (var i = 0; i < hitCount; i++)
                {
                    RaycastHit2D hit = _raycastHits[i];
                    if (hit.distance < distance)
                    {
                        hitPos = hit.point;
                    }
                }

                minY = hitPos.y + 0.4f + 0.1f;

                Debug.Log("MinY: " + minY.ToString("0.00"));
            }




            Vector3 horVelocity = new Vector3(_currentVelocity.x, 0, 0);
            horVelocity = Vector3.Lerp(horVelocity, _horTargetVelocity, Time.fixedDeltaTime * _horAcceleration);


            Vector3 verVelocity = new Vector3(0, _currentVelocity.y, 0);
            verVelocity += Time.fixedDeltaTime * _gravity * Vector3.down;


            Vector3 newVelocity = horVelocity + verVelocity;

            _currentVelocity = newVelocity;


            Vector3 newPosition = transform.position + _currentVelocity * Time.fixedDeltaTime;

            newPosition.y = Mathf.Max(newPosition.y, minY);

            transform.position = newPosition;
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
