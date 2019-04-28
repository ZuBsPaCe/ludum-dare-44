using UnityEngine;
using zs.Logic;

public class JumpPad : MonoBehaviour
{
	#region Serializable Fields
	#endregion Serializable Fields

	#region Private Vars
	#endregion Private Vars

	#region Public Vars
	#endregion Public Vars

	#region Public Methods
	#endregion Public Methods

	#region MonoBehaviour
	
	void Awake()
	{
	}

	void Start()
	{
	}

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "Player")
        {
            //Debug.Log("JumpPad");
            Game.Instance.CurrentPlayer.JumpPad();
        }
    }

    #endregion MonoBehaviour

	#region Private Methods
	#endregion Private Methods
}
