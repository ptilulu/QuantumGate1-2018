using System;
using UnityEngine;

/// <summary>
/// Propriétés des entrées (qubits)
/// </summary>
public class EntryObject : MonoBehaviour
{
    public QCS.Circuit.EntryStruct entryStruct;

    public GameObject root;
    public GameObject entry;
    public GameObject collar;

    public void Destroy()
    {
        UnityEngine.Object.Destroy(root);
        UnityEngine.Object.Destroy(entry);
        UnityEngine.Object.Destroy(collar);

        UnityEngine.Object.Destroy(this);
    }

    public void Select()
    {
        //foreach(Renderer renderer in collar.GetComponentsInChildren<Renderer>())
        //renderer.material = ObjectFactory.pipeSelectedMaterial;
        collar.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipeSelected2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipeSelected2DMaterial.width, ObjectFactory.pipeSelected2DMaterial.height), new Vector2(0.5f, 0.5f));
    }
    public void Deselect()
    {
        collar.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipeStart2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipeStart2DMaterial.width, ObjectFactory.pipeStart2DMaterial.height), new Vector2(0.5f, 0.5f));
    }

    public void FadeOutAndDestroy()
    {
        AnimationManager.Fade(root, 0f, GridBoard.animationTime, Destroy);
    }
}
