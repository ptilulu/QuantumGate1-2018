
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
        UnityEngine.Object.Destroy(this);
    }

    /// <summary>
    /// Change le mateial du tuyau pour le selectionner
    /// </summary>
    public void Select()
    {
        foreach (GameObject pipe in pipes)
        {
            // UnityEngine.Object.Destroy(pipe);
            pipe.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipeSelected2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipeSelected2DMaterial.width, ObjectFactory.pipeSelected2DMaterial.height), new Vector2(0.5f, 0.5f));
        }
        //pipe.GetComponentInChildren<Renderer>().material = ObjectFactory.pipeSelectedMaterial;
    }
    public void Select(int entry)
    {
        pipes[entry].GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipeSelected2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipeSelected2DMaterial.width, ObjectFactory.pipeSelected2DMaterial.height), new Vector2(0.5f, 0.5f));
    }
    public void Deselect()
    {
        foreach (GameObject pipe in pipes)
        {
            pipe.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipe2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipe2DMaterial.width, ObjectFactory.pipe2DMaterial.height), new Vector2(0.5f, 0.5f));
        }
           
    }
    public void Deselect(int entry)
    {
        pipes[entry].GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(ObjectFactory.pipe2DMaterial, new Rect(0.0f, 0.0f, ObjectFactory.pipe2DMaterial.width, ObjectFactory.pipe2DMaterial.height), new Vector2(0.5f, 0.5f));
    }

    public void FadeOutAndDestroy()
    {
        foreach (GameObject pipe in pipes)
            AnimationManager.Fade(pipe, 0f, GridBoard.animationTime, delegate () { UnityEngine.Object.Destroy(pipe); });

        if (body != null)
            AnimationManager.Fade(body, 0f, GridBoard.animationTime, delegate () { UnityEngine.Object.Destroy(body); });
    }
}
