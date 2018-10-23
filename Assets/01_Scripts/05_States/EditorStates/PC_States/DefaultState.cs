using System;
using UnityEngine;

namespace PC_States
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

    public override void OnGridClick(Tuple<int, int, Editor.ClickPosition> infos)
    {
        int row = infos.Item1;
        int col = infos.Item2;
        GateObject selectedGate = context.gridBoard.GetGateObject(row, col);

        context.CurrentState = new SelectGateAction(context, selectedGate, context.CurrentState);
    }
    public override void OnGridPress(Tuple<int, int, Editor.ClickPosition> infos)
    {
        int row = infos.Item1;
        int col = infos.Item2;
        context.CurrentState = new SelectGridAction(context, row, col, context.CurrentState);
    }

    public override void OnPreviousCircuitClick() { context.PreviousCircuit(); }
    public override void OnNextCircuitClick() { context.NextCircuit(); }
    public override void OnSaveCircuitClick() { context.CurrentState = new InputCircuitGateName(context, context.CurrentState); }
	public override void OnDeleteCircuitClick() { context.DeleteCurrentCircuit(); }

    }
}
