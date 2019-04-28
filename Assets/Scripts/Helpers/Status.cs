using UnityEngine;
using UnityEngine.UI;

namespace zs.Helpers
{
    public class Status : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Image _heart = null;

        [SerializeField]
        private Text _deathCount = null;

        [SerializeField]
        private Image _crown = null;

        #endregion Serializable Fields

        #region Private Vars
        #endregion Private Vars

        #region Public Vars

        public void ShowHeart(int deaths)
        {
            _heart.gameObject.SetActive(true);
            _deathCount.text = "- " + deaths;
            _deathCount.gameObject.SetActive(true);

            _crown.gameObject.SetActive(false);
        }

        public void ShowCrown()
        {
            _heart.gameObject.SetActive(false);
            _deathCount.gameObject.SetActive(false);
            _crown.gameObject.SetActive(true);
        }

        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_heart);
            Debug.Assert(_crown);
            Debug.Assert(_deathCount);
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
