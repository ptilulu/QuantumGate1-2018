using System;
using UnityEngine;

namespace Smartphone_States
{
    public class DefaultState : EditionState
    {
        public DefaultState(Editor context) : base(context) { }

        public override void OnEnter() { Debug.Log("DefaultState"); }

        public override void OnEntryClick(GameObject entryObject)
        {
            EntryObject entryProperties = entryObject.GetComponent<EntryObject>();
            QCS.Circuit.EntryStruct entryStruct = entryProperties.entryStruct;

            context.gridBoard.SwapEntryValue(entryStruct);
        }

        public override void OnToggleMenuClick()
        {
            context.CurrentState = new SelectGate(context, context.CurrentState);
        }

        public override void OnGridClick(Tuple<int, int, Editor.ClickPosition> infos)
        {
            int row = infos.Item1;

            context.CurrentState = new ShowResult(context, row, context.CurrentState);
        }

        public override void OnDragStart(Vector3 position)
        {
            GameObject gameObject = context.GetOnScreenObject(position);

            if (gameObject == null)
                return;

            if(gameObject.tag == "gate")
            {
                GateObject gateObject = gameObject.GetComponent<GateObject>();
                context.CurrentState = new DragGate(context, gateObject, context.CurrentState);
            }

            if (gameObject.tag == "pipe")
            {
                /*GateObject gateObject = gameObject.GetComponent<GateObject>();
                int col = gateObject.gateStruct.col;
                context.CurrentState = new DragCol(context, col, context.CurrentState);*/
            }
        }

        public override void OnInsertColClick()
        {
            context.currentCircuit.InsertCol(context.currentCircuit.NbCol);
        }

        public override void OnDeleteColClick()
        {
            context.currentCircuit.RemoveCol(context.currentCircuit.NbCol - 1);
        }

        public override void OnPreviousCircuitClick() { context.PreviousCircuit(); }
        public override void OnNextCircuitClick() { context.NextCircuit(); }

        
        public override void OnSaveCircuitClick() { context.CurrentState = new InputCircuitGateName(context, context.CurrentState); }
        public override void OnDeleteCircuitClick() { context.DeleteCurrentCircuit(); }
    }
}
