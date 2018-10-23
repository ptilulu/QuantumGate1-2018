using UnityEngine;

namespace PC_States
{
    public class InputSessionName : EditorState
    {
        private EditorState _previousState;

        public InputSessionName(Editor context, EditorState previousState) : base(context)
        {
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("InputSessionName");
            context.chooseSessionNameInput.text = "";
            context.ShowSessionNameInputPanel(true);
        }

        public override void OnExit()
        {
            context.ShowSessionNameInputPanel(false);
        }

        public override void OnBackButton()
        {
            context.CurrentState = _previousState;
        }

        public override void OnBackSessionNameClick()
        {
            OnBackButton();
        }

        public override void OnValidSessionNameClick()
        {
            string sessionName = context.chooseSessionNameInput.text;

            if (sessionName.Length == 0)
                return;

            SandBoxSession sandBoxSession = new SandBoxSession.Builder(sessionName)
                .AddCircuits(context.GetCircuits())
                .AddCustomGates(context.GetCustomGates())
                .Build();

            if (!GameManager.UserManager.CreateSandBoxSessionOfCurrentUser(sandBoxSession))
                return;

            GameMode.nameGame = sessionName;

            context.CurrentState = _previousState;
        }
    }
}