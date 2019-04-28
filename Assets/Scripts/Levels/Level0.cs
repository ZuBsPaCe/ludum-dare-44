using UnityEngine;
using UnityEngine.Tilemaps;
using zs.Helpers;

namespace zs.Levels
{
    public class Level0 : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Portal _portal = null;

        [SerializeField]
        private Tilemap _tilemap = null;

        #endregion Serializable Fields

        #region Private Vars

        private bool _outroStarted = false;
        private float _outroStartTime = 0;
        private int _sequence = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void StartOutro()
        {
            if (_outroStarted)
            {
                return;
            }

            _outroStarted = true;
            _outroStartTime = Time.time;
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_portal);
            Debug.Assert(_tilemap);
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (!_outroStarted)
            {
                return;
            }

            if (_sequence == 0 && Time.time - _outroStartTime > 2)
            {
                _tilemap.SetTile(new Vector3Int(52, 8, 0), null);
                _tilemap.SetTile(new Vector3Int(57, 8, 0), null);
                _sequence = 1;
            }
            else if (_sequence == 1 && Time.time - _outroStartTime > 3)
            {
                _tilemap.SetTile(new Vector3Int(53, 8, 0), null);
                _tilemap.SetTile(new Vector3Int(56, 8, 0), null);
                _sequence = 2;
            }
            else if (_sequence == 2 && Time.time - _outroStartTime > 4)
            {
                _tilemap.SetTile(new Vector3Int(54, 8, 0), null);
                _tilemap.SetTile(new Vector3Int(55, 8, 0), null);
                _sequence = 3;
            }
            else if (_sequence == 3 && Time.time - _outroStartTime > 6)
            {
                _portal.PerformPortal();
                _sequence = 4;
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
