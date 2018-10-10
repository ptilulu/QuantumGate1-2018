using UnityEngine;

namespace Smartphone_States
{
    public class InputCircuitGateName : EditorState
    {
        private EditorState _previousState;

        public InputCircuitGateName(Editor context, EditorState previousState) : base(context)
        {
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("InputCircuitGateName");
            context.GateNameInputField.text = "";
            context.GateNameInputPanel.SetActive(true);
        }

        public override void OnExit()
        {
            context.GateNameInputPanel.SetActive(false);
        }

        public override void OnBackButton()
        {
            context.CurrentState = _previousState;
        }

        public override void OnBackGateNameClick()
		{
            OnBackButton();
        }

        public override void OnValidGateNameClick()
        {
            string name = context.GateNameInputField.text;

			if (name.Length == 0)
				return;
			
            context.AddCustomGateToMenu(context.currentCircuit.GetCircuitGate(name));

            context.CurrentState = _previousState;
        }
    }
}
