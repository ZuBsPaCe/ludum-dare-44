using UnityEngine;

namespace zs.Logic
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
        private float _jumpSpeed = 5f;

        [SerializeField]
        private Transform _spritesTransform = null;

        [SerializeField]
        private float _spriteLerp = 20f;

        [SerializeField]
        private SpriteRenderer _aliveSprite = null;

        [SerializeField]
        private SpriteRenderer _deadSprite = null;

        [SerializeField]
        private GameObject _raySourceDownLeft = null;

        [SerializeField]
        private GameObject _raySourceDownCenter = null;

        [SerializeField]
        private GameObject _raySourceDownRight = null;

        [SerializeField]
        private GameObject _raySourceUpLeft = null;

        [SerializeField]
        private GameObject _raySourceUpCenter = null;

        [SerializeField]
        private GameObject _raySourceUpRight = null;

        [SerializeField]
        private GameObject _raySourceLeftTop = null;

        [SerializeField]
        private GameObject _raySourceLeftCenter = null;

        [SerializeField]
        private GameObject _raySourceLeftBottom = null;

        [SerializeField]
        private GameObject _raySourceRightTop = null;

        [SerializeField]
        private GameObject _raySourceRightCenter = null;

        [SerializeField]
        private GameObject _raySourceRightBottom = null;

        #endregion Serializable Fields

        #region Private Vars

        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _horTargetVelocity = Vector3.zero;


        private Vector3 _spritePos = Vector3.zero;


        private RaycastHit2D[] _raycastHits = new RaycastHit2D[5];

        private GameObject[] _upRaycastSources = null;
        private GameObject[] _downRaycastSources = null;
        private GameObject[] _leftRaycastSources = null;
        private GameObject[] _rightRaycastSources = null;


        private bool _grounded;
        private bool _jumpStarted;
        private bool _jumping;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void Kill()
        {
            Debug.Log("Killed!");

            _spritesTransform.position = transform.position;

            _aliveSprite.enabled = false;
            _deadSprite.enabled = true;

            gameObject.layer = LayerMask.NameToLayer("DeadPlayer");

            enabled = false;
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_spritesTransform);

            Debug.Assert(_aliveSprite, _deadSprite);

            Debug.Assert(
                _raySourceDownLeft && _raySourceDownCenter && _raySourceDownRight &&
                _raySourceUpLeft && _raySourceUpCenter && _raySourceUpRight &&
                _raySourceLeftTop && _raySourceLeftCenter && _raySourceLeftBottom &&
                _raySourceRightTop && _raySourceRightCenter && _raySourceRightBottom);


            _downRaycastSources = new GameObject[]
            {
                _raySourceDownLeft, _raySourceDownCenter, _raySourceDownRight
            };

            _upRaycastSources = new GameObject[]
            {
                _raySourceUpLeft, _raySourceUpCenter, _raySourceUpRight
            };

            _leftRaycastSources = new GameObject[]
            {
                _raySourceLeftTop, _raySourceLeftCenter, _raySourceLeftBottom 
            };

            _rightRaycastSources = new GameObject[]
            {
                _raySourceRightTop, _raySourceRightCenter, _raySourceRightBottom
            };
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

            if (_grounded &&
                !_jumping &&
                Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jump!");
                _jumpStarted = true;
                _jumping = true;
            }
            else if (
                _jumping &&
                Input.GetButtonUp("Jump"))
            {
                _jumpStarted = false;
                _jumping = false;
            }


            _spritePos = Vector3.Lerp(_spritePos, transform.position, _spriteLerp * Time.deltaTime);
            _spritesTransform.position = _spritePos;
        }
	
        void FixedUpdate()
        {
            float minY = GetNextCollisionPos(_downRaycastSources, Direction.Down);
            float maxY = GetNextCollisionPos(_upRaycastSources, Direction.Up);

            float maxX = GetNextCollisionPos(_rightRaycastSources, Direction.Right);
            float minX = GetNextCollisionPos(_leftRaycastSources, Direction.Left);

            //Debug.Log("Velocity X: " + _currentVelocity.x.ToString("0.00"));

            Vector3 horVelocity = new Vector3(_currentVelocity.x, 0, 0);
            horVelocity = Vector3.Lerp(horVelocity, _horTargetVelocity, Time.fixedDeltaTime * _horAcceleration);


            Vector3 verVelocity = new Vector3(0, _currentVelocity.y, 0);
            verVelocity += Time.fixedDeltaTime * _gravity * Vector3.down;


            if (_jumpStarted)
            {
                _jumpStarted = false;
                verVelocity += new Vector3(0, _jumpSpeed, 0);
            }

            Vector3 newVelocity = horVelocity + verVelocity;

            _currentVelocity = newVelocity;


            Vector3 newPosition = transform.position + _currentVelocity * Time.fixedDeltaTime;

            if (newPosition.y < minY)
            {
                newPosition.y = minY;

                _currentVelocity.y = 0;

                _grounded = true;
                _jumpStarted = false;
                _jumping = false;
            }
            else
            {
                _grounded = false;
            }

            if (newPosition.y > maxY)
            {
                newPosition.y = maxY;

                _currentVelocity.y = 0;
                _jumpStarted = false;
                _jumping = false;
            }

            if (newPosition.x > maxX)
            {
                newPosition.x = maxX;
                _currentVelocity.x = 0;
            }

            if (newPosition.x < minX)
            {
                newPosition.x = minX;
                _currentVelocity.x = 0;
            }


            transform.position = newPosition;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "KillTiles")
            {
                Game.Instance.KillPlayer(this);
            }
        }

        #endregion MonoBehaviour

        #region Private Methods

        private float GetNextCollisionPos(GameObject[] raySourceObjects, Direction direction)
        {
            float collisionPos;

            switch (direction)
            {
                case Direction.Up:
                    collisionPos = float.PositiveInfinity;
                    break;

                case Direction.Left:
                    collisionPos = float.NegativeInfinity;
                    break;

                case Direction.Down:
                    collisionPos = float.NegativeInfinity;
                    break;

                case Direction.Right:
                    collisionPos = float.PositiveInfinity;
                    break;

                default:
                    Debug.Assert(false, $"Unknown Direction [{direction}]");
                    return 0;
            }   
                
            foreach (GameObject raySourceObject in raySourceObjects)
            {
                float currentCollisionPos = GetNextCollisionPos(raySourceObject, direction);

                switch (direction)
                {
                    case Direction.Up:
                        collisionPos = Mathf.Min(collisionPos, currentCollisionPos);
                        break;

                    case Direction.Left:
                        collisionPos = Mathf.Max(collisionPos, currentCollisionPos);
                        break;

                    case Direction.Down:
                        collisionPos = Mathf.Max(collisionPos, currentCollisionPos);
                        break;

                    case Direction.Right:
                        collisionPos = Mathf.Min(collisionPos, currentCollisionPos);
                        break;
                }   
            }

            return collisionPos;
        }

        private float GetNextCollisionPos(GameObject raySourceObject, Direction direction)
        {
            Vector2 dirVec;

            switch (direction)
            {
                case Direction.Up:
                    dirVec = Vector2.up;
                    break;

                case Direction.Left:
                    dirVec = Vector2.left;
                    break;

                case Direction.Down:
                    dirVec = Vector2.down;
                    break;

                case Direction.Right:
                    dirVec = Vector2.right;
                    break;

                default:
                    Debug.Assert(false, $"Unknown Direction [{direction}]");
                    dirVec = Vector2.zero;
                    break;
            }


            const float maxDistance = 10f;



            float collisionPos = float.PositiveInfinity;

            int layerMask = ~LayerMask.GetMask("Player");;
            int hitCount = Physics2D.RaycastNonAlloc(raySourceObject.transform.position, dirVec, _raycastHits, maxDistance, layerMask);


            if (hitCount > 0)
            {
                float distance = float.PositiveInfinity;
                Vector2 hitPos = Vector2.zero;
                bool hitFound = false;

                for (var i = 0; i < hitCount; i++)
                {
                    RaycastHit2D hit = _raycastHits[i];

                    if (!hit.collider.isTrigger && hit.distance < distance)
                    {
                        hitFound = true;
                        distance = hit.distance;
                        hitPos = hit.point;
                    }
                }

                if (hitFound)
                {
                    switch (direction)
                    {
                        case Direction.Up:
                            collisionPos = hitPos.y - 0.4f - 0.1f;
                            break;

                        case Direction.Left:
                            collisionPos = hitPos.x + 0.4f + 0.1f;
                            break;

                        case Direction.Down:
                            collisionPos = hitPos.y + 0.4f + 0.1f;
                            break;

                        case Direction.Right:
                            collisionPos = hitPos.x - 0.4f - 0.1f;
                            break;

                        default:
                            Debug.Assert(false, $"Unknown Direction [{direction}]");
                            collisionPos = 0;
                            break;
                    }

                    Debug.DrawLine(
                        raySourceObject.transform.position,
                        hitPos,
                        Color.red);
                }
                else
                {
                    hitCount = 0;
                }
            }

            if (hitCount == 0)
            {
                switch (direction)
                {
                    case Direction.Up:
                        collisionPos = float.PositiveInfinity;
                        break;

                    case Direction.Left:
                        collisionPos = float.NegativeInfinity;
                        break;

                    case Direction.Down:
                        collisionPos = float.NegativeInfinity;
                        break;

                    case Direction.Right:
                        collisionPos = float.PositiveInfinity;
                        break;

                    default:
                        Debug.Assert(false, $"Unknown Direction [{direction}]");
                        break;
                }

                Debug.DrawLine(
                    raySourceObject.transform.position,
                    raySourceObject.transform.position + dirVec.with_z(0) * maxDistance,
                    Color.green);
            }

            return collisionPos;
        }

        #endregion Private Methods
    }
}
