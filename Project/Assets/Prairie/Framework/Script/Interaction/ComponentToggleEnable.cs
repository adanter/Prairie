using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Enable/Disable Component")]
public class ComponentToggleEnable : PromptInteraction
{
	public GameObject[] targets = new GameObject[0];

	void OnDrawGizmosSelected()
	{
		// sets line specifications
		Gizmos.color = Color.red;
		for (int i = 0; i < targets.Length; i++)
		{
			// Draw red line(s) between the object and the objects whose Behaviours it toggles
			if (targets[i] != null)
			{
				Gizmos.DrawLine(transform.position, targets[i].transform.position);
			}

		}
	}

	// for all attached targets, when the object with this component attached is
	// clicked, switch the enable boolean of each target
	// turns behaviors on/off for light switches
	protected override void PerformAction ()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			targets [i].SetActive (!targets [i].activeSelf);
		}
	}

	override public string defaultPrompt {
		get {
			return "Enable/Disable Something";
		}
	}
}
