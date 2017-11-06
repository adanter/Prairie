using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryInteraction : Interaction
{
	public bool pickable = true;
	public bool inInventory = false;
	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag("Player");
	}

	protected override void PerformAction () 
	{
		if (pickable && !inInventory) {
			this.gameObject.SetActive (false);
			player.GetComponent<Inventory> ().AddToInventory (this.gameObject);
			inInventory = true;
			return;
		} 
		if (inInventory) {
			player.GetComponent<Inventory> ().RemoveFromInventory (this.gameObject);
			DropAtCurrentLocation ();
			this.gameObject.SetActive (true);
			inInventory = false;
			return;
		}
	}


	private void DropAtCurrentLocation () 
	{
		this.gameObject.transform.SetPositionAndRotation (player.transform.position, this.gameObject.transform.rotation);
	}
}

