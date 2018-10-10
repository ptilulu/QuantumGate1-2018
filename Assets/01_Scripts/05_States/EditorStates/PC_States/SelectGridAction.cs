using System;
using UnityEngine;

/// <summary>
/// Etat pour afficher le panel des actions pour la grille.
/// </summary>
namespace PC_States
{
    public class SelectGridAction : EditionState
    {
        private int _row, _col;
        private EditorState _previousState;

        public SelectGridAction(Editor context, int row, int col, EditorState previousState) : base(context)
        {
            _row = row;
            _col = col;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("SelectGridAction : " + _row + " " + _col);
            context.gridBoard.SelectCol(_col);
            PositionPanel();
            context.ShowActionsGridPanel(true);
        }

        public override void Update()
        {
            base.Update();
            PositionPanel();
        }

        public override void OnExit()
        {
            context.gridBoard.DeselectCol(_col);
            context.ShowActionsGridPanel(false);
        }

        public override void OnBackButton() { context.CurrentState = _previousState; }

        public override void OnInsertRowClick()
        {
            context.currentCircuit.InsertRow(_row);

            context.CurrentState = _previousState;
        }

        public override void OnDeleteRowClick()
        {
            context.currentCircuit.RemoveRow(_row);

            context.CurrentState = _previousState;
        }

        public override void OnMoveRowClick()
        {
            context.CurrentState = new MoveRow(context, _row, _previousState);
        }

        public override void OnInsertColClick()
        {
            // pas propre mais osef
            if (context.currentCircuit.InsertCol(_col))
                if(_col < context.currentCircuit.NbCol)
                    context.gridBoard.DeselectCol(_col + 1);

            context.CurrentState = _previousState;
        }

        public override void OnDeleteColClick()
        {
            context.currentCircuit.RemoveCol(_col);

            context.CurrentState = _previousState;
        }

        public override void OnMoveColClick()
        {
            context.CurrentState = new MoveCol(context, _col, _previousState);
        }

        public void PositionPanel()
        {
            Vector3 screenPos = context.cam.WorldToScreenPoint(
                context.gridBoard.GetGateObject(_row, _col).transform.position);
            screenPos.y -= 40;
            screenPos.z = 0f;
            context.SetActionGridPanelPosition(screenPos);
        }
    }
}