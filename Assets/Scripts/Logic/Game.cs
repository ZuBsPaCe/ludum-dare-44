using UnityEngine;
using zs.Helpers;

namespace zs.Logic
{
    public class Game : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Player _playerPrefab = null;


        [SerializeField]
        private Vector3 _spawnPointLevel1 = Vector3.zero;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars

        public static Game Instance { get; set; }

        #endregion Public Vars

        #region Public Methods

        public void KillPlayer(Player player)
        {
            player.Kill();

            Player newPlayer = Instantiate(_playerPrefab, _spawnPointLevel1, Quaternion.identity);

            Camera.main.GetComponent<CameraFollow>().FollowTarget = newPlayer.transform;
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_playerPrefab);

            Instance = this;
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
