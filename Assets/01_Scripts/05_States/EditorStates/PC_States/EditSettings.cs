using UnityEngine;

namespace PC_States
{
    public class EditSettings : EditorState
    {
        private EditorState _previousState;

        public EditSettings(Editor context, EditorState previousState) : base(context)
        {
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("EditSettings");
            context.ShowSettingsPanel(true);
        }

        public override void OnExit() { context.ShowSettingsPanel(false); }

        public override void OnBackSettingsClick() { context.CurrentState = _previousState; }

        public override void OnBackButton() { OnBackSettingsClick(); }

        public override void OnSaveSandBoxSession()
        {
            if (GameMode.nameGame.Length == 0)
            {
                context.CurrentState = new InputSessionName(context, context.CurrentState);

                return;
            }

            SandBoxSession sandBoxSession = new SandBoxSession.Builder(GameMode.nameGame)
               .AddCircuits(context.GetCircuits())
               .AddCustomGates(context.GetCustomGates())
               .Build();

            GameManager.UserManager.SaveSandBoxSessionOfCurrentUser(sandBoxSession);

        }

        public override void OnDeleteSandBoxSession()
        {
            GameManager.UserManager.DeleteSandBoxSessionOfCurrentUser(GameMode.nameGame);
            context.BackToPreviousScene();
        }
    }
}
