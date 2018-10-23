using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameButton : MonoBehaviour {

	[SerializeField]
	private GameObject chooseSavePanel;

	[SerializeField]
	private GameObject parentPanel;

	[SerializeField]
	private GameObject savesScrollViewContent;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public void OnClick()
	{
		if (GameManager.UserManager.GetCurrentUser ().GetNameOfSessionsSandBox ().Count == 0)
			return;

		parentPanel.SetActive (false);
		chooseSavePanel.SetActive (true);

		List<string> savesName = GameManager.UserManager.GetCurrentUser ().GetNameOfSessionsSandBox ();


		for (int indexChild = 0; indexChild < savesScrollViewContent.transform.childCount; indexChild++) 
			Destroy (savesScrollViewContent.transform.GetChild (indexChild));
		

		for (int indexSaveName = 0; indexSaveName < savesName.Count ; indexSaveName++) 
		{
			string saveName = savesName [indexSaveName];

			GameObject gateButton = Instantiate(
				Resources.Load("02_prefabs/BtnSaveMenu", typeof(GameObject)),
				savesScrollViewContent.transform) as GameObject;

			gateButton.name = "button_" + saveName;

			Button buttonComponent = gateButton.GetComponent<Button>();

			buttonComponent.onClick.AddListener(delegate
				{
					LoadSessionSandBox(saveName);
				});

			gateButton.GetComponent<Text>().text = saveName;

			RectTransform rectTransform = gateButton.GetComponent<RectTransform>();

			rectTransform.localPosition = new Vector3(
				75.0f, 
				(indexSaveName + 1) * -30.0f, 
				0.0f);
		}
	}

	public void LoadSessionSandBox(string name)
	{
		SandBoxSession session = GameManager.UserManager.GetCurrentUser ().GetSessionSandBox (name);

		GameMode.nameGame = name;
		GameMode.gates = new List<QCS.Gate>() { QCS.Gate.NOT, QCS.Gate.CONTROL, QCS.Gate.SWAP, QCS.Gate.HADAMARD };
		GameMode.customGates = SerializableClass.Helper.ToUsableGates(session.customGates);
		GameMode.circuits = SerializableClass.Helper.ToUsableCircuits(session.circuits, GameMode.gates, GameMode.customGates);

		SceneManager.LoadScene("SandBox", LoadSceneMode.Single);
	}
}
