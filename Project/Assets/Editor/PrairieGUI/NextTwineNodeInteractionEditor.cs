using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(NextTwineNodeInteraction))]

public class NextTwineNodeInteractionEditor : Editor
{
	NextTwineNodeInteraction nextTwineNode;
	public int index = 0;
	public TwineNode[] nodes;

	public override void OnInspectorGUI()
	{
		nextTwineNode = (NextTwineNodeInteraction)target;
		nextTwineNode.nextTwineNodeObject = (GameObject)EditorGUILayout.ObjectField ("Next Twine Node Object", nextTwineNode.nextTwineNodeObject, typeof(GameObject), true);

		nodes = GameObject.FindObjectsOfType (typeof(TwineNode)) as TwineNode[];

		List<string> nodeNames = new List<string> ();

		foreach (TwineNode node in nodes) {
			nodeNames.Add (node.name);
		}
		string[] options = nodeNames.ToArray ();

		index = EditorGUILayout.Popup ("Next Node", index, options);
		TwineNode newNode = nextTwineNode.nextTwineNodeObject.AddComponent<TwineNode>() as TwineNode;
		newNode = nodes [index];
	}
}