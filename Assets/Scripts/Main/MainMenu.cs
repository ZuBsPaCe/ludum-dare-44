using UnityEngine;
using zs.Logic;

namespace zs.Main
{
    public class MainMenu : MonoBehaviour
    {
        #region Serializable Fields
        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void OnStartButton_Click()
        {
            Game.Instance.LoadLevel(0);
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
        }

        void Start()
        {
        }
	
        void Update()
        {
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
