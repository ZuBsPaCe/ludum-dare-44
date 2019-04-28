using UnityEngine;
using UnityEngine.UI;
using zs.Helpers;
using zs.Logic;

namespace zs.Main
{
    public class LevelScreen : MonoBehaviour
    {
        #region Serializable Fields
        
        [SerializeField]
        private Image _life;

        #endregion Serializable Fields

        #region Private Vars

        private LifeCycle _cycle = LifeCycle.RedUp;
        private float _green = 255;
        private float _red = 0;
        private float _blue = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void StartLevel(int level)
        {

        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_life);
        }

        void Start()
        {
        }
	
        void Update()
        {
            Game.Instance.PerformLifeCycle(ref _cycle, ref _red, ref _green, ref _blue, ref _life);
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
