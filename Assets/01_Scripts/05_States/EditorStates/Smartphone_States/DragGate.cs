using System;
using UnityEngine;

namespace Smartphone_States
{
    public class DragGate : EditionState
    {
        private GateObject _draggedGate;
        private Vector3 _previousPosition;
        private EditorState _previousState;

        public DragGate(Editor context, GateObject selectedGate, EditorState previousState) : base(context)
        {
            _draggedGate = selectedGate;
            _previousState = previousState;
            _previousPosition = selectedGate.body.transform.localPosition;
        }

        public override void OnEnter()
        {
            Debug.Log("DragGate");
            if (GameManager.CanVibrate())
                Handheld.Vibrate();
            
            _draggedGate.body.transform.Translate(0, 0, -GridBoard.gateThikness);
        }
        
        public override void OnDrag(Vector3 screenPosition)
        {
            Vector3 position = context.GetGridBoardPosition(screenPosition);

            _draggedGate.body.transform.localPosition = position;
        }

        public override void OnDragEnd(Vector3 position)
        {
            Vector3 gridBoardPosition = context.GetGridBoardPosition(position);

            int nbEntries = _draggedGate.gateStruct.gate.NbEntries;

            gridBoardPosition.x -= GridBoard.localColWidth * (nbEntries - 1) / 2;

            System.Tuple<int, int> coordinates = context.gridBoard.GetGridCoordinates(gridBoardPosition);
            
            int targetRow = coordinates.Item1;
            int targetCol = coordinates.Item2;

            int sourceRow = _draggedGate.gateStruct.row;
            int sourceCol = _draggedGate.gateStruct.col;

            if (targetRow < 0 || targetRow >= context.currentCircuit.NbRow || targetCol < 0 || targetCol >= context.currentCircuit.NbCol)
            {
                context.currentCircuit.RemoveGate(sourceRow, sourceCol);
                context.CurrentState = _previousState;
                return;
            }

            if (!context.currentCircuit.MoveGate(sourceRow, sourceCol, targetRow, targetCol))
            {
                Cancel();
                return;
            }
            
            context.CurrentState = _previousState;
        }

        public override void OnDragCancelled(Vector3 position)
        {
            Cancel();
        }

        private void Cancel()
        {
            AnimationManager.Move(_draggedGate.body, _previousPosition, GridBoard.animationTime);
            context.CurrentState = _previousState;
        }
    }
}
