using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Prairie/Utility/AssociatedInteractor")]
public class AssociatedInteractor : MonoBehaviour
{
	public GameObject[] associatedColliders = new GameObject[0];
	public bool repeatable = true;

	void OnDrawGizmosSelected()
	{
		// sets line specifications
		Gizmos.color = Color.red;
		for (int i = 0; i < associatedColliders.Length; i++)
		{
			// Draw red line(s) between the object and the objects whose Colliders it triggers
			if (associatedColliders[i] != null)
			{
				Gizmos.DrawLine(transform.position, associatedColliders[i].transform.position);
			}

		}
	}

	private bool checkGameObjectList(GameObject[] arr, GameObject obj)
	{
		foreach (GameObject g in arr)
		{
			if (g == obj)
			{
				return true;
			}
		}
		return false;
	}

  	public void OnTriggerEnter(Collider other)
	{
		GameObject inside = other.gameObject;
		bool check = checkGameObjectList (associatedColliders, inside);
		// automatically trigger area we're now inside of's interactions
		if (check)
    	{
  			foreach (Interaction i in inside.GetComponents<Interaction> ())
  			{
  				if (!(i is Annotation))
  				{
  					i.Interact (this.gameObject);
  				}
  			}
			if (!repeatable) 
			{
				this.enabled = false;
			}
  		}
  	}
}
