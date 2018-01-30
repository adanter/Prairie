﻿using UnityEngine;
using System.Collections.Generic;
//using TwineVariables;
using System;

[AddComponentMenu("Prairie/Utility/Twine Node")]
public class TwineNode : MonoBehaviour
{

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
    public List<GameObject> parents = new List<GameObject>();
    public bool isDecisionNode;
    public bool isConditionNode;

    public List<string> assignmentVars;
    public List<string> assignmentVals;

    public List<string> conditionalVars;
    public List<string> conditionalVals;
    public List<string> conditionalLinks;

    public TwineVariables globalVariables;

    private bool isMinimized = false;
    private bool isOptionsGuiOpen = false;

    private int selectedOptionIndex = 0;

    public static List<TwineNode> TwineNodeList = new List<TwineNode>();
    public static int visibleNodeIndex = 0;
    public static int insertIndex = -1;
    private static bool fanfold = true;
    public static string storyTitle = "";


    void Update()
    {
        if (this.enabled)
        {
            if (Input.GetKeyDown(KeyCode.C) && TwineNodeList.IndexOf(this) == 0)
            {
                fanfold = !fanfold;
            }
            //            if (visibleNodeIndex == null) {
            //                visibleNodeIndex = 0;
            //            }
            //            print(visibleNodeIndex);
            //            print(TwineNodeList.IndexOf(this));
            if (!TwineNodeList.Contains(this))
            {
                TwineNodeList.Add(this);
                //                storyTitle = this.name;
                //                print(this.name);
                //                insertIndex = TwineNodeList.IndexOf(this);
                //                print("printing");
                //                print(TwineNodeList.Count);
                //                    foreach (TwineNode item in TwineNodeList) {
                //                        print(item.name);
                //                    }
                //                    if (TwineNodeList.Count > 1){
                //                        this.isMinimized = true;
                //                    }
            }
            if (Input.GetKeyDown(KeyCode.Tab) && TwineNodeList.IndexOf(this) == 0)
            {
                if (visibleNodeIndex == TwineNodeList.Count - 1)
                {
                    visibleNodeIndex = 0;
                }
                else
                {
                    visibleNodeIndex++;
                }
            }
            if (TwineNodeList.IndexOf(this) == visibleNodeIndex)
            {
                this.isMinimized = false;
            }
            else
            {
                this.isMinimized = true;
            }
            //            if (Input.GetKeyDown (KeyCode.Alpha0)) {
            //				visibleNodeIndex = 0;
            //			}
            //            if (Input.GetKeyDown (KeyCode.Alpha1)) {
            //				visibleNodeIndex = 1;
            //			}
            //			if (Input.GetKeyDown (KeyCode.Q) {
            //				this.isMinimized = ;
            //			}

            if (this.isDecisionNode)
            {
                this.isOptionsGuiOpen = true;
            }
            else if (this.isConditionNode)
            {
                // get the $color value from global list
                // check the platform name by check $color:platform pair stored in condition node
                // check child node name to match platform name
                // activate childnode
                this.ActivateChildAtIndex(0);
                Debug.Log("This is a Condition Node: " + this.name);
            }
        }
    }

    public void OnGUI()
    {
        if (fanfold)
        {
            float frameWidth = Math.Min(Screen.width / 3, 150);
            float frameHeight = Math.Min(Screen.height / 2, 500);
            //            height += frameHeight/10;
            //            this.heightAdded = true;
            int index = TwineNodeList.IndexOf(this);
            Rect frame = new Rect(10 + index * 150, 10, frameWidth, frameHeight);
            GUI.BeginGroup(frame);
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.wordWrap = true;
            style.fixedWidth = frameWidth;
            GUILayout.Box(this.content, style);
            //            print(height);

            if (isDecisionNode)
            {
                GUIStyle decisionHintStyle = new GUIStyle(style);
                decisionHintStyle.fontStyle = FontStyle.BoldAndItalic;

                if (!isOptionsGuiOpen)
                {
                    GUILayout.Box("Press TAB to progress in the story...", decisionHintStyle);
                }
                else
                {
                    GUILayout.Box("Press TAB to scroll, E to close, ENTER to choose", decisionHintStyle);
                }
            }

            if (this.isOptionsGuiOpen)
            {
                // Draw list of buttons for the possible children nodes to visit:
                GUIStyle optionButtonStyle = new GUIStyle(GUI.skin.button);
                optionButtonStyle.fontStyle = FontStyle.Italic;
                optionButtonStyle.wordWrap = true;

                // Set highlighted button to have green text (this state is called `onNormal`):
                optionButtonStyle.onNormal.textColor = Color.white;
                // Set non-highlighted buttons to have grayed out text (state is called `normal`)
                optionButtonStyle.normal.textColor = Color.gray;

                selectedOptionIndex = GUILayout.SelectionGrid(selectedOptionIndex, this.childrenNames, 1, optionButtonStyle);
            }

            GUI.EndGroup();

        }
        else if (this.enabled && !this.isMinimized)
        {
            float frameWidth = Math.Min(Screen.width / 3, 350);
            float frameHeight = Math.Min(Screen.height / 2, 500);
            Rect frame = new Rect(10, 10, frameWidth, frameHeight);
            GUI.BeginGroup(frame);
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.wordWrap = true;
            style.fixedWidth = frameWidth;
            GUILayout.Box(this.content, style);

            if (isDecisionNode)
            {
                GUIStyle decisionHintStyle = new GUIStyle(style);
                decisionHintStyle.fontStyle = FontStyle.BoldAndItalic;

                if (!isOptionsGuiOpen)
                {
                    GUILayout.Box("Press TAB to progress in the story...", decisionHintStyle);
                }
                else
                {
                    GUILayout.Box("Press TAB to scroll, E to close, ENTER to choose", decisionHintStyle);
                }
            }

            if (this.isOptionsGuiOpen)
            {
                // Draw list of buttons for the possible children nodes to visit:
                GUIStyle optionButtonStyle = new GUIStyle(GUI.skin.button);
                optionButtonStyle.fontStyle = FontStyle.Italic;
                optionButtonStyle.wordWrap = true;

                // Set highlighted button to have green text (this state is called `onNormal`):
                optionButtonStyle.onNormal.textColor = Color.white;
                // Set non-highlighted buttons to have grayed out text (state is called `normal`)
                optionButtonStyle.normal.textColor = Color.gray;

                selectedOptionIndex = GUILayout.SelectionGrid(selectedOptionIndex, this.childrenNames, 1, optionButtonStyle);
            }

            GUI.EndGroup();

        }
        else if (this.enabled && this.isMinimized)
        {

            // Draw minimized GUI instead
            Rect frame = new Rect(10, 10, 10, 10);

            GUI.Box(frame, "");

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
                gameObject.InteractAll(interactor);
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
        //        print(TwineNodeList);
        if (!this.enabled && this.HasActiveParentNode())
        {
            this.enabled = true;
            this.isMinimized = false;
            this.TakeAction();
            this.isOptionsGuiOpen = false;
            this.DeactivateAllParents();
            TwineNodeList.Insert(insertIndex, this);
            //            TwineNodeList.Add(this);
            visibleNodeIndex = TwineNodeList.IndexOf(this);
            this.StartInteractions(interactor);

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
        FirstPersonInteractor interactor = (FirstPersonInteractor)FindObjectOfType(typeof(FirstPersonInteractor));

        if (interactor != null)
        {
            GameObject interactorObject = interactor.gameObject;

            // Now activate the child using this interactor!
            TwineNode child = this.children[index].GetComponent<TwineNode>();
            child.Activate(interactorObject);
        }
    }

    public void Deactivate()
    {
        this.enabled = false;
        insertIndex = TwineNodeList.IndexOf(this);
        //        print(insertIndex);
        TwineNodeList.Remove(this);
        //        print("deactivate" + TwineNodeList.Count);
    }

    //    public void AddToList()
    //    {
    //        TwineNodeList.Add(this);
    //    }

    /// <summary>
    /// Check if this Twine Node has an active parent node.
    /// </summary>
    /// <returns><c>true</c>, if there is an active parent node, <c>false</c> otherwise.</returns>
    public bool HasActiveParentNode()
    {
        foreach (GameObject parent in parents)
        {
            if (parent.GetComponent<TwineNode>().enabled)
            {
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
            parent.GetComponent<TwineNode>().Deactivate();
        }
    }


    /// <summary>
    /// Read the content of the node and make according actions
    /// </summary>
    private void TakeAction()
    {
        if (globalVariables == null)
        {
            globalVariables = TwineVariables.GetVariableObject();
        }
        if (assignmentVars != null)
        {
            print("Node '" + this.name + "' has assignments!");
            for (int i = 0; i < assignmentVars.Count; i++)
            {
                string varName = assignmentVars[i];
                string varValue = assignmentVals[i];
                globalVariables.AssignValue(varName, varValue);
            }
        }
        if (conditionalVars != null)
        {
            print("Node '" + name + "' has conditions!");
            Dictionary<string, string> dict = globalVariables.GetVariables();
            for (int i = 0; i < conditionalVars.Count; i++)
            {
                string varName = conditionalVars[i];
                string varValue = conditionalVals[i];
                if (varValue == dict[varName])
                {
                    //TODO: Actually enable and disable links
                    print("Enable link '" + conditionalLinks[i] + "'");
                }
                else
                {
                    print("Disable link '" + conditionalLinks[i] + "'");
                }
            }
        }

        // else if (this.conditionals.Count != 0) {
        // 	List<string[]> ifList = new List<string[]> (this.dicIf.Keys);
        // 	foreach (string[] i in ifList) {
        // 		if (string.Equals(variables [i[0]], i[1])) {
        // 			show = dicIf [i].ToString();
        // 		}
        // 	}
        // }

    }

    public void AddAssignment(string var, string value)
    {
        Debug.Log("Add Assignment");
        if (assignmentVars == null)
        {
            assignmentVars = new List<string>();
            assignmentVals = new List<string>();
        }
        assignmentVars.Add(var);
        assignmentVals.Add(value);
    }

    public void AddConditional(string var, string value, string link)
    {
        Debug.Log("Add conditional");
        if (conditionalVars == null)
        {
            conditionalVars = new List<string>();
            conditionalVals = new List<string>();
            conditionalLinks = new List<string>();
        }
        conditionalVars.Add(var);
        conditionalVals.Add(value);
        conditionalLinks.Add(link);
    }
}
