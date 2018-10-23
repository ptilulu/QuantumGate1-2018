using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PC_States
{
    public abstract class EditionState : EditorState
    {
        public static bool IsPointerOverUIObject()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public EditionState(Editor context) : base(context) { }

        public override void Update()
        {
            if (Input.GetKey(KeyCode.Z) && context.gridBoard.CameraCanGoTop())
                context.cam.transform.Translate(0f, 1f, 0f, Space.World);
            else if (Input.GetKey(KeyCode.S) && context.gridBoard.CameraCanGoBot())
                context.cam.transform.Translate(0f, -1f, 0f, Space.World);
            if (Input.GetKey(KeyCode.Q) && context.gridBoard.CameraCanGoLeft())
                context.cam.transform.Translate(1f, 0f, 0f, Space.World);
            else if (Input.GetKey(KeyCode.D) && context.gridBoard.CameraCanGoRight())
                context.cam.transform.Translate(-1f, 0f, 0f, Space.World);

            if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus)
                || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                context.cam.transform.Translate(0f, 0f, 1f, Space.World);
            }
            else if (Input.GetKey(KeyCode.Minus)
                || Input.GetKey(KeyCode.KeypadMinus)
                || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                context.cam.transform.Translate(0f, 0f, -1f, Space.World);

                if (context.gridBoard.CameraCanGoBot() ^ context.gridBoard.CameraCanGoTop())
                {
                    if (!context.gridBoard.CameraCanGoTop())
                        context.cam.transform.position = new Vector3(
                            context.cam.transform.position.x,
                            context.gridBoard.CameraTopBound(),
                            context.cam.transform.position.z);

                    if (!context.gridBoard.CameraCanGoBot())
                        context.cam.transform.position = new Vector3(
                            context.cam.transform.position.x,
                            context.gridBoard.CameraBotBound(),
                            context.cam.transform.position.z);
                }
                if (context.gridBoard.CameraCanGoLeft() ^ context.gridBoard.CameraCanGoRight())
                {
                    if (!context.gridBoard.CameraCanGoLeft())
                        context.cam.transform.position = new Vector3(
                            context.gridBoard.CameraLeftBound(),
                            context.cam.transform.position.y,
                            context.cam.transform.position.z);

                    if (!context.gridBoard.CameraCanGoRight())
                        context.cam.transform.position = new Vector3(
                            context.gridBoard.CameraRightBound(),
                            context.cam.transform.position.y,
                            context.cam.transform.position.z);
                }
            }
        }
        
        public override void OnClick(Vector3 screenPosition)
        {
            if (IsPointerOverUIObject())
                return;

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
