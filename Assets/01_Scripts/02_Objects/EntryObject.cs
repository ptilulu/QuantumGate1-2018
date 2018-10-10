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
    }

    public void Select()
    {
        foreach(Renderer renderer in collar.GetComponentsInChildren<Renderer>())
            renderer.material = ObjectFactory.pipeSelectedMaterial;
    }
    public void Deselect()
    {
        foreach (Renderer renderer in collar.GetComponentsInChildren<Renderer>())
            renderer.material = ObjectFactory.pipeMaterial;
    }

    public void FadeOutAndDestroy()
    {
        AnimationManager.Fade(root, 0f, GridBoard.animationTime, Destroy);
    }
}
