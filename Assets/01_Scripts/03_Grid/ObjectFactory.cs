using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet d'instancier les gameobjects de la grille
/// </summary>
public class ObjectFactory
{
    /// <summary>
    /// Material Unity pour les tuyaux.
    /// </summary>
    public static Material pipeMaterial =
        Object.Instantiate(Resources.Load(
            "03_materials/Old3DMaterials/MaterialPipe",
            typeof(Material))) as Material;

    /// <summary>
    /// Material Unity pour les tuyaux selectionnés.
    /// </summary>
    public static Material pipeSelectedMaterial =
        Object.Instantiate(Resources.Load(
            "03_materials/Old3DMaterials/MaterialPipeSelected",
            typeof(Material))) as Material;

    /// <summary>
    /// Material Unity pour les débuts de tuyaux en 2D.
    /// </summary>
    public static Texture2D pipeStart2DMaterial =
        Object.Instantiate(Resources.Load(
            "04_Textures/Editor/start_sprite_tuyau_petit",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Material Unity pour les tuyaux en 2D.
    /// </summary>
    public static Texture2D pipe2DMaterial =
        Object.Instantiate(Resources.Load(
            "04_Textures/Editor/sprite_tuyau_petit",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Material Unity pour les tuyaux selectionnés en 2D.
    /// </summary>
    public static Texture2D pipeSelected2DMaterial =
        Object.Instantiate(Resources.Load(
            "04_Textures/Editor/sprite_selected_tuyau_petit",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Material Unity pour les portes.
    /// </summary>
    private static Material _boxGateMaterial =
        Object.Instantiate(Resources.Load(
            "03_materials/Old3DMaterials/MaterialGateBox",
            typeof(Material))) as Material;

    /// <summary>
    /// Texture Unity pour les portes.
    /// </summary>
    private static Texture2D _boxGateMaterial2D =
        Object.Instantiate(Resources.Load(
            "04_Textures/Editor/porte_petit",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Material Unity représantant un qubit à 0.
    /// </summary>
    public static Material materialQubitZero3D = Object.Instantiate(Resources.Load(
            "03_materials/Old3DMaterials/MaterialEntry0",
            typeof(Material))) as Material;

    /// <summary>
    /// Material Unity représentant un qubit à 1.
    /// </summary>
    public static Material materialQubitOne3D = Object.Instantiate(Resources.Load(
            "03_materials/Old3DMaterials/MaterialEntry1",
            typeof(Material))) as Material;

    /// <summary>
    /// Material Unity représantant un qubit à 0.
    /// </summary>
    public static Texture2D materialQubitZero = Object.Instantiate(Resources.Load(
            "04_Textures/Editor/boule_blanche",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Material Unity représentant un qubit à 1.
    /// </summary>
    public static Texture2D materialQubitOne = Object.Instantiate(Resources.Load(
            "04_Textures/Editor/boule_noir",
            typeof(Texture2D))) as Texture2D;

    /// <summary>
    /// Construit un Objet unity englobé dans la classe "GateObject" représentant une porte du jeu.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    public static GateObject Build(QCS.Gate gate, Transform parent)
    {
        if (gate.Equals(QCS.Gate.IDENTITY))
            return IDENTITY_GATE(parent);

        return DYNAMIC_GATE(gate, parent);
    }

    /// <summary>
    /// Construit un Objet unity englobé dans la classe "EntryObject" représentant une entrée du jeu.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    public static EntryObject ENTRY(Transform parent)
    {
        /* root */
        GameObject root = new GameObject();

        root.tag = "entry";
        root.name = "entry";

        /* root Trasnform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, 0, 0);

        /* entry */
        //GameObject entry = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject entry = new GameObject();
        SpriteRenderer sp = entry.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        //Sprite pipeSprite = Sprite.Create(materialQubitZero, new Rect(0.0f, 0.0f, materialQubitZero.width, materialQubitZero.height), new Vector2(0.5f, 0.5f));

        /* entry BoxCollider */
        BoxCollider boxCollider = entry.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.5f * GridBoard.localColWidth, GridBoard.localRowHeight, GridBoard.pipeDiameter + 0.5f);

        entry.tag = "entry";
        entry.name = "entry";
        
        /* entry Transform */
        entry.transform.parent = root.transform;
        entry.transform.localScale = new Vector3(GridBoard.pipeDiameter, GridBoard.pipeDiameter, GridBoard.pipeDiameter);
        entry.transform.localPosition = new Vector3(0f, 1f, 0f);

        /* collar */
        GameObject collar = STARTING_PIPE2D(entry.transform);

        /* collar Transform */
        collar.transform.parent = root.transform;
        collar.transform.localScale = new Vector3(1, 1, 1);
        collar.transform.localPosition = new Vector3(0, -1f, 0);

        /* bind gateObject */
        EntryObject entryObject = entry.AddComponent<EntryObject>();
        entryObject.root = root;
        entryObject.entry = entry;
        entryObject.collar = collar;

        return entryObject;
    }
    /// <summary>
    /// Construit l'objet unity représentant les tuyaux sous les entrée du circuit.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject STARTING_PIPE(Transform parent)
    {
        /* root */
        GameObject root = new GameObject();

        root.tag = "collar";
        root.name = "collar";

        /* root Trasnform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, 0, 0);

        /* pipe */
        GameObject pipe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        /* pipe Material */
        pipe.GetComponent<Renderer>().material = pipeMaterial;

        /* pipe Trasnform */
        pipe.transform.parent = root.transform;
        pipe.transform.localScale = new Vector3(GridBoard.pipeDiameter, 0.5f, GridBoard.pipeDiameter);
        pipe.transform.localPosition = new Vector3(0f, 0.5f, 0f);

        /* collar */
        GameObject collar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        /* collar Material */
        collar.GetComponent<Renderer>().material = pipeMaterial;

        /* collar Trasnform */
        collar.transform.parent = root.transform;
        collar.transform.localScale = new Vector3(1f, 0.2f, 1f);
        collar.transform.localPosition = new Vector3(0, 0.8f, 0);

        return root;
    }

    /// <summary>
    /// Construit un Objet unity représentant un tuyau.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject PIPE(Transform parent)
    {
        /* root */
        GameObject root = new GameObject();

        root.tag = "pipe";
        root.name = "pipe";

        /* root BoxCollider */
        BoxCollider boxCollider = root.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.5f * GridBoard.localColWidth, GridBoard.localRowHeight, GridBoard.pipeDiameter + 0.5f);

        /* root Transform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, 0, 0);

        /* pipe */
        GameObject pipe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        /* pipe Material */
        pipe.GetComponent<Renderer>().material = pipeMaterial;

        /* pipe Transform */
        pipe.transform.parent = root.transform;
        pipe.transform.localScale = new Vector3(GridBoard.pipeDiameter, GridBoard.localRowHeight / 2, GridBoard.pipeDiameter);
        pipe.transform.localPosition = new Vector3(0, 0, 0);

        /* bind gateObject */
        return root;
    }

    /// <summary>
    /// Construit l'objet unity représentant les tuyaux sous les entrée du circuit.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject STARTING_PIPE2D(Transform parent)
    {
        /* root */
        GameObject root = new GameObject();

        root.tag = "collar";
        root.name = "collar";

        /* root Transform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, 0, 0);

        /* pipe */
        //GameObject pipe = GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject pipe = new GameObject();
        SpriteRenderer sp = pipe.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        Sprite pipeSprite = Sprite.Create(pipeStart2DMaterial, new Rect(0.0f, 0.0f, pipeStart2DMaterial.width, pipeStart2DMaterial.height), new Vector2(0.5f, 0.5f));
        //SpriteRenderer m_SpriteRenderer = new SpriteRenderer();
        //m_SpriteRenderer = GetComponent<SpriteRenderer>();
        sp.sprite = pipeSprite;

        /* pipe Material */
        //pipe.GetComponent<Renderer>().material = pipeMaterial;

        /* pipe Transform */
        pipe.transform.parent = root.transform;
        pipe.transform.localScale = new Vector3(GridBoard.pipeDiameter, GridBoard.localRowHeight / 2, GridBoard.pipeDiameter);
        pipe.transform.localPosition = new Vector3(0, 0, 0);

        return root;
    }

    /// <summary>
    /// Construit un Objet unity représentant un tuyau en 2D.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject PIPE2D(Transform parent)
    {
        /* root */
        GameObject root = new GameObject();

        root.tag = "pipe";
        root.name = "pipe";

        /* root BoxCollider */
        BoxCollider boxCollider = root.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.5f * GridBoard.localColWidth, GridBoard.localRowHeight, GridBoard.pipeDiameter + 0.5f);

        /* root Transform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, 0, 0);

        /* pipe */
        //GameObject pipe = GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject pipe = new GameObject();
        SpriteRenderer sp = pipe.AddComponent(typeof( SpriteRenderer)) as SpriteRenderer;
        Sprite pipeSprite = Sprite.Create(pipe2DMaterial, new Rect(0.0f, 0.0f, pipe2DMaterial.width, pipe2DMaterial.height), new Vector2(0.5f, 0.5f));
        //SpriteRenderer m_SpriteRenderer = new SpriteRenderer();
        //m_SpriteRenderer = GetComponent<SpriteRenderer>();
        sp.sprite = pipeSprite;

        /* pipe Material */
        //pipe.GetComponent<Renderer>().material = pipeMaterial;

        /* pipe Transform */
        pipe.transform.parent = root.transform;
        pipe.transform.localScale = new Vector3(GridBoard.pipeDiameter, GridBoard.localRowHeight / 2, GridBoard.pipeDiameter);
        pipe.transform.localPosition = new Vector3(0, 0, 0);

        /* bind gateObject */
        return root;
    }

    /// <summary>
    /// Construit un Objet unity représentant une porte.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject GATE3D(QCS.Gate gate, Transform parent)
    {
        GameObject root = new GameObject();

        root.tag = "gate";
        root.name = "gate";

        /* root BoxCollider */
        BoxCollider boxCollider = root.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(GridBoard.localColWidth * gate.NbEntries, GridBoard.localRowHeight * GridBoard.gateHeightRatio, 2);
        boxCollider.transform.localPosition = new Vector3(0, (GridBoard.localRowHeight / 2) - (GridBoard.localRowHeight * GridBoard.gateHeightRatio / 2), 0);

        /* gate TextMesh */
        TextMesh textMesh = root.AddComponent<TextMesh>();
        textMesh.text = gate.Name;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.offsetZ = -(GridBoard.gateThikness / 2);
        textMesh.tabSize = 4f;
        textMesh.fontSize = 50;
        textMesh.lineSpacing = 1f;
        textMesh.richText = true;
        textMesh.color = Color.white;
        textMesh.characterSize = 0.1f;
        textMesh.fontStyle = FontStyle.Bold;

        /* gate Transform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, (GridBoard.localRowHeight / 2) - (GridBoard.localRowHeight * GridBoard.gateHeightRatio / 2), 0);

        /* cube */
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        /* cube Material */
        cube.GetComponent<Renderer>().material = _boxGateMaterial;

        /* cube Transform */
        cube.transform.parent = root.transform;
        cube.transform.localScale = new Vector3(GridBoard.localColWidth * gate.NbEntries - GridBoard.gateSideSpace,
            GridBoard.localRowHeight * GridBoard.gateHeightRatio,
            GridBoard.gateThikness);
        cube.transform.localPosition = new Vector3(0, 0, 0);

        return root;
    }

    /// <summary>
    /// Construit un Objet unity représentant une porte.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    private static GameObject GATE(QCS.Gate gate, Transform parent)
    {
        GameObject root = new GameObject();

        root.tag = "gate";
        root.name = "gate";

        /* root BoxCollider */
        BoxCollider boxCollider = root.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(GridBoard.localColWidth * gate.NbEntries, GridBoard.localRowHeight * GridBoard.gateHeightRatio, 2);
        boxCollider.transform.localPosition = new Vector3(0, (GridBoard.localRowHeight / 2) - (GridBoard.localRowHeight * GridBoard.gateHeightRatio / 2), 0);

        /* gate TextMesh */
        TextMesh textMesh = root.AddComponent<TextMesh>();
        textMesh.text = gate.Name;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.offsetZ = -(GridBoard.gateThikness / 2);
        textMesh.tabSize = 4f;
        textMesh.fontSize = 50;
        textMesh.lineSpacing = 1f;
        textMesh.richText = true;
        textMesh.color = Color.white;
        textMesh.characterSize = 0.1f;
        textMesh.fontStyle = FontStyle.Bold;

        /* gate Transform */
        root.transform.parent = parent;
        root.transform.localScale = new Vector3(1, 1, 1);
        root.transform.localPosition = new Vector3(0, (GridBoard.localRowHeight / 2) - (GridBoard.localRowHeight * GridBoard.gateHeightRatio / 2), 0);

        /* cube */
        GameObject cube = new GameObject();
        SpriteRenderer sp = cube.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        Sprite pipeSprite = Sprite.Create(_boxGateMaterial2D, new Rect(0.0f, 0.0f, _boxGateMaterial2D.width, _boxGateMaterial2D.height), new Vector2(0.5f, 0.5f));
        sp.sprite = pipeSprite;

        /* cube Transform */
        cube.transform.parent = root.transform;
        cube.transform.localScale = new Vector3(GridBoard.localColWidth * gate.NbEntries - GridBoard.gateSideSpace,
            GridBoard.localRowHeight * GridBoard.gateHeightRatio, 1);
        float offset = -(GridBoard.gateThikness / 2) + 0.1f; 
        cube.transform.localPosition = new Vector3(0, 0, offset);

        return root;
    }

    /// <summary>
    /// Construit un Objet unity représentant une porte identité.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    public static GateObject IDENTITY_GATE(Transform parent)
    {
        GameObject pipe = PIPE2D(parent);

        GateObject gateObject = pipe.AddComponent<GateObject>();
        gateObject.body = null;
        gateObject.pipes = new List<GameObject>() { pipe };

        return gateObject;
    }

    /// <summary>
    /// Construit dynamiquement une porte de la scène unity.
    /// </summary>
    /// <param name="parent">Parent de l'objet qui doit être créé</param>
    public static GateObject DYNAMIC_GATE(QCS.Gate gate, Transform parent)
    {
        GameObject gateGameObject = GATE(gate, parent);

        /* bind gateObject */
        GateObject gateObject = gateGameObject.AddComponent<GateObject>();
        gateObject.body = gateGameObject;
        gateObject.pipes = new List<GameObject>();

        /* gate pipes */
        for (int i = 0; i < gate.NbEntries; i++)
        {
            GameObject pipe = PIPE2D(parent);
            gateObject.pipes.Add(pipe);
        }

        return gateObject;
    }
}
