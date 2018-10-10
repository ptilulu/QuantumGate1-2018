using QCS;
using UnityEngine;

namespace Smartphone_States
{
    public class SelectGate : EditorState
    {
        private EditorState _previousState;

        public SelectGate(Editor context, EditorState previousState)
            : base(context)
        {
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("SelectGate");
            context.ShowGatesMenuPanel(true);
        }

        public override void OnExit() { context.ShowGatesMenuPanel(false); }

        public override void OnBackButton() { context.CurrentState = _previousState; }

        public override void OnToggleMenuClick() { context.CurrentState = _previousState; }

        public override void OnSelectGate(Gate selectedGate)
        {
            context.CurrentState = new PutGate(context, selectedGate);
        }
    }
}
