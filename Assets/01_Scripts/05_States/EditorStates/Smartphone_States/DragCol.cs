using System;
using UnityEngine;

namespace Smartphone_States
{
    public class DragCol : EditionState
    {
        private int _sourceCol;
        private EditorState _previousState;

        public DragCol(Editor context, int sourceCol, EditorState previousState) : base(context)
        {
            _sourceCol = sourceCol;
            _previousState = previousState;
        }

        public override void OnEnter()
        {
            Debug.Log("MoveCol from " + _sourceCol);
        }

        public override void OnDrag(Vector3 position)
        {
            Cancel();
        }

        public override void OnDragEnd(Vector3 position)
        {
            Cancel();
        }

        public override void OnDragCancelled(Vector3 position)
        {
            Cancel();
        }

        public void Cancel()
        {
            context.CurrentState = _previousState;
        }
    }
}
