
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  Stocke des informations complémentaire 
///  dans les portes de la grille de jeu de la scene Unity.
/// </summary>
public class GateObject : MonoBehaviour
{
    public QCS.Circuit.GateStruct gateStruct;
    
    public GameObject body;
    public List<GameObject> pipes;

    public void Destroy()
    {
        if (body != null)
            UnityEngine.Object.Destroy(body);

        pipes.ForEach(UnityEngine.Object.Destroy);
    }

    public void Select()
    {
        foreach (GameObject pipe in pipes)
            pipe.GetComponentInChildren<Renderer>().material = ObjectFactory.pipeSelectedMaterial;
    }
    public void Select(int entry)
    {
        pipes[entry].GetComponentInChildren<Renderer>().material = ObjectFactory.pipeSelectedMaterial;
    }
    public void Deselect()
    {
        foreach (GameObject pipe in pipes)
            pipe.GetComponentInChildren<Renderer>().material = ObjectFactory.pipeMaterial;
    }
    public void Deselect(int entry)
    {
        pipes[entry].GetComponentInChildren<Renderer>().material = ObjectFactory.pipeMaterial;
    }

    public void FadeOutAndDestroy()
    {
        foreach (GameObject pipe in pipes)
            AnimationManager.Fade(pipe, 0f, GridBoard.animationTime, delegate () { UnityEngine.Object.Destroy(pipe); });

        if (body != null)
            AnimationManager.Fade(body, 0f, GridBoard.animationTime, delegate () { UnityEngine.Object.Destroy(body); });
    }
}
