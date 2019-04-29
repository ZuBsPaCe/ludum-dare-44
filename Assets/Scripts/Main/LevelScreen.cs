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
        private Status _statusLevel1 = null;

        [SerializeField]
        private Status _statusLevel2 = null;

        [SerializeField]
        private Status _statusLevel3 = null;

        [SerializeField]
        private Status _statusLevel4 = null;

        [SerializeField]
        private Status _statusLevel5 = null;

        [SerializeField]
        private Status _statusLevel6 = null;

        [SerializeField]
        private Status _statusLevel7 = null;

        [SerializeField]
        private Status _statusLevel8 = null;

        [SerializeField]
        private Status _statusLevel9 = null;

        [SerializeField]
        private Status _statusLevel10 = null;

        [SerializeField]
        private Status _statusLevel11 = null;

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

        [SerializeField]
        private FadeScreen _fadeScreen = null;

        [SerializeField]
        private AudioSource _audioSource = null;

        #endregion Serializable Fields

        #region Private Vars

        private LifeCycle _cycle = LifeCycle.RedUp;
        private float _green = 255;
        private float _red = 0;
        private float _blue = 0;

        private List<Button> _levelButtons = null;
        private List<Status> _levelStatuses = null;

        private int _startLevelAfterIntro = 0;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public void StartLevel(int level)
        {
            Sound.Instance.PlayButton(_audioSource);

            if (level == 1 && !PlayerPrefs.HasKey("Act1 Intro Shown"))
            {
                PlayerPrefs.SetInt("Act1 Intro Shown", 1);
                PlayerPrefs.Save();

                _startLevelAfterIntro = level;

                _fadeScreen.PerformFade(
                    "Act 1",
                    "A life's waste",
                    StartLevelAfterIntro);

            }
            else if (level == 5 && !PlayerPrefs.HasKey("Act2 Intro Shown"))
            {
                PlayerPrefs.SetInt("Act2 Intro Shown", 1);
                PlayerPrefs.Save();

                _startLevelAfterIntro = level;

                _fadeScreen.PerformFade(
                    "Act 2",
                    "Torn apart",
                    StartLevelAfterIntro);
            }
            else if (level == 9 && !PlayerPrefs.HasKey("Act3 Intro Shown"))
            {
                PlayerPrefs.SetInt("Act3 Intro Shown", 1);
                PlayerPrefs.Save();

                _startLevelAfterIntro = level;

                _fadeScreen.PerformFade(
                    "Act 3",
                    "Enlightenment",
                    StartLevelAfterIntro);
            }
            else
            {
                Game.Instance.LoadLevel(level);
            }
        }

        private void StartLevelAfterIntro()
        {
            Game.Instance.LoadLevel(_startLevelAfterIntro);
        }

        public void UnlockAct2()
        {
            Sound.Instance.PlayButton(_audioSource);

            int earnedHearts = PlayerPrefs.GetInt("EarnedHearts");
            PlayerPrefs.SetInt("EarnedHearts", earnedHearts - 10);

            PlayerPrefs.SetInt("Act2 Unlocked", 1);
            PlayerPrefs.Save();

            UpdatePlayerProgress();
        }

        public void UnlockAct3()
        {
            Sound.Instance.PlayButton(_audioSource);

            int earnedHearts = PlayerPrefs.GetInt("EarnedHearts");
            PlayerPrefs.SetInt("EarnedHearts", earnedHearts - 10);

            PlayerPrefs.SetInt("Act3 Unlocked", 1);
            PlayerPrefs.Save();

            UpdatePlayerProgress();
        }

        public void BackToStartScreen()
        {
            MainMenu.ForceStartScreen = true;
            _mainMenu.ShowStartScreen();
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

            Debug.Assert(_statusLevel1);
            Debug.Assert(_statusLevel2);
            Debug.Assert(_statusLevel3);
            Debug.Assert(_statusLevel4);
            Debug.Assert(_statusLevel5);
            Debug.Assert(_statusLevel6);
            Debug.Assert(_statusLevel7);
            Debug.Assert(_statusLevel8);
            Debug.Assert(_statusLevel9);
            Debug.Assert(_statusLevel10);
            Debug.Assert(_statusLevel11);

            Debug.Assert(_act2);
            Debug.Assert(_act2Unlock);
            Debug.Assert(_act3);
            Debug.Assert(_act3Unlock);

            Debug.Assert(_act2UnlockButton);
            Debug.Assert(_act3UnlockButton);

            Debug.Assert(_totalHeartsText);

            Debug.Assert(_fadeScreen);

            Debug.Assert(_audioSource);

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
            };

            _levelStatuses = new List<Status>
            {
                null,
                _statusLevel1,
                _statusLevel2,
                _statusLevel3,
                _statusLevel4,
                _statusLevel5,
                _statusLevel6,
                _statusLevel7,
                _statusLevel8,
                _statusLevel9,
                _statusLevel10,
                _statusLevel11,
            };
        }

        void Start()
        {
            UpdatePlayerProgress();
        }
	
        void Update()
        {
            Game.Instance.PerformLifeCycle(ref _cycle, ref _red, ref _green, ref _blue, ref _life);

#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainMenu.ForceStartScreen = true;
                _mainMenu.ShowStartScreen();
            }
#endif
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

            foreach (Status status in _levelStatuses)
            {
                if (status == null)
                {
                    continue;
                }

                string levelString = status.name.Split(' ')[1];
                int level = int.Parse(levelString);

                if (PlayerPrefs.HasKey("Level " + level))
                {
                    int deaths = PlayerPrefs.GetInt("Level " + level);
                    if (deaths == 0)
                    {
                        status.ShowCrown();
                    }
                    else
                    {
                        status.ShowHeart(deaths);
                    }

                    status.gameObject.SetActive(true);
                }
                else
                {
                    status.gameObject.SetActive(false);
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
