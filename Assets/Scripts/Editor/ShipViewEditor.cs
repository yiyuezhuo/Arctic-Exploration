using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipView))]
public class ShipViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var shipView = (ShipView)target;

        if(GUILayout.Button("Step (1 hour)"))
        {
            shipView.model.Move(1);
        }
    }
}
