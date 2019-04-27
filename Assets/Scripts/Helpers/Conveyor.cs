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

        #endregion Serializable Fields

        #region Private Vars

        private SpriteRenderer _gearSpriteRenderer = null;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

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
            UpdateTag();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
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

            UpdateTag();
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
        #endregion Private Methods
    }
}
