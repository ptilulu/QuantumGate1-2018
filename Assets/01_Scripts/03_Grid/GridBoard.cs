using System.Collections.Generic;
using UnityEngine;

using QCS;
using System.Collections;

/// <summary>
///  S'occupe de la gestion de la grille de jeu sur le "front-end".
///  Elle permet de déplacer les éléments graphiques du jeu, de les supprimer, ou d'en ajouter.
/// </summary>
public class GridBoard
{
	private Circuit _circuit;

    private GameObject _gridGame;

    /// <summary>
    /// Dictionnaire associant les EntryStruct du Circuit aux GameObject du GridBoard.
    /// (car on ne veut pas cette association dans la structure de données Circuit)
    /// </summary>
    public Dictionary<Circuit.EntryStruct, EntryObject> entryObjects;
    /// <summary>
    /// Dictionnaire associant les GateStruct du Circuit aux GameObject du GridBoard.
    /// (car on ne veut pas cette association dans la structure de données Circuit)
    /// </summary>
    public Dictionary<Circuit.GateStruct, GateObject> gateObjects;

    public static readonly float localRowHeight = 2f;
    public static readonly float localColWidth = 3f;

    public static readonly float pipeDiameter = 0.7f;

    public static readonly float gateHeightRatio = 0.6f;
    public static readonly float gateSideSpace = 0.4f;
    public static readonly float gateThikness = 0.8f;

    /* approximations pour la projection sur le canvas, à vérifier */
    public static readonly float realRowHeight = 100 * localRowHeight;
    public static readonly float realColumnWidth = 100 * localColWidth;

    public static readonly float animationTime = 0.5f;

    public GridBoard(GameObject gridGame, Circuit circuit)
    {
        _gridGame = gridGame;

        entryObjects = new Dictionary<Circuit.EntryStruct, EntryObject>();
        gateObjects = new Dictionary<Circuit.GateStruct, GateObject>();

        this._circuit = circuit;

        LoadCircuit(circuit);

        AttachEvents(circuit);
    }
    public GridBoard(GameObject gridGame) : this(gridGame, GameMode.BaseCircuit()) { }

    /**
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ############################################ GAME OBJECTS ############################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Calcule la position locale du centre de la case de la grille décrite par les paramètres
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private Vector3 ComputeGridPosition(int row, int col)
    {
        return new Vector3(-((_circuit.NbCol - 1) * localColWidth / 2) + localColWidth * col, 0, -localRowHeight * row);
    }

    /// <summary>
    /// Calcule les coordonnées de la case de la grille correspondant à la position donnée en paramètre
    /// </summary>
    /// <param name="position"></param>
    public System.Tuple<int, int> GetGridCoordinates(Vector3 position)
    {
        int col = Mathf.RoundToInt((position.x + ((_circuit.NbCol - 1) * localColWidth / 2)) / localColWidth);
        int row = Mathf.RoundToInt(position.z / -localRowHeight);

        return new System.Tuple<int, int>(row, col);
    }

    /**
	 * ######################################################################################################
	 * ############################################### ENTRY ################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Crée le GameObject correspondant à une entrée (qubit).
    /// </summary>
    /// <param name="entryStruct"></param>
    private EntryObject CreateEntryObject(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = ObjectFactory.ENTRY(_gridGame.transform);

        entryObject.entryStruct = entryStruct;

        return entryObject;
    }

    /// <summary>
    /// Actualise le material de l'objet correspondant à une entrée (qubit).
    /// </summary>
    /// <param name="entry"></param>
    private void SetEntryMaterial(Circuit.EntryStruct entryStruct)
    {
        Qubit qubit = entryStruct.qubit;

        EntryObject entryObject = entryObjects[entryStruct];
        GameObject entryGameObject = entryObject.entry;
        Renderer renderer = entryGameObject.GetComponent<Renderer>();

        if (qubit.Equals(Qubit.One))
            renderer.material = ObjectFactory.materialQubitOne;
        else
            renderer.material = ObjectFactory.materialQubitZero;
    }

    /// <summary>
    /// Renvoie la position que doit occuper l'objet représentant l'entry correspondante.
    /// </summary>
    /// <param name="entryStruct"></param>
    /// <returns></returns>
    private Vector3 ComputeEntryPosition(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = entryObjects[entryStruct];

        int col = entryStruct.col;

        return new Vector3(-((_circuit.NbCol - 1) * localColWidth / 2) + localColWidth * col, 0, 3);
    }

    /// <summary>
    /// Calcule et positionne une entry à sa nouvelle position.
    /// </summary>
    /// <param name="entryStruct"></param>
    private void PositionEntry(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = entryObjects[entryStruct];
        GameObject entryGameObject = entryObject.root;

        Vector3 newPosition = ComputeEntryPosition(entryStruct);

        entryGameObject.transform.localPosition = newPosition;
    }

    /// <summary>
    /// Clacule et fait transiter une entry vers sa nouvelle position.
    /// </summary>
    /// <param name="entryStruct"></param>
    private void TransitionEntry(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = entryObjects[entryStruct];
        GameObject entryGameObject = entryObject.root;

        Vector3 newPosition = ComputeEntryPosition(entryStruct);

        AnimationManager.Move(entryGameObject, newPosition, animationTime);
    }

    /**
	 * ######################################################################################################
	 * ############################################### PIPES ################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Calcule et positionne les tuyaux d'une porte à leurs nouvelles positions
    /// </summary>
    /// <param name="gateStruct"></param>
    private void PositionPipes(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = gateObjects[gateStruct];

        int row = gateStruct.row;
        int col = gateStruct.col;

        for (int i = 0; i < gateObject.pipes.Count; i++)
        {
            Vector3 newPosition = ComputeGridPosition(row, col + i);
            gateObject.pipes[i].transform.localPosition = newPosition;
        }
    }

    /// <summary>
    /// Calcule et fait transiter les tuyaux d'une porte vers leurs nouvelles positions.
    /// </summary>
    /// <param name="gateStruct"></param>
    private void TransitionPipes(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = gateObjects[gateStruct];

        int row = gateStruct.row;
        int col = gateStruct.col;

        for (int i = 0; i < gateObject.pipes.Count; i++)
        {
            Vector3 newPosition = ComputeGridPosition(row, col + i);
            AnimationManager.Move(gateObject.pipes[i], newPosition, animationTime);
        }
    }

    /**
	 * ######################################################################################################
	 * ################################################ GATE ################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Calcule la position que doit occuper l'objet représentant la porte correspondante.
    /// </summary>
    /// <param name="gateObject"></param>
    private Vector3 ComputeGatePosition(GateObject gateObject)
    {
        Circuit.GateStruct gateStruct = gateObject.gateStruct;
        int row = gateStruct.row;
        int col = gateStruct.col;
        int nbEntries = gateStruct.gate.NbEntries;

        float col_offset = (nbEntries - 1) / 2f;

        Vector3 newPosition = new Vector3(-((_circuit.NbCol - 1) * localColWidth / 2) + localColWidth * (col + col_offset), 0, -localRowHeight * row + localRowHeight / 2 - gateHeightRatio * localRowHeight / 2);

        return newPosition;
    }

    /// <summary>
    /// Crée le GameObject correspondant à une porte.
    /// </summary>
    /// <param name="gateStruct"></param>
    private GateObject CreateGateObject(Circuit.GateStruct gateStruct)
    {
        Gate gate = gateStruct.gate;

        GateObject gateObject = ObjectFactory.Build(gate, _gridGame.transform);

        gateObject.gateStruct = gateStruct;

        return gateObject;
    }

    /// <summary>
    /// Calcule et positionne une porte vers sa nouvelle position.
    /// </summary>
    /// <param name="gateStruct"></param>
    private void PositionGate(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = gateObjects[gateStruct];
        GameObject gateGameObject = gateObject.body;

        if (gateGameObject == null)
            return;

        Vector3 newPosition = ComputeGatePosition(gateObject);

        gateGameObject.transform.localPosition = newPosition;
    }

    /// <summary>
    /// Calcule et fait transiter une porte vers sa nouvelle position.
    /// </summary>
    /// <param name="gateStruct"></param>
    private void TransitionGate(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = gateObjects[gateStruct];
        GameObject gateGameObject = gateObject.body;

        if (gateGameObject == null)
            return;

        Vector3 newPosition = ComputeGatePosition(gateObject);

        AnimationManager.Move(gateGameObject, newPosition, animationTime);
    }


    /// <summary>
    /// Crée le GameObject correspondant à une entrée (qubit) et l'ajoute au dictionnaire associant les entrées et leur object.
    /// </summary>
    /// <param name="entryStruct"></param>
    private void AddEntry(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = CreateEntryObject(entryStruct);

        entryObjects.Add(entryStruct, entryObject);
    }
    /// <summary>
    /// Détruis le GameObject correspondant à une entrée et retire leur association du dictionnaire.
    /// </summary>
    /// <param name="entryStruct"></param>
    private void RemoveEntry(Circuit.EntryStruct entryStruct)
    {
        EntryObject entryObject = entryObjects[entryStruct];

        entryObjects.Remove(entryStruct);

        entryObject.FadeOutAndDestroy();
    }

    /// <summary>
    /// Crée le GameObject correspondant à une porte, l'ajoute au dictionnaire associant les entrées et leur object et met à jour sa position.
    /// </summary>
    /// <param name="gateStruct"></param>
    private void AddGate(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = CreateGateObject(gateStruct);

        gateObjects.Add(gateStruct, gateObject);
    }
    /// <summary>
    /// Détruis le GameObject correspondant à une porte et retire leur association du dictionnaire.
    /// </summary>
    /// <param name="gateStruct"></param>
    private void RemoveGate(Circuit.GateStruct gateStruct)
    {
        GateObject gateObject = gateObjects[gateStruct];

        gateObjects.Remove(gateStruct);

        gateObject.FadeOutAndDestroy();
    }

    /**
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ############################################## CIRCUIT ###############################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Détruis tous les GameObjects du GridBoard et vide les structures de données les stockant.
    /// </summary>
    /// <param name="entryStruct"></param>
    private void ClearGameObjects()
    {
        foreach (EntryObject entryObject in entryObjects.Values)
            entryObject.Destroy();
        entryObjects.Clear();

        foreach (GateObject gateObject in gateObjects.Values)
            gateObject.Destroy();
        gateObjects.Clear();
    }

    /// <summary>
    /// Vide le gridBoard puis crée les GameObjects corresponhdants à un circuit et attache les evennement de mise à jour au circuit.
    /// </summary>
    /// <param name="circuit"></param>
    public void LoadCircuit(Circuit circuit)
    {
        ClearGameObjects();

        if (this._circuit != null)
            ClearEvents(this._circuit);

        this._circuit = circuit;

        circuit.ForEachEntryStruct((Circuit.EntryStruct entryStruct) =>
        {
            AddEntry(entryStruct);
            SetEntryMaterial(entryStruct);
            PositionEntry(entryStruct);
        });

        circuit.ForEachGateStruct((Circuit.GateStruct gateStruct) =>
        {
            AddGate(gateStruct);
            PositionGate(gateStruct);
            PositionPipes(gateStruct);
        });

        AttachEvents(circuit);
    }

    /**
	 * ######################################################################################################
	 * ############################################## EVENTS ################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Detache tous les evennements du circuit passé en paramètre.
    /// </summary>
    /// <param name="circuit"></param>
    private void ClearEvents(Circuit circuit)
    {
        circuit.OnSetEntry = null;

        circuit.OnCreateGate = null;
        circuit.OnPutGate = null;
        circuit.OnRemoveGate = null;
        circuit.OnMoveGate = null;

        circuit.OnInsertRow = null;
        circuit.OnMoveRow = null;
        circuit.OnRemoveRow = null;

        circuit.OnInsertCol = null;
        circuit.OnMoveCol = null;
        circuit.OnRemoveCol = null;
    }

    /// <summary>
    /// Attache les evennement qui mettront à jour l'affichage du circuit.
    /// </summary>
    /// <param name="circuit"></param>
    private void AttachEvents(Circuit circuit)
    {
        circuit.OnSetEntry = SetEntryMaterial;

        circuit.OnCreateGate = AddGate;
        circuit.OnPutGate = delegate (Circuit.GateStruct gateStruct)
        {
            PositionGate(gateStruct);
            PositionPipes(gateStruct);

            if (gateStruct.gate.Equals(QCS.Gate.IDENTITY))
                return;

            int row = gateStruct.row;

            if (row == circuit.NbRow - 1)
                circuit.InsertRow(circuit.NbRow);
        };
        circuit.OnRemoveGate = delegate (Circuit.GateStruct gateStruct)
        {
            RemoveGate(gateStruct);

            if (gateStruct.gate.Equals(QCS.Gate.IDENTITY))
                return;

            for (int i = circuit.NbRow - 1; i > 0 && circuit.IsRowEmpty(i - 1); i--)
                circuit.RemoveRow(i);
        };
        circuit.OnMoveGate = delegate (Circuit.GateStruct gateStruct)
        {
            PositionPipes(gateStruct);
            TransitionGate(gateStruct);

            if (gateStruct.gate.Equals(QCS.Gate.IDENTITY))
                return;

            int row = gateStruct.row;

            if (row == circuit.NbRow - 1)
                circuit.InsertRow(circuit.NbRow);
            else
                for (int i = circuit.NbRow - 1; i > 0 && circuit.IsRowEmpty(i - 1); i--)
                    circuit.RemoveRow(i);
        };

        circuit.OnInsertRow = delegate (List<Circuit.GateStruct> gateStructs)
        {
            gateStructs.ForEach((Circuit.GateStruct gateStruct) =>
            {
                PositionPipes(gateStruct);
                PositionGate(gateStruct);
            });

            int row = gateStructs[0].row;

            for (int i = row + 1; i < circuit.NbRow; i++)
            {
                circuit.GetRow(i).ForEach((Circuit.GateStruct gateStruct) =>
                {
                    TransitionPipes(gateStruct);
                    TransitionGate(gateStruct);
                });
            }
        };
        circuit.OnRemoveRow = delegate (List<Circuit.GateStruct> gateStructs)
        {
            gateStructs.ForEach(RemoveGate);

            int row = gateStructs[0].row;

            for (int i = row; i < circuit.NbRow; i++)
            {
                circuit.GetRow(i).ForEach((Circuit.GateStruct gateStruct) =>
                {
                    TransitionPipes(gateStruct);
                    TransitionGate(gateStruct);
                });
            }
        };
        circuit.OnMoveRow = delegate (List<Circuit.GateStruct> gateStructs)
        {
            gateStructs.ForEach((Circuit.GateStruct gateStruct) =>
            {
                TransitionPipes(gateStruct);
                TransitionGate(gateStruct);
            });
        };

        circuit.OnInsertCol = delegate (Circuit.EntryStruct entryStruct, List<Circuit.GateStruct> gateStructs)
        {
            AddEntry(entryStruct);
            PositionEntry(entryStruct);
            gateStructs.ForEach((Circuit.GateStruct gateStruct) =>
            {
                PositionPipes(gateStruct);
                PositionGate(gateStruct);
            });

            for (int i = 0; i < circuit.NbCol; i++)
            {
                if (i == entryStruct.col)
                    continue;

                TransitionEntry(circuit.GetEntry(i));
                circuit.GetCol(i).ForEach((Circuit.GateStruct gateStruct) =>
                {
                    TransitionPipes(gateStruct);
                    TransitionGate(gateStruct);
                });
            }
        };
        circuit.OnRemoveCol = delegate (Circuit.EntryStruct entryStruct, List<Circuit.GateStruct> gateStructs)
        {
            RemoveEntry(entryStruct);
            gateStructs.ForEach(RemoveGate);

            circuit.ForEachEntryStruct(TransitionEntry);
            circuit.ForEachGateStruct((Circuit.GateStruct gateStruct) =>
            {
                TransitionPipes(gateStruct);
                TransitionGate(gateStruct);
            });

            for (int i = circuit.NbRow - 1; i > 0 && circuit.IsRowEmpty(i - 1); i--)
                circuit.RemoveRow(i);
        };
        circuit.OnMoveCol = delegate (Circuit.EntryStruct entryStruct, List<Circuit.GateStruct> gateStructs)
        {
            TransitionEntry(entryStruct);
            gateStructs.ForEach((Circuit.GateStruct gateStruct) =>
            {
                TransitionPipes(gateStruct);
                TransitionGate(gateStruct);
            });
        };
    }

    /**
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ####################################### INTERACTIONS WITH COLS #######################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 */

    /// <summary>
    ///  Echange en 0 ou 1 la valeur d'une entrée (qubit).
    /// </summary>
    public void SwapEntryValue(Circuit.EntryStruct entryStruct)
    {
        Qubit qubit = entryStruct.qubit;
        int col = entryStruct.col;

        if (qubit.Equals(Qubit.Zero))
            _circuit.SetEntry(col, Qubit.One);
        else
            _circuit.SetEntry(col, Qubit.Zero);
    }

    public void SelectCol(int col)
    {
        EntryObject entryObject = GetEntryObject(col);
        entryObject.Select();

        for (int i = 0; i < _circuit.NbRow; i++)
        {
            GateObject gateObject = GetGateObject(i, col);
            gateObject.Select();
        }
    }

    public void DeselectCol(int col)
    {
        EntryObject entryObject = GetEntryObject(col);
        entryObject.Deselect();

        for (int i = 0; i < _circuit.NbRow; i++)
        {
            GateObject gateObject = GetGateObject(i, col);
            gateObject.Deselect();
        }
    }

    public void SelectRow(int row)
    {
        for (int i = 0; i < _circuit.NbCol; i++)
        {
            GateObject gateObject = GetGateObject(row, i);
            gateObject.Select();
        }
    }

    public void DeselectRow(int row)
    {
        for (int i = 0; i < _circuit.NbCol; i++)
        {
            GateObject gateObject = GetGateObject(row, i);
            gateObject.Deselect();
        }
    }

    /**
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ############################################### CAMERA ###############################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 */

    /// <summary>
    ///  Centre la caméra horizontalement sur le circuit
    /// </summary>
    public void HorizontalCenterCircuit()
    {
        GameObject LeftmostGateObject = GetEntryObject(0).root;
        GameObject RightmostGateObject = GetEntryObject(_circuit.NbCol - 1).root;

        float center = (LeftmostGateObject.transform.position.x + RightmostGateObject.transform.position.x) / 2;

        Camera.main.transform.position = new Vector3(center, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    /// <summary>
    ///  Centre la caméra verticalement sur le circuit
    /// </summary>
    public void VerticalCenterCircuit()
    {
        GameObject TopmostGateObject = GetGateObject(0, 0).pipes[0];

        float center = TopmostGateObject.transform.position.y;

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, center, Camera.main.transform.position.z);
    }

    /// <summary>
    ///  Initialise la position de la camera
    /// </summary>
    public void PositionInitialCamera()
    {
        HorizontalCenterCircuit();
        VerticalCenterCircuit();
    }

    /**
	* ################################################################################################
	* ################################################################################################
	* ################################# METHODS FOR TEST BOUNDS CAMERA ###############################
	* ################################################################################################
	* ################################################################################################
	*/

    /// <summary>
    /// Test l'existence d'objet à gauche du hors champs de la caméra
    /// </summary>
    /// <returns><c>Vrai</c>, s'il y a des objets à gauche du hors champs de la caméra, <c>Faux</c> sinon.</returns>
    public bool CameraCanGoLeft()
    {
        return CameraLeftBound() > Camera.main.transform.position.x;
    }

    /// <summary>
    /// Test l'existence d'objet à droite du hors champs de la caméra
    /// </summary>
    /// <returns><c>Vrai</c>, s'il y a des objets à droite du hors champs de la caméra, <c>Faux</c> sinon.</returns>
    public bool CameraCanGoRight()
    {
        return CameraRightBound() < Camera.main.transform.position.x;
    }

    /// <summary>
    /// Test l'existence d'objet en haut du hors champs de la caméra
    /// </summary>
    /// <returns><c>Vrai</c>, s'il y a des objets en haut du hors champs de la caméra, <c>Faux</c> sinon.</returns>
    public bool CameraCanGoTop()
    {
        return CameraTopBound() > Camera.main.transform.position.y;
    }

    /// <summary>
    /// Test l'existence d'objet en bas du hors champs de la caméra
    /// </summary>
    /// <returns><c>Vrai</c>, s'il y a des objets en bas du hors champs de la caméra, <c>Faux</c> sinon.</returns>
    public bool CameraCanGoBot()
    {
        return CameraBotBound() < Camera.main.transform.position.y;
    }

    /**
	* ################################################################################################
	* ######################################## CAMERA BOUNDS #########################################
	* ################################################################################################
	*/

    public float CameraLeftBound()
    {
        /*float distanceFromGrid = Mathf.Abs(entryObjects[circuit.entries[0]].transform.position.z - Camera.main.transform.position.z);
        float radHorizontalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView;*/

        float d = 0;// Mathf.Tan(radHorizontalFOV / 2) * distanceFromGrid;

        return entryObjects[_circuit.entries[_circuit.NbCol - 1]].transform.position.x - d;
    }

    public float CameraRightBound()
    {
        /*float distanceFromGrid = Mathf.Abs(entryObjects[circuit.entries[0]].transform.position.z - Camera.main.transform.position.z);
        float radHorizontalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView;*/

        float d = 0;//Mathf.Tan(radHorizontalFOV / 2) * distanceFromGrid;

        return entryObjects[_circuit.entries[0]].transform.position.x + d;
    }

    public float CameraTopBound()
    {
        /*float distanceFromGrid = Mathf.Abs(entryObjects[circuit.entries[0]].transform.position.z - Camera.main.transform.position.z);
        float radHorizontalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView;*/

        float d = 0;// Mathf.Tan(radHorizontalFOV / 2) * distanceFromGrid / Camera.main.aspect;

        return entryObjects[_circuit.entries[0]].transform.position.y - d;
    }

    public float CameraBotBound()
    {
        /*float distanceFromGrid = Mathf.Abs(entryObjects[circuit.entries[0]].transform.position.z - Camera.main.transform.position.z);
        float radHorizontalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView;*/

        float d = 0;// Mathf.Tan(radHorizontalFOV / 2) * distanceFromGrid / Camera.main.aspect;

        return GetGateObject(_circuit.NbRow - 1, 0).transform.position.y + d;
    }

    /**
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ############################################### GETTERS ##############################################
	 * ######################################################################################################
	 * ######################################################################################################
	 * ######################################################################################################
	 */

    /// <summary>
    /// Recupère le "EntryObject" de la colonne "col"
    /// </summary>
    /// <returns> Le "GateObject"</returns>
    /// <param name="indexCol">Indice de la colonne </param>
    /// <param name="indexRow">Indice de la ligne </param>
    public EntryObject GetEntryObject(int col)
    {
        Circuit.EntryStruct entryStruct = _circuit.GetEntry(col);
        return entryObjects[entryStruct];
    }

    /// <summary>
    /// Recupère le "GateObject" de la ligne "row" et de la colonne "col"
    /// </summary>
    /// <returns> Le "GateObject"</returns>
	/// <param name="indexCol">Indice de la colonne </param>
	/// <param name="indexRow">Indice de la ligne </param>
    public GateObject GetGateObject(int row, int col)
    {
        Circuit.GateStruct gateStruct = _circuit.GetGate(row, col);
        return gateObjects[gateStruct];
    }
}
