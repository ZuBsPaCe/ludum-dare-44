using System.Collections.Generic;
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
        private Image _life = null;

        [SerializeField]
        private MainMenu _mainMenu = null;

        [SerializeField]
        private Button _act1_level1 = null;
        [SerializeField]
        private Button _act1_level2 = null;
        [SerializeField]
        private Button _act1_level3 = null;
        [SerializeField]
        private Button _act1_level4 = null;

        [SerializeField]
        private Button _act2_level1 = null;
        [SerializeField]
        private Button _act2_level2 = null;
        [SerializeField]
        private Button _act2_level3 = null;
        [SerializeField]
        private Button _act2_level4 = null;

        [SerializeField]
        private Button _act3_level1 = null;
        [SerializeField]
        private Button _act3_level2 = null;
        [SerializeField]
        private Button _act3_level3 = null;
        [SerializeField]
        private Button _act3_level4 = null;

        [SerializeField]
        private RectTransform _act2 = null;

        [SerializeField]
        private RectTransform _act2Unlock = null;

        [SerializeField]
        private RectTransform _act3 = null;

        [SerializeField]
        private RectTransform _act3Unlock = null;

        [SerializeField]
        private Button _act2UnlockButton = null;

        [SerializeField]
        private Button _act3UnlockButton = null;

        [SerializeField]
        private Text _totalHeartsText = null;

        #endregion Serializable Fields

        #region Private Vars

        private LifeCycle _cycle = LifeCycle.RedUp;
        private float _green = 255;
        private float _red = 0;
        private float _blue = 0;

        private List<Button> _levelButtons = null;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void StartLevel(int level)
        {
            Game.Instance.LoadLevel(level);
        }

        public void UnlockAct2()
        {
            int earnedHearts = PlayerPrefs.GetInt("EarnedHearts");
            PlayerPrefs.SetInt("EarnedHearts", earnedHearts - 10);

            PlayerPrefs.SetInt("Act2 Unlocked", 1);
            PlayerPrefs.Save();

            UpdatePlayerProgress();
        }

        public void UnlockAct3()
        {
            int earnedHearts = PlayerPrefs.GetInt("EarnedHearts");
            PlayerPrefs.SetInt("EarnedHearts", earnedHearts - 10);

            PlayerPrefs.SetInt("Act3 Unlocked", 1);
            PlayerPrefs.Save();

            UpdatePlayerProgress();
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_life);
            Debug.Assert(_mainMenu);

            Debug.Assert(_act1_level1);
            Debug.Assert(_act1_level2);
            Debug.Assert(_act1_level3);
            Debug.Assert(_act1_level4);

            Debug.Assert(_act2_level1);
            Debug.Assert(_act2_level2);
            Debug.Assert(_act2_level3);
            Debug.Assert(_act2_level4);

            Debug.Assert(_act3_level1);
            Debug.Assert(_act3_level2);
            Debug.Assert(_act3_level3);
            Debug.Assert(_act3_level4);

            Debug.Assert(_act2);
            Debug.Assert(_act2Unlock);
            Debug.Assert(_act3);
            Debug.Assert(_act3Unlock);

            Debug.Assert(_act2UnlockButton);
            Debug.Assert(_act3UnlockButton);

            Debug.Assert(_totalHeartsText);

            _levelButtons = new List<Button>
            {
                null,
                _act1_level1,
                _act1_level2,
                _act1_level3,
                _act1_level4,

                _act2_level1,
                _act2_level2,
                _act2_level3,
                _act2_level4,

                _act3_level1,
                _act3_level2,
                _act3_level3,
                _act3_level4
            };
        }

        void Start()
        {
            UpdatePlayerProgress();
        }
	
        void Update()
        {
            Game.Instance.PerformLifeCycle(ref _cycle, ref _red, ref _green, ref _blue, ref _life);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainMenu.ForceStartScreen = true;
                _mainMenu.ShowStartScreen();
            }
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void UpdatePlayerProgress()
        {
            foreach (Button levelButton in _levelButtons)
            {
                if (levelButton == null)
                {
                    continue;
                }

                string levelString = levelButton.name.Split(' ')[1];
                int level = int.Parse(levelString);

                if (PlayerPrefs.HasKey("Level " + (level - 1)))
                {
                    levelButton.interactable = true;
                }
                else
                {
                    levelButton.interactable = false;
                }
            }

            _act2.gameObject.SetActive(PlayerPrefs.HasKey("Act2 Unlocked"));
            _act2Unlock.gameObject.SetActive(!PlayerPrefs.HasKey("Act2 Unlocked"));

            _act3.gameObject.SetActive(PlayerPrefs.HasKey("Act3 Unlocked"));
            _act3Unlock.gameObject.SetActive(!PlayerPrefs.HasKey("Act3 Unlocked"));

            int totalHearts = Game.Instance.TotalHearts;
            _totalHeartsText.text = totalHearts.ToString();

            _act2UnlockButton.interactable = PlayerPrefs.HasKey("Level 4") && totalHearts >= 10;

            _act3UnlockButton.interactable = PlayerPrefs.HasKey("Level 8") && totalHearts >= 10;
        }

        #endregion Private Methods
    }
}
