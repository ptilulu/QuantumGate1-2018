using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RenameUserButton : MonoBehaviour {

	[SerializeField]
	private InputField inputField;

	[SerializeField]
	private GameObject panelRenameUser;

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

		if (GameManager.UserManager.RenameCurrentUser (inputField.text)) 
		{
			int index = dropDown.options.FindIndex (oD => string.Compare (oD.text, oldUserName) == 0);

			dropDown.options [index].text = inputField.text;

			dropDown.captionText.text = inputField.text;
		}
			
		panelRenameUser.SetActive (false);
	}
}
