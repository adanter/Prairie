using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[AddComponentMenu("Prairie/Utility/Twine Node")]
public class TwineNode : MonoBehaviour {

	public GameObject[] objectsToTrigger;

	[HideInInspector]
	public string pid;
	public new string name;
	[HideInInspector]
	public string[] tags;
	public string content;
	public string show = "";
	public GameObject[] children;
	[HideInInspector]
	public string[] childrenNames;
	public List<GameObject> parents = new List<GameObject> ();
	public bool isDecisionNode;

	private bool isMinimized = false;
	private bool isOptionsGuiOpen = false;

	private int selectedOptionIndex = 0;

	private Dictionary<string, string> variables = new Dictionary<string, string>();
	private Dictionary<string, string> dicSet = new Dictionary<string, string>();
	private Dictionary<Tuple<string, string>, string> dicIf = new Dictionary<Tuple<string, string>, string>();

	void Update ()
	{
		if (this.enabled) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				this.isMinimized = !this.isMinimized;
			}

			if (this.isDecisionNode) {
				if (this.isOptionsGuiOpen && Input.GetKeyDown (KeyCode.Tab)) {
					// Press TAB to scroll through the children nodes
					this.selectedOptionIndex = (this.selectedOptionIndex + 1) % (children.Length);
				} else if (this.isOptionsGuiOpen && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return))) {
					// Press ENTER or RETURN to select that child
					this.ActivateChildAtIndex (selectedOptionIndex);
				} else if (this.isOptionsGuiOpen && Input.GetKeyDown (KeyCode.E)) {
					// E closes the options list
					this.isOptionsGuiOpen = false;
				}

				// TAB opens the options list if it's not already open
				if (!this.isOptionsGuiOpen && Input.GetKeyDown (KeyCode.Tab)) {
					this.isOptionsGuiOpen = true;
				}
			}
		}
	}

	public void OnGUI()
	{
		if (this.enabled && !this.isMinimized) {

			float frameWidth = Math.Min(Screen.width / 3, 350);
			float frameHeight = Math.Min(Screen.height / 2, 500);
			Rect frame = new Rect (10, 10, frameWidth, frameHeight);

			GUI.BeginGroup (frame);
			GUIStyle style = new GUIStyle (GUI.skin.box);
			style.wordWrap = true;
			style.fixedWidth = frameWidth;
			GUILayout.Box (this.show, style);

			if (isDecisionNode) {
				GUIStyle decisionHintStyle = new GUIStyle (style);
				decisionHintStyle.fontStyle = FontStyle.BoldAndItalic;

				if (!isOptionsGuiOpen) {
					GUILayout.Box ("Press TAB to progress in the story...", decisionHintStyle);
				} else {
					GUILayout.Box ("Press TAB to scroll, E to close, ENTER to choose", decisionHintStyle);
				}
			}

			if (this.isOptionsGuiOpen) {
				// Draw list of buttons for the possible children nodes to visit:
				GUIStyle optionButtonStyle = new GUIStyle (GUI.skin.button);
				optionButtonStyle.fontStyle = FontStyle.Italic;
				optionButtonStyle.wordWrap = true;

				// Set highlighted button to have green text (this state is called `onNormal`):
				optionButtonStyle.onNormal.textColor = Color.white;
				// Set non-highlighted buttons to have grayed out text (state is called `normal`)
				optionButtonStyle.normal.textColor = Color.gray;

				selectedOptionIndex = GUILayout.SelectionGrid(selectedOptionIndex, this.childrenNames, 1, optionButtonStyle);
			}
			
			GUI.EndGroup ();

		} else if (this.enabled && this.isMinimized) {

			// Draw minimized GUI instead
			Rect frame = new Rect (10, 10, 10, 10);

			GUI.Box (frame, "");

		}
	
	}

	/// <summary>
	/// Trigger the interactions associated with this Twine Node.
	/// </summary>
	/// <param name="interactor"> The interactor acting on this Twine Node, typically a player. </param>
	public void StartInteractions(GameObject interactor) 
	{
		if (this.enabled) 
		{
			foreach (GameObject gameObject in objectsToTrigger) 
			{
				gameObject.InteractAll (interactor);
			}
		}
	}

	/// <summary>
	/// Activate this TwineNode (provided it isn't already
	/// 	active/enabled and it has some active parent)
	/// </summary>
	/// <param name="interactor">The interactor.</param>
	public bool Activate(GameObject interactor)
	{
		if (!this.enabled && this.HasActiveParentNode()) 
		{
			this.enabled = true;
			this.isMinimized = false;
			this.isOptionsGuiOpen = false;
			this.ParseContent ();
			this.SetVariables ();
			this.ChoiceByVarValue ();
			this.DeactivateAllParents ();
			this.StartInteractions (interactor);

			return true;
		}

		return false;
	}

	/// <summary>
	/// Find the FirstPersonInteractor in the world, and use it to activate
	/// 	the TwineNode's child at the given index.
	/// </summary>
	/// <param name="index">Index of the child to activate.</param>
	private void ActivateChildAtIndex(int index) 
	{
		// Find the interactor:
		FirstPersonInteractor interactor = (FirstPersonInteractor) FindObjectOfType(typeof(FirstPersonInteractor));

		if (interactor != null) {
			GameObject interactorObject = interactor.gameObject;
		
			// Now activate the child using this interactor!
			TwineNode child = this.children [index].GetComponent<TwineNode> ();
			child.Activate (interactorObject);
		}
	}

	public void Deactivate() 
	{
		this.enabled = false;
	}

	/// <summary>
	/// Check if this Twine Node has an active parent node.
	/// </summary>
	/// <returns><c>true</c>, if there is an active parent node, <c>false</c> otherwise.</returns>
	public bool HasActiveParentNode() 
	{
		foreach (GameObject parent in parents) 
		{
			if (parent.GetComponent<TwineNode> ().enabled) 
			{
				// Pass variables from active parent node to child node
				this.variables = parent.variables
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Deactivate all parents of this Twine Node.
	/// </summary>
	private void DeactivateAllParents()
	{
		foreach (GameObject parent in parents) 
		{
			parent.GetComponent<TwineNode> ().Deactivate ();
		}
	}

	/// <summary>
	/// Parse the content of this Twine Node.
	/// </summary>
	private void ParseContent()
	{
		List<string> raw = this.content.Split("\n");
		foreach (string section in raw) 
		{
			if (section.Contains("set")) {
				Regex.Replace(section, "()", "");
				string[] pair = section.Split(" ");
				dicSet[pair[1].Substring(1)] = pair[3]
			}

			if (section.Contains("if")) {
				Regex.Replace(section, "()", "");
				string[] series = section.Split(" ");
				Tuple<string, string> tuple = new Tuple<string, string>(series[1].Substring(1), series[3]);
				Regex.Replace(series[4], "[]", "");
				dicIf[tuple] = series[4]
			}
		}
	}

	/// <summary>
	/// Set the variables of this Twine Node.
	/// </summary>
	private void SetVariables()
	{	
		if (dicSet.Count != 0) {
			List<string> keyList = new List<string>(this.variables.Keys);
			foreach (string v in keyList) {
				if (dicSet.ContainsKey(v)) {
					variables[v] = dicSet[v]
				}
			}
		}
	}

	/// <summary>
	/// Choose what to show based on variable value of this Twine Node.
	/// </summary>
	private void ChoiceByVarValue()
	{
		if (dicSet.Count != 0) {
			List<string> ifList = new List<string>(this.dicIf.Keys);
			foreach (Tuple<string, string> i in ifList) {
				if (string.Equals(variables[i.Item1], i.Item2)) {
					show = dicIf[i.Item1]
				}
			}
	}
}
