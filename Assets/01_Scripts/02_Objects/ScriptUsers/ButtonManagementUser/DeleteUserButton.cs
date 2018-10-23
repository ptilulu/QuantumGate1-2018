using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class DeleteUserButton : MonoBehaviour {

	[SerializeField]
	private GameObject panelRemoveUser;

	[SerializeField]
	private Dropdown dropDown;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		string oldUserName = GameManager.UserManager.GetCurrentUser ().nameUser;

		if (GameManager.UserManager.RemoveCurrentUser()) 
		{
			int index = dropDown.options.FindIndex (oD => string.Compare (oD.text, oldUserName) == 0);

			dropDown.options.RemoveAt (index);

			dropDown.captionText.text = dropDown.options[0].text;
			dropDown.value = 0;
		}

		panelRemoveUser.SetActive (false);
	}
}
