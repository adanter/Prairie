using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleRotate))]
public class ComponentToggleRotateEditor : Editor {

	ComponentToggleRotate componentToggle;

	public void Awake()
	{
		this.componentToggle = (ComponentToggleRotate)target;
	}

	public override void OnInspectorGUI ()
	{
		// Configuration:
		bool _repeatable = EditorGUILayout.Toggle ("Repeatable?", componentToggle.repeatable);
		GameObject[] _targets = PrairieGUI.drawObjectList<GameObject> ("Objects To Rotate:", componentToggle.targets);
		int _rotX = EditorGUILayout.IntField("X-axis rotation amount:", componentToggle.rotX);
		int _rotY = EditorGUILayout.IntField("Y-axis rotation amount:", componentToggle.rotY);
		int _rotZ = EditorGUILayout.IntField("Z-axis rotation amount:", componentToggle.rotZ);

		// Save:
		if (GUI.changed) {
			Undo.RecordObject(componentToggle, "Modify Component Rotation");
			componentToggle.repeatable = _repeatable;
			componentToggle.targets = _targets;
			componentToggle.rotX = _rotX;
			componentToggle.rotY = _rotY;
			componentToggle.rotZ = _rotZ;
		}

		// Warnings (after properties have been updated):
		this.DrawWarnings();
	}

	public void DrawWarnings()
	{
		foreach (GameObject obj in componentToggle.targets)
		{
			if (obj == null)
			{
				PrairieGUI.warningLabel ("You have one or more empty slots in your list of toggles.  Please fill these slots or remove them.");
				break;
			}
		}
	}
}
