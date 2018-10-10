using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Editor : MonoBehaviour
{

    public GameMode gm;

    public enum ClickPosition { TopLeft, TopRight, BotLeft, BotRight };

	private string _previousScene = "Menu";

    // UI
    [SerializeField]
    private List<GameObject> hiddenPanelAtStart;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private GameObject resultHeader;
    [SerializeField]
    private GameObject textHeader;
    [SerializeField]
    private GameObject gatesMenu;
    [SerializeField]
    private GameObject settingsPanel;

    
    [SerializeField]
    private GameObject gateMenu;
    [SerializeField]
    private GameObject customGateMenu;
    [SerializeField]
    private GameObject textCurrentCircuit;

    public GameObject background;

    public Camera cam;

    public GameObject GateNameInputPanel;
    public InputField GateNameInputField;

    [SerializeField]
    private GameObject sessionNameInputPanel;

    public InputField chooseSessionNameInput;


    public GameObject gridGame;

    public GameObject actionsGatePanel;
    public GameObject actionsGridPanel;

    private EditorState _currentState;
    public EditorState CurrentState
    {
        set
        {
            _currentState?.OnExit();
            _currentState = value;
            _currentState.OnEnter();
        }
        get
        {
            return _currentState;
        }
    }

    private List<QCS.Circuit> circuits;

    private int _currentCircuitIndex;
    public int CurrentCircuitIndex
    {
        set
        {
            _currentCircuitIndex = value;
            textCurrentCircuit.GetComponent<Text>().text = _currentCircuitIndex.ToString();
            gridBoard.LoadCircuit(currentCircuit);
        }
        get { return _currentCircuitIndex; }
    }
    public QCS.Circuit currentCircuit
    {
        get { return circuits[CurrentCircuitIndex]; }
    }

    /// <summary>
    ///  Liste des portes par défaut que peux avoir l'utilisateur
    /// </summary>
	private Dictionary<string, QCS.Gate> _defaultGates;

    /// <summary>
    ///  Liste des portes créé par l'utilisateur
    /// </summary>
	private Dictionary<string, QCS.Gate> _customGates;

    /// <summary>
    ///  Facade permettant la gestion de la grille de jeux de la scène Unity
    /// </summary>
    public GridBoard gridBoard;

    void Start()
    {
        HidePanels();
        
        _defaultGates = new Dictionary<string, QCS.Gate>();
        _customGates = new Dictionary<string, QCS.Gate>();

        GameMode.gates.ForEach(AddDefaultGate);
		GameMode.customGates.ForEach (AddCustomGateToMenu);

		circuits = GameMode.circuits;

        _currentCircuitIndex = 0;

        gridBoard = new GridBoard(gridGame, currentCircuit);
        gridBoard.PositionInitialCamera();

        
        if(Input.touchSupported)
            CurrentState = new Smartphone_States.DefaultState(this);
        else
            CurrentState = new PC_States.DefaultState(this);
        
        inputController = new InputController(this);
    }

    private InputController inputController;

    void Update()
    {
        if (CurrentState == null)
            return;

        inputController.Update();
    }

    /**
	 * ###############################################################################################
	 * ###############################################################################################
	 * ###############################################################################################
	 * ############################################# UI ##############################################
	 * ###############################################################################################
	 * ###############################################################################################
	 * ###############################################################################################
	 */

    private void HidePanels()
    {
        foreach (GameObject panel in hiddenPanelAtStart)
            panel.SetActive(false);
    }

    // Result panel

    public void ShowResultPanel(bool active)
    {
        resultPanel.SetActive(active);
    }

    public void SetResultHeader(string header)
    {
        textHeader.GetComponent<Text>().text = header;
    }

    public void SetResultText(string text)
    {
        resultHeader.GetComponent<Text>().text = text;
    }


    // Setting panel

    public void ShowSettingsPanel(bool active)
    {
        settingsPanel.SetActive(active);
    }

    public void ShowSessionNameInputPanel(bool active)
    {
        sessionNameInputPanel.SetActive(active);
    }


    // Gate panel

    public void ShowGatesMenuPanel(bool active)
    {
        if (active)
            gatesMenu.GetComponent<OpenCloseMenu>().Open();
        else
            gatesMenu.GetComponent<OpenCloseMenu>().Close();
    }

    public void AddDefaultGate(QCS.Gate gate)
    {
        _defaultGates.Add(gate.Name, gate);
        MakeGateButton(gate);
    }

    public void AddCustomGateToMenu(QCS.Gate gate)
    {
        if (!_customGates.ContainsKey(gate.Name))
        {
            _customGates.Add(gate.Name, gate);
            MakeCustomGateButton(gate);
        }
        else
        {
            _customGates.Remove(gate.Name);
            _customGates.Add(gate.Name, gate);
        }
    }

    public void MakeGateButton(QCS.Gate gate)
    {
        GameObject gateButton = Instantiate(
            Resources.Load("02_prefabs/BtnGate", typeof(GameObject)),
            gateMenu.transform) as GameObject;

        gateButton.name = gate.Name;

        Button buttonComponent = gateButton.GetComponent<Button>();

        buttonComponent.onClick.AddListener(delegate
        {
            CurrentState.OnSelectGate(gate);
        });

        gateButton.GetComponent<Text>().text = gate.Name;

        RectTransform rectTransform = gateButton.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(
            71.5f,
            (_defaultGates.Count) * -60,
            0);
    }

    public void MakeCustomGateButton(QCS.Gate gate)
    {
        GameObject gateButton = Instantiate(
            Resources.Load("02_prefabs/BtnGate", typeof(GameObject)),
            customGateMenu.transform) as GameObject;

        gateButton.name = gate.Name;

        Button buttonComponent = gateButton.GetComponent<Button>();

        buttonComponent.onClick.AddListener(delegate
        {
            CurrentState.OnSelectGate(_customGates[gate.Name]);
        });

        gateButton.GetComponent<Text>().text = gate.Name;

        RectTransform rectTransform = gateButton.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(
            71.5f,
            (_customGates.Count) * -60,
            0);
    }

    // Action gate/grid menu

    public void ShowActionsGatePanel(bool active)
    {
        actionsGatePanel.SetActive(active);
    }

    public void SetActionGatePanelPosition(Vector3 position)
    {
        actionsGatePanel.transform.position = position;
    }

    public void ShowActionsGridPanel(bool active)
    {
        actionsGridPanel.SetActive(active);
    }

    
    public void SetActionGridPanelPosition(Vector3 position)
    {
        actionsGridPanel.transform.position = position;
    }

    public Vector3 GetGridBoardPosition(Vector3 screenPosition)
    {
        Collider collider = background.GetComponent<Collider>();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (collider.Raycast(ray, out hit, 1000))
            return new Vector3(hit.point.x / 10, GridBoard.gateThikness, hit.point.y / 10);

        throw new Exception("Raycast entre Background et Camera échoué !");
    }
    
    public GameObject GetOnScreenObject(Vector3 onScreenPosition)
    {
        GameObject gameObject = null;
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(onScreenPosition);
        if (Physics.Raycast(ray, out hitInfo))
            gameObject = hitInfo.collider.gameObject;

        if (gameObject == null)
            return null;

        return gameObject;
    }

    public Tuple<int, int, Editor.ClickPosition> GetOnScreenGridPosition(Vector3 onScreenPosition)
    {
        GameObject gameObject = null;
        RaycastHit hitInfo;
        Ray ray = cam.ScreenPointToRay(onScreenPosition);
        if (Physics.Raycast(ray, out hitInfo))
            gameObject = hitInfo.collider.gameObject;

        if (gameObject == null)
            return null;
        if (gameObject.tag != "pipe" && gameObject.tag != "gate")
            return null;

        GateObject gateObject = gameObject.GetComponent<GateObject>();
        QCS.Circuit.GateStruct gateStruct = gateObject.gateStruct;

        int row = gateStruct.row;
        int col = gateStruct.col;
        int nb_entries = gateStruct.gate.NbEntries;

        Editor.ClickPosition position;
        Vector3 posOnCase = hitInfo.point - hitInfo.collider.transform.position;

        // if the gate has many entries we need to do some modulos ..
        if (nb_entries > 1)
        {
            posOnCase.x += (nb_entries / 2f) * GridBoard.realColumnWidth;
            col += (int)(posOnCase.x / GridBoard.realColumnWidth);
            posOnCase.x %= GridBoard.realColumnWidth;
            posOnCase.x -= GridBoard.realColumnWidth / 2;
        }

        if (posOnCase.y > 0)
            if (posOnCase.x < 0)
                position = Editor.ClickPosition.TopLeft;
            else
                position = Editor.ClickPosition.TopRight;
        else
            if (posOnCase.x < 0)
            position = Editor.ClickPosition.BotLeft;
        else
            position = Editor.ClickPosition.BotRight;

        return Tuple.Create(row, col, position);
    }
    
    public void BackToPreviousScene()
    {
        SceneManager.LoadScene(_previousScene);
    }
    
    public void PreviousCircuit()
    {
        if (CurrentCircuitIndex == 0)
            return;
        CurrentCircuitIndex--;
    }

    public void NextCircuit()
    {
        if (CurrentCircuitIndex == circuits.Count - 1)
            circuits.Add(GameMode.BaseCircuit());
        CurrentCircuitIndex++;
    }

	public void DeleteCurrentCircuit()
	{
		if (circuits.Count == 1)
			return;

		circuits.RemoveAt (CurrentCircuitIndex);
		gridBoard.LoadCircuit(currentCircuit);
	}

	/**
	 * ###############################################################################################
	 * ###############################################################################################
	 * ###############################################################################################
	 * ######################################### GETTERS #############################################
	 * ###############################################################################################
	 * ###############################################################################################
	 * ###############################################################################################
	 */

	public List<QCS.Circuit> GetCircuits()
	{
		return circuits;
	}

	public Dictionary<string, QCS.Gate> GetCustomGates()
	{
		return _customGates;
	}
}
