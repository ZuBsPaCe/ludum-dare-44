using UnityEngine;
using zs.Helpers;
using zs.Logic;

namespace zs.Main
{
    [CreateAssetMenu(menuName = "GameInfo", fileName = "Create GameInfo", order = 1)]
    public class GameInfo : ScriptableObject
    {
        #region Private Vars

        [SerializeField]
        public Player PlayerPrefab = null;

        [SerializeField]
        public LevelInfo[] LevelInfos = null;

        #endregion Private Vars

        #region Public Vars

        #endregion Public Vars

        #region Public Methods

        #endregion Public Methods

        #region MonoBehaviour

        void Awake()
        {
            Debug.Assert(PlayerPrefab);
            Debug.Assert(LevelInfos != null && LevelInfos.Length > 0);
        }

        #endregion MonoBehaviour

        #region Private Methods

        #endregion Private Methods
    }
}
