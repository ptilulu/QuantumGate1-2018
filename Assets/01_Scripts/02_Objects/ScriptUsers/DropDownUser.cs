using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class DropDownUser : MonoBehaviour
{

	[SerializeField]
	private Dropdown dropDown;

	// Use this for initialization
	IEnumerator Start ()
    {
        yield return new WaitForEndOfFrame();

        foreach (string userName in GameManager.UserManager.GetAllNameUsers()) 
		{
            Dropdown.OptionData optionData = new Dropdown.OptionData
            {
                text = userName
            };

            dropDown.options.Add (optionData);
		}

		dropDown.captionText.text = dropDown.options [0].text;

		dropDown.onValueChanged.AddListener (delegate {
			DropDownValueChanged(dropDown);
		});

	}

	private void DropDownValueChanged(Dropdown change)
	{
		string userName = dropDown.options [change.value].text;

		GameManager.UserManager.LoadUser (userName);
	}
}
