using System;
using System.Collections.Generic;
using UnityEngine;

public class EditorState : State<Editor>
{

    public EditorState(Editor context) : base(context) { }

    /// <summary>
    /// Evenement lors de l'entre dans l'etat.
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// Evenement lors de la sortie de l'etat.
    /// </summary>
    public virtual void OnExit() { }

    /// <summary>
    /// Actions qui s'execute a chaque frame.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Evenement lors d'un clic rapide.
    /// </summary>
    /// <param name="screenPosition"></param>
    public virtual void OnClick(Vector3 screenPosition) { }

    /// <summary>
    /// Evenement lors d'un clic long.
    /// </summary>
    /// <param name="screenPosition"></param>
    public virtual void OnPress(Vector3 screenPosition) { }

    //TODO : commentaires.
    public virtual void OnDragStart(Vector3 position) { }
    public virtual void OnDrag(Vector3 position) { }
    public virtual void OnDragEnd(Vector3 position) { }
    public virtual void OnDragCancelled(Vector3 position) { }
    public virtual void OnMove(Vector2 delta) { }
    public virtual void OnPinch(float ratio) { }

    /// <summary>
    /// Evenement lors d'un clic sur un bouton pour revenir en arriere.
    /// </summary>
    public virtual void OnBackButton() { }

    /// <summary>
    /// Evenement lors d'un clic rapide sur un des inputs (qubit)
    /// </summary>
    /// <param name="entry">L'objet du qubit</param>
    public virtual void OnEntryClick(GameObject entry) { }

    /// <summary>
    /// Evenement lors d'un clic long sur un des inputs (qubit)
    /// </summary>
    /// <param name="entry">L'objet du qubit</param>
    public virtual void OnEntryPress(GameObject entry) { }

    /// <summary>
    /// Evenement lors d'un clic rapide sur la grille de jeu
    /// </summary>
    /// <param name="infos">Tuple avec (row, col, position du dernier clic)</param>
    public virtual void OnGridClick(Tuple<int, int, Editor.ClickPosition> infos) { }

    /// <summary>
    /// Evenement lors d'un clic long sur la grille de jeu
    /// </summary>
    /// <param name="infos">Tuple avec (row, col, position du dernier clic)</param>
    public virtual void OnGridPress(Tuple<int, int, Editor.ClickPosition> infos) { }


    public virtual void OnToggleMenuClick() { }

    // Evenement sur le bouton des paramètres.
    public virtual void OnSettingsClick() { }
    public virtual void OnBackSettingsClick() { }

    // Gate Actions.
    public virtual void OnInsertGateClick() { }
    public virtual void OnDeleteGateClick() { }
    public virtual void OnMoveGateClick() { }
    public virtual void OnProcessCircuitClick() { }

    // Gate subactions.
    public virtual void OnSelectGate(QCS.Gate gate) { }
    public virtual void OnBackResultClick() { }

    // Grid Actions.
    public virtual void OnInsertRowClick() { }
    public virtual void OnDeleteRowClick() { }
    public virtual void OnMoveRowClick() { }
    public virtual void OnInsertColClick() { }
    public virtual void OnDeleteColClick() { }
    public virtual void OnMoveColClick() { }

    // Circuit Actions
    public virtual void OnPreviousCircuitClick() { }
    public virtual void OnNextCircuitClick() { }
    public virtual void OnSaveCircuitClick() { }
    public virtual void OnDeleteCircuitClick() { }

    // Circuit subactions
    public virtual void OnBackGateNameClick() { }
    public virtual void OnValidGateNameClick() { }

    public virtual void OnBackSessionNameClick() { }
    public virtual void OnValidSessionNameClick() { }


    // Save Game
    public virtual void OnSaveSandBoxSession() { }
	public virtual void OnDeleteSandBoxSession() { }


}
