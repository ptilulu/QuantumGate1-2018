using QCS;
using UnityEngine;

namespace PC_States
{
    public class SelectGateToInsert : EditorState
    {
        private int _row, _col;
        private EditorState _previousState;

        public SelectGateToInsert(Editor context, int row, int col, EditorState previousState)
            : base(context)
        {
            _row = row;
            _col = col;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("SelectGateToInsert");
            context.ShowGatesMenuPanel(true);
        }

        public override void OnExit() { context.ShowGatesMenuPanel(false); }

        public override void OnBackButton() { context.CurrentState = new DefaultState(context); }

        public override void OnSelectGate(Gate selectedGate)
        {
            context.currentCircuit.PutGate(_row, _col, selectedGate);

            context.CurrentState = _previousState;
        }
    }
}
