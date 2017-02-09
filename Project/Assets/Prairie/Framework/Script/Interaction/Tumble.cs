﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Tumble")]
public class Tumble : PromptInteraction
{
	/// <summary>
	/// Allows user to rotate object.
	/// </summary>
	private bool pickedUp = false;

	// When the user interacts with object, they invoke the ability to 
	// tumble the object with the I, J, K and L keys. Interacting
	// with the object again revokes this ability.

	protected void Update()
	{
		if (pickedUp)
		{
			if (Input.GetKey (KeyCode.L)) // right
			{
				transform.RotateRelativeToCamera (-10, 0);
			}
			else if (Input.GetKey (KeyCode.J)) // left
			{
				transform.RotateRelativeToCamera (10, 0);
			}
			else if (Input.GetKey (KeyCode.K)) // down
			{
				transform.RotateRelativeToCamera (0, 10);
			}
			else if (Input.GetKey (KeyCode.I)) // up
			{
				transform.RotateRelativeToCamera (0, -10);
			}
		}
	}

	protected override void PerformAction() {
		pickedUp = !pickedUp;
		FirstPersonInteractor player = this.GetPlayer ();
		if (player != null) {
			if (pickedUp)
			{
				player.SetCanMove (false);
				player.SetDrawsGUI (false);
			}
			else
			{
				player.SetCanMove (true);
				player.SetDrawsGUI (true);
			}
		}
	}

	override public string defaultPrompt {
		get {
			return "Pick Up Object";
		}
	}
}
