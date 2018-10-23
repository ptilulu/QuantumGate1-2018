using System;
using UnityEngine;

namespace PC_States
{
    public class MoveRow : EditionState
    {
        private int _sourceRow;
        private EditorState _previousState;

        public MoveRow(Editor context, int sourceRow, EditorState previousState) : base(context)
        {
            _sourceRow = sourceRow;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("MoveRow");
            //context.ShowRowHighlighters();
        }

        public override void OnExit()
        {
            //context.HideHighlighters();
        }

        public override void OnBackButton()
        {
            context.CurrentState = _previousState;
        }

        public override void OnGridClick(Tuple<int, int, Editor.ClickPosition> infos)
        {
            int targetRow = infos.Item1;
            Editor.ClickPosition position = infos.Item3;

            if (position == Editor.ClickPosition.BotLeft
                || position == Editor.ClickPosition.BotRight)
                targetRow++;

            Debug.Log("MoveCol to " + infos + " = " + targetRow);

            if (_sourceRow < targetRow)
                targetRow--;

            context.currentCircuit.MoveRow(_sourceRow, targetRow);

            context.CurrentState = _previousState;
        }
    }
}
