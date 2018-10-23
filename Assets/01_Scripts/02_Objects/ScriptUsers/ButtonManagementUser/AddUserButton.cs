using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class AddUserButton : MonoBehaviour {

	[SerializeField]
	private InputField inputField;

	[SerializeField]
	private GameObject panelAddUser;

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
		if (GameManager.UserManager.AddUser (inputField.text)) 
		{
			Dropdown.OptionData optionData = new Dropdown.OptionData ();
			optionData.text = inputField.text;

			dropDown.options.Add (optionData);

			dropDown.captionText.text = optionData.text;
			dropDown.value = dropDown.options.Count - 1;
		}

		panelAddUser.SetActive (false);
	}

}
