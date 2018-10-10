using System;
using UnityEngine;

namespace PC_States
{
    public class MoveGate : EditionState
{
    private GateObject _selectedGate;
    private EditorState _previousState;

    public MoveGate(Editor context, GateObject selectedGate, EditorState previousState) : base(context)
    {
        _selectedGate = selectedGate;
        _previousState = previousState;
    }

    public override void OnEnter()
    {
        QCS.Circuit.GateStruct gateStruct = _selectedGate.gateStruct;
        //TODO : pas terminé ?
        int row = gateStruct.row;
        int col = gateStruct.col;
        Debug.Log("MoveGate from (" + row + ", " + col + ")");
    }

    public override void OnBackButton()
    {
        context.CurrentState = _previousState;
    }

    public override void OnGridClick(Tuple<int, int, Editor.ClickPosition> infos)
    {
        QCS.Circuit.GateStruct gateStruct = _selectedGate.gateStruct;

        int sourceRow = gateStruct.row;
        int sourceCol = gateStruct.col;
        int nbEntries = gateStruct.gate.NbEntries;

        int targetRow = infos.Item1;
        int targetCol = infos.Item2;
        Editor.ClickPosition position = infos.Item3;

        if (nbEntries % 2 == 0 && (position == Editor.ClickPosition.TopLeft
            || position == Editor.ClickPosition.BotLeft))
            targetCol -= nbEntries / 2;
        else
            targetCol -= (nbEntries - 1) / 2;

        if (targetCol < 0)
            targetCol = 0;

        Debug.Log("MoveGate to (" + targetRow + ", " + targetCol + ")");

        context.currentCircuit.MoveGate(sourceRow, sourceCol, targetRow, targetCol);

        context.CurrentState = _previousState;
    }
    }
}
