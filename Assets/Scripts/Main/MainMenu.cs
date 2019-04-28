using UnityEngine;
using zs.Helpers;

public class MainMenu : MonoBehaviour
{
	#region Serializable Fields

    [SerializeField]
    private GameObject _startScreen = null;

    [SerializeField]
    private GameObject _levelScreen = null;

    [SerializeField]
    private FadeScreen _fadeScreen = null;

	#endregion Serializable Fields

	#region Private Vars
	#endregion Private Vars

	#region Public Vars

    public static bool ForceStartScreen = true;

	#endregion Public Vars

	#region Public Methods

    public void ShowStartScreen()
    {
        _startScreen.SetActive(true);
        _levelScreen.SetActive(false);

        ForceStartScreen = false;
    }

    public void ShowLevelScreen()
    {
        _startScreen.SetActive(false);
        _levelScreen.SetActive(true);

        ForceStartScreen = false;
    }

	#endregion Public Methods

	#region MonoBehaviour
	
	void Awake()
	{
        Debug.Assert(_startScreen);
        Debug.Assert(_levelScreen);
        Debug.Assert(_fadeScreen);

        if (ForceStartScreen || !PlayerPrefs.HasKey("Level 0"))
        {
            _startScreen.SetActive(true);
            _levelScreen.SetActive(false);
        }
        else
        {
            _startScreen.SetActive(false);
            _levelScreen.SetActive(true);
        }

        _fadeScreen.gameObject.SetActive(false);

        ForceStartScreen = false;
    }

	void Start()
	{
	}
	
	void Update()
	{
        if (_fadeScreen.Fading)
        {
            return;
        }

        #if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(0);
        }
        #endif
	}

	#endregion MonoBehaviour

	#region Private Methods
	#endregion Private Methods
}
