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
	#endregion Public Vars

	#region Public Methods

    public void ShowStartScreen()
    {
        _startScreen.SetActive(true);
        _levelScreen.SetActive(false);
    }

    public void ShowLevelScreen()
    {
        _startScreen.SetActive(false);
        _levelScreen.SetActive(true);
    }

	#endregion Public Methods

	#region MonoBehaviour
	
	void Awake()
	{
        Debug.Assert(_startScreen);
        Debug.Assert(_levelScreen);
        Debug.Assert(_fadeScreen);

        _startScreen.SetActive(true);
        _levelScreen.SetActive(false);
        _fadeScreen.gameObject.SetActive(false);
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
