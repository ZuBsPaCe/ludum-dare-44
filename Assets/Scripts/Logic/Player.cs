using UnityEngine;
using zs.Helpers;

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

        [SerializeField]
        private bool _invincible = false;

        [SerializeField]
        private AudioSource _audioSource = null;

        [SerializeField]
        private AudioSource _walkAudioSource = null;

        #endregion Serializable Fields

        #region Private Vars

        private bool _killed;

        private Rigidbody2D _rigidbody = null;

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

        private bool _isCarrying;
        private Player _carriedPlayer = null;

        private bool _levelDone = false;

        private float _maxWalkVolume;

        #endregion Private Vars

        #region Public Vars

        public bool Invincible
        {
            get { return _invincible; }
        }

        #endregion Public Vars

        #region Public Methods

        public void LevelDone()
        {
            _levelDone = true;
            _currentVelocity = _currentVelocity.with_x(0);

            Sound.Instance.StopWalk(_walkAudioSource);
        }

        public void Kill(bool stuck = false)
        {
            if (_levelDone)
            {
                return;
            }

            Sound.Instance.StopWalk(_walkAudioSource);

            Debug.Assert(!_killed);

            Debug.Log("Killed!");
            _killed = true;

            _spritesTransform.position = transform.position;

            _aliveSprite.enabled = false;
            _deadSprite.enabled = true;

            gameObject.layer = LayerMask.NameToLayer("DeadPlayer");

            enabled = false;

            if (!stuck)
            {
                gameObject.tag = "DeadPlayer";
                _rigidbody.isKinematic = false;
            }
            else
            {
                gameObject.tag = "DeadPlayerStuck";
            }

            Uncarry();
        }

        public float GetNextCollisionPosDown()
        {
            return GetNextCollisionPos(_downRaycastSources, Direction.Down, out _);
        }

        public float GetNextCollisionPosUp()
        {
            return GetNextCollisionPos(_upRaycastSources, Direction.Up, out _);
        }

        public float GetNextCollisionPosRight()
        {
            return GetNextCollisionPos(_rightRaycastSources, Direction.Right, out _);
        }

        public float GetNextCollisionPosLeft()
        {
            return GetNextCollisionPos(_leftRaycastSources, Direction.Left, out _);
        }

        public void JumpPad()
        {
            const float defaultSpeed = 20f;

            if (_currentVelocity.y < -defaultSpeed)
            {
                //Debug.Log("Reversing Velocity");
                _currentVelocity.y = -_currentVelocity.y;

                // Need to add this.. don't know why... Otherwise we lose velocity... Rounding errors?
                _currentVelocity.y += 1f;
                //_currentVelocity.y += defaultSpeed;
            }
            else
            {
                //Debug.Log("Default Velocity");
                _currentVelocity.y = defaultSpeed;
            }
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


            _rigidbody = GetComponent<Rigidbody2D>();

            Debug.Assert(_audioSource);
            Debug.Assert(_walkAudioSource);

            _maxWalkVolume = _walkAudioSource.volume;


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

            if (_levelDone)
            {
                _horTargetVelocity = Vector3.zero;
            }
            else if (hor < 0.1f && hor > -0.1f)
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
                Input.GetButtonDown("Jump") &&
                !_levelDone)
            {
                Debug.Log("Jump!");
                _jumpStarted = true;
                _jumping = true;

                Sound.Instance.PlayJump(_audioSource);
            }
            else if (
                _jumping &&
                Input.GetButtonUp("Jump") &&
                !_levelDone)
            {
                _jumpStarted = false;
                _jumping = false;
            }


            _spritePos = Vector3.Lerp(_spritePos, transform.position, _spriteLerp * Time.deltaTime);
            _spritesTransform.position = _spritePos;
        }
	
        void FixedUpdate()
        {
            float initialXVelocity = _currentVelocity.x;


            float minY = GetNextCollisionPos(_downRaycastSources, Direction.Down, out string downCollisionTag);
            float maxY = GetNextCollisionPos(_upRaycastSources, Direction.Up, out string upCollisionTag);

            float maxX = GetNextCollisionPos(_rightRaycastSources, Direction.Right, out string rightCollisionTag);
            float minX = GetNextCollisionPos(_leftRaycastSources, Direction.Left, out string leftCollisionTag);

            if (_isCarrying)
            {
                float carryMaxY = _carriedPlayer.GetNextCollisionPosUp() - 1f;
                maxY = Mathf.Min(maxY, carryMaxY);

                float carryMaxX = _carriedPlayer.GetNextCollisionPosRight();
                float carryMinX = _carriedPlayer.GetNextCollisionPosLeft();

                if (carryMaxX < transform.position.x ||
                    carryMinX > transform.position.x) 
                {
                   Uncarry(); 
                }
            }

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

            if (_grounded)
            {
                if (downCollisionTag == "ConveyorCW")
                {
                    horVelocity += Vector3.right * 5;
                }
                else if (downCollisionTag == "ConveyorCCW")
                {
                    horVelocity += Vector3.left * 5;
                }
            }

            Vector3 newVelocity = horVelocity + verVelocity;

            _currentVelocity = newVelocity;


            Vector3 newPosition = transform.position + _currentVelocity * Time.fixedDeltaTime;

            if (downCollisionTag != "JumpPad" &&
                newPosition.y < minY)
            {
                newPosition.y = minY;

                _currentVelocity.y = 0;

                if (!_grounded)
                {
                    Sound.Instance.PlayBump(_audioSource);
                }

                //Debug.Log("Grounded");
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
                if (initialXVelocity >= 1f || initialXVelocity <= -1f)
                {
                    Sound.Instance.PlayBump(_audioSource);
                }

                newPosition.x = maxX;
                _currentVelocity.x = 0;
            }

            if (newPosition.x < minX)
            {
                if (initialXVelocity >= 1f || initialXVelocity <= -1f)
                {
                    Sound.Instance.PlayBump(_audioSource);
                }

                newPosition.x = minX;
                _currentVelocity.x = 0;
            }

            transform.position = newPosition;

            if (_isCarrying)
            {
                _carriedPlayer.transform.position = transform.position.with_y(transform.position.y + 1f);
            }

            if (_grounded && (_currentVelocity.x > 0.1 || _currentVelocity.x < -0.1f))
            {
                float factor = Mathf.Min(_currentVelocity.magnitude / _horSpeed, 1f);
                Sound.Instance.PlayWalk(_walkAudioSource, factor * _maxWalkVolume);
            }
            else
            {
                Sound.Instance.PlayWalk(_walkAudioSource, 0);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (_killed)
            {
                return;
            }

            if (collider.tag == "KillTiles")
            {
                Game.Instance.KillPlayer(true);
            }
            else if (collider.tag == "Portal")
            {
                Portal portal = collider.GetComponent<Portal>();
                portal.EnterPortal();
            }
        }

        #endregion MonoBehaviour

        #region Private Methods

        private float GetNextCollisionPos(GameObject[] raySourceObjects, Direction direction, out string tag)
        {
            float collisionPos;
            tag = null;

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
                string currentTag;
                float currentCollisionPos = GetNextCollisionPos(raySourceObject, direction, out currentTag);

                switch (direction)
                {
                    case Direction.Up:
                        if (currentCollisionPos < collisionPos)
                        {
                            collisionPos = currentCollisionPos;
                            tag = currentTag;
                        }
                        break;

                    case Direction.Left:
                        if (currentCollisionPos > collisionPos)
                        {
                            collisionPos = currentCollisionPos;
                            tag = currentTag;
                        }
                        break;

                    case Direction.Down:
                        if (currentCollisionPos > collisionPos)
                        {
                            collisionPos = currentCollisionPos;
                            tag = currentTag;
                        }
                        break;

                    case Direction.Right:
                        if (currentCollisionPos < collisionPos)
                        {
                            collisionPos = currentCollisionPos;
                            tag = currentTag;
                        }
                        break;
                }   
            }

            return collisionPos;
        }

        private float GetNextCollisionPos(GameObject raySourceObject, Direction direction, out string tag)
        {
            tag = null;

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

            int layerMask = ~LayerMask.GetMask("Player", "Bullet");
            int hitCount = Physics2D.RaycastNonAlloc(raySourceObject.transform.position, dirVec, _raycastHits, maxDistance, layerMask);


            if (hitCount > 0)
            {
                float distance = float.PositiveInfinity;
                Vector2 hitPos = Vector2.zero;
                bool hitFound = false;

                for (var i = 0; i < hitCount; i++)
                {
                    RaycastHit2D hit = _raycastHits[i];

                    if ((!hit.collider.isTrigger || hit.collider.tag == "JumpPad") && hit.distance < distance)
                    {
                        if (hit.collider.tag == "DeadPlayer" && direction != Direction.Down)
                        {
                            if (!_isCarrying && _grounded && direction == Direction.Up && hit.collider.transform.position.y >= transform.position.y + 1f)
                            {
                                Player deadPlayerOnTop = hit.collider.GetComponent<Player>();
                                deadPlayerOnTop.GetComponent<Rigidbody2D>().isKinematic = true;

                                _isCarrying = true;
                                _carriedPlayer = deadPlayerOnTop;
                            }

                            continue;
                        }

                        if (hit.collider.tag == "SpikesRunning")
                        {
                            // Spikes.cs will handle Player Death.
                            continue;
                        }

                        hitFound = true;
                        distance = hit.distance;
                        hitPos = hit.point;

                        tag = hit.collider.tag;
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

        private void Uncarry()
        {
            if (!_isCarrying)
            {
                return;
            }

            _carriedPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
            _isCarrying = false;
            _carriedPlayer = null;
        }

        #endregion Private Methods
    }
}
