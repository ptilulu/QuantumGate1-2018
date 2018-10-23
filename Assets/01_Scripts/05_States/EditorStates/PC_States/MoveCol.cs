using System;
using UnityEngine;

namespace PC_States
{
    public class MoveCol : EditionState
    {
        private int _sourceCol;
        private EditorState _previousState;

        public MoveCol(Editor context, int sourceCol, EditorState previousState) : base(context)
        {
            _sourceCol = sourceCol;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("MoveCol from " + _sourceCol);
            //context.ShowColHighlighters();
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
            int targetCol = infos.Item2;
            Editor.ClickPosition position = infos.Item3;

            if (position == Editor.ClickPosition.TopRight
                || position == Editor.ClickPosition.BotRight)
                targetCol++;

            Debug.Log("MoveCol to " + infos + " = " + targetCol);

            if (_sourceCol < targetCol)
            {
                targetCol--;
                Debug.Log("MoveCol, source is lesser then, to " + targetCol);
            }

            context.currentCircuit.MoveCol(_sourceCol, targetCol);

            context.CurrentState = _previousState;
        }
    }
}
