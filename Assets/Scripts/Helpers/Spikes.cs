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

        #endregion Serializable Fields

        #region Private Vars

        private SpriteRenderer _spriteRenderer = null;

        private bool _toEnd = true;

        #endregion Private Vars

        #region Public Vars

        public void Stop()
        {
            _CW = false;
            _CCW = false;
            UpdateTag();
        }

        public void TurnCW()
        {
            _CW = true;
            _CCW = false;
            UpdateTag();
        }

        public void TurnCCW()
        {
            _CW = false;
            _CCW = true;
            UpdateTag();
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
            if (_start != null && _end != null && _speed != 0)
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
            if (collider.tag == "Player")
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
        #endregion Private Methods
    }
}
