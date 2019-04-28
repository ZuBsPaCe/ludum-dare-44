using UnityEngine;
using UnityEngine.Tilemaps;

namespace zs.Levels
{
    public class Level3 : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Tilemap _levelTilemap = null;

        [SerializeField]
        private TileBase _wallTile = null;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void OpenWall()
        {
            _levelTilemap.SetTile(new Vector3Int(41, -3, 0), null);
            _levelTilemap.SetTile(new Vector3Int(41, -2, 0), null);
            _levelTilemap.SetTile(new Vector3Int(41, -1, 0), null);
            _levelTilemap.SetTile(new Vector3Int(41, 0, 0), null);
            _levelTilemap.SetTile(new Vector3Int(41, 1, 0), null);
            _levelTilemap.SetTile(new Vector3Int(41, 2, 0), null);
        }

        public void CloseWall()
        {
            _levelTilemap.SetTile(new Vector3Int(41, -3, 0), _wallTile);
            _levelTilemap.SetTile(new Vector3Int(41, -2, 0), _wallTile);
            _levelTilemap.SetTile(new Vector3Int(41, -1, 0), _wallTile);
            _levelTilemap.SetTile(new Vector3Int(41, 0, 0), _wallTile);
            _levelTilemap.SetTile(new Vector3Int(41, 1, 0), _wallTile);
            _levelTilemap.SetTile(new Vector3Int(41, 2, 0), _wallTile);
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_levelTilemap);
        }

        void Start()
        {
        }
	
        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
