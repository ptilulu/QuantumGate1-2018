using UnityEngine;

/// <summary>
/// Etat pour montrer le résultat du circuit quantique jusqu'a la ligne _row.
/// </summary>
namespace Smartphone_States
{
    public class ShowResult : EditorState
    {
        /// <summary>
        /// Numero de la ligne
        /// </summary>
        private int _row;

        /// <summary>
        /// Etat precedent
        /// </summary>
        private EditorState _previousState;

        /// <summary>
        /// </summary>
        /// <param name="context">Le SandBoxManager qui est le contexte du jeu</param>
        /// <param name="row">Numero de la ligne où l'evaluation du circuit s'arrete</param>
        /// <param name="previousState">Etat precedent</param>
        public ShowResult(Editor context, int row, EditorState previousState) : base(context)
        {
            _row = row;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("ShowResult");
            context.gridBoard.SelectRow(_row);
            context.SetResultHeader("Result of row : " + _row);
            context.SetResultText(context.currentCircuit.Evaluate(_row).ToString());
            context.ShowResultPanel(true);
        }

        public override void OnExit() {
            context.gridBoard.DeselectRow(_row);
            context.ShowResultPanel(false);
        }

        public override void OnBackResultClick() { context.CurrentState = _previousState; }

        public override void OnBackButton() { OnBackResultClick(); }
    }
}
