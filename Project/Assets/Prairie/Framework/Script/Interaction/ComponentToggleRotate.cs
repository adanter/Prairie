using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Rotate Component")]
public class ComponentToggleRotate : PromptInteraction
{
	public GameObject[] targets = new GameObject[0];
	public int rotX = 0;
	public int rotY = 0;
	public int rotZ = 0;

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
			//targets[i].enabled = !targets[i].enabled;
			targets[i].transform.Rotate(rotX, rotY, rotZ);
		}
	}

	override public string defaultPrompt {
		get {
			return "Rotate Something";
		}
	}
}
