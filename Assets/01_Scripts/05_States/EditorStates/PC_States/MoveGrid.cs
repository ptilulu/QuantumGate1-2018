using UnityEngine;

namespace PC_States
{
    public class MoveGrid : EditorState
{
    public MoveGrid(Editor context) : base(context) { }
    
    public override void OnEnter()
    {
        Debug.Log("MoveGrid");
    }
    }
}
