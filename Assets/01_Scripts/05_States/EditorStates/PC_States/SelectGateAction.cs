using System;
using UnityEngine;

namespace PC_States
{
    public class SelectGateAction : EditionState
    {
        private GateObject _selectedGate;
        private EditorState _previousState;

        public SelectGateAction(Editor context, GateObject selectedGate, EditorState previousState)
            : base(context)
        {
            _selectedGate = selectedGate;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("SelectGateAction");
            _selectedGate.Select();
            PositionPanel();
            context.ShowActionsGatePanel(true);
        }

        public override void Update()
        {
            base.Update();
            PositionPanel();
        }

        public override void OnExit()
        {
            GateObject gateObject = _selectedGate.GetComponent<GateObject>();

            gateObject.Deselect();
            context.ShowActionsGatePanel(false);
        }

        public override void OnBackButton() { context.CurrentState = _previousState; }

        public override void OnInsertGateClick()
        {
            QCS.Circuit.GateStruct gateStruct = _selectedGate.gateStruct;

            int row = gateStruct.row;
            int col = gateStruct.col;

            context.CurrentState = new SelectGateToInsert(context, row, col, _previousState);
        }

        public override void OnDeleteGateClick()
        {
            QCS.Circuit.GateStruct gateStruct = _selectedGate.gateStruct;

            int row = gateStruct.row;
            int col = gateStruct.col;

            context.currentCircuit.RemoveGate(row, col);

            context.CurrentState = _previousState;
        }

        public override void OnMoveGateClick()
        {
            context.CurrentState = new MoveGate(context, _selectedGate, _previousState);
        }

        public override void OnProcessCircuitClick()
        {
            QCS.Circuit.GateStruct gateStruct = _selectedGate.gateStruct;

            int row = gateStruct.row;

            context.CurrentState = new ShowResult(context, row, _previousState);
        }

        public void PositionPanel()
        {
            Vector3 screenPos = context.cam.WorldToScreenPoint(_selectedGate.pipes[0].transform.position);
            screenPos.y -= 40;
            screenPos.z = 0;
            context.SetActionGatePanelPosition(screenPos);
        }
    }
}
