using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

/// <summary>
/// Etat pour montrer le résultat du circuit quantique jusqu'a la ligne _row.
/// </summary>
namespace PC_States
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

        /// <summary>
        /// Converti l'etat pour etre affichable avec les sprites TextMesh Pro
        /// </summary>
        public string ConvertWithSprites(string binary_string)
        {
            string working_string = "";
            for (int k = 0; k < binary_string.Length; k++)
            {
                if (binary_string[k].Equals('0'))
                    working_string += "<sprite=\"boule_blanche\" name=\"boule_blanche\">";
                else if (binary_string[k].Equals('1'))
                    working_string += "<sprite=\"boule_noir\" name=\"boule_noir\">";
                else
                    working_string += binary_string[k];
            }
            return working_string;
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
                context.ShowNextLevelButton(true);
            }
            else
            {
                Debug.Log("Retry");
                string expectedResults = ConvertWithSprites(GameMode.level.Resultats);
                context.SetCompareResultText("Expected results : \n" + expectedResults + "\nYour results :\n" + context.currentCircuit.Evaluate(_row).ToStringWithSprites());
            }
        }

        public override void OnExit() { context.ShowCompareResultPanel(false); }

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
