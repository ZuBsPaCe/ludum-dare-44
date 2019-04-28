using UnityEngine;

namespace zs.Helpers
{
    public class Conveyor : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private bool _CW = false;

        [SerializeField]
        private bool _CCW = false;

        [SerializeField]
        private bool _showGear = true;

        [SerializeField]
        private Collider2D _hackyHackHack = null;

        #endregion Serializable Fields

        #region Private Vars

        private SpriteRenderer _gearSpriteRenderer = null;
        private AreaEffector2D _areaEffector2D = null;

        private bool _hackTriggerOn = true;
        private float _hackTriggerOnTime = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void Stop()
        {
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

        public void Reverse()
        {
            if (!_CW && !_CCW)
            {
                return;
            }

            _CW = !_CW;
            _CCW = !_CCW;
            UpdateTag();
            UpdateAreaEffector();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_hackyHackHack);

            foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
            {
                if (child.name == "Gear")
                {
                    _gearSpriteRenderer = child;
                    break;
                }
            }

            if (_gearSpriteRenderer != null && !_showGear)
            {
                _gearSpriteRenderer.enabled = false;
            }

            _areaEffector2D = GetComponent<AreaEffector2D>();
            Debug.Assert(_areaEffector2D);

            UpdateTag();
            UpdateAreaEffector();
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (!_showGear || _gearSpriteRenderer == null || !_CCW && !_CW)
            {
                return;
            }

            if (_CW)
            {
                _gearSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -180 * Time.deltaTime) * _gearSpriteRenderer.transform.rotation;
            }
            else
            {
                _gearSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 180 * Time.deltaTime) * _gearSpriteRenderer.transform.rotation;
            }
        }

        void FixedUpdate()
        {
            if (_hackyHackHack && Time.time - _hackTriggerOnTime > 1f)
            {
                _hackTriggerOnTime = Time.time;
                _hackyHackHack.isTrigger = !_hackTriggerOn;
                _hackTriggerOn = !_hackTriggerOn;
            }

        }

        #endregion MonoBehaviour

        #region Private Methods

        private void UpdateTag()
        {
            if (!_CCW && !_CW)
            {
                gameObject.tag = "ConveyorIdle";
            }
            else if (_CCW)
            {
                gameObject.tag = "ConveyorCCW";
            }
            else
            {
                gameObject.tag = "ConveyorCW";
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
                gameObject.tag = "ConveyorCCW";
                _areaEffector2D.enabled = true;
                _areaEffector2D.forceAngle = 180f;
            }
            else
            {
                gameObject.tag = "ConveyorCW";
                _areaEffector2D.enabled = true;
                _areaEffector2D.forceAngle = 0f;
            }
            
        }

        #endregion Private Methods
    }
}
