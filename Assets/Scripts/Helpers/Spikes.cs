using UnityEngine;
using zs.Logic;

namespace zs.Helpers
{
    public class Spikes : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private bool _CW = false;

        [SerializeField]
        private bool _CCW = false;

        [SerializeField]
        private Transform _start = null;

        [SerializeField]
        private Transform _end = null;

        [SerializeField]
        private float _speed = 3;

        [SerializeField]
        private bool _travelling = true;

        #endregion Serializable Fields

        #region Private Vars

        private SpriteRenderer _spriteRenderer = null;

        private bool _toEnd = true;
        private bool _lastCW = false;

        private AreaEffector2D _areaEffector2D = null;

        #endregion Private Vars

        #region Public Vars

        public void Stop()
        {
            if (!_CW && !_CCW)
            {
                return;
            }

            _lastCW = _CW;

            _CW = false;
            _CCW = false;
            UpdateTag();
            UpdateAreaEffector();
        }

        public void TurnCW()
        {
            _CW = true;
            _CCW = false;
            UpdateTag();
            UpdateAreaEffector();
        }

        public void TurnCCW()
        {
            _CW = false;
            _CCW = true;
            UpdateTag();
            UpdateAreaEffector();
        }

        public void Toggle()
        {
            if (_CW || _CCW)
            {
                Stop();
            }
            else
            {
                if (_lastCW)
                {
                    TurnCW();
                }
                else
                {
                    TurnCCW();
                }
            }
        }

        public void ToggleTravel()
        {
            _travelling = !_travelling;
        }

        public void Reverse()
        {
            if (!_CW && !_CCW)
            {
                return;
            }

            _CW = !_CW;
            _CCW = !_CCW;
        }

        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Debug.Assert(_spriteRenderer);
            
            _areaEffector2D = GetComponent<AreaEffector2D>();
            Debug.Assert(_areaEffector2D);

            UpdateTag();
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (_CCW || _CW)
            {
                if (_CW)
                {
                    _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -180 * Time.deltaTime) * _spriteRenderer.transform.rotation;
                }
                else
                {
                    _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 180 * Time.deltaTime) * _spriteRenderer.transform.rotation;
                }
            }
        }

        void FixedUpdate()
        {
            if (_start != null && _end != null && _speed != 0 && _travelling)
            {
                if (_toEnd)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _end.position, _speed * Time.fixedDeltaTime);

                    if (Vector3.Distance(transform.position, _end.position) < 0.1f)
                    {
                        _toEnd = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _start.position, _speed * Time.fixedDeltaTime);

                    if (Vector3.Distance(transform.position, _start.position) < 0.1f)
                    {
                        _toEnd = true;
                    }
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player" && (_CW || _CCW))
            {
                Game.Instance.KillPlayer();
            }
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void UpdateTag()
        {
            if (!_CCW && !_CW)
            {
                gameObject.tag = "SpikesIdle";
            }
            else
            {
                gameObject.tag = "SpikesRunning";
            }
        }
        
        private void UpdateAreaEffector()
        {
            if (!_CCW && !_CW)
            {
                _areaEffector2D.enabled = false;
            }
            else if (_CCW)
            {
                _areaEffector2D.enabled = true;
                _areaEffector2D.forceAngle = 180f;
            }
            else
            {
                _areaEffector2D.enabled = true;
                _areaEffector2D.forceAngle = 0f;
            }
            
        }

        #endregion Private Methods
    }
}
