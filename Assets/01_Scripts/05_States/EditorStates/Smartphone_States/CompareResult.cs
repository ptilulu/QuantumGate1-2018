using UnityEngine;

/// <summary>
/// Etat pour montrer le résultat du circuit quantique jusqu'a la ligne _row.
/// </summary>
namespace Smartphone_States
{
    public class CompareResult : EditorState
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
        public CompareResult(Editor context, int row, EditorState previousState) : base(context)
        {
            _row = row;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("CompareResult");
            context.ShowCompareResultPanel(true);

            Debug.Log("Expected results : \"" + GameMode.level.Resultats + "\" Got : \"" + context.currentCircuit.Evaluate(_row).ToString() + "\"");
            if (GameMode.level.Resultats.Equals(context.currentCircuit.Evaluate(_row).ToString()))
            {
                Debug.Log("Won");
                context.SetCompareResultText("<color=\"green\">You got the right result!</color> \n" + context.currentCircuit.Evaluate(_row).ToStringWithSprites());
                if (PlayerPrefs.GetInt("vibration") == 1)
                    Handheld.Vibrate();
                context.ShowNextLevelButton(true);
            }
            else
            {
                Debug.Log("Retry");
                string expectedResults = "";
                for (int k = 0; k < GameMode.level.Resultats.Length; k++)
                {
                    if (GameMode.level.Resultats[k].Equals('0'))
                        expectedResults += "<sprite=\"boule_blanche\" name=\"boule_blanche\">";
                    else if (GameMode.level.Resultats[k].Equals('1'))
                        expectedResults += "<sprite=\"boule_noir\" name=\"boule_noir\">";
                    else
                        expectedResults += GameMode.level.Resultats[k];
                }
                context.SetCompareResultText("Expected results : \n" + expectedResults + "\nYour results :\n" + context.currentCircuit.Evaluate(_row).ToStringWithSprites());
            }
        }

        public override void OnExit() {
            context.gridBoard.DeselectRow(_row);
            context.ShowCompareResultPanel(false);
        }

        public override void OnBackResultClick() { context.CurrentState = _previousState; }

        public override void OnBackButton() { OnBackResultClick(); }

        public override void OnNextLevelClick()
        {
            if (GameMode.level.id + 1 < GameMode.levelCollection.Levels.Count)
            {
                context.nextLevelButton.GetComponent<LoadLevel>().level = GameMode.levelCollection.Levels[GameMode.level.id + 1];
                context.nextLevelButton.GetComponent<LoadLevel>().OnClick();
            }

        }

        public override void OnNextLevelButton() { OnNextLevelClick(); }
    }
}
