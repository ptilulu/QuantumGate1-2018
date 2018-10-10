using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Smartphone_States
{
    public abstract class EditionState : EditorState
    {

        public static bool IsPointerOverUIObject()
        {
            // marche pas :(
            foreach(Touch touch in Input.touches)
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return true;

            return false;
        }

        public EditionState(Editor context) : base(context) { }

        public override void Update()
        {
            
        }

        public override void OnMove(Vector2 delta)
        {
            delta /= -13;

            if ((delta.y > 0 && !context.gridBoard.CameraCanGoTop()) || (delta.y < 0 && !context.gridBoard.CameraCanGoBot()))
                delta.y = 0;

            if ((delta.x > 0 && !context.gridBoard.CameraCanGoLeft()) || (delta.x < 0 && !context.gridBoard.CameraCanGoRight()))
                delta.x = 0;

            context.cam.transform.Translate(delta.x, delta.y, 0f, Space.World);
        }

        public override void OnPinch(float delta)
        {
            delta /= -5000;

            float x = context.cam.transform.position.x;
            float y = context.cam.transform.position.y;
            float z = context.cam.transform.position.z;

            /*bool canGoTop = context.gridBoard.CameraCanGoTop();
            bool canGoBot = context.gridBoard.CameraCanGoBot();
            bool canGoLeft = context.gridBoard.CameraCanGoLeft();
            bool canGoRight = context.gridBoard.CameraCanGoRight();
            float topBound = context.gridBoard.CameraTopBound();
            float botBound = context.gridBoard.CameraBotBound();
            float leftBound = context.gridBoard.CameraLeftBound();
            float rightBound = context.gridBoard.CameraBotBound();

            if (!canGoTop && canGoBot)
                x = topBound;
            if (canGoTop && !canGoBot)
                x = botBound;

            if (!canGoLeft && canGoRight)
                y = leftBound;
            if (canGoLeft && !canGoRight)
                y = rightBound;*/

            z += delta;

            if (z > -50)
                z = -50;

            if (z < -300)
                z = -300;

            context.cam.transform.position = new Vector3(x, y, z);
        }

        public override void OnClick(Vector3 screenPosition)
        {
            /*if (IsPointerOverUIObject())
                return;*/

            Tuple<int, int, Editor.ClickPosition> infos = null;

            infos = context.GetOnScreenGridPosition(screenPosition);

            if (infos != null)
            {
                OnGridClick(infos);
                return;
            }

            GameObject gameObject = context.GetOnScreenObject(screenPosition);

            if (gameObject != null)
                if (gameObject.tag == "entry")
                    OnEntryClick(gameObject);
        }

        public override void OnPress(Vector3 screenPosition)
        {
            /*if (IsPointerOverUIObject())
                return;*/

            Tuple<int, int, Editor.ClickPosition> infos = null;

            infos = context.GetOnScreenGridPosition(screenPosition);

            if (infos != null)
            {
                OnGridPress(infos);
                return;
            }

            GameObject gameObject = context.GetOnScreenObject(screenPosition);

            if (gameObject != null)
                if (gameObject.tag == "entry")
                    OnEntryClick(gameObject);
        }

        public override void OnBackButton() { context.BackToPreviousScene(); }

        public override void OnSettingsClick()
        {
            context.CurrentState = new EditSettings(context, context.CurrentState);
        }
    }
}
